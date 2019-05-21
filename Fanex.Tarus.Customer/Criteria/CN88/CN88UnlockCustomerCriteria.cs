using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.CN88
{
    public class CN88UnlockCustomerCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public bool Closed => false;

        public int SubAccId => 0;

        public int UserId { get; set; }

        public string IP { get; set; }

        public string Language => "E";

        public override bool IsValid()
        {
            return UserId > 0 && CustId > 0;
        }

        public override string GetSettingKey()
        {
            return "CS_Acc_CustomerUpdUnlockAccess";
        }
    }
}
