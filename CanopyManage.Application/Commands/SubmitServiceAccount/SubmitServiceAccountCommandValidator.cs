using FluentValidation;

namespace CanopyManage.Application.Commands.SubmitServiceAccount
{
    public class SubmitServiceAccountCommandValidator : AbstractValidator<SubmitServiceAccountCommand>
    {
        public SubmitServiceAccountCommandValidator()
        {
            RuleFor(x=>x.ServiceNowSettingID).GreaterThan(0);
            RuleFor(x=>x.TenantId).NotNull().NotEmpty();
            RuleFor(x => x.ServiceNowUsername).NotNull().NotEmpty();
            RuleFor(x => x.ServiceNowPassword).NotNull().NotEmpty();
        }
    }
}
