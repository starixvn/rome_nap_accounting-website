using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.DangerousGrouping.Criterias
{
    public class ValidateCustomerNamesCriteria : CriteriaBase
    {
        public string UsernameList { get; set; }

        public int UserID { get; set; }

        public byte DangerType { get; set; }

        public int DangerLevel { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey() => "Acc_CustomerCheckDangerCustList";
    }
}