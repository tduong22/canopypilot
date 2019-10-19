﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CanopyManage.Application.Commands.SubmitServiceAccount
{
    public class SubmitServiceAccountCommand : IRequest
    {
        public int ServiceNowSettingID { get; set; }
        public string TenantId { get; set; }
        public string ServiceNowUsername { get; set; }
        public string ServiceNowPassword { get; set; }
    }
}
