using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.CreditCustomer
{
    public class UnlockAccessCriteria : CriteriaBase
    {
        public UnlockAccessCriteria(
            int custId,
            int userId,
            byte locked,
            string language,
            string ip,
            byte systemId)
        {
            CustId = custId;
            UserId = userId;
            Locked = locked;
            Language = language;
            IP = ip;
            SystemId = systemId;
        }

        public int CustId { get; }

        public int UserId { get; }

        public byte Locked { get; }

        public string Language { get; }

        public string IP { get; }

        public byte SystemId { get; }

        public override string GetSettingKey()
            => "Acc_CustomerUpdLocked";

        public override bool IsValid()
            => CustId > 0 && UserId > 0;
    }
}
