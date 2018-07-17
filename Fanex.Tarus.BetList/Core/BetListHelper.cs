namespace Fanex.Tarus.BetList.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Business;
    using Constants;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Builder.StatusBuilder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.Templates;
    using Fanex.BetList.Core.Utils.StringExt;
    using Models;
    using Resources;
    using Security.Services;
    using Sunplus.Accounting.Core.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListHelper
    {
        private const string CashOutXmlTag = "p25";

        public static readonly Regex IsNumericRegex = new Regex("^(" +
            /*Hex*/ @"0x[0-9a-f]+" + "|" +
            /*Bin*/ @"0b[01]+" + "|" +
            /*Oct*/ @"0[0-7]+" + "|" +
            /*Dec*/ @"((?!0)|[-+])(\d*\.)?\d+(e\d+)?" +
                                                    ")$");

        public static bool IsNumeric(string value)
        {
            return IsNumericRegex.IsMatch(value);
        }

        public bool IsShowCommission(TicketDetail ticketDetail)
        {
            bool isShowCommision = true;
            switch (ticketDetail.Ticket.BetTypeId)
            {
                case 500:
                    isShowCommision = true;
                    break;

                case 501:
                    isShowCommision = !IsIMBCMatch(ticketDetail);
                    break;

                default:
                    isShowCommision = true;
                    break;
            }

            return isShowCommision;
        }

        public bool IsIMBCMatch(TicketDetail ticketDetail)
        {
            return ticketDetail.Ticket.SuperDiscount < 0 || ticketDetail.Ticket.MasterDiscount < 0 || ticketDetail.Ticket.AgentDiscount < 0;
        }

        public static bool IsMetricGamingBetType(int betType)
        {
            return BetTypeContants.MetricGaming60sofKick <= betType &&
                betType <= BetTypeContants.MetricGamingEndBetType;
        }

        public static bool IsColossusBetType(int betType)
        {
            return betType == BetTypeContants.ColossusBetsSummary ||
                betType == BetTypeContants.ColossusBetsPoolBet ||
                betType == BetTypeContants.ColossusBetsCashOut ||
                betType == BetTypeContants.ColossusBetsWinningShare ||
                betType == BetTypeContants.ColossusBetsConsolationPrize ||
                betType == BetTypeContants.ColossusBetsJackpotPrize ||
                betType == BetTypeContants.ColossusBetsBonusPrize;
        }

        public static bool IsCockFightingBetType(int betType)
        {
            return betType == BetTypeContants.CockFighting;
        }

        public string IsNegative(decimal input)
        {
            return input < 0 ? "negative" : "positive";
        }

        public bool IsHRWinPlaceBettype(int betType)
        {
            return betType == BetTypeContants.WinPlace ||
                betType == BetTypeContants.WinPlaceUKTote ||
                betType == BetTypeContants.VirtualSportWinPlace;
        }

        public bool IsHRFixOdds(int betType)
        {
            return betType == BetTypeContants.WinUKTote ||
              betType == BetTypeContants.PlaceUKTote ||
              betType == BetTypeContants.WinPlaceUKTote;
        }

        public bool IsHRBettype(int betType)
        {
            return betType == BetTypeContants.Win ||
                betType == BetTypeContants.Place ||
                betType == BetTypeContants.WinPlace ||
                betType == BetTypeContants.WinUKTote ||
                betType == BetTypeContants.PlaceUKTote ||
                betType == BetTypeContants.WinPlaceUKTote;
        }

        public string PercentFormat(decimal input)
        {
            var isInputInterger = (input * 100) % 1 == 0;
            return isInputInterger ?
                Math.Round(input * 100, 0).ToString() + BetListResources.Symbol_Percent :
                Math.Round(input * 100, 2).ToString() + BetListResources.Symbol_Percent;
        }

        public bool IsShowOddsWithChoice(int betTypeId)
        {
            return betTypeId == BetTypeContants.MixParlay || betTypeId == BetTypeContants.SystemParlay;
        }

        public string BuildStatus(
            ITicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData,
            BetListHTMLFactory htmlFactory = null,
            BetListHTMLBuilder builder = null)
        {
            htmlFactory = htmlFactory ?? new BetListHTMLFactory();
            builder = builder ?? htmlFactory.CreateBuilder();

            IStatus statusBuilder = builder.CreateStatusBuilder(ticket.BetTypeId);
            Status_Template statusBlock = statusBuilder.Render(ticket, ticketHelper, ticketData, true);

            if (ticket.SportTypeId == SportContants.MicroGamingLiveCasino ||
                ticket.SportTypeId == SportContants.MicroGamingRNGCasino)
            {
                statusBlock.StatusResult.Clear();
                statusBlock.StatusResult.Append(BuildMicroGamingResult((int)ticket.CustId, ticket.BetCheck));
            }

            string cashOutDetailsLink = CheckCashOutAndBuildDetails(ticket);

            if (!string.IsNullOrEmpty(cashOutDetailsLink))
            {
                statusBlock.CashOutHistory.Clear();
                statusBlock.CashOutHistory.Append(cashOutDetailsLink);
            }

            statusBlock.ShowIP = null;

            return statusBlock.ToString();
        }

        private string BuildMicroGamingResult(int custId, string betCheck)
        {
            IBetListService betListService = new BetListService();
            string siteName = betListService.GetSiteNameByCustId(custId);

            return string.Format(
                "<div class=\"result\" onclick=\"ViewMicroGamingPopup({0},'{1}','{2}');\">{3}</div>",
                custId,
                siteName,
                betCheck,
                BetListResources.results);
        }

        #region For Cash Out

        private string CheckCashOutAndBuildDetails(ITicket ticket)
        {
            string title = string.Empty;

            if (ticket.BetTypeId == BetTypeContants.CashOut)
            {
                title = BetListResources.CashOut;
            }
            else if (ticket.BetTypeId == BetTypeContants.CashOutBought)
            {
                title = BetListResources.Bought;
            }
            else if (IsCashOutOriginalTicket(ticket))
            {
                string upperStatus = ticket.Status.ToUpperInvariant();

                if (upperStatus != "VOID" && upperStatus != "REFUND")
                {
                    title = BetListResources.CashOut;
                }
            }
            else if (IsCashOutTicket(ticket))
            {
                title = ticket.BetdaqId == 1 ? BetListResources.Bought : BetListResources.CashOut;
            }

            if (title.Length > 0)
            {
                return BuildCashOutResult(ticket, title);
            }

            return title;
        }

        private bool IsCashOutOriginalTicket(ITicket ticket)
        {
            return ticket.BetdaqId == 0 && ticket.Stake == 0 && HasCashOutExtraInfo(ticket.ExtraInfo);
        }

        private bool IsCashOutTicket(ITicket ticket)
        {
            return ticket.BetdaqId == 1 || HasCashOutExtraInfo(ticket.ExtraInfo);
        }

        private bool HasCashOutExtraInfo(string extraInfo)
        {
            return extraInfo.HasAttributeInXmlTag(CashOutXmlTag, "s");
        }

        private string BuildCashOutResult(ITicket ticket, string title)
        {
            return string.Format(
                "<div class=\"result\" onclick=\"ViewCashOutDetails({0},{1},{2},{3},'{4}');\">{5}</div>",
                ticket.TransId,
                ticket.RefNo,
                ticket.BetTypeId,
                ticket.BetdaqId,
                title,
                BetListResources.Details);
        }

        #endregion

        public static byte GetSiteTypeParamValue(int userId)
        {
            var permissionDictionary = PermissionService.GetCreditAndDepositPolicy(userId);

            if (permissionDictionary.Count != 0)
            {
                var hasCreditSitePermission = permissionDictionary.ContainsKey(Enumerations.Functions.CreditSiteFuncName);
                var hasDepositSitePermission = permissionDictionary.ContainsKey(Enumerations.Functions.DepositSiteFuncName);

                if (hasCreditSitePermission && hasDepositSitePermission)
                {
                    return 0; // ALL
                }

                if (hasCreditSitePermission)
                {
                    return 1; // CREDIT
                }

                if (hasDepositSitePermission)
                {
                    return 2; // DEPOSIT
                }
            }

            return byte.MaxValue;
        }
    }
}