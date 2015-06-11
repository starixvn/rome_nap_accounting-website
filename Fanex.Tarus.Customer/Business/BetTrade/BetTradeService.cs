namespace Fanex.Tarus.Customer.Business.BetTrade
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Criteria.BetTrade;
    using Fanex.Tarus.Customer.DataAccess.BetTrade;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.BetTrade;

    public class BetTradeService : IBetTradeService
    {
        private readonly IBetTradeRepository _betTradeRepository;

        public BetTradeService(IBetTradeRepository betTradeRepository = null)
        {
            _betTradeRepository = betTradeRepository ?? new BetTradeRepository();
        }

        public BetTradeStatus GetStatus(GetBetTradeStatusCriteria criteria)
        {
            return _betTradeRepository.GetStatus(criteria);
        }

        public Result UpdateStatus(ICriteria criteria)
        {
            return _betTradeRepository.UpdateStatus(criteria);
        }
    }
}
