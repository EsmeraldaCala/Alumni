using System.ComponentModel.DataAnnotations.Schema;

namespace Alumni.Models
{
    public class Admin : BaseEntity
    {
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey(nameof(ApplicationUser))]
        public int UserId { get; set; }
    }
}