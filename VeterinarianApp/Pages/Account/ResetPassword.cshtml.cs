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

        private readonly UserManager<AdminUser> _userManager;


        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public bool IsAdmin { get; set; }

        public ResetPasswordModel(IPasswordHasher<Veterinarian> passwordHasher, ApplicationDbContext context, ILogger<ResetPasswordModel> logger, UserManager<AdminUser> userManager)
        {
            _context = context;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGet(string email, string token, bool isAdmin)
        {
            if (isAdmin)
            {
                var adminUser = await _userManager.FindByEmailAsync(email);
                if (adminUser == null)
                {
                    TempData["ErrorMessage"] = "Invalid Email";
                    return Page();

                }
                else if (adminUser.Token != token.ToLower())
                {
                    TempData["ErrorMessage"] = "Invalid Token";
                    return Page();

                }

                else if (adminUser.TokenExpiry.Value.AddMinutes(5) <= DateTime.Now)
                {
                    TempData["ErrorMessage"] = "Token ha been expired";
                    return Page();
                }


            }
            else
            {
                var vetUser = await _context.Veterinarians.FirstOrDefaultAsync(f => f.Email == email);
                if (vetUser == null)
                {
                    TempData["ErrorMessage"] = "Invalid Email";
                    return Page();

                }
                else if (vetUser.Token != token.ToLower())
                {
                    TempData["ErrorMessage"] = "Invalid Token";
                    return Page();

                }

                else if (vetUser.TokenExpiry.Value.AddMinutes(5) <= DateTime.Now)
                {
                    TempData["ErrorMessage"] = "Token ha been expired";
                    return Page();

                }
            }



            Email = email;
            IsAdmin = isAdmin;
            TempData["ErrorMessage"] = null;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string email, string token)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if the passwords match
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords and confirmed password do not match.");
                return Page();
            }


            if (IsAdmin)
            {
                var adminUser = await _userManager.FindByEmailAsync(Email);
                if (adminUser == null)
                {
                    TempData["ErrorMessage"] = "Error resetting password.";
                    return Page();
                }

                var newToken = await _userManager.GeneratePasswordResetTokenAsync(adminUser);

                var resetPasswordResult = await _userManager.ResetPasswordAsync(adminUser, newToken, Password);

            }
            else
            {
                // Find the veterinarian by email
                var veterinarian = await _context.Veterinarians.FirstOrDefaultAsync(v => v.Email == Email);
                if (veterinarian == null)
                {
                    TempData["ErrorMessage"] = "Error resetting password.";
                    return Page();
                }


                // Reset the password (update it directly)
                veterinarian.Password = _passwordHasher.HashPassword(new Veterinarian(), Password);

                _context.Veterinarians.Update(veterinarian);
                await _context.SaveChangesAsync();
            }




            //TempData["SuccessMessage"] = "Your password has been reset successfully.";

            if (IsAdmin)
                return RedirectToPage("/AdminLogin");


            return RedirectToPage("/VeterinarianLogin");

        }
    }
}