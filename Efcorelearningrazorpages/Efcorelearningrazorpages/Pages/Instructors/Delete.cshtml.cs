using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Efcorelearningrazorpages.Data;
using Efcorelearningrazorpages.Models;

namespace Efcorelearningrazorpages.Pages.Instructors
{
    public class DeleteModel : PageModel
    {
        private readonly Efcorelearningrazorpages.Data.SchoolContext _context;

        public DeleteModel(Efcorelearningrazorpages.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Instructor Instructor { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);

            if (instructor == null)
            {
                return NotFound();
            }
            else 
            {
                Instructor = instructor;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }
            var instructor = await _context.Instructors.Include(x=>x.Courses).SingleAsync(x=>x.ID == id);

            if (instructor == null)
            {
                return RedirectToPage("./Index");
            }
            if (instructor != null)
            {
                var departments = await _context.Departments.Where(x => x.InstructorID == instructor.ID).ToListAsync();
                departments.ForEach(x => x.Administrator = null);
                Instructor = instructor;
                _context.Instructors.Remove(Instructor);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
