namespace Fanex.Tarus.Customer.Criteria.ViewPT
{
    using System.Diagnostics.CodeAnalysis;

    public class IsDepositCustomerCriteria
    {
        public string UserName { get; set; }

        public string GetSettingKey()
        {
            return "Acc_CustSetting_CheckIsDeposit";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
