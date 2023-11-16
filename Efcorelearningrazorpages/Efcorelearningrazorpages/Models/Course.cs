using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Efcorelearningrazorpages.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        public int CourseID { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        
        public int Credits { get; set; }
        public int Credits2 { get; set; }
        // this is how ef core treats fks
        // now the fk below is not required btw!!!
        // this is a fk that 
        public int DepartmentID { get; set; }
        public Department? Department { get; set; }
        public ICollection<Instructor>? Instructors { get; set; }
        public HashSet<Enrollment>? Enrollments { get; set; }
    }
}
