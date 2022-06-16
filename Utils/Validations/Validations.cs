using System.ComponentModel.DataAnnotations;

namespace csharp_blog_backend.Utils.Validations
{
    public class MoreThanOneWordValidationAttribute : ValidationAttribute
    {
        // ...
        protected override ValidationResult IsValid(
           object value, ValidationContext validationContext)
        {
            string fieldValue = (string)value;

            if (fieldValue == null || fieldValue.Trim().IndexOf(" ") == -1)
            {
                return new ValidationResult("Il campo deve contenere almeno due parole");
            }

            return ValidationResult.Success;
        }
    }
}