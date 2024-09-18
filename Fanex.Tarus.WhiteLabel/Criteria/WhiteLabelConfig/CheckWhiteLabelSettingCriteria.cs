using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.WhiteLabel.Criteria.WhiteLabelConfig
{
    public class CheckWhiteLabelSettingCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public bool IsNew { get; set; }

        public override string GetSettingKey()
            => "Acc_WhiteLabelSetting_Check";

        public override bool IsValid() => true;
    }
}
