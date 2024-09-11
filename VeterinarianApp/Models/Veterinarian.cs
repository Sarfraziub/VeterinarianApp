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
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Instagram { get; set; }
        public string Youtube { get; set; }
        public string X { get; set; }
        public string TIkTok { get; set; }
        public int HuntVetScore { get; set; }
        public int? ApprovedBy { get; set; }
    }
}
