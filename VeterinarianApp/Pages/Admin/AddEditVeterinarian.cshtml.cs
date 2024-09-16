using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using VeterinarianApp.Data;
using VeterinarianApp.Models;

namespace VeterinarianApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AddEditVeterinarianModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public AddEditVeterinarianModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Veterinarian Veterinarian { get; set; }

        [BindProperty]
        public Clinic Clinic { get; set; }

        [BindProperty]
        public List<int> SelectedServiceIds { get; set; }

        public IList<Service> Services { get; set; }
        public bool IsEditMode { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Services = await _context.Services.ToListAsync();

            if (id.HasValue)
            {
                Veterinarian = await _context.Veterinarians
                    .Include(v => v.Clinic)
                    .Include(v => v.VeterinarianServices)
                    .ThenInclude(vs => vs.Service)
                    .FirstOrDefaultAsync(v => v.Id == id.Value);

                if (Veterinarian == null)
                {
                    return NotFound();
                }

                Clinic = Veterinarian.Clinic;
                SelectedServiceIds = Veterinarian.VeterinarianServices.Select(vs => vs.ServiceId).ToList();
                IsEditMode = true;
            }
            else
            {
                Veterinarian = new Veterinarian();
                Clinic = new Clinic();
                SelectedServiceIds = new List<int>();
                IsEditMode = false;
            }

            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Services = await _context.Services.ToListAsync();
                return Page();
            }

            if (Veterinarian.Id > 0) // Edit mode
            {
                var veterinarianToUpdate = await _context.Veterinarians
                    .Include(v => v.Clinic)
                    .Include(v => v.VeterinarianServices)
                    .FirstOrDefaultAsync(v => v.Id == Veterinarian.Id);

                if (veterinarianToUpdate == null)
                {
                    return NotFound();
                }

                // Update veterinarian
                veterinarianToUpdate.FirstName = Veterinarian.FirstName;
                veterinarianToUpdate.LastName = Veterinarian.LastName;
                veterinarianToUpdate.Email = Veterinarian.Email;
                veterinarianToUpdate.Password = Veterinarian.Password;
                veterinarianToUpdate.Phone = Veterinarian.Phone;
                veterinarianToUpdate.LicenseNo = Veterinarian.LicenseNo;
                veterinarianToUpdate.X = Veterinarian.X;
                veterinarianToUpdate.Youtube = Veterinarian.Youtube;
                veterinarianToUpdate.FaceBook = Veterinarian.FaceBook;
                veterinarianToUpdate.TIkTok = Veterinarian.TIkTok;
                veterinarianToUpdate.Instagram = Veterinarian.Instagram;
                veterinarianToUpdate.ApprovedBy = Veterinarian.ApprovedBy;
                veterinarianToUpdate.HuntVetScore = Veterinarian.HuntVetScore;


                // Update clinic
                if (veterinarianToUpdate.Clinic != null)
                {
                    veterinarianToUpdate.Clinic.ClinicName = Clinic.ClinicName;
                    veterinarianToUpdate.Clinic.Country = Clinic.Country;
                    veterinarianToUpdate.Clinic.AddressLine1 = Clinic.AddressLine1;
                    veterinarianToUpdate.Clinic.AddressLine2 = Clinic.AddressLine2;
                    veterinarianToUpdate.Clinic.City = Clinic.City;
                    veterinarianToUpdate.Clinic.State = Clinic.State;
                    veterinarianToUpdate.Clinic.ZipCode = Clinic.ZipCode;
                    veterinarianToUpdate.Clinic.ClinicPhone = Clinic.ClinicPhone;
                    veterinarianToUpdate.Clinic.ClinicEmail = Clinic.ClinicEmail;
                    veterinarianToUpdate.Clinic.ClinicWebsite = Clinic.ClinicWebsite;
                }
                else
                {
                    // Handle case where clinic does not exist
                    // (Usually, this should not happen in edit mode if data is consistent)
                }

                // Remove existing services
                _context.VeterinarianServices.RemoveRange(veterinarianToUpdate.VeterinarianServices);

                // Add new services
                foreach (var serviceId in SelectedServiceIds)
                {
                    var vetService = new VeterinarianService
                    {
                        VeterinarianId = Veterinarian.Id,
                        ServiceId = serviceId
                    };
                    _context.VeterinarianServices.Add(vetService);
                }
            }
            else // Add mode
            {
                _context.Veterinarians.Add(Veterinarian);
                await _context.SaveChangesAsync();

                // Add clinic with foreign key
                Clinic.VeterinarianId = Veterinarian.Id;
                _context.Clinics.Add(Clinic);
                await _context.SaveChangesAsync();

                // Add veterinarian services
                foreach (var serviceId in SelectedServiceIds)
                {
                    var vetService = new VeterinarianService
                    {
                        VeterinarianId = Veterinarian.Id,
                        ServiceId = serviceId
                    };
                    _context.VeterinarianServices.Add(vetService);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/Admin/ManageVeterinarians"); // Redirect to a list or another page
        }








    }



}
