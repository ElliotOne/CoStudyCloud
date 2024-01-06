using System.ComponentModel.DataAnnotations;

namespace CoStudyCloud.Core.ViewModels
{
    public class DocumentIndexViewModel
    {
        public IEnumerable<DocumentWithOwnerStatusViewModel>? DocumentWithOwnerStatusViewModels { get; set; }
    }

    public class DocumentWithOwnerStatusViewModel
    {
        public string? Title { get; set; }

        public string? FileUrl { get; set; }

        [Display(Name = "Group")]
        public string? StudyGroupTitle { get; set; }

        [Display(Name = "User")]
        public string? UserFullName { get; set; }

        [Display(Name = "Uploaded on")]
        public DateTime CreateDate { get; set; }

        public bool IsOwned { get; set; }
    }
}
