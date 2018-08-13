namespace Fanex.Tarus.BetList.Business
{
    using Constants;
    using Core;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Tarus.BetList.Criteria;
    using Fanex.Tarus.BetList.DataAccess;
    using Models;
    using System.Collections.Generic;

    public class MicroGamingService : IMicroGamingService
    {
        private readonly IMicroGamingRepository _microGamingRepository;

        public MicroGamingService(IMicroGamingRepository microGamingRepository = null)
        {
            _microGamingRepository = microGamingRepository ?? new MicroGamingRepository();
        }

        public IEnumerable<TicketDetail> BetDetails(GetMicroGamingBetDetailsCriteria criteria)
        {
            var detailsTickets = _microGamingRepository.BetDetails(criteria);

            if (detailsTickets != null)
            {
                ITicketHelper ticketHelper = new TicketHelper();
                BetListHTMLFactory factory = new BetListHTMLFactory();
                BetListHTMLBuilder builder = factory.CreateBuilder();
                BetListHelper betListHelper = new BetListHelper();
                GetCachePropertyById funcGetOddsTypeName = ticketHelper.GetOddsTypeNameById;

                var outputLiveCasino = new List<ITicketData>();

                foreach (TicketDetail betDetail in detailsTickets)
                {
                    ticketHelper.IsShowOddsWithChoice = this.IsShowOddsWithChoice(betDetail.Ticket.BetTypeId);
                    betDetail.TransDateHtml = builder.BuildTrans(betDetail.Ticket);
                    betDetail.ChoiceHtml = builder.BuildChoice(betDetail.Ticket, ticketHelper, outputLiveCasino, false);
                    betDetail.StakeHtml = BetListExtension.BuildStake(betDetail, ref builder);
                    betDetail.OddHtml = builder.BuildOdds(betDetail.Ticket, outputLiveCasino, funcGetOddsTypeName);
                    betDetail.StatusHtml = betListHelper.BuildStatus(betDetail.Ticket, ticketHelper, outputLiveCasino, factory, builder);
                }
            }

            return detailsTickets;
        }

        private bool IsShowOddsWithChoice(int betTypeId)
        {
            return betTypeId == BetTypeContants.MixParlay || betTypeId == BetTypeContants.SystemParlay;
        }
    }
}
