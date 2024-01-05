using System.ComponentModel.DataAnnotations;

namespace CoStudyCloud.Core.ViewModels
{
    public class GroupIndexViewModel
    {
        public IEnumerable<StudyGroupWithJoinStatusViewModel>? StudyGroupWithJoinStatusViewModels { get; set; }
    }

    public class StudyGroupWithJoinStatusViewModel
    {
        public string? StudyGroupId { get; set; }
        public string? MappingId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        [Display(Name = "Created on")]
        public DateTime CreateDate { get; set; }
        public bool IsJoined { get; set; }
    }
}
