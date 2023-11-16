using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Efcorelearningrazorpages.Data;
using Efcorelearningrazorpages.Models;

namespace Efcorelearningrazorpages.Pages.Departments
{
    public class EditModel : PageModel
    {
        private readonly Efcorelearningrazorpages.Data.SchoolContext _context;

        public EditModel(Efcorelearningrazorpages.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Department Department { get; set; } = default!;
        public SelectList InstructorNameSL { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(x => x.Administrator).AsNoTracking().
                FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }
            Department = department;
            InstructorNameSL = new SelectList(_context.Instructors, "ID", "FirstMidName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //  _context.Attach(Department).State = EntityState.Modified;
            var departmentToUpdate = await _context.Departments.Include(x => x.Administrator).FirstOrDefault(x => x.DepartmentID == id);
            if (departmentToUpdate == null)
            {
               // return HandleDeletedDepartment();
            }
            _context.Entry<Department>(departmentToUpdate)
                .Property(x => x.token).OriginalValue = Department.token;
            // this will trick the ef core that this is the value that we got from database 
            // then ef core will issue a where clause for this whith the real value that updated in database!
            if(await TryUpdateModelAsync<Department>(departmentToUpdate, "Department", s => s.Name, s => s.StartDate, s => s.Budget, s => s.InstructorID)))
              {
                try {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    // this one will get the values from the database as a copy and if it does not exist then null is found!
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save due to non found error");
                        return Page();
                    }
                    var dbvalues = (Department)databaseEntry.ToObject();
                    //await SetDbErrorMessage(dbValues, clientValues, _context);
                    Department.token = (byte[])dbvalues.token;
                    // set the token back the token
                    ModelState.Remove($"{nameof(Department)}.{nameof(Department.token)}");

                }

            }




            return RedirectToPage("./Index");
        }

        private bool DepartmentExists(int id)
        {
          return (_context.Departments?.Any(e => e.DepartmentID == id)).GetValueOrDefault();
        }
    }
}
