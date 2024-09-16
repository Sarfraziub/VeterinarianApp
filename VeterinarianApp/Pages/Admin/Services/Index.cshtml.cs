using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VeterinarianApp.Data;
using VeterinarianApp.Models;

namespace VeterinarianApp.Pages.Admin.Services
{
    [Authorize(Roles = "Admin")]

    public class IndexModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Service> Services { get; set; }

        public async Task OnGetAsync()
        {
            Services = await _context.Services.ToListAsync();
        }
    }
}
