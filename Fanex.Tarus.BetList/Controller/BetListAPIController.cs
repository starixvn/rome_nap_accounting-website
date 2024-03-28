namespace Fanex.Tarus.BetList.Controllers
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Business;
    using Core;
    using Fanex.BetDetails.BetBuilder.Criteria;
    using Fanex.BetDetails.BetBuilder.Services;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Common;
    using Fanex.Tarus.BetList.Authentication;
    using Fanex.Tarus.BetList.Constants;
    using Fanex.Tarus.BetList.DataAccess;
    using Fanex.Tarus.BetList.Models;
    using Fanex.Tarus.BetList.Param;

    [ApiAuthorization]
    public class BetListApiController : Controller
    {
        private readonly IBetListService _betListService;
        private readonly IBetBuilderService _betBuilderService;

        public BetListApiController()
        {
            _betListService = new BetListService();
            _betBuilderService = new BetBuilderService(new BetBuilderRepository());
        }

        public ActionResult GetBetListByRefNo(BetListByRefNoParam param)
        {
            if (param != null && !string.IsNullOrEmpty(param.RefNo) && BetListHelper.IsNumeric(param.RefNo.Trim()))
            {
                param.UserId = Converter.To<int>(ConfigurationManager.AppSettings["BetListApi_UserId"]);
                var result = _betListService.GetBetListByRefNo(param);
                _betListService.AssignCountry(result);

                var firstTicket = result.FirstOrDefault();
                if (firstTicket != null)
                {
                    var ticket = firstTicket.Ticket;
                    ITicketHelper ticketHelper = new TicketHelper();
                    ticketHelper.ShowSystemParlayDetail = false;
                    BetListHTMLFactory factory = new BetListHTMLFactory();
                    BetListHTMLBuilder builder = factory.CreateBuilder();
                    List<ITicketData> outputLiveCasino = new List<ITicketData>();

                    var outPut = new StringBuilder();
                    var choiceHtml = new StringBuilder();
                    var choiceMainTicket = builder.BuildChoice(ticket, ticketHelper, outputLiveCasino, isShowScoreMap: false);
                    if (ticket.BetTypeId == BetTypeContants.SystemParlay ||
                        ticket.BetTypeId == BetTypeContants.MixParlayVirtualSports2 ||
                        ticket.BetTypeId == BetTypeContants.MixParlayEdgeGamingVS4)
                    {
                        var partern = $"<a id=\"hidden0\" [^>]*>(.+?)</a>";
                        var mainTicket = Regex.Match(choiceMainTicket, partern);
                        if (mainTicket.Success)
                        {
                            choiceHtml.Append(mainTicket.Value);
                        }

                        var ticketDetail = GetMixParlayDetail(ticket.TransId, ticket.RefNo, ticket.Handicap1, ticket.Handicap2, ticket.BetCheck, ticket.BetTeam, ticket.CustId);
                        if (!string.IsNullOrEmpty(ticketDetail.CombinationTypeComboNames))
                        {
                            choiceHtml.Append(ticketDetail.CombinationTypeComboNames);
                            choiceHtml.Append("<div class=\"line\"></div>");
                        }

                        foreach (var t in ticketDetail.Tickets)
                        {
                            choiceHtml.Append(t.ChoiceHtml);
                        }
                    }
                    else
                    {
                        choiceHtml.Append(firstTicket.ChoiceHtml);
                        if (ticket.BetTypeId == BetTypeContants.BetBuilder)
                        {
                            var betBuilder = GetBetBuilderDetail(ticket.TransId, ticket.CustId, ticket.Status);
                            var detailSection = $"<div id=\"divEvent_{ticket.TransId}{ticketHelper.Index}\" style=\"display: none\" ></div>";
                            choiceHtml = choiceHtml.Replace(detailSection, betBuilder);
                        }
                    }

                    var odds = $"{builder.BuildOddsValue(ticket, null, ticketHelper.GetOddsTypeNameById)} {builder.BuildOddsTypeName(ticket, null, ticketHelper.GetOddsTypeNameById)}";
                    outPut.AppendFormat("{0}</br>{1}",
                        builder.BuildTrans(ticket),
                        RemoveDetailLink(choiceHtml.ToString()));

                    return Json(new ResponseBetListApi("0", outPut.ToString(), odds), JsonRequestBehavior.AllowGet);
                }

                return Json(new ResponseBetListApi("020", "No information is available", string.Empty), JsonRequestBehavior.AllowGet);
            }

            return Json(new ResponseBetListApi("010", "RefNo is invalid", string.Empty), JsonRequestBehavior.AllowGet);
        }

        private ParlayTicket GetMixParlayDetail(long transId, string refNo, decimal? hdp1, decimal? hdp2, string betCheck, string betTeam, long custId)
        {
            var result = _betListService.GetMixParlayTickets(transId, Converter.To<long>(refNo), (int)custId, removeSabaWord: true);
            var combinationTypeComboNames = string.IsNullOrEmpty(betCheck) ?
                string.Empty :
                _betListService.GetCombinationTypeComboNames(betTeam, betCheck, hdp1.Value, hdp2.Value, result);

            return new ParlayTicket { CombinationTypeComboNames = combinationTypeComboNames, Tickets = result };
        }

        private string GetBetBuilderDetail(long transactionId, long customerId, string status)
        {
            var criteria = new GetBetBuilderDetailParameter { CustomerId = customerId, TransactionId = transactionId, Status = status };
            var result = _betBuilderService.GetBetListDetail(criteria);
            var html = new StringBuilder();
            html.Append("<div>");
            foreach (var item in result.Details)
            {
                html.Append("<div>" + item.SelectionName);
                if (result.IsSettled && !string.IsNullOrEmpty(item.Result))
                {
                    html.Append("<span class='sport'> - " + item.Result + "</span>");
                }
            }
            html.Append("</div>");

            return html.ToString();
        }

        private string RemoveDetailLink(string choiceHtml)
        {
            var partern = $"<div class=\"detail[^>]*>(.+?)</div>";

            return Regex.Replace(choiceHtml, partern, string.Empty);
        }
    }

    public class ParlayTicket
    {
        public string CombinationTypeComboNames { get; set; }

        public IList<TicketDetail> Tickets { get; set; }
    }

    public class ResponseBetListApi
    {
        public ResponseBetListApi(string code, string description, string odds)
        {
            ErrorCode = code;
            Description = description;
            Odds = odds;
        }

        public string ErrorCode { get; set; }

        public string Description { get; set; }

        public string Odds { get; set; }
    }
}