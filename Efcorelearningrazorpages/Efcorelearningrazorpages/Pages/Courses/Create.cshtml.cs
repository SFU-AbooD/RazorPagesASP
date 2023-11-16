using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Efcorelearningrazorpages.Data;
using Efcorelearningrazorpages.Models;

namespace Efcorelearningrazorpages.Pages.Courses
{
    public class CreateModel : DepartmentNamePageModel
    {
        private readonly Efcorelearningrazorpages.Data.SchoolContext _context;

        public CreateModel(Efcorelearningrazorpages.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            populateselectlist(_context);
            //ViewData["DepartmentID"] = new SelectList(_context.Departments, nameof(Department.DepartmentID), nameof(Department.Name));
            return Page();
        }
        public Course Course { get; set; } = new Course();
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!(await TryUpdateModelAsync<Course>(Course, "course", s => s.CourseID, s => s.DepartmentID, s => s.Title, s => s.Credits)))
            {
                populateselectlist(_context, Course.DepartmentID);
                return Page();
            }
            else
            {

                _context.Courses.Add(Course);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
        }
    }
}
