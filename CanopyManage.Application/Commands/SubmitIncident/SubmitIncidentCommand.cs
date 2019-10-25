﻿using MediatR;

namespace CanopyManage.Application.Commands.SubmitIncident
{
    public class SubmitIncidentCommand : IRequest
    {
        public int ServiceNowSettingID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}

