using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VeterinarianApp.Pages.Veterinarians
{
    [Authorize(Roles = "Veterinarian")]
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
