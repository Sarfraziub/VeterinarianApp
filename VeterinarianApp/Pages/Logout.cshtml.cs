using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VeterinarianApp.Models;

namespace VeterinarianApp.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<AdminUser> _signInManager;

        public LogoutModel(SignInManager<AdminUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (User.IsInRole("Admin"))
                return RedirectToPage("/AdminLogin");
            else if (User.IsInRole("Veterinarian"))
                return RedirectToPage("/VeterinarianLogin");

            else
                return RedirectToPage("/Index");

        }



    }
}
