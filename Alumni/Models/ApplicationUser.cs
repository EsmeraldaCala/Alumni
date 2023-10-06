using Microsoft.AspNetCore.Identity;

namespace Alumni.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? ProfilePicture { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Role { get; set; }
        public Alumni? Alumni { get; set; }
        public FacultyRepresentative? FacultyRepresentative { get; set; }
        public Admin? Admin { get; set; }

    }
}
