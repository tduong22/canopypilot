namespace CanopyManage.Domain.Aggregates
{
    public class AlertIncident : ServiceIncident {

        public string AlertType { get; set; }
        public string AlertId { get; set; }
    }
}
