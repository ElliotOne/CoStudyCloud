using System.ComponentModel.DataAnnotations;

namespace CoStudyCloud.Validators
{
    /// <summary>
    /// Represents Permitted File Extensions Validation Attribute
    /// </summary>
    public class PermittedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _permittedExtensions;

        public PermittedExtensionsAttribute(params string[] permittedExtensions)
        {
            _permittedExtensions = permittedExtensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (string.IsNullOrEmpty(fileExtension) || !_permittedExtensions.Contains(fileExtension))
                {
                    return new ValidationResult($"Only files with {string.Join(", ", _permittedExtensions)} extensions are allowed.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
