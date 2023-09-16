using System.ComponentModel.DataAnnotations.Schema;

namespace Alumni.Models
{
    public class Events : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }


        [ForeignKey(nameof(ApplicationUser))]
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string? EventLocation { get; set; }
        public DateTime? Date { get; set; }
        public decimal? TicketPrice { get; set; }
    }
}
