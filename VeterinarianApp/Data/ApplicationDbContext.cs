

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VeterinarianApp.Models;

namespace VeterinarianApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AdminUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }



        public DbSet<Veterinarian> Veterinarians { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<VeterinarianService> VeterinarianServices { get; set; }


    }
}
