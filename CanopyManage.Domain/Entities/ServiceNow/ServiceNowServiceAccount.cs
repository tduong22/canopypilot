using CanopyManage.Domain.Exceptions;
using CanopyManage.Domain.SeedWork;

namespace CanopyManage.Domain.Entities
{
    public class ServiceNowServiceAccount : ServiceAccount, IAzureResource
    {
        private const char _delimeter = '-';
        private string _tenantId;

        public override string Id
        {
            get => ComposeServiceNowServiceAccountId();
            protected set => base.Id = value;
        }
        public ServiceNowServiceAccount(int serviceNowSettingId, string tenantId, string userName, string password)
        {
            ServiceNowSettingId = serviceNowSettingId;
            TenantId = tenantId;
            ServiceUserName = userName;
            ServiceSecret = password;
        }
        public int ServiceNowSettingId { get; set; }
        public ServiceNowSetting ServiceNowSetting { get; set; }

        public string TenantId { get => _tenantId; set => _tenantId = value; }

        public static bool ValidateServiceNowAccountId(string id)
        {
            //TODO: Using regex check
            var splits = id.Split(_delimeter);
            if (splits != null)
                return (splits.Length == 2) ? true: 
                    throw new NotValidServiceNowServiceAccountIdException(id);
            throw new NotValidServiceNowServiceAccountIdException(id);
        }

        public static string GetTenantIdFromId(string serviceNowServiceAccountId)
        {
            ValidateServiceNowAccountId(serviceNowServiceAccountId);
            return serviceNowServiceAccountId.Split(_delimeter)[0];
        }
        public static int GetServiceNowSettingIdFromId(string serviceNowServiceAccountId)
        {
            ValidateServiceNowAccountId(serviceNowServiceAccountId);
            return int.Parse(serviceNowServiceAccountId.Split(_delimeter)[1]);
        }

        public string ComposeServiceNowServiceAccountId()
        {
            return TenantId + _delimeter + ServiceNowSettingId;
        }
    }
}
