namespace Fanex.Tarus.Customer.Criteria.CN88.MoveSuper
{
    using Fanex.CustomerSetting.Module.Core;
    using Fanex.NAP.Framework.Data;

    public class MoveSuperCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int SuperId { get; set; }

        public string HeadlineUsername { get; set; }

        [SpParam(Ignored = true)]
        public int SportsbookPossess { get; set; }

        [SpParam(Ignored = true)]
        public int NonSportPossess { get; set; }

        public string ExtraPTSetting
        {
            get
            {
                return $"<ROOT>" +
                            $"<ExtraPT ProductId=\"1\" ExtraPT=\"{Utils.ToStringFormat((decimal)SportsbookPossess / 100)}\" />" +
                            $"<ExtraPT ProductId=\"3\" ExtraPT=\"{Utils.ToStringFormat((decimal)NonSportPossess / 100)}\" />" +
                        $"</ROOT>";
            }
        }

        public string EffectiveDate { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "CS_Customer_ChangeHeadline";
        }
    }
}
