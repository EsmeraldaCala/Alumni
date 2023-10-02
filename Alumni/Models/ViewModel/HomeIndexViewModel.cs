
namespace Alumni.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Alumni> AlumniList { get; set; }
        public IEnumerable<Events> EventList { get; set; }
    }
}
