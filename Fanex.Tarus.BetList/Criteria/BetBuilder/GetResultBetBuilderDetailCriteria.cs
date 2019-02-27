namespace Fanex.Tarus.BetList.Criteria.BetBuilder
{
    using NAP.Framework.Data;

    public class GetResultBetBuilderDetailCriteria : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public int SportType { get; set; }

        [SpParam(Ignored = true)]
        public int BetType { get; set; }

        public int MatchId { get; set; }

        public int MarketId { get; set; }

        public override string GetSettingKey() => "Acc_TicketResult_BetBuilder";

        public override bool IsValid() => MatchId > 0 && MarketId > 0;
    }
}
