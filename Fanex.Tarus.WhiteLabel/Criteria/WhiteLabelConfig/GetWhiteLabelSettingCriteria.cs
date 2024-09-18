using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.WhiteLabel.Criteria.WhiteLabelConfig
{
    public class GetWhiteLabelSettingCriteria : CriteriaBase
    {
        public int? CustId { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey()
            => "Acc_WhiteLabelSetting_Sel";

        public override bool IsValid() => true;
    }
}
