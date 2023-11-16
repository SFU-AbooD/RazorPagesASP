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
    public class IndexModel : PageModel
    {
        private readonly Efcorelearningrazorpages.Data.SchoolContext _context;

        public IndexModel(Efcorelearningrazorpages.Data.SchoolContext context)
        {
            _context = context;
        }
        public InstructorIndexData InstructorData { get; set; }
        public int InstructorID { get; set; }
        public int CourseID { get; set; }
        public IList<Instructor> Instructor { get;set; } = default!;
        public async Task OnGetAsync(int? id, int? courseID)
        {
            // no trakcing means that even if you changed anything it will be only on the clr

            if (_context.Instructors != null)
            {
                InstructorData = new InstructorIndexData();
                InstructorData.Instructors = await _context.Instructors.Include(x=>x.OfficeAssignment).Include(x => x.Courses).ThenInclude(x => x.Department)
                    .OrderBy(x=>x.LastName).AsNoTracking()
                    .ToListAsync();
                if (id != null)
                {
                    // here we will issue the instrcutor by the current thead instead of calling await 
                    InstructorID = id.Value;
                    Instructor ins = InstructorData.Instructors.Where(x => x.ID == id).Single();
                    // calling here the courses from the one i got above!
                    InstructorData.Courses = ins.Courses;   
                }
                if (courseID != null)
                {
                    CourseID = courseID.Value;
                    IEnumerable<Enrollment> entrolls = await _context.Enrollments.Where(x => x.CourseID == courseID)
                        .Include(x => x.Student).AsNoTracking().ToListAsync();
                    InstructorData.Enrollments = entrolls;
                }
            }
        }
    }
}
