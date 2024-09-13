using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeMember
{
    public class RollbackInsertAccountCriteria : CriteriaBase
    {
        public RollbackInsertAccountCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Acc_CustomerIns_Rollback";
    }
}