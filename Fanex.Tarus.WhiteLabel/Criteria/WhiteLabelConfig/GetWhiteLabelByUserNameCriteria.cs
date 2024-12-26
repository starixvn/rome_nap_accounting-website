using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.WhiteLabel.Criteria.WhiteLabelConfig
{
    public class GetWhiteLabelByUsernamesCriteria : CriteriaBase
    {
        public string Usernames { get; set; }

        public override string GetSettingKey()
            => "Acc_WhiteLabel_SelByUpline";

        public override bool IsValid() => true;
    }
}
