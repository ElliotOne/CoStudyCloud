namespace CoStudyCloud.Core.Models
{
    public class StudyGroupWithJoinStatus
    {
        public string? StudyGroupId { get; set; }
        public string? MappingId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsJoined { get; set; }
    }
}
