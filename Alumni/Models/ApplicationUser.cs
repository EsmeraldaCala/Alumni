using Microsoft.AspNetCore.Identity;

namespace Alumni.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
    }

    public class Alumni
    {
        protected Alumni()
        {

        }
        public int UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string FieldOfStudy { get; set; }
        public string JobPosition { get; set; }
        public DateTime GraduationDate { get; set; }
        public string Company { get; set; }
    }
}
