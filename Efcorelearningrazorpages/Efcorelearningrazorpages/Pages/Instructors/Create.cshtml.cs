using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Efcorelearningrazorpages.Data;
using Efcorelearningrazorpages.Models;
using Microsoft.EntityFrameworkCore;

namespace Efcorelearningrazorpages.Pages.Instructors
{
    public class CreateModel : InstructorCoursesPageModel
    {
        private readonly SchoolContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(Efcorelearningrazorpages.Data.SchoolContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;   
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Instructor t = new();
            await Poplulate(_context!, t);
            return Page();
        }

        [BindProperty]
        public Instructor Instructor { get; set; } = default!;

        Instructor newins = new();
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string[] selectedcourses)
        {

            if (selectedcourses.Length > 0)
            {
                newins.Courses = new List<Course>();
                _context.Courses.Load();
                // so this is used to load the entire course thing but without the list!!
            }
            foreach (var i in selectedcourses)
            {
                // this line is used to fetch + make sure this is not fake value and make database inconsistant!
                var find_course = await _context.Courses.FindAsync(int.Parse(i));
                if (find_course != null)
                {
                    newins.Courses.Add(find_course);
                }
            }
            try
            {
                if ((await TryUpdateModelAsync<Instructor>(newins,
                                "Instructor",
                                i => i.FirstMidName, i => i.LastName,
                                i => i.HireDate, i => i.OfficeAssignment)))
                {
                    _context.Instructors.Add(Instructor);
                    await _context.SaveChangesAsync();

                    return RedirectToPage("./Index");
                }
            }
            catch { }
            await Poplulate(_context, newins);
            return Page();
        }

    }
}
