using CoStudyCloud.Core.Constants;
using System.ComponentModel.DataAnnotations;

namespace CoStudyCloud.Core.ViewModels
{
    public class GroupFormViewModel
    {
        [Required(ErrorMessage = ValidationMessagesConstant.RequiredMsg)]
        [MaxLength(100, ErrorMessage = ValidationMessagesConstant.MaxLengthMsg)]
        public string? Title { get; set; }

        [Required(ErrorMessage = ValidationMessagesConstant.RequiredMsg)]
        public string? Description { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
