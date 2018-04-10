namespace Fanex.Tarus.BetList.Business
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.BetList.Criteria;
    using Fanex.Tarus.BetList.DataAccess;
    using Fanex.Tarus.BetList.Models.Exchange;
    using System;
    using System.Collections.Generic;

    public interface IExchangeBetListService
    {
        ExchangeViewModel GetBetList(ExchangeBetListCriteria criteria);
        IEnumerable<ExchangeTicket> GetBetDetail(ExchangeBetDetailCriteria criteria);
    }

    public class ExchangeBetListService : IExchangeBetListService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IExchangeBetListRepository exchangeRepository;

        public ExchangeBetListService() : this(null, null)
        {

        }

        public ExchangeBetListService(IDynamicRepository dynamicRepository, IExchangeBetListRepository exchangeRepository)
        {
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
            this.exchangeRepository = exchangeRepository ?? new ExchangeBetListRepository();
        }


        public ExchangeViewModel GetBetList(ExchangeBetListCriteria criteria)
        {
            var model = exchangeRepository.GetBetList(criteria);
            model.Index = ((criteria.PageIndex - 1) * criteria.PageSize) + 1;
            return model;
        }

        public IEnumerable<ExchangeTicket> GetBetDetail(ExchangeBetDetailCriteria criteria)
        {
            var details = dynamicRepository.Fetch<ExchangeTicket>(criteria);
            foreach (var detail in details)
            {
                detail.HasLiability = !criteria.Type.Equals("back", StringComparison.InvariantCultureIgnoreCase);
            }

            return details;
        }
    }
}
