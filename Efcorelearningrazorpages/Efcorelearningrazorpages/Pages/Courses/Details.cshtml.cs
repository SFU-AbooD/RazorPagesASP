using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Efcorelearningrazorpages.Data;
using Efcorelearningrazorpages.Models;

namespace Efcorelearningrazorpages.Pages.Courses
{
    public class DetailsModel : PageModel
    {
        private readonly Efcorelearningrazorpages.Data.SchoolContext _context;

        public DetailsModel(Efcorelearningrazorpages.Data.SchoolContext context)
        {
            _context = context;
        }

      public Course Course { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }
            else 
            {
                Course = course;
            }
            return Page();
        }
    }
}
