namespace CoStudyCloud.Core.Models
{
    public class DocumentWithOwnerStatus
    {
        public string? Id { get; set; }
        public string? StudyGroupId { get; set; }
        public string? UploaderUserId { get; set; }
        public string? Title { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public string? StudyGroupTitle { get; set; }
        public string? UploaderUserFirstName { get; set; }
        public string? UploaderUserLastName { get; set; }
        public bool IsOwned { get; set; }
    }
}
