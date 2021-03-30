using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.CreditCustomer
{
    public class UnlockAccessCriteria : CriteriaBase
    {
        public UnlockAccessCriteria(int custId, int userId, string ip)
        {
            CustId = custId;
            UserId = userId;
            IP = ip;
        }

        public int CustId { get; }

        public int UserId { get; }

        public string IP { get; }

        public override string GetSettingKey()
            => "";

        public override bool IsValid()
            => CustId > 0 && UserId > 0;
    }
}
