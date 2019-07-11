namespace Fanex.Tarus.Customer.Criteria.CN88.HeadlinePossess
{
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.NAP.Framework.Data;
    using System;

    public class UpdateHeadlinePossessCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        [SpParam(Ignored = true)]
        public int SportbookPossess { get; set; }

        [SpParam(Ignored = true)]
        public int NonSportPossess { get; set; }

        public string ExtraPTSetting
        {
            get
            {
                return $"<ROOT>" +
                            $"<ExtraPT ProductId=\"1\" ExtraPT=\"{Utils.ToStringFormat((decimal)SportbookPossess / 100)}\" />" +
                            $"<ExtraPT ProductId=\"3\" ExtraPT=\"{Utils.ToStringFormat((decimal)NonSportPossess / 100)}\" />" +
                        $"</ROOT>";
            }
        }

        public DateTime EffectiveDate { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "CS_GroupExtraPT_Upd";
        }

        public override bool IsValid()
        {
            return CustId > 0
                && EffectiveDate >= DateTime.Today;
        }
    }
}
