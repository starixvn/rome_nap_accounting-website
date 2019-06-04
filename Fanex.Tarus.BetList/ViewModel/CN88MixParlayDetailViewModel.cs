using System.Collections.Generic;
using CN88.BetList.Core.Builders;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.TicketHelper;
using Fanex.Tarus.BetList.Models.CN88;

namespace Fanex.Tarus.BetList.ViewModel
{
    public class CN88MixParlayDetailViewModel
    {
        public CN88MixParlayDetailViewModel(IList<TicketDto> tickets)
        {
            Tickets = tickets;
            TicketHelper = new TicketHelper();
            CN88Builder = new BetListCN88HtmlBuilder();
            Builder = new BetListHTMLBuilder();
        }

        public IList<TicketDto> Tickets { get; }

        public ITicketHelper TicketHelper { get; }

        public IBetListCN88HtmlBuilder CN88Builder { get; }

        public IBetListHTMLBuilder Builder { get; }
    }
}
