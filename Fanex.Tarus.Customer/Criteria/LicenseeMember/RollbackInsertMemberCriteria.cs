using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeMember
{
    public class RollbackInsertMemberCriteria : CriteriaBase
    {
        public RollbackInsertMemberCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Acc_CustomerIns_Rollback";
    }
}