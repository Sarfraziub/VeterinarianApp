using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VeterinarianApp.Data;
using VeterinarianApp.Helpers;
using VeterinarianApp.Models;
using VeterinarianApp.ViewModels;

namespace VeterinarianApp.Pages.Veterinarians
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly ILogger<ForgotPasswordModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SmtpSettings _smtpSettings;

        private readonly UserManager<AdminUser> _userManager;

        public ForgotPasswordModel(ApplicationDbContext context, IOptions<SmtpSettings> smtpSettings, UserManager<AdminUser> userManager)
        {
            _context = context;
            _smtpSettings = smtpSettings.Value;
            _userManager = userManager;
        }


        public async Task<IActionResult> OnGet(bool isAdmin = false)
        {
            IsAdmin = isAdmin;
            return Page();
        }


        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public bool IsAdmin { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            var resetLink = "";

            if (IsAdmin)
            {
                var adminUser = await _userManager.FindByEmailAsync(Email);
                if (adminUser == null)
                {
                    TempData["ErrorMessage"] = "This email is not registered";
                    return Page();
                }


                var adminToken = Guid.NewGuid().ToString();
                resetLink = Url.Page("/Account/ResetPassword", pageHandler: null, values: new { email = Email, token = adminToken, isAdmin = IsAdmin }, protocol: Request.Scheme);
                adminUser.Token = adminToken;
                adminUser.TokenExpiry = DateTime.Now;
                await _userManager.UpdateAsync(adminUser);
            }

            else
            {
                // Find the veterinarian by email
                var veterinarian = await _context.Veterinarians.FirstOrDefaultAsync(v => v.Email == Email);
                if (veterinarian == null)
                {
                    TempData["ErrorMessage"] = "This email is not registered";
                    return Page();
                }

                var vetToken = Guid.NewGuid().ToString();

                resetLink = Url.Page("/Account/ResetPassword", pageHandler: null, values: new { email = Email, token = vetToken, isAdmin = IsAdmin }, protocol: Request.Scheme);

                veterinarian.Token = vetToken;
                veterinarian.TokenExpiry = DateTime.Now;

                await _context.SaveChangesAsync();
            }



            // Create password reset link

            // Send email with the reset link


            //Prepare email object
            SMTPEmailObject emailObject = new SMTPEmailObject
            {
                Port = int.TryParse(_smtpSettings.Port, out int port) && port > 0 ? port : 587,
                Server = _smtpSettings.Server,
                From = _smtpSettings.Username,
                Username = _smtpSettings.Username,
                Password = _smtpSettings.Password,

                To = Email,
                Subject = "Reset Password",
                Body = $"Hi,<br />Please click on the link below to Reset your password.<br /><a href='{resetLink}'>Reset Password</a>"
            };


            var response = await SMTP.SendEmailAsync(emailObject);
            if (response)
                TempData["SuccessMessage"] = "An email has been sent with instructions to reset your password.";
            else
                TempData["ErrorMessage"] = "Error while sending email";


            // Set success message

            return Page();
        }


        // Example method to generate a token (replace with your actual logic)
        private string GenerateResetToken(Veterinarian veterinarian)
        {
            // Implement your token generation logic here
            // This is just a placeholder
            return Guid.NewGuid().ToString();
        }




    }
}
