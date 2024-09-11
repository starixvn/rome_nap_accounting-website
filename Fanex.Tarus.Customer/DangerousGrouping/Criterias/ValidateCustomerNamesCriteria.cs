using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.DangerousGrouping.Criterias
{
    public class ValidateCustomerNamesCriteria : CriteriaBase
    {
        public string UsernameList { get; set; }

        public int UserID { get; set; }

        public string AdName { get; set; }

        public byte DangerType { get; set; }

        public int DangerLevel { get; set; }

        public string IP { get; set; }

        public string Permission { get; set; }

        public string DeniedList { get; set; }

        public override string GetSettingKey() => "Acc_CustomerCheckDangerCustList";
    }
}