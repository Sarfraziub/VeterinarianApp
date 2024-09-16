using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinarianApp.Models
{
    public class VeterinarianService
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public int VeterinarianId { get; set; }
        [Required]
        public int ServiceId { get; set; }
        [ForeignKey(nameof(VeterinarianId))]
        public virtual Veterinarian Veterinarian { get; set; }
        [ForeignKey(nameof(ServiceId))]
        public virtual Service Service { get; set; }

    }
}
