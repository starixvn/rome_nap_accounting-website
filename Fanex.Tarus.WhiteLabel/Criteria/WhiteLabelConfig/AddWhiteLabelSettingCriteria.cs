using Fanex.NAP.Framework.Data;
using System;

namespace Fanex.Tarus.WhiteLabel.Criteria.WhiteLabelConfig
{
    public class AddWhiteLabelSettingCriteria : EditWhiteLabelSettingCriteria
    {
        public string WhiteLabel { get; set; }

        public DateTime LaunchedDate { get; set; }

        public override string GetSettingKey()
            => "Acc_WhiteLabelSetting_Ins";

        public override bool IsValid() => CustId > 0 && !string.IsNullOrWhiteSpace(WhiteLabel);
    }
}
