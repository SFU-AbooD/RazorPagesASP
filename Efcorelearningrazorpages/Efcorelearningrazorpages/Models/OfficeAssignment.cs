using System.ComponentModel.DataAnnotations;

namespace Efcorelearningrazorpages.Models
{
    public class OfficeAssignment
    {
        /*
         * well!
         * simple lets talk about why adding a key sometimes will remobe database generated stuff
         * since adding key is used for defining merged cases where a primary key of table can be a primary key for another table 
         * they assume that they can`t put data from nothing since pk is fk so you need to insert this on your own since key will let
         * you define how relation will be added!
        */
        [Key]
        public int InstructorID { get; set; }
        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }
        public Instructor Instructor { get; set; }
    }
}
