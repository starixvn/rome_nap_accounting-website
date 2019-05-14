using CN88.BetList.Core.Builders;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.TicketHelper;
using Fanex.Tarus.BetList.Models.CN88;

namespace Fanex.Tarus.BetList.ViewModel
{
    public class CN88BetListViewModel
    {
        public CN88BetListViewModel(
            int custId,
            string language,
            BetListData betListData)
        {
            CustId = custId;
            BetListData = betListData;
            TicketHelper = new TicketHelper();
            CN88Builder = new BetListCN88HtmlBuilder();
            Builder = new BetListHTMLBuilder();
            Languages = new LanguageViewModel(language);
        }

        public LanguageViewModel Languages { get; }

        public int CustId { get; }

        public BetListData BetListData { get; }

        public ITicketHelper TicketHelper { get; }

        public IBetListCN88HtmlBuilder CN88Builder { get; }

        public IBetListHTMLBuilder Builder { get; }

        public decimal CompanyPositionTaking(TicketDto ticket)
            => 1 - ticket.AgentPositionTaking - ticket.MasterPositionTaking - ticket.SuperPositionTaking;

        public int Divisor { get; } = 100;
    }
}
