using System.Security.Claims;

namespace Alumni.Models
{
    public class ScopedAuthentication : IScopedAuthentication
    {
        public ScopedAuthentication()
        {

        }
        public System.Security.Principal.IIdentity Identity { get; set; }
        public int? GetUserId()
        {

            if (Identity is ClaimsIdentity claimsIdentity)
            {
                // Get the user ID
                string? userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (Int32.TryParse(userId, out int result))
                {
                    return result;
                }
                else return null;
            }

            else return null;
        }

        public string[] GetUserRoles()
        {

            if (Identity is ClaimsIdentity claimsIdentity)
            {
                var roles = claimsIdentity.FindAll(ClaimTypes.Role);
                return roles.Select(r => r.Value).ToArray();
            }

            else return Array.Empty<string>();
        }

        public bool IsFacultyRepresentative()
        {

            var roles = GetUserRoles();
            return roles.Any(u => u.ToLower() == "Faculty Representative".ToLower());
        }

        public bool IsAlumni()
        {

            var roles = GetUserRoles();
            return roles.Any(u => u.ToLower() == "Alumni".ToLower());
        }
    }
}
