using System.ComponentModel.DataAnnotations;

namespace Saay.Infrastructure.Validation.Validation
{
    public class FutureTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            var dueTime = validationContext.ObjectType.GetProperty("DueTime");

            if (dueTime != null) { 
                TimeOnly? dueTimeValue = (TimeOnly?)dueTime.GetValue(validationContext.ObjectInstance);
                if (dueTimeValue.HasValue)
                {
                    DateOnly dueDate = (DateOnly)validationContext.ObjectType.GetProperty("DueDate")!.GetValue(validationContext.ObjectInstance)!;
                    DateTime dueDateTime = dueDate.ToDateTime(dueTimeValue.Value);
                    if (dueDateTime <= DateTime.Now)
                    {
                        return new ValidationResult("Due time must be in the future.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
