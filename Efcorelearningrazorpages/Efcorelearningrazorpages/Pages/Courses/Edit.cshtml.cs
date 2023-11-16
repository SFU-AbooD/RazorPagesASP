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

namespace Efcorelearningrazorpages.Pages.Courses
{
    public class EditModel : DepartmentNamePageModel
    {
        private readonly Efcorelearningrazorpages.Data.SchoolContext _context;

        public EditModel(Efcorelearningrazorpages.Data.SchoolContext context)
        {
            _context = context;
        }

       // [BindProperty(SupportsGet =false)] // only on post requests
        public Course Course { get; set; } = new Course();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course =  await _context.Courses.Include(x=>x.Department).FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }
            Course = course;
            
            populateselectlist(_context, course.Department.DepartmentID);
        //   ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!(await TryUpdateModelAsync<Course>(Course,"course",c=>c.CourseID ,c => c.Credits, c => c.DepartmentID, c => c.Title)))
            {
                populateselectlist(_context, Course.DepartmentID);
                return Page();
            }

            _context.Attach(Course).State = EntityState.Modified; // issuing a update sql command!

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(Course.CourseID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CourseExists(int id)
        {
          return (_context.Courses?.Any(e => e.CourseID == id)).GetValueOrDefault();
        }
    }
}
