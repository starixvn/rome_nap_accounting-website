using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.WhiteLabel.Criteria.WhiteLabelConfig
{
    public class EditWhiteLabelSettingCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public decimal AgentRate { get; set; }

        public decimal HeadlineRate { get; set; }

        public decimal SuperRate { get; set; }

        public decimal MasterRate { get; set; }

        public string Remark { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
            => "Acc_WhiteLabelSetting_Upd";

        public override bool IsValid() => true;
    }
}
