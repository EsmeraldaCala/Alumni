using System.ComponentModel.DataAnnotations.Schema;

namespace Alumni.Models
{
    public class Alumni : BaseEntity
    {
        public Alumni()
        {

        }

        [ForeignKey(nameof(ApplicationUser))]
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string FieldOfStudy { get; set; }
        public string JobPosition { get; set; }
        public DateTime GraduationDate { get; set; }
        public string Company { get; set; }
    }
}
