namespace Fanex.Tarus.Customer.Criteria.InitializingStakeLevel
{
    using System.Collections.Generic;
    using System.Text;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Entities.InitializingStakeLevel;

    public class StakeLevelInsCriteria : CriteriaBase
    {
        public StakeLevelInsCriteria()
        {
            StakeLevels = new List<StakeLevelModel>();
        }

        [SpParam(Ignored = true)]
        public IEnumerable<StakeLevelModel> StakeLevels { get; set; }

        public string InputData
        {
            get
            {
                var builder = new StringBuilder();
                builder.Append("<ROOT>");
                foreach (var item in StakeLevels)
                {
                    builder.Append("<File ");
                    builder.Append("SportType = \"" + item.SportTypeId + "\" ");
                    builder.Append("BetType = \"" + item.BetTypeId + "\" ");
                    builder.Append("Site = \"" + item.SiteId + "\" ");
                    builder.Append("CreditCurrency = \"" + item.CreditCurrency + "\" ");
                    builder.Append("DepositCurrency = \"" + item.DepositCurrency + "\" ");
                    builder.Append("NewMinBet = \"" + item.NewMinBet + "\" ");
                    builder.Append("NewMaxBet = \"" + item.NewMaxBet + "\" ");
                    builder.Append("CurMinBet = \"" + item.MinBet + "\" ");
                    builder.Append("CurMaxBet = \"" + item.MaxBet + "\" ");
                    builder.Append("GameGroupID = \"" + item.GameGroupId + "\" ");
                    builder.Append("/>");
                }

                builder.Append("</ROOT>");

                return builder.ToString();
            }
        }

        public int UserID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingStake_StakelevelIns";
    }
}
