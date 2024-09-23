using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VeterinarianApp.Data;
using VeterinarianApp.Models;

namespace VeterinarianApp.Pages.Veterinarians
{
    [Authorize(Roles = "Veterinarian")]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Veterinarian> _passwordHasher;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Veterinarian Veterinarian { get; set; }

        public async Task<IActionResult> OnGet()
        {

            // Get the user ID from claims
            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    var VeterinarianId = int.Parse(userIdClaim.Value);
                    Veterinarian = await _context.Veterinarians.FirstOrDefaultAsync(f => f.Id == VeterinarianId);
                    if (Veterinarian.ProfilePhoto != null)
                    {
                        Veterinarian.ProfilePhoto += $"{Veterinarian.Id}/Profilepicture.png";
                    }
                }
            }

            return Page();

        }
    }
}
