using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinarianApp.Models
{
    public class SurveyOption
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }

        public int? SurveyQuestionId { get; set; }
        [ForeignKey(nameof(SurveyQuestionId))]
        public virtual SurveryQuestion? SurveryQuestion { get; set; }
    }
}
