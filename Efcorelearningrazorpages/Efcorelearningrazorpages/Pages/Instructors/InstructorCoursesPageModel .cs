using Efcorelearningrazorpages.Data;
using Efcorelearningrazorpages.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Efcorelearningrazorpages.Pages.Instructors
{
    public class InstructorCoursesPageModel : PageModel
    {
        public List<AssignedCourseData> AssignedandNonCoursedata { get; set; }

        public async Task Poplulate(SchoolContext context,Instructor? instrcutor)
        {
            instrcutor.Courses = instrcutor.Courses ?? new List<Course>();
            AssignedandNonCoursedata = new();
            HashSet<int> ins_courses = new();
            List<Course> allcourses = await context.Courses.ToListAsync();
            ins_courses = new(instrcutor.Courses.Select(x => x.CourseID));
            foreach (var course in allcourses)
            {
                AssignedandNonCoursedata.Add(new()
                {
                    Assigned = ins_courses.Contains(course.CourseID),
                    Title = course.Title,
                    CourseID = course.CourseID
                });
            }
        }
    }
}
