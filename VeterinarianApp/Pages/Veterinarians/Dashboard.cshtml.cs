using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DashboardModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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
                    var profilePhotoPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Veterinarian", Veterinarian.Id.ToString(), "Profilepicture.png");
                    if (System.IO.File.Exists(profilePhotoPath))
                    {
                        Veterinarian.ProfilePhoto = Path.Combine("/assets", "Veterinarian", Veterinarian.Id.ToString(), "Profilepicture.png");
                    }
                    else
                    {
                        Veterinarian.ProfilePhoto = Path.Combine("/assets", "Veterinarian", "user-placeholder.png");
                    }
                }
            }

            return Page();

        }
    }
}
