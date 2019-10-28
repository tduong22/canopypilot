using FluentValidation;

namespace CanopyManage.Application.Commands.SubmitAlert
{
    public class SubmitAlertCommandValidator : AbstractValidator<SubmitAlertCommand>
    {
        public SubmitAlertCommandValidator()
        {
            //Uncomment for real case HTTP use, 
            //Currently, this Command as a testing purpose to submit message to Service Bus 
            //and leave validation for SubmitAlertIncidentCommand
            
            /*RuleFor(x => x.TenantId).NotNull().NotEmpty();
            RuleFor(x => x.AlertId).NotNull().NotEmpty();
            RuleFor(x => x.AlertType).NotNull().NotEmpty();
            RuleFor(x => x.Title).NotNull().NotEmpty();
            RuleFor(x => x.Message).NotNull().NotEmpty();
            RuleFor(x => x.ServiceNowSettingID).GreaterThan(0);*/
        }
    }
}
