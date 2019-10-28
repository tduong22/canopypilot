using MediatR;

namespace CanopyManage.Application.Commands.SubmitAlert
{
    public class SubmitAlertCommand : IRequest
    {
        public string TenantId { get; set; }
        public string AlertId { get; set; }
        public int ServiceNowSettingID { get; set; }
        public string AlertType { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
