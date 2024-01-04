namespace CoStudyCloud.Core.Models
{
    public class StudyGroup
    {
        public string? Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime CreateDate { get; set; }

        public string? AdminUserId { get; set; }

        public User? AdminUser { get; set; }
    }
}
