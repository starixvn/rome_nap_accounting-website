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
    }
}
