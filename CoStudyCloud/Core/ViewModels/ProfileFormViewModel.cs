using System.ComponentModel.DataAnnotations;

namespace CoStudyCloud.Core.ViewModels
{
    public class ProfileFormViewModel
    {
        public string? Email { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        public string? ProfileImageUrl { get; set; }

        [Display(Name = "Role")]
        public string? UserRole { get; set; }

        [Display(Name = "Member Since")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime LastEditDate { get; set; }
    }
}
