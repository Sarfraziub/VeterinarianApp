using Microsoft.AspNetCore.Identity;

namespace VeterinarianApp.Models
{
    public class AdminUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenExpiry { get; set; }
    }
}
