using System.ComponentModel.DataAnnotations;

namespace CoStudyCloud.Core.ViewModels
{
    public class StudySessionIndexViewModel
    {
        public IEnumerable<StudySessionWithGroupViewModel>? StudySessionWithGroupViewModels { get; set; }
    }

    public class StudySessionWithGroupViewModel
    {
        public string? Summary { get; set; }

        [Display(Name = "Starts at")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Ends at")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Group")]
        public string? StudyGroupTitle { get; set; }
    }
}
