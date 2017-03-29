using System.Linq;
using Fanex.BetList.Accounting.Extensions;
using Fanex.BetList.Accounting.Models;
using Fanex.BetList.Accounting.Repositories;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using Fanex.NAP.Framework.Data;

namespace Fanex.BetList.Accounting.Services
{
    public class BetListDetailService : IBetListDetailService
    {
        private readonly IBetListDetailRepository _betListDetailRepository;

        public BetListDetailService(IBetListDetailRepository betListDetailRepository = null)
        {
            _betListDetailRepository = betListDetailRepository ?? new BetListDetailRepository();
        }

        public CashOutHistory GetCashOutHistory(ICriteria criteria)
        {
            var cashOutHistory = _betListDetailRepository.GetCashOutHistory(criteria);
            var ticketDetail = cashOutHistory.TicketDetail;

            if (ticketDetail != null)
            {
                ITicketHelper ticketHelper = new TicketHelper();
                var factory = new BetListHTMLFactory();
                var builder = factory.CreateBuilder();
                var ticket = ticketDetail.Ticket;

                ticketDetail.TransDateHtml = builder.BuildTrans(ticketDetail.Ticket);
                ticketDetail.ChoiceHtml = builder.BuildChoice(ticket, ticketHelper, null, false);
                ticketDetail.OddHtml = builder.BuildOdds(ticket, null, CacheRef.GetOddsTypeName);
                ticketDetail.StakeHtml = BetListExtension.BuildStake(ticketDetail, ref builder);
                ticketDetail.StatusHtml = ticket.Status.ToUpperInvariant();
            }

            cashOutHistory.TicketDetail = null;
            cashOutHistory.CashOutTickets = new System.Collections.Generic.List<CashOutTicket>();
            return cashOutHistory;
        }
    }
}
