using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VeterinarianApp.Pages
{
    public class IndexModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            // Check if the user is already authenticated
            if (User.Identity.IsAuthenticated)
            {
                // Check if the user is in a specific role
                if (User.IsInRole("Admin")) 
                {
                    return RedirectToPage("/Admin/Dashboard"); 
                }
                else if (User.IsInRole("Veterinarian"))
                {
                    return RedirectToPage("/Veterinarians/Dashboard");
                }
                // Add additional roles as needed

                // Default redirect if no specific role matched
                return RedirectToPage("/Index"); // Default page if user is authenticated but not in specific roles
            }

            return Page(); 
        }
    }
}
