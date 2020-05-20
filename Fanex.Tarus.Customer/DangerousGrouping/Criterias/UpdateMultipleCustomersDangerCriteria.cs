namespace Fanex.Tarus.Customer.DangerousGrouping.Criterias
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.DangerousGrouping.Enums;

    public class UpdateMultipleCustomersDangerCriteria : CriteriaBase
    {
        public string UsernameList { get; set; }

        public string UserName { get; set; }

        public string IP { get; set; }

        public bool UpdateDownline { get; set; }

        public DangerType DangerType { get; set; }

        public int DangerLevel { get; set; }

        public bool IsBA { get; set; }

        public override string GetSettingKey() => "Acc_CustomerUpdDangerCustList";
    }
}