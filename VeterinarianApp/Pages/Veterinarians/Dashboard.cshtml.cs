using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using VeterinarianApp.Models;

namespace VeterinarianApp.Pages.Veterinarians
{
    [Authorize(Roles = "Veterinarian")]
    public class DashboardModel : PageModel
    {
        public int VeterinarianId { get; set; }

        public void OnGet()
        {

            // Get the user ID from claims
            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    VeterinarianId = int.Parse(userIdClaim.Value); // Convert to int
                }
            }

        }
    }
}
