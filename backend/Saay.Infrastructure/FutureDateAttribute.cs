using System.ComponentModel.DataAnnotations;

namespace Saay.Infrastructure
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            var dueDateProperty = validationContext.ObjectType
            .GetProperty("DueDate");

            if (dueDateProperty != null)
            {
                DateOnly dueDate = (DateOnly)dueDateProperty.GetValue(validationContext.ObjectInstance)!;

                if (dueDate < DateOnly.FromDateTime(DateTime.Now))
                {
                    return new ValidationResult("Due date must be in the future.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
