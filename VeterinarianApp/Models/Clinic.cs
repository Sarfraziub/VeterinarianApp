using System.ComponentModel.DataAnnotations;

namespace VeterinarianApp.Models
{
    public class Clinic
    {
        [Key]
        public int Id { get; set; }
        public string ClinicName { get; set; }
        public string ClinicWebsite { get; set; }
        public string ClinicPhone { get; set; }
        public string ClinicEmail { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}