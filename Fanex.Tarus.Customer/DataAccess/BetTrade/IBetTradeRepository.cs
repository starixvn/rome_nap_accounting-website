namespace Fanex.Tarus.Customer.DataAccess.BetTrade
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Criteria.BetTrade;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.BetTrade;

    public interface IBetTradeRepository
    {
        BetTradeStatus GetStatus(GetBetTradeStatusCriteria criteria);

        Result UpdateStatus(ICriteria criteria);
    }
}
