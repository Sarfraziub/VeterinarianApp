using System.ComponentModel.DataAnnotations;

namespace VeterinarianApp.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
