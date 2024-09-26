using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VeterinarianApp.Data;
using VeterinarianApp.Helpers;
using VeterinarianApp.Models;
using VeterinarianApp.ViewModels;

namespace VeterinarianApp.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageVeterinariansModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly SmtpSettings _smtpSettings;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ManageVeterinariansModel(ApplicationDbContext context, IOptions<SmtpSettings> smtpSettings, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _smtpSettings = smtpSettings.Value;
            _webHostEnvironment = webHostEnvironment;
        }

        public IList<Veterinarian> Veterinarians { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        private const int PageSize = 1;


        public async Task OnGetAsync([FromQuery] int page = 1)
        {
            CurrentPage = page;
            // Get the total count of veterinarians
            var totalCount = await _context.Veterinarians.CountAsync();
            TotalPages = (int)System.Math.Ceiling(totalCount / (double)PageSize);

            // Get the paginated list of veterinarians
            Veterinarians = await _context.Veterinarians
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            //Set the profile photo path
            foreach (var item in Veterinarians)
            {
                if (item.ProfilePhoto != null)
                {
                    var profilePhotoPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "Veterinarian", item.Id.ToString(), "Profilepicture.png");
                    if (System.IO.File.Exists(profilePhotoPath))
                    {
                        item.ProfilePhoto += $"{item.Id}/Profilepicture.png";
                    }
                    else
                    {
                        item.ProfilePhoto += "/user-placeholder.png";
                    }
                }
            }
        }



        [HttpPost]
        public async Task<IActionResult> OnPostSendEmail([FromBody] EmailRequest emailRequest)
        {
            if (string.IsNullOrEmpty(emailRequest.Email))
            {
                return BadRequest("Email is required.");
            }


            var existingRecord = await _context.Veterinarians.FirstOrDefaultAsync(f => f.Email == emailRequest.Email);
            if (existingRecord != null)
                return BadRequest("Veterinarian with this email already exist in the system.");


            var resetLink = Url.Page("/Account/Signup", pageHandler: null, values: new { email = emailRequest.Email, token = Guid.NewGuid().ToString() }, protocol: Request.Scheme);


            //Prepare email object
            SMTPEmailObject emailObject = new SMTPEmailObject
            {
                Port = int.TryParse(_smtpSettings.Port, out int port) && port > 0 ? port : 587,
                Server = _smtpSettings.Server,
                From = _smtpSettings.Username,
                Username = _smtpSettings.Username,
                Password = _smtpSettings.Password,

                To = emailRequest.Email,
                Subject = "Invitation from Vet Hunt",
                Body = $"Hi,<br />Please click on the link below to sign up.<br /><a href='{resetLink}'>Sign Up</a>"
            };

            var response = await SMTP.SendEmailAsync(emailObject);

            if (response)
            {
                return new JsonResult(new { message = "Invitation email sent successfully." })
                {
                    StatusCode = 200
                };
            }
            else
                return StatusCode(500, $"Error sending email");
        }

    }
}
