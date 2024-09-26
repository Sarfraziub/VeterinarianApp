using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VeterinarianApp.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {
        public string UserRole { get; set; }
        public void OnGet()
        {
            // Check user roles and set the UserRole property
            if (User.Identity.IsAuthenticated)
            {
                // You can check for multiple roles if needed
                if (User.IsInRole("Admin"))
                {
                    UserRole = "Admin";
                }
                else if (User.IsInRole("Veterinarian"))
                {
                    UserRole = "Veterinarian";
                }
                // Add more roles as necessary
            }
        }
    }
}
