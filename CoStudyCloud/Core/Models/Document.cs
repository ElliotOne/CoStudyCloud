namespace CoStudyCloud.Core.Models
{
    public class Document
    {
        public string? Id { get; set; }

        public string? StudyGroupId { get; set; }

        public string? UploaderUserId { get; set; }

        public string? Title { get; set; }

        public string? FileName { get; set; }

        public string? FileUrl { get; set; }

        public DateTime CreateDate { get; set; }

        public StudyGroup? StudyGroup { get; set; }

        public User? UploaderUser { get; set; }
    }
}
