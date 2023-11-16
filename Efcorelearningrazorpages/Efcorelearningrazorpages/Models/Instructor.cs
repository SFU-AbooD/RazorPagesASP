using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Efcorelearningrazorpages.Models
{
    public class Instructor
    {
        // creating the primary key via <ID> OR <classnameID>
        public int ID { get; set; }
        [Required]
        [Display(Name ="Last Name")]
        [StringLength(maximumLength:50)]
        public string LastName { get; set; }
        [Column("FirstName")]
        [Display(Name = "First Name")]
        [Required]
        [StringLength(50)]
        public string FirstMidName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-mm-dd}", ApplyFormatInEditMode =true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }
        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }
        }
        public ICollection<Course>? Courses { get; set; }  
        public OfficeAssignment? OfficeAssignment { get; set; }  
    }
}
