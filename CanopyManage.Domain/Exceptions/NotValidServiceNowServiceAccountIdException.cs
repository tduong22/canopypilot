using System;

namespace CanopyManage.Domain.Exceptions
{
    public class NotValidServiceNowServiceAccountIdException : ArgumentException
    {
        public NotValidServiceNowServiceAccountIdException(string accountId)
            : base($"AccountId { accountId} is not valid")
        { }
    }
}
