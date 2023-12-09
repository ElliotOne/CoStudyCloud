using CoStudyCloud.Validators;
using System.ComponentModel.DataAnnotations;

namespace CoStudyCloud.Core.ViewModels
{
    public class ProfileFormViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        public string? ProfilePicURL { get; set; }

        [MaxFileSize(2 * 1024 * 1024)]
        [PermittedExtensions(".jpg", ".jpeg", ".png")]
        public IFormFile? ProfileImageFile { get; set; }

        [Display(Name = "Role")]
        public string? UserRole { get; set; }

        [Display(Name = "Member Since")]
        public string? CreateDate { get; set; }

        [Display(Name = "Last Updated")]
        public string LastEditDate { get; set; }
    }
}
