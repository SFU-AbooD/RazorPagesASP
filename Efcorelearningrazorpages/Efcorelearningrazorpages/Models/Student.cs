using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Efcorelearningrazorpages.Models
{
   // [Table("Student!")]
    public class Student
    {
        // this is the domain model since it has all the props for the student entity!
        // a View model is something to prevent overposing since it will be used for ui showing data only!
        public int ID { get; set; }
        [Display(Name ="Last Name")]
        [StringLength(50)]
        [Required]
        public string lastname { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")] // you can use this for mapping things so Firstname in daatabase will be mapped to the pop below:
        [Display(Name = "First Name")]
        public string firstmidname { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }
        // public string secret { get; set; }
        public string secret { get; set; } = String.Empty;
        public string FullName
        {
            get
            {
                return lastname + ", " +firstmidname;
            }
        }
        // sinse fullname does not have setters it will not be added
        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}
