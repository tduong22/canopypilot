using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CanopyManage.Application.Commands.SubmitServiceAccount
{
    public class SubmitServiceAccountCommand : IRequest<SubmitServiceAccountCommandResponse>
    {
        public int ServiceNowSettingID { get; set; }
        public string TenantId { get; set; }
        public string ServiceNowUsername { get; set; }
        public string ServiceNowPassword { get; set; }
    }

    public class SubmitServiceAccountCommandResponse
    {
        public bool IsSuccessful { get; set; }
        public string Username { get; set; }
    }
}
