namespace CoStudyCloud.Core.Models
{
    public class StudySession
    {
        public string? Id { get; set; }

        public string? StudyGroupId { get; set; }

        public string? Title { get; set; }

        public string? CalendarSyncId { get; set; }

        public string? Summary { get; set; }

        public string? Description { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public StudyGroup? StudyGroup { get; set; }
    }
}
