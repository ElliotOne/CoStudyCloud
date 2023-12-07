using System.ComponentModel.DataAnnotations;

namespace CoStudyCloud.Validators
{
    /// <summary>
    /// Represents Maximum File Size Validation Attribute
    /// </summary>
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult($"The file size must not exceed {_maxFileSize / 1024} KB.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
