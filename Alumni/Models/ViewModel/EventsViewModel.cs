using Alumni.Migrations;

namespace Alumni.Models.ViewModel
{
    public class EventsViewModel : Events
    {

        public IFormFile? Picture { get; set; }
      
    }
}
