using MediatR;

namespace CanopyManage.Application.Commands.SubmitIncident
{
    public class SubmitIncidentCommand : IRequest
    {
        public string AlertId { get; set; }
        public int ServiceNowSettingID { get; set; }
        public string AlertType { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}

