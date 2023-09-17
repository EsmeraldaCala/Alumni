using System.Security.Claims;
using System.Security.Principal;

namespace Alumni.Models
{
    public interface IScopedAuthentication
    {
        IIdentity? Identity { get; set; }
        public int? GetUserId();
        public string[] GetUserRoles();
        public bool IsAlumni();
        public bool IsFacultyRepresentative();
    }

}