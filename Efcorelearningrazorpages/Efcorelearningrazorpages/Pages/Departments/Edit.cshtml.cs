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
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (id == null)
                return BadRequest();
            //  _context.Attach(Department).State = EntityState.Modified;
            var departmentToUpdate = await _context.Departments.Include(x => x.Administrator).FirstOrDefaultAsync(x => x.DepartmentID == id);
            if (departmentToUpdate == null)
            {
                return HandleDeletedDepartment();
            }
            /*
             * SET NOCOUNT ON;
             * update the department!
                UPDATE [Departments] SET [Name] = @p0
                WHERE [DepartmentID] = @p1 AND [ConcurrencyToken] = @p2;
                SELECT [ConcurrencyToken]
                FROM [Departments]
                WHERE @@ROWCOUNT = 1 AND [DepartmentID] = @p1;
            */
            //departmentToUpdate.token = Department.token; this is so wrong since ef core will trigger the update command!
            _context.Entry<Department>(departmentToUpdate)
                .Property(x => x.token).OriginalValue = Department.token;
            // OriginalValue is what EF Core uses in the WHERE clause. Before the highlighted line of code executes 
            // so for that ef core will issue any where clause using the original values!!
            // the orignal value is the value that is used for comparesion pracises like @p2 = original value for token
            // this will trick the ef core that this is the value that we got from database 
            // then ef core will issue a where clause for this whith the real value that updated in database!
            if (await TryUpdateModelAsync<Department>(departmentToUpdate, "Department", s => s.Name, s => s.StartDate, s => s.Budget, s => s.InstructorID))
              {
                try {
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // take the expception entry using this
                    // ef core will throw all entities that has error in the object!
                    // we want to get it!!!
                    // i think its not updated!
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    // using this we can get the value from user and also the value from the database using the object primary key!!
                    var databaseEntry = await exceptionEntry.GetDatabaseValuesAsync();
                    // this one will get the values from the database as a copy and if it does not exist then null is found!
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save due to non found error");
                        return Page();
                    }
                    var dbvalues = (Department)databaseEntry.ToObject();
                    SetDbErrorMessages(dbvalues, clientValues);
                    Department.token = (byte[])dbvalues.token;
                    // this is called cast!
                    // set the token back the token
                    ModelState.Remove($"{nameof(Department)}.{nameof(Department.token)}");
                    // we need to remove this since rendring engine will take a precedance of modelstate instead of Department property!
                }

            }
            InstructorNameSL = new SelectList(_context.Instructors, "ID", "FullName", Department.InstructorID);
            return Page();
        }
        private IActionResult HandleDeletedDepartment()
        {
            // ModelState contains the posted data because of the deletion error
            // and overides the Department instance values when displaying Page().
            ModelState.AddModelError(string.Empty,
                "Unable to save. The department was deleted by another user.");
            InstructorNameSL = new SelectList(_context.Instructors, "ID", "FullName", Department.InstructorID);
            return Page();
        }
        private bool DepartmentExists(int id)
        {
          return (_context.Departments?.Any(e => e.DepartmentID == id)).GetValueOrDefault();
        }
        private void SetDbErrorMessages(Department dbvalues, Department clientvalues)
        {
            if (dbvalues.InstructorID != clientvalues.InstructorID)
            {
                ModelState.AddModelError($"{nameof(Department)}.{nameof(Department.InstructorID)}",
                    "InstrcuorID are not the sade btww!"); 
            }

            if (dbvalues.Name != clientvalues.Name)
            {
                ModelState.AddModelError($"{nameof(Department)}.{nameof(Department.Name)}",
                    "Name are not the sade btww!");
            }
            ModelState.AddModelError(string.Empty,
    "The record you attempted to edit "
  + "was modified by another user after you. The "
  + "edit operation was canceled and the current values in the database "
  + "have been displayed. If you still want to edit this record, click "
  + "the Save button again.");
        }
}
}
