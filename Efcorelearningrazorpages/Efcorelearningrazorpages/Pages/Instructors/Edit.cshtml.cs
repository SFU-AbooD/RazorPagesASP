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

namespace Efcorelearningrazorpages.Pages.Instructors
{
    public class EditModel : InstructorCoursesPageModel
    {
        private readonly Efcorelearningrazorpages.Data.SchoolContext _context;

        public EditModel(Efcorelearningrazorpages.Data.SchoolContext context)
        {
            _context = context;
        }

        public Instructor Instructor { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // get this methid

            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(x => x.Courses)
                .Include(x => x.OfficeAssignment)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (instructor == null)
            {
                return NotFound();
            }
            Poplulate(_context, instructor);
            Instructor = instructor;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedcourses)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // this will issue an update query!
            //_context.Attach(Instructor).State = EntityState.Modified;
            var instrectorToUpdate = _context.Instructors
                .Include(x => x.Courses)
                .Include(x => x.OfficeAssignment)
                .FirstOrDefault(x => x.ID == id);
            if (instrectorToUpdate == null)
                return NotFound();
            if (await TryUpdateModelAsync<Instructor>(instrectorToUpdate, "Instructor", i => i.FirstMidName, i => i.LastName,
                i => i.HireDate, i => i.OfficeAssignment))
            {
                if (String.IsNullOrWhiteSpace(instrectorToUpdate.OfficeAssignment?.Location))
                {
                    instrectorToUpdate.OfficeAssignment = null;
                }
                try
                {
                    UpdateInstrcutorCourses(selectedcourses, instrectorToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(Instructor.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Page();
        }
        private void UpdateInstrcutorCourses(string[] selectedcourses, Instructor instrcutortoupdate)
        {
            if (selectedcourses == null)
            {
                instrcutortoupdate.Courses = new List<Course>();
                return;
            }
            var selected_courses = new HashSet<string>(selectedcourses);
            var instructorcourses = new HashSet<int>(instrcutortoupdate.Courses.Select(x => x.CourseID));
            foreach (var i in _context.Courses)
            {
                if (selectedcourses.Contains(i.CourseID.ToString()))
                {
                    if (!instructorcourses.Contains(i.CourseID))
                    {
                        instrcutortoupdate.Courses.Add(i);
                        // ef core will issue a add command to the pjt
                    }
                }
                else
                {
                    if (instructorcourses.Contains(i.CourseID))
                    {
                        var element = instrcutortoupdate.Courses.FirstOrDefault(x => x.CourseID == i.CourseID);
                        instrcutortoupdate.Courses.Remove(element!);
                    }
                }
            }
        }
        private bool InstructorExists(int id)
        {
          return (_context.Instructors?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
