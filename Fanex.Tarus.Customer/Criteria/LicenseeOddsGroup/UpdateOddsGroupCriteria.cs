namespace Fanex.Tarus.Customer.Criteria.LicenseeOddsGroup
{
    using Fanex.NAP.Framework.Data;

    public class UpdateOddsGroupCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string OddsGroup { get; set; }

        public bool IsActive { get; set; }

        public bool ApplyExistingMember { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
            => "Acc_Dep_LicenseeOddsGroupUpd";

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}
