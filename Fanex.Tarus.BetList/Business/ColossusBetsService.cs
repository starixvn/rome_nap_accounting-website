namespace Fanex.Tarus.BetList.Business
{
    using Fanex.BetList.ColossusBets.Builder;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Tarus.BetList.Criteria;
    using Fanex.Tarus.BetList.DataAccess;

    public class ColossusBetsService : IColossusBetsService
    {
        private readonly IColossusBetsRepository _colossusBetsRepository;

        public ColossusBetsService()
            : this(null)
        {
        }

        public ColossusBetsService(IColossusBetsRepository colossusBetsRepository)
        {
            _colossusBetsRepository = colossusBetsRepository ?? new ColossusBetsRepository();
        }

        public string BetDetails(GetColossusBetsBetDetailsCriteria criteria)
        {
            ITicketHelper ticketHelper = new TicketHelper();
            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            var detailsTickets = _colossusBetsRepository.BetDetails(criteria);
            return builder.BuildColossusBetDetails(detailsTickets, ticketHelper);
        }
    }
}
