using System.ComponentModel.DataAnnotations;
using Services;

namespace ConsoleTeleBotMaster.Data;

public class StartDateTimeValidator : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is AppConfigView schedulerInstance)
        {
            if (schedulerInstance.SchedulerStartDate is null && schedulerInstance.SchedulerStartTime is not null)
            {
                return new ValidationResult("The \"Date start\" fields are required if the \"Time start\" field is filled.");
            }
        
            if (schedulerInstance.SchedulerStartDate is not null && schedulerInstance.SchedulerStartTime is null)
            {
                return new ValidationResult("The \"Time start\" fields are required if the \"Date start\" field is filled.");
            }
        }

        return ValidationResult.Success;
    }
}