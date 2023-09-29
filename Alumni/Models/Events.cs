using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alumni.Models
{
    public class Events : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        public string? Photo {  get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public int UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }

        [Display(Name = "Event Location")]
        public string? EventLocation { get; set; }
        public DateTime? Date { get; set; }

        [Display(Name = "Ticket Price")]
        public decimal? TicketPrice { get; set; }
    }
}
