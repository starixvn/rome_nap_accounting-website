using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Security.Criteria
{
    public class CheckDummyAccountCriteria : CriteriaBase
    {
        public string CustName { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerIsDummyAccount";
        }
    }
}
