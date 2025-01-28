using System.ComponentModel.DataAnnotations;
using Task = ChronoLink.Models.Task;

namespace ChronoLink.Validation
{
    public class EndDateTimeAfterStartDateTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var taskModel = (Task)validationContext.ObjectInstance;

            if (taskModel.EndDateTime <= taskModel.StartDateTime)
            {
                return new ValidationResult("End date/time must be after the start date/time.");
            }

            return ValidationResult.Success;
        }
    }
}
