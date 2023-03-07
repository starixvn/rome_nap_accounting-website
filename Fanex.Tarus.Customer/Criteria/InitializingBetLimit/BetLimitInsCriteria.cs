namespace Fanex.Tarus.Customer.Criteria.InitializingBetLimit
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Entities.InitializingBetLimit;
    using Sunplus.Accounting.Core.Configurations;
    using System.Collections.Generic;
    using System.Text;

    public class BetLimitInsCriteria : CriteriaBase
    {
        public BetLimitInsCriteria()
        {
            BetLimits = new List<BetLimitModel>();
        }

        [SpParam(Ignored = true)]
        public IEnumerable<BetLimitModel> BetLimits { get; set; }

        public string InputData
        {
            get
            {
                var builder = new StringBuilder();
                builder.Append("<ROOT>");
                foreach (var item in BetLimits)
                {
                    builder.Append("<BetLimit ");
                    builder.Append("SportType = \"" + item.SportTypeId + "\" ");
                    builder.Append("BetType = \"" + item.BetTypeId + "\" ");
                    builder.Append("Site = \"" + item.SiteId + "\" ");
                    builder.Append("CreditCurrency = \"" + item.CreditCurrency + "\" ");
                    builder.Append("DepositCurrency = \"" + item.DepositCurrency + "\" ");
                    builder.Append("NewMinBet = \"" + item.NewMinBet + "\" ");
                    builder.Append("NewMaxBet = \"" + item.NewMaxBet + "\" ");
                    builder.Append("NewMaxPerMatch = \"" + item.NewMaxPerMatch + "\" ");
                    builder.Append("NewMaxPerBall = \"" + item.NewMaxPerBall + "\" ");
                    builder.Append("NewMaxGameStake = \"" + item.NewMaxGameStake + "\" ");
                    builder.Append("CurMinBet = \"" + item.MinBet + "\" ");
                    builder.Append("CurMaxBet = \"" + item.MaxBet + "\" ");
                    builder.Append("CurMaxPerMatch = \"" + item.MaxPerMatch + "\" ");
                    builder.Append("CurMaxPerBall = \"" + item.MaxPerBall + "\" ");
                    builder.Append("CurMaxGameStake = \"" + item.MaxGameStake + "\" ");
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

        public byte SettingStatus => WebSettings.SettingStatus;

        public override string GetSettingKey()
            => "Acc_InitializingGlobal_BetLimitIns";
    }
}
