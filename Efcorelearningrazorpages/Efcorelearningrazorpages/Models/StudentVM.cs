using System.ComponentModel.DataAnnotations.Schema;

namespace Efcorelearningrazorpages.Models
{
    public class StudenVM
    {
        // this is the domain model since it has all the props for the student entity!
        // a View model is something to prevent overposing since it will be used for ui showing data only!
        public int ID { get; set; }
        public string lastname { get; set; }
        public string firstmidname { get; set; }
        public DateTime EnrollmentDate { get; set; }
        // public string secret { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}
