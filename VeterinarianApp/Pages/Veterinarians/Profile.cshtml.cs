using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VeterinarianApp.Data;
using VeterinarianApp.Models;

namespace VeterinarianApp.Pages.Veterinarians
{
    [Authorize(Roles = "Veterinarian")]
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Veterinarian> _passwordHasher;
        public ProfileModel(ApplicationDbContext context, IPasswordHasher<Veterinarian> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [BindProperty]
        public Veterinarian Veterinarian { get; set; }

        [BindProperty]
        public IFormFile? ProfilePhoto { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Veterinarian = await _context.Veterinarians.FindAsync(id);
            if (Veterinarian == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var veterinarianInDb = await _context.Veterinarians.FindAsync(Veterinarian.Id);
            if (veterinarianInDb == null)
            {
                return NotFound();
            }

            veterinarianInDb.FirstName = Veterinarian.FirstName;
            veterinarianInDb.LastName = Veterinarian.LastName;
            veterinarianInDb.Email = Veterinarian.Email;
            veterinarianInDb.Phone = Veterinarian.Phone;
            veterinarianInDb.Instagram = Veterinarian.Instagram;
            veterinarianInDb.Youtube = Veterinarian.Youtube;
            veterinarianInDb.X = Veterinarian.X;
            veterinarianInDb.FaceBook = Veterinarian.FaceBook;
            veterinarianInDb.TIkTok = Veterinarian.TIkTok;
            veterinarianInDb.LicenseNo = Veterinarian.LicenseNo;
            if (!string.IsNullOrWhiteSpace(Veterinarian.Password))
                veterinarianInDb.Password = _passwordHasher.HashPassword(Veterinarian, Veterinarian.Password);

            // Handle Profile Photo upload
            if (ProfilePhoto != null && ProfilePhoto.Length > 0)
            {
                // Assuming you save the file in a specific path
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ProfilePhoto", ProfilePhoto.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ProfilePhoto.CopyToAsync(stream);
                }
                veterinarianInDb.ProfilePhoto = ProfilePhoto.FileName; // Save the file name in the database
            }

            veterinarianInDb.UpdatedAt = DateTime.Now; // Update the timestamp

            _context.Attach(veterinarianInDb).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Veterinarians/DashBoard");
        }
    }
}
