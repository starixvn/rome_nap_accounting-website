namespace Fanex.Tarus.Customer.Business.BetTrade
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Criteria.BetTrade;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.BetTrade;

    public interface IBetTradeService
    {
        BetTradeStatus GetStatus(GetBetTradeStatusCriteria criteria);

        Result UpdateStatus(ICriteria criteria);
    }
}
