namespace Fanex.Tarus.Customer.DangerousGrouping.Criterias
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.DangerousGrouping.Enums;

    public class UpdateCustomerDangerCriteria : CriteriaBase
    {
        public int UserID { get; set; }

        public string Username { get; set; }

        public string AdName { get; set; }

        public bool UpdateDownline { get; set; }

        public DangerType DangerType { get; set; }

        public int DangerLevel { get; set; }

        public string IP { get; set; }

        public string Permission { get; set; }

        public string DeniedList { get; set; }

        public override string GetSettingKey() => "Acc_CustomerUpdDangercust";
    }
}