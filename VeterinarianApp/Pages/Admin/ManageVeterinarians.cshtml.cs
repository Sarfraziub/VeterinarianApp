using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VeterinarianApp.Data;
using VeterinarianApp.Models;

namespace VeterinarianApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageVeterinariansModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageVeterinariansModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Veterinarian> Veterinarians { get; set; }

        public async Task OnGetAsync()
        {
            Veterinarians = await _context.Veterinarians.ToListAsync();
        }
    }
}
