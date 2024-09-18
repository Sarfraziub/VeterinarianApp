using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using VeterinarianApp.Data;
using VeterinarianApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace VeterinarianApp.Pages
{
    public class VeterinarianLoginModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Veterinarian> _passwordHasher;

        public VeterinarianLoginModel(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Veterinarian>();
        }

        [BindProperty]
        public VeterinarianLoginInputModel InputModel { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Clear all existing errors from ModelState to focus only on login errors
            ModelState.Clear();

            if (string.IsNullOrEmpty(InputModel.Email) || string.IsNullOrEmpty(InputModel.Password))
            {
                ModelState.AddModelError(string.Empty, "Email and Password are required.");
                return Page();
            }



            var vet = await _context.Veterinarians
                .SingleOrDefaultAsync(v => v.Email == InputModel.Email);

            if (vet == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            var result = _passwordHasher.VerifyHashedPassword(vet, vet.Password, InputModel.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, InputModel.Email),
                new Claim(ClaimTypes.NameIdentifier, vet.Id.ToString()),
                new Claim(ClaimTypes.Role, "Veterinarian")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToPage("/Veterinarians/Dashboard");
        }



        public class VeterinarianLoginInputModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

    }

}
