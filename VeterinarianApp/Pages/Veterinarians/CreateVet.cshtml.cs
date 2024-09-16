using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VeterinarianApp.Data;
using VeterinarianApp.Models;

namespace VeterinarianApp.Pages.Veterinarians
{
    public class CreateVetModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Veterinarian> _passwordHasher;

        public CreateVetModel(ApplicationDbContext context, IPasswordHasher<Veterinarian> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
         
        [BindProperty]
        public Veterinarian Veterinarian { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Veterinarian.Password = _passwordHasher.HashPassword(Veterinarian, Veterinarian.Password);
            Veterinarian.CreatedAt = DateTime.UtcNow;
            Veterinarian.UpdatedAt = DateTime.UtcNow;

            _context.Veterinarians.Add(Veterinarian);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Veterinarians/Dashboard");

        }
    }
}
