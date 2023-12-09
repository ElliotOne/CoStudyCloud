using CoStudyCloud.Validators;
using System.ComponentModel.DataAnnotations;

namespace CoStudyCloud.Core.ViewModels
{
    public class DocumentFormViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Group")]
        public int StudyGroupId { get; set; }
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public DateTime CreateDate { get; set; }

        [MaxFileSize(5 * 1024 * 1024)]
        [PermittedExtensions(".pdf", ".doc", ".docx", ".ppt", ".pptx")]
        [Display(Name = "Document")]
        public IFormFile? DocumentFile { get; set; }
    }
}
