namespace Fanex.Tarus.BetList.Business
{
    using System.Collections.Generic;
    using System.Linq;
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
    using Sunplus.Accounting.Core.Extensions;

    public class AGCasinoService : IBBinService
    {
        private readonly IAGCasinoRepository _agCasinoRepository;

        public AGCasinoService()
            : this(null)
        {
        }

        public AGCasinoService(IAGCasinoRepository agCasinoRepository)
        {
            _agCasinoRepository = agCasinoRepository ?? new AGCasinoRepository();
        }

        public IEnumerable<TicketDetail> BetDetails(GetAGCasinoBetDetailsCriteria criteria)
        {
            var tickets = new List<TicketDetail>();
            var siteDB = ObjectDbExtensions.GetSiteDBConfigByCustId(criteria.CustId);
            if (siteDB != null)
            {
                if (siteDB.IsSiteDB && criteria.WinLossDate > ReportService.BalanceUp.MoveBetsDay)
                {
                    var siteDBCriteria = new GetAGCasinoBetDetailsInSiteDBCriteria
                    {
                        CustId = criteria.CustId,
                        PageIndex = criteria.PageIndex,
                        PageSize = criteria.PageSize,
                        Refno = criteria.Refno,
                        TotalRows = criteria.TotalRows,
                        TransID = criteria.TransID,
                        TransType = criteria.TransType,
                        WinLossDate = criteria.WinLossDate
                    };
                    tickets = _agCasinoRepository.BetDetails(siteDBCriteria, siteDB.AGIP).ToList();
                    criteria.TotalRows = siteDBCriteria.TotalRows;
                }

                if (!siteDB.IsSiteDB ||
                    (siteDB.IsSiteDB && criteria.WinLossDate <= ReportService.BalanceUp.MoveBetsDay))
                {
                    criteria.ExcludeBettrans = siteDB.IsSiteDB;
                    tickets = _agCasinoRepository.BetDetails(criteria).ToList();
                }

                var ticketHelper = new TicketHelper();
                var factory = new BetListHTMLFactory();
                var builder = factory.CreateBuilder();
                var betListHelper = new BetListHelper();
                GetCachePropertyById funcGetOddsTypeName = ticketHelper.GetOddsTypeNameById;

                List<ITicketData> outputLiveCasino = new List<ITicketData>();
                foreach (TicketDetail betDetail in tickets)
                {
                    ticketHelper.IsShowOddsWithChoice = this.IsShowOddsWithChoice(betDetail.Ticket.BetTypeId);
                    betDetail.TransDateHtml = builder.BuildTrans(betDetail.Ticket);
                    betDetail.ChoiceHtml = builder.BuildChoice(betDetail.Ticket, ticketHelper, outputLiveCasino, false);
                    betDetail.StakeHtml = BetListExtension.BuildStake(betDetail, ref builder, ticketHelper);
                    betDetail.OddHtml = builder.BuildOdds(betDetail.Ticket, outputLiveCasino, funcGetOddsTypeName);
                    betDetail.StatusHtml = betListHelper.BuildStatus(betDetail.Ticket, ticketHelper, outputLiveCasino, factory, builder);
                }
            }

            return tickets;
        }

        public IEnumerable<TicketDetail> BetDetails(GetBBinBetDetailsCriteria criteria)
        {
            var detailsTickets = _agCasinoRepository.BetDetails(criteria);
            if (detailsTickets != null)
            {
                ITicketHelper ticketHelper = new TicketHelper();
                BetListHTMLFactory factory = new BetListHTMLFactory();
                BetListHTMLBuilder builder = factory.CreateBuilder();
                BetListHelper betListHelper = new BetListHelper();
                GetCachePropertyById funcGetOddsTypeName = ticketHelper.GetOddsTypeNameById;

                List<ITicketData> outputLiveCasino = new List<ITicketData>();
                foreach (TicketDetail betDetail in detailsTickets)
                {
                    ticketHelper.IsShowOddsWithChoice = this.IsShowOddsWithChoice(betDetail.Ticket.BetTypeId);
                    betDetail.TransDateHtml = builder.BuildTrans(betDetail.Ticket);
                    betDetail.ChoiceHtml = builder.BuildChoice(betDetail.Ticket, ticketHelper, outputLiveCasino, false);
                    betDetail.StakeHtml = BetListExtension.BuildStake(betDetail, ref builder, ticketHelper);
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
