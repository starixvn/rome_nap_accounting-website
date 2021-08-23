using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.CreditCustomer
{
    public class UpdateProtectedStatusCriteria : CriteriaBase
    {
        public UpdateProtectedStatusCriteria(
            int custId,
            bool isProtectionMode,
            int userId,
            string ip)
        {
            CustId = custId;
            IsProtectionMode = isProtectionMode;
            UserId = userId;
            IP = ip;
        }

        public int CustId { get; }

        public bool IsProtectionMode { get; }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey()
            => "Age_CustomerUpdProtectedMode";

        public override bool IsValid()
            => CustId > 0 && UserId > 0;
    }
}
