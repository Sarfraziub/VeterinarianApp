using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using VeterinarianApp.Models;

namespace VeterinarianApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserManager<AdminUser> _userManager;
        private readonly SignInManager<AdminUser> _signInManager;

        public LoginModel(SignInManager<AdminUser> signInManager, UserManager<AdminUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, Input.Email),
                    new Claim(ClaimTypes.Role, "Admin") // Ensure the role is added here
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToPage("/Admin/Dashboard");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
