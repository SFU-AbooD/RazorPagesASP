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
    public class IndexModel : PageModel
    {
        private readonly Efcorelearningrazorpages.Data.SchoolContext _context;

        public IndexModel(Efcorelearningrazorpages.Data.SchoolContext context)
        {
            _context = context;
        }

        public IList<CourseViewModel> Course { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Courses != null)
            {
                // this is eager loading technique! since we are using the include method!
                Course = await _context.Courses.Select(p => new CourseViewModel()
                {
                    CourseID = p.CourseID,
                    Title = p.Title,
                    Credits = p.Credits,
                    DepartmentName = p.Department.Name
                }).ToListAsync();
            }
        }
    }
}
