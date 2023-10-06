using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alumni.Models
{
    public class JobOpportunity : BaseEntity
    {
        public string Title { get; set; }
        public string? Summary { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public int UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        public string? Details { get; set; }
        public string? Company { get; set; }
        public DateTime? Deadline { get; set; }
        public int Experience { get; set; }
        public int Salary { get; set; }
        public string? Email { get; set; }
        public decimal? TicketPrice { get; set; }
    }
}
