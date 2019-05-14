using Fanex.BetList.Core.Entities;

namespace Fanex.Tarus.BetList.Models.CN88
{
    public class TicketDto : Ticket
    {
        public string UGroup { get; set; }

        public decimal PlayerDiscount { get; set; }

        public decimal Payout { get; set; }

        public decimal CompanyPositionTaking()
            => 1 - AgentPositionTaking - MasterPositionTaking - SuperPositionTaking;
    }
}
