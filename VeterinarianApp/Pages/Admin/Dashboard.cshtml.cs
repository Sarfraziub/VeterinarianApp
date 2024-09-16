using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VeterinarianApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
