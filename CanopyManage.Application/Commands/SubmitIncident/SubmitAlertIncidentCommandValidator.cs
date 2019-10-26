using FluentValidation;

namespace CanopyManage.Application.Commands.SubmitIncident
{
    public class SubmitAlertIncidentCommandValidator : AbstractValidator<SubmitAlertIncidentCommand>
    {
        public SubmitAlertIncidentCommandValidator()
        {
            RuleFor(x=>x.AlertId).NotNull().NotEmpty();
            RuleFor(x => x.AlertType).NotNull().NotEmpty();
            RuleFor(x => x.Title).NotNull().NotEmpty();
            RuleFor(x => x.Message).NotNull().NotEmpty();
            RuleFor(x => x.ServiceNowSettingID).GreaterThan(0);
        }
    }
}
