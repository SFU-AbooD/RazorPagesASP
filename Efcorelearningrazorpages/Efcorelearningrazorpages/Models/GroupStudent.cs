using System.ComponentModel.DataAnnotations;

namespace Efcorelearningrazorpages.Models
{
    public class GroupStudent
    {
        [DataType(DataType.Date)]
        public DateTime? EnrollmentDate { get; set; }
        public int StudentCount { get; set; }  

    }
}
