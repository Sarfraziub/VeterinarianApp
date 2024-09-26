using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinarianApp.Models
{
    public class Veterinarian
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? Instagram { get; set; }
        public string? Youtube { get; set; }
        public string? X { get; set; }
        public string? FaceBook { get; set; }
        public string? TIkTok { get; set; }
        public int HuntVetScore { get; set; }
        public int? ApprovedBy { get; set; }
        public int? VetHuntScore { get; set; }
        public string LicenseNo { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenExpiry { get; set; }

        public virtual Clinic? Clinic { get; set; }
        public virtual ICollection<VeterinarianService>? VeterinarianServices { get; set; }

        [NotMapped]
        public string? ConfirmPassword { get; set; }


    }
}
