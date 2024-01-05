using CoStudyCloud.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace CoStudyCloud.Core.ViewModels
{
    public class StudySessionFormViewModel
    {
        [Display(Name = "Group")]
        public string? StudyGroupId { get; set; }

        [Required(ErrorMessage = ValidationMessagesConstant.RequiredMsg)]
        [MaxLength(100, ErrorMessage = ValidationMessagesConstant.MaxLengthMsg)]
        public string? Title { get; set; }

        [Required(ErrorMessage = ValidationMessagesConstant.RequiredMsg)]
        [MaxLength(255, ErrorMessage = ValidationMessagesConstant.MaxLengthMsg)]
        public string? Summary { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Starts at")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Ends at")]
        public DateTime EndDate { get; set; }
    }
}
