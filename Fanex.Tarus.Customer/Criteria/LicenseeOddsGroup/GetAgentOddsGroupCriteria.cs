namespace Fanex.Tarus.Customer.Criteria.LicenseeOddsGroup
{
    using Fanex.NAP.Framework.Data;

    public class GetAgentOddsGroupCriteria: CriteriaBase
    {
        public string CustIds { get; set; }

        public GetAgentOddsGroupCriteria(string custids)
        {
            this.CustIds = custids;
        }

        public override string GetSettingKey()
            => "Acc_Dep_LicenseeOddsGroupSelByAgent";

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(CustIds);
        }
    }
}
