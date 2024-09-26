using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VeterinarianApp.Data;
using VeterinarianApp.Models;

namespace VeterinarianApp.Pages.Veterinarians
{
    [Authorize(Roles = "Veterinarian")]
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Veterinarian> _passwordHasher;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProfileModel(ApplicationDbContext context, IPasswordHasher<Veterinarian> passwordHasher, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _webHostEnvironment = webHostEnvironment;
        }



        [BindProperty]
        public Veterinarian Veterinarian { get; set; }

        [BindProperty]
        public Clinic Clinic { get; set; }

        [BindProperty]
        public List<int> SelectedServiceIds { get; set; }

        public IList<Service> Services { get; set; }

        [BindProperty]
        public Dictionary<int, SurveyResponse> SurveyResponses { get; set; }

        public IList<SurveryQuestion> SurveyQuestions { get; set; }
        public IList<SurveyOption> SurveyOptions { get; set; }

        [BindProperty]
        public IFormFile? ProfilePhoto { get; set; } // To handle the file upload 



        public async Task<IActionResult> OnGetAsync(int id)
        {
            Services = await _context.Services.ToListAsync();
            // Retrieve survey questions and options
            SurveyQuestions = await _context.SurveryQuestions.ToListAsync();
            SurveyOptions = await _context.SurveyOptions.ToListAsync();

            Veterinarian = await _context.Veterinarians
                .Include(v => v.Clinic)
                .Include(v => v.VeterinarianServices)
                .ThenInclude(vs => vs.Service)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (Veterinarian == null)
            {
                return NotFound();
            }
            if (Veterinarian.ProfilePhoto != null)
            {
                var profilePhotoPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Veterinarian", Veterinarian.Id.ToString(), "Profilepicture.png");
                if (System.IO.File.Exists(profilePhotoPath))
                {
                    Veterinarian.ProfilePhoto += $"{Veterinarian.Id}/Profilepicture.png";
                }
                else
                {
                    Veterinarian.ProfilePhoto += "/user-placeholder.png";
                }
            }
            Clinic = Veterinarian.Clinic;
            SelectedServiceIds = Veterinarian.VeterinarianServices.Select(vs => vs.ServiceId).ToList();


            var responses = await _context.SurveyResponse
                .Where(sr => sr.VeterinarianId == id)
                .ToListAsync();

            SurveyResponses = SurveyQuestions.ToDictionary(
                q => q.SurveyQuestionId,
                q => responses.FirstOrDefault(r => r.SurveyQuestionId == q.SurveyQuestionId) ?? new SurveyResponse
                {
                    SurveyQuestionId = q.SurveyQuestionId,
                    VeterinarianId = Veterinarian.Id
                });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                Services = await _context.Services.ToListAsync();
                SurveyQuestions = await _context.SurveryQuestions.ToListAsync();
                SurveyOptions = await _context.SurveyOptions.ToListAsync();

                SurveyResponses = _context.SurveryQuestions.ToDictionary(
                    q => q.SurveyQuestionId,
                    q => new SurveyResponse { SurveyQuestionId = q.SurveyQuestionId }
                );
                return Page();
            }

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
            veterinarianToUpdate.Phone = Veterinarian.Phone;
            veterinarianToUpdate.LicenseNo = Veterinarian.LicenseNo;
            veterinarianToUpdate.X = Veterinarian.X;
            veterinarianToUpdate.Youtube = Veterinarian.Youtube;
            veterinarianToUpdate.FaceBook = Veterinarian.FaceBook;
            veterinarianToUpdate.TIkTok = Veterinarian.TIkTok;
            veterinarianToUpdate.Instagram = Veterinarian.Instagram;
            veterinarianToUpdate.ApprovedBy = Veterinarian.ApprovedBy;
            veterinarianToUpdate.VetHuntScore = Veterinarian.VetHuntScore;
            veterinarianToUpdate.ProfilePhoto = Veterinarian.ProfilePhoto.IsNullOrEmpty() ? veterinarianToUpdate.ProfilePhoto : Veterinarian.ProfilePhoto;
            if (!string.IsNullOrWhiteSpace(Veterinarian.Password))
                veterinarianToUpdate.Password = _passwordHasher.HashPassword(Veterinarian, Veterinarian.Password);

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

            var existingResponses = await _context.SurveyResponse
                .Where(sr => sr.VeterinarianId == Veterinarian.Id)
                .ToListAsync();

            _context.SurveyResponse.RemoveRange(existingResponses);
            await _context.SaveChangesAsync();


            // Add new responses
            foreach (var kvp in SurveyResponses)
            {
                var questionId = kvp.Key;
                var response = kvp.Value;

                if (!string.IsNullOrEmpty(response.ResponseText))
                {
                    response.VeterinarianId = Veterinarian.Id;
                    response.SurveyQuestionId = questionId;
                    _context.SurveyResponse.Add(response);
                }
            }
            await _context.SaveChangesAsync();
            //Add update profile photo
            if (ProfilePhoto != null)
            {
                var fileName = "Profilepicture.png";
                var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Veterinarian", Veterinarian.Id.ToString());

                // Ensure the directory exists, if not, create it
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, fileName);

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfilePhoto.CopyToAsync(stream);
                }
                Veterinarian.ProfilePhoto = $"/assets/Veterinarian/";
                var vetCurrent = _context.Veterinarians.Where(x => x.Id == Veterinarian.Id).FirstOrDefault();
                if (vetCurrent != null)
                {
                    vetCurrent.ProfilePhoto = Veterinarian.ProfilePhoto;
                    _context.SaveChanges();
                }
            }
            return RedirectToPage("/Veterinarians/DashBoard");
        }
    }
}
