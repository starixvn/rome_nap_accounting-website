namespace Fanex.Tarus.Customer.Criteria.LockUnsuspend
{
    using Fanex.NAP.Framework.Data;

    public class UpdateCustomerCriteria : CriteriaBase
    {
        public string CustList { get; set; }

        public bool Lock { get; set; }

        public string DeniedList { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerLockSuspendUpd";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(CustList);
        }
    }
}
