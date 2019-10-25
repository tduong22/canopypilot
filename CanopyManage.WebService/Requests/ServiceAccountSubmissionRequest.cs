namespace CanopyManage.WebService.Requests
{
    public class ServiceAccountSubmissionRequest
    {
        public int ServiceNowSettingID { get; set; }
        public string TenantId { get; set; }
        public string ServiceNowUsername { get; set; }
        public string ServiceNowPassword { get; set; }
    }
}
