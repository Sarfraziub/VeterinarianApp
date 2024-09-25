using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VeterinarianApp.Data;
using VeterinarianApp.Models;

namespace VeterinarianApp.Pages.Account
{
    public class SignupModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Veterinarian> _passwordHasher;
        public SignupModel(ApplicationDbContext context, IPasswordHasher<Veterinarian> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [BindProperty]
        public Veterinarian Veterinarian { get; set; }

        [BindProperty]
        public Clinic Clinic { get; set; }

        [BindProperty]
        public List<int> SelectedServiceIds { get; set; }

        public IList<Service> Services { get; set; }

        [BindProperty]
        public Dictionary<int, SurveyResponse>? SurveyResponses { get; set; }

        public IList<SurveryQuestion> SurveyQuestions { get; set; }
        public IList<SurveyOption> SurveyOptions { get; set; }

        [BindProperty]
        public IFormFile? ProfilePhoto { get; set; } // To handle the file upload 


        public async Task<IActionResult> OnGet(string email, string token)
        {
            Services = await _context.Services.ToListAsync();
            // Retrieve survey questions and options
            SurveyQuestions = await _context.SurveryQuestions.ToListAsync();
            SurveyOptions = await _context.SurveyOptions.ToListAsync();

            Veterinarian = new Veterinarian();
            Veterinarian.Email = email;

            Clinic = new Clinic();
            SelectedServiceIds = new List<int>();

            SurveyResponses = SurveyQuestions.ToDictionary(
                q => q.SurveyQuestionId,
                q => new SurveyResponse { SurveyQuestionId = q.SurveyQuestionId }
            );

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

            //Add update profile photo
            if (ProfilePhoto != null)
            {
                // Generate a unique file name
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfilePhoto.FileName);
                // Path where the file will be stored
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ProfilePhotos", fileName);
                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfilePhoto.CopyToAsync(stream);
                }

                // Update Veterinarian's Picture property with the path
                Veterinarian.ProfilePhoto = $"/images/ProfilePhotos/{fileName}";
            }



            //Hashed password
            Veterinarian.Password = _passwordHasher.HashPassword(Veterinarian, Veterinarian.Password);

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
            return RedirectToPage("/VeterinarianLogin");
        }

    }

}
