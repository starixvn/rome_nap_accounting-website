namespace Fanex.Tarus.BetList.Business
{
    using Constants;
    using Core;
    using DataAccess;
    using Extensions;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.NAP.Framework.Data;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class BetListDetailService : IBetListDetailService
    {
        private readonly IBetListDetailRepository _betListDetailRepository;
        
        public BetListDetailService(IBetListDetailRepository betListDetailRepository = null)
        {
            _betListDetailRepository = betListDetailRepository ?? new BetListDetailRepository();
        }

        public CashOutDetails GetCashOutDetails(ICriteria criteria)
        {
            var cashOutDetails = _betListDetailRepository.GetCashOutDetails(criteria);
            var ticketDetail = cashOutDetails.TicketDetail;

            if (ticketDetail != null)
            {
                ITicketHelper ticketHelper = new TicketHelper();
                ticketHelper.HideStatusCashOutHistory = true;

                var factory = new BetListHTMLFactory();
                var builder = factory.CreateBuilder();
                var ticket = ticketDetail.Ticket;

                ticketDetail.TransDateHtml = builder.BuildTrans(ticketDetail.Ticket);
                ticketDetail.ChoiceHtml = builder.BuildChoice(ticket, ticketHelper, null, false);
                ticketDetail.OddHtml = builder.BuildOdds(ticket, null, CacheRef.GetOddsTypeName);
                ticketDetail.StakeHtml = BetListExtension.BuildStake(ticketDetail, ref builder);

                var statusBuilder = builder.CreateStatusBuilder(ticket.BetTypeId);
                var statusBlock = statusBuilder.Render(ticket, ticketHelper, null, true);
                statusBlock.ShowIP = null;
                statusBlock.StatusResult = null;
                ticketDetail.StatusHtml = statusBlock.ToString();

                if (cashOutDetails.CashOutTickets != null)
                {
                    foreach (var cashoutTicket in cashOutDetails.CashOutTickets)
                    {
                        statusBuilder = builder.CreateStatusBuilder(cashoutTicket.BetType);
                        statusBlock = statusBuilder.Render(
                            new Ticket
                            {
                                BetTypeId = cashoutTicket.BetType,
                                Status = cashoutTicket.Status
                            },
                            ticketHelper,
                            null,
                            true);
                        statusBlock.ShowIP = null;
                        statusBlock.StatusResult = null;
                        cashoutTicket.StatusHtml = statusBlock.ToString();
                    }
                }
            }

            return cashOutDetails;
        }

        public IEnumerable<TicketDetail> GetLicenseeBonusDetails(ICriteria criteria)
        {
            var ticketDetails = new List<TicketDetail>();

            var tickets = _betListDetailRepository.GetLicenseeBonusDetails(criteria);

            if (tickets.Any())
            {
                var betListHelper = new BetListHelper();

                ITicketHelper ticketHelper = new TicketHelper();
                var factory = new BetListHTMLFactory();
                var builder = factory.CreateBuilder();

                foreach (ITicket ticket in tickets)
                {
                    ticketHelper.IsShowOddsWithChoice = IsShowOddsWithChoice(ticket.BetTypeId);

                    TicketDetail ticketDetail = new TicketDetail
                    {
                        Ticket = ticket,
                        TransDateHtml = builder.BuildTrans(ticket),
                        ChoiceHtml = builder.BuildChoice(ticket, ticketHelper, null, false),
                        OddHtml = builder.BuildOdds(ticket, null, CacheRef.GetOddsTypeName)
                    };

                    ticketDetail.StakeHtml = BetListExtension.BuildStake(ticketDetail, ref builder);
                    ticketDetail.StatusHtml = betListHelper.BuildStatus(ticketDetail.Ticket, ticketHelper, new List<ITicketData>(), factory, builder);

                    ticketDetails.Add(ticketDetail);
                }
            }

            return ticketDetails;
        }

        private bool IsShowOddsWithChoice(int betTypeId)
        {
            return betTypeId == BetTypeContants.MixParlay || betTypeId == BetTypeContants.SystemParlay;
        }
    }
}
