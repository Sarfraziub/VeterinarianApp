using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VeterinarianApp.Data;
using VeterinarianApp.Models;

namespace VeterinarianApp.Pages.Veterinarians
{
    public class ResetPasswordModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ResetPasswordModel> _logger;
        private readonly IPasswordHasher<Veterinarian> _passwordHasher;


        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public ResetPasswordModel(IPasswordHasher<Veterinarian> passwordHasher, ApplicationDbContext context, ILogger<ResetPasswordModel> logger)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public void OnGet(string email, string token)
        {
            Email = email;
        }

        public async Task<IActionResult> OnPostAsync(string email, string token)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Find the veterinarian by email
            var veterinarian = await _context.Veterinarians.FirstOrDefaultAsync(v => v.Email == Email);
            if (veterinarian == null)
            {
                TempData["ErrorMessage"] = "Error resetting password.";
                return Page();
            }

            // Check if the passwords match
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords and confirmed password do not match.");
                return Page();
            }

            // Reset the password (update it directly)
            veterinarian.Password = _passwordHasher.HashPassword(new Veterinarian(), Password);

            _context.Veterinarians.Update(veterinarian);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your password has been reset successfully.";
            return RedirectToPage("/VeterinarianLogin"); // Redirect to login or another page
        }
    }
}