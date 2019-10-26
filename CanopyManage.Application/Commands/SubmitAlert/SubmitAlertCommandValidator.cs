using FluentValidation;

namespace CanopyManage.Application.Commands.SubmitAlert
{
    public class SubmitAlertCommandValidator : AbstractValidator<SubmitAlertCommand>
    {
        public SubmitAlertCommandValidator()
        {
            RuleFor(x => x.AlertId).NotNull().NotEmpty();
            RuleFor(x => x.AlertType).NotNull().NotEmpty();
            RuleFor(x => x.Title).NotNull().NotEmpty();
            RuleFor(x => x.Message).NotNull().NotEmpty();
            RuleFor(x => x.ServiceNowSettingID).GreaterThan(0);
        }
    }
}
