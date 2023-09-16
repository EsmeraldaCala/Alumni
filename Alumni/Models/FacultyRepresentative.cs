using System.ComponentModel.DataAnnotations.Schema;

namespace Alumni.Models
{
    public class FacultyRepresentative : BaseEntity
    {
        public string Faculty { get; set; }
        public string FieldOfStudy { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
