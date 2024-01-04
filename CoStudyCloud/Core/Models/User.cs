namespace CoStudyCloud.Core.Models
{
    public class User
    {
        public string? Id { get; set; }

        public string? Email { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? GoogleId { get; set; }

        public string? ProfileImageUrl { get; set; }

        public string? UserRole { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastEditDate { get; set; }
    }
}
