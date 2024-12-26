using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.WhiteLabel.Criteria.WhiteLabelConfig
{
    public class GetAgentInfoCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
            => "Acc_WhiteLabel_SelCustInfo";

        public override bool IsValid() => true;
    }
}
