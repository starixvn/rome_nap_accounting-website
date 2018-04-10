namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class BetListBetTradeDetailCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public long RefNo { get; set; }

        public override string GetSettingKey()
        {
            return "BetList_BetTradeDetails";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}