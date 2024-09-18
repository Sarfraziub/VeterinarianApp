using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinarianApp.Models
{
    public class SurveyResponse
    {
        [Key]
        public int Id { get; set; }
        public string ResponseText { get; set; }

        public int? SurveyQuestionId { get; set; }
        [ForeignKey(nameof(SurveyQuestionId))]
        public virtual SurveryQuestion? SurveryQuestion { get; set; }

        public int? VeterinarianId { get; set; }
        [ForeignKey(nameof(VeterinarianId))]
        public virtual Veterinarian? Veterinarian { get; set; }


    }
}
