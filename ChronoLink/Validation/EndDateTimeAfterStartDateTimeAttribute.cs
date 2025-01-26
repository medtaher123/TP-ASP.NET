using System.ComponentModel.DataAnnotations;
using ChronoLink.Models;

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

            var eventModel = (Event)validationContext.ObjectInstance;

            if (eventModel.EndDateTime <= eventModel.StartDateTime)
            {
                return new ValidationResult("End date/time must be after the start date/time.");
            }

            return ValidationResult.Success;
        }
    }
}
