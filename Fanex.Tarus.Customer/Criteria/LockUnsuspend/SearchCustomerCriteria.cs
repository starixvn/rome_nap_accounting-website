namespace Fanex.Tarus.Customer.Criteria.LockUnsuspend
{
    using Fanex.NAP.Framework.Data;

    public class SearchCustomerCriteria : CriteriaBase
    {
        public string DeniedList { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerLockSuspendSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
