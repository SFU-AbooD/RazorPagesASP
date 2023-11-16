using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Efcorelearningrazorpages.Data;
using Efcorelearningrazorpages.Models;

namespace Efcorelearningrazorpages.Pages.Students
{
    public class DetailsModel : PageModel
    {
        private readonly Efcorelearningrazorpages.Data.SchoolContext _context;

        public DetailsModel(Efcorelearningrazorpages.Data.SchoolContext context)
        {
            _context = context;
        }

      public Student? Student { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }
            Student = await _context.Students
               .Include(x => x.Enrollments)
               .ThenInclude(x => x.Course)
                .AsNoTracking()
                .FirstOrDefaultAsync(x=>x.ID == id);
            if (Student == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
