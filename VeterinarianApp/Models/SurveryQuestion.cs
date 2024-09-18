using System.ComponentModel.DataAnnotations;

namespace VeterinarianApp.Models
{
    public class SurveryQuestion
    {
        [Key]
        public int SurveyQuestionId { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }
        public bool IsRequired { get; set; }
    }
}
