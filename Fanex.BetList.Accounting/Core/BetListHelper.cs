// --------------------------------------------------------------------------
// <copyright file="BetListHelper.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.11.22">Created</change>
// </history>

namespace Fanex.BetList.Accounting.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;
    using Fanex.BetList.Accounting.Constants;
    using Fanex.BetList.Accounting.Extensions;
    using Fanex.BetList.Accounting.Models;
    using Fanex.BetList.Accounting.Resources;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Tarus.Infrastructure.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListHelper
    {
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

        public static bool IsUseRefNoInsteadOfTransId(ITicket ticket)
        {
            return ticket.BetTypeId == 73 ||
                ticket.BetTypeId == 71 ||
                ticket.SportTypeId == SportContants.Keno ||
                ticket.SportTypeId == 255 ||
                ticket.SportTypeId == SportContants.MicroGamingLiveCasino ||
                IsMetricGamingBetType(ticket.BetTypeId) ||
                IsColossusBetType(ticket.BetTypeId);
                IsCockFightingBetType(ticket.BetTypeId);
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

        public static bool IsHideResultLink(int sportType)
        {
            return sportType == SportContants.Keno ||
                sportType == SportContants.CockFighting;
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

        public bool IsVirtualHRBettype(int betType)
        {
            return betType == BetTypeContants.VirtualSportWin ||
                betType == BetTypeContants.VirtualSportPlace ||
                betType == BetTypeContants.VirtualSportWinPlace;
        }

        public bool IsBetTradeBettype(int betType)
        {
            return betType == BetTypeContants.BetTradeBought ||
                betType == BetTypeContants.BetTradeSold ||
                betType == BetTypeContants.BetTradeCommision;
        }

        public bool IsRNGCasino(int sportType)
        {
            return sportType == SportContants.RNG_RNGGame ||
                sportType == SportContants.RNG_MiniGame ||
                sportType == SportContants.RNG_Mobile;
        }

        public string PercentFormat(decimal input)
        {
            var isInputInterger = (input * 100) % 1 == 0;
            return isInputInterger ?
                Math.Round(input * 100, 0).ToString() + Labels.Symbol_Percent :
                Math.Round(input * 100, 2).ToString() + Labels.Symbol_Percent;
        }

        public string GetBetTradeStatus(TicketDetail betDetail)
        {
            var result = CacheRef.GetBettype(betDetail.Ticket.BetTypeId);
            switch (betDetail.Ticket.BetTypeId)
            {
                case BetTypeContants.BetTradeBought:
                    result = BetListResources.Bought;
                    break;
                case BetTypeContants.BetTradeSold:
                    result = BetListResources.Sold;
                    break;
                case BetTypeContants.BetTradeCommision:
                    result = BetListResources.Comm;
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            var status = betDetail.Ticket.Status.ToLower();
            if (status.Equals("void") || status.Equals("refund"))
            {
                result = string.Format(
                    "{0}<br/><div style='font-weight: normal;'>({1})</div>",
                    betDetail.Ticket.Status,
                    result);
            }

            return "<div class=\"bl_status c\">" + result + "</div>";
        }

        public string GetRNGCasinoStatus(TicketDetail ticketDetail)
        {
            var statusLabel = string.Empty;
            var status = ticketDetail.Ticket.Status.ToLower();
            var isRunningStatus = string.Equals(status, "running", StringComparison.OrdinalIgnoreCase);
            bool isWonStatus = string.Equals(status, "won", StringComparison.OrdinalIgnoreCase);
            bool isLoseStatus = string.Equals(status, "lose", StringComparison.OrdinalIgnoreCase);
            bool isDrawStatus = string.Equals(status, "draw", StringComparison.OrdinalIgnoreCase);

            if (isRunningStatus && !string.IsNullOrEmpty(ticketDetail.Ticket.BetCheck))
            {
                var viewBrokenGame = "ViewRNGCasinoBrokenGame(" +
                    ticketDetail.Ticket.MatchId + "," +
                    (string.IsNullOrEmpty(ticketDetail.Ticket.Race) ? "0" : ticketDetail.Ticket.Race) + "," +
                    ticketDetail.Ticket.BetTypeId + "," +
                    ticketDetail.Ticket.SportTypeId + "," +
                    "'" + (ticketDetail.Ticket.BetCheck ?? string.Empty) + "'," +
                    "'" + ticketDetail.Ticket.WinlostDate.ToShortFormatedDate() + "')";
                statusLabel = "<div id=\"rngcasino_brokengame_" + ticketDetail.Ticket.TransId + "\" onclick=\"" + viewBrokenGame + "\" class=\"viewbrokengame-link\">" + status.ToUpper() + "</div>";
            }
            else
            {
                statusLabel = "<div class=\"bl_status c\">" + status.ToUpper() + "</div>";
            }

            var resultLink = string.Empty;
            if (isWonStatus || isLoseStatus || isDrawStatus)
            {
                resultLink = string.IsNullOrEmpty(ticketDetail.Ticket.BetCheck) ?
                     string.Empty :
                     "ViewRNGCasinoResult(" +
                     ticketDetail.Ticket.MatchId + "," +
                     (string.IsNullOrEmpty(ticketDetail.Ticket.Race) ? "0" : ticketDetail.Ticket.Race) + "," +
                     ticketDetail.Ticket.BetTypeId + "," +
                     ticketDetail.Ticket.SportTypeId + "," +
                     "'" + (ticketDetail.Ticket.BetCheck ?? string.Empty) + "'," +
                     "\'" + ticketDetail.Ticket.WinlostDate.ToShortFormatedDate() + "\')";
            }

            var resultLink1 = string.IsNullOrEmpty(resultLink) ?
                 string.Empty :
                 "<br />" +
                 "<a id=\"result_" + ticketDetail.Ticket.TransId + "\" onclick=\"" + resultLink + "\">" + Labels.Result + "</a>" +
                 "<br />";
            return statusLabel + resultLink1;
        }

        /// <summary>
        /// Gets the status reference no.
        /// </summary>
        /// <param name="ticketDetail">The ticket detail.</param>
        /// <returns>The reference no.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public string StatusRefNo(TicketDetail ticketDetail)
        {
            BetListHelper betlistHelper = new BetListHelper();
            if (betlistHelper.IsBetTradeBettype(ticketDetail.Ticket.BetTypeId))
            {
                return betlistHelper.GetBetTradeStatus(ticketDetail);
            }

            if (IsRNGCasino(ticketDetail.Ticket.SportTypeId))
            {
                return betlistHelper.GetRNGCasinoStatus(ticketDetail);
            }

            string status = ticketDetail.Ticket.Status.ToLower();
            int _liveAwayScore = ticketDetail.Ticket.LiveAwayScore;
            int _liveHomeScore = ticketDetail.Ticket.LiveHomeScore;
            string statusLabel = string.Empty;
            bool isWonStatus = string.Equals(status, "won", StringComparison.OrdinalIgnoreCase);
            bool isLoseStatus = string.Equals(status, "lose", StringComparison.OrdinalIgnoreCase);
            bool isDrawStatus = string.Equals(status, "draw", StringComparison.OrdinalIgnoreCase);

            if (betlistHelper.IsHRWinPlaceBettype(ticketDetail.Ticket.BetTypeId) &&
                (isWonStatus || isLoseStatus))
            {
                var _homeTemp = BetListExtension.GetScoreStatusName(_liveHomeScore);
                var _placeTemp = BetListExtension.GetScoreStatusName(_liveAwayScore);
                statusLabel =
                    "<div>" +
                        "<span style=\"color: #2673CE;\">" + Labels.Win + ": </span>" +
                        "<span class=\"positive\">" + _homeTemp + "</span>" +
                    "</div>" +
                    "<div class=\"margin-top-5\">" +
                        "<span>" + Labels.Place + ": </span>" +
                        "<span>" + _placeTemp + "</span>" +
                     "</div>";
            }
            else
            {
                statusLabel = "<div class=\"bl_status c\">" + status.ToUpper() + "</div>";
            }

            if (IsHideResultLink(ticketDetail.Ticket.SportTypeId))
            {
                return statusLabel;
            }

            string resultLink = string.Empty;
            if (betlistHelper.IsHRBettype(ticketDetail.Ticket.BetTypeId))
            {
                if (isWonStatus || isLoseStatus)
                {
                    int horseRacingBettype = 0;
                    int height = 200;
                    if (betlistHelper.IsHRFixOdds(ticketDetail.Ticket.BetTypeId))
                    {
                        height = 250;
                        horseRacingBettype = BetTypeContants.Handicap;
                    }

                    string hrefRaceResult = "Report/HRRaceResult.aspx?" +
                        "leagueid=" + ticketDetail.Ticket.LeagueId +
                        "&raceno=" + ticketDetail.Ticket.Race +
                        "&eventdate=" + ticketDetail.Ticket.EventDate.ToShortDateString() +
                        "&HRBettype=" + horseRacingBettype.ToString();

                    resultLink = "openHRresult('" + hrefRaceResult + "','Race_Result',300," + height + ")";
                }
            }
            else if (betlistHelper.IsVirtualHRBettype(ticketDetail.Ticket.BetTypeId))
            {
                if (isWonStatus || isLoseStatus)
                {
                    resultLink = "virtualHRPopup(" + ticketDetail.Ticket.SportTypeId + "," +
                     ticketDetail.Ticket.MatchId + "," +
                     ticketDetail.Ticket.Race + ")";
                }
            }
            else if (ticketDetail.Ticket.BetTypeId.Equals(500))
            {
                resultLink = string.Empty;
            }
            else
            {
                if ((isWonStatus || isLoseStatus || isDrawStatus) &&
                    !IsColossusBetType(ticketDetail.Ticket.BetTypeId))
                {
                    int sportTypeId = ticketDetail.Ticket.SportTypeId;
                    switch (ticketDetail.Ticket.SportTypeId)
                    {
                        case SportContants.Politics:
                            resultLink = string.Empty;
                            break;
                        case SportContants.NumberGame:
                            resultLink = "ViewResultNumberGame('Betlist/BetListNumberGameResult.aspx?" +
                                "matchId=" + ticketDetail.Ticket.MatchId.ToString() +
                                "')";
                            break;
                        case SportContants.LiveCasino:
                            resultLink = "ViewResultLiveCasino('LiveCasino/Betlist/BetListLiveCasinoResult.aspx?" +
                                "matchId=" + ticketDetail.Ticket.MatchId.ToString() +
                                "&bettype=" + ticketDetail.Ticket.BetTypeId +
                                "&league=" + CacheRef.GetLeagueName(ticketDetail.Ticket.LeagueId) +
                                "')";
                            break;
                        case SportContants.VirtualTennis:
                            resultLink = "virtualHRPopup(" + ticketDetail.Ticket.SportTypeId + "," +
                                ticketDetail.Ticket.MatchId + ")";
                            break;
                        case SportContants.Tennis:
                            resultLink = "ViewTennisPopup(" + ticketDetail.Ticket.MatchId + "," +
                                ticketDetail.Ticket.BetTypeId + "," +
                                ticketDetail.Ticket.BetId + ",'" +
                                CacheRef.GetLeagueName(ticketDetail.Ticket.LeagueId) + "')";
                            break;
                        case SportContants.MicroGamingLiveCasino:
                            resultLink = "ViewMicroGamingPopup(" + AppSession.CustID + "," +
                                "'" + AppSession.SiteName + "'," +
                                ticketDetail.Ticket.BetCheck + ")";
                            break;
                        case SportContants.MicroGamingRNGCasino:
                            resultLink = string.IsNullOrEmpty(ticketDetail.Ticket.TransDesc) ?
                                string.Empty :
                                "ViewMicroGamingPopup(" + AppSession.CustID + "," +
                                "'" + AppSession.SiteName + "'," +
                                ticketDetail.Ticket.BetCheck + ")";
                            break;
                        case SportContants.AGCasino:
                            resultLink = string.IsNullOrEmpty(ticketDetail.Ticket.TransDesc) ?
                                string.Empty :
                                "ViewAGCasinoPopup(" + ticketDetail.Ticket.TransId + "," + ticketDetail.Ticket.BetTypeId + ")";
                            break;
                        default:
                            switch (ticketDetail.Ticket.BetTypeId)
                            {
                                case BetTypeContants.SystemParlay:
                                    resultLink = "ViewCombinationMixParlayResult(" + ticketDetail.Ticket.RefNo + "," +
                                        ticketDetail.Ticket.MatchId.ToString() + "," +
                                        ticketDetail.Ticket.BetTypeId + "," +
                                        (ticketDetail.Ticket.BetId == null ?
                                        0 :
                                        ticketDetail.Ticket.BetId) + ")";
                                    break;

                                case BetTypeContants.MixParlay:
                                    resultLink = "ViewMixParlayResult(" + ticketDetail.Ticket.RefNo + "," +
                                        ticketDetail.Ticket.MatchId.ToString() + "," +
                                        ticketDetail.Ticket.BetTypeId + "," +
                                        ticketDetail.Ticket.BetId + ")";
                                    break;

                                case BetTypeContants.Outright:
                                case BetTypeContants.CasinoGames:
                                case BetTypeContants.P2P:
                                case BetTypeContants.Bingo:
                                case BetTypeContants.P2PGames:
                                    resultLink = string.Empty;
                                    break;
                                case BetTypeContants.GoldDeluxeBaccarat:
                                case BetTypeContants.GoldDeluxeParlayBaccarat:
                                case BetTypeContants.GoldDeluxeRoulette:
                                    resultLink = "ViewGoldDeluxeResult(" + ticketDetail.Ticket.TransId + ", " + ticketDetail.Ticket.BetTypeId.ToString() + ")";
                                    break;
                                default:
                                    if (BetListHelper.IsMetricGamingBetType(ticketDetail.Ticket.BetTypeId))
                                    {
                                        resultLink = "ViewResultMetricGaming(" +
                                            ticketDetail.Ticket.MatchId.ToString() + "," +
                                            ticketDetail.Ticket.BetTypeId + "," +
                                            ticketDetail.Ticket.BetId + "," +
                                            ticketDetail.Ticket.SportTypeId + "," +
                                            "'" + CacheRef.GetSportName(ticketDetail.Ticket.SportTypeId) + "'" + "," +
                                            "'" + CacheRef.GetLeagueName(ticketDetail.Ticket.LeagueId) + "'" + ")";
                                    }
                                    else
                                    {
                                        resultLink = "ViewResult(" + ticketDetail.Ticket.MatchId.ToString() + "," +
                                            ticketDetail.Ticket.BetTypeId + "," +
                                            ticketDetail.Ticket.BetId + ")";
                                    }

                                    break;
                            }

                            break;
                    }
                }
                else
                {
                    resultLink = string.Empty;
                }
            }

            var resultLink1 = string.IsNullOrEmpty(resultLink) ?
                string.Empty :
                "<br />" +
                "<a id=\"result_" + ticketDetail.Ticket.TransId + "\" onclick=\"" + resultLink + "\">" + Labels.Result + "</a>" +
                "<br />";
            return statusLabel + resultLink1;
        }

        /// <summary>
        /// Gets the status statement.
        /// </summary>
        /// <param name="ticketDetail">The ticket detail.</param>
        /// <returns>The status statement.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public string StatusStatement(TicketDetail ticketDetail)
        {
            BetListHelper betlistHelper = new BetListHelper();
            if (betlistHelper.IsBetTradeBettype(ticketDetail.Ticket.BetTypeId))
            {
                return betlistHelper.GetBetTradeStatus(ticketDetail);
            }

            if (IsRNGCasino(ticketDetail.Ticket.SportTypeId))
            {
                return betlistHelper.GetRNGCasinoStatus(ticketDetail);
            }

            if (IsRNGCasino(ticketDetail.Ticket.SportTypeId))
            {
                return betlistHelper.GetRNGCasinoStatus(ticketDetail);
            }

            string statusHtml = string.Empty;
            int betTypeId = ticketDetail.Ticket.BetTypeId;
            int sportTypeId = ticketDetail.Ticket.SportTypeId;
            string status = ticketDetail.Ticket.Status;
            string finalHomeScore = ticketDetail.FinalHomeScore;
            string finalAwayScore = ticketDetail.FinalAwayScore;
            string halfTimeHomeScore = ticketDetail.HTHomeScore;
            string halfTimeAwayScore = ticketDetail.HTAwayScore;
            string statusFormat = string.Equals(status, "reject", StringComparison.OrdinalIgnoreCase) ||
              string.Equals(status, "refund", StringComparison.OrdinalIgnoreCase) ||
              string.Equals(status, "void", StringComparison.OrdinalIgnoreCase) ?
              "<span style=\"color:red;\">" + status.ToUpper() + "</span>" :
              status;
            bool isWonStatus = string.Equals(status, "won", StringComparison.OrdinalIgnoreCase);
            bool isLoseStatus = string.Equals(status, "lose", StringComparison.OrdinalIgnoreCase);
            bool isDrawStatus = string.Equals(status, "draw", StringComparison.OrdinalIgnoreCase);

            if (sportTypeId <= 42 ||
                sportTypeId == SportContants.OtherSports ||
                sportTypeId == SportContants.ESport)
            {
                switch (betTypeId)
                {
                    case BetTypeContants.CasinoGames:
                    case BetTypeContants.RNGCasinoBonus:
                    case BetTypeContants.Bingo:
                    case 113:
                    case 114:
                    case 112:
                    case BetTypeContants.MixParlay:
                    case BetTypeContants.P2PGames:
                    case BetTypeContants.Outright:
                        statusHtml = statusFormat;
                        break;
                    case BetTypeContants.SystemParlay:
                        statusHtml = status;
                        break;
                    default:
                        if (33 <= sportTypeId && sportTypeId <= 42)
                        {
                            statusHtml = string.Format(
                                "{0}@{1}-{2}",
                                statusFormat,
                                finalHomeScore,
                                finalAwayScore);
                        }
                        else
                        {
                            statusHtml =
                                string.Format("{0}@{1}-{2}",
                                                statusFormat,
                                                finalHomeScore,
                                                finalAwayScore) +
                                "<br />" +
                                string.Format("[{0}:{1}-{2}]",
                                                Labels.HT,
                                                halfTimeHomeScore,
                                                halfTimeAwayScore);
                        }

                        break;
                }
            }
            else if (betlistHelper.IsHRWinPlaceBettype(betTypeId) &&
                (isWonStatus || isLoseStatus))
            {
                string winStatus = BetListExtension.GetScoreStatusName(ticketDetail.Ticket.LiveHomeScore);
                string placeStatus = BetListExtension.GetScoreStatusName(ticketDetail.Ticket.LiveAwayScore);
                string winStatusHtml =
                    "<div>" +
                        "<span style='color: #2673CE;'>" + Labels.Win + ": </span>" +
                        "<span class='positive'>" + winStatus + "</span>" +
                    "</div>";
                string placeStatusHtml =
                    "<div class='margin-top-5'>" +
                        "<span>" + Labels.Place + ": </span>" +
                        "<span>" + placeStatus + "</span>" +
                    "<div>";
                statusHtml = winStatusHtml + placeStatusHtml;
            }
            else
            {
                switch (betTypeId)
                {
                    case 1201:
                    case 1203:
                    case 1204:
                    case 1205:
                    case 1206:
                    case 1220:
                    case 1224:
                    case 1235:
                    case 1236:
                        statusHtml = string.Format(
                                        "{0}@{1}-{2}",
                                        statusFormat,
                                        finalHomeScore,
                                        finalAwayScore) +
                                    "<br />" +
                                    string.Format(
                                        "[{0}:{1}-{2}]",
                                        Labels.HT,
                                        halfTimeHomeScore,
                                        halfTimeAwayScore);
                        break;
                    default:
                        statusHtml = status;
                        break;
                }
            }

            if (IsHideResultLink(ticketDetail.Ticket.SportTypeId))
            {
                return statusHtml;
            }

            string resultLink = string.Empty;
            if (betlistHelper.IsHRBettype(ticketDetail.Ticket.BetTypeId))
            {
                if (isWonStatus || isLoseStatus)
                {
                    int horseRacingBetType = 0;
                    int height = 200;
                    if (betlistHelper.IsHRFixOdds(ticketDetail.Ticket.BetTypeId))
                    {
                        height = 250;
                        horseRacingBetType = BetTypeContants.Handicap;
                    }

                    string hrefRaceResult = "Report/HRRaceResult.aspx?" +
                        "leagueid=" + ticketDetail.Ticket.LeagueId +
                        "&raceno=" + ticketDetail.Ticket.Race +
                        "&eventdate=" + ticketDetail.Ticket.EventDate.ToShortDateString() +
                        "&HRBettype=" + horseRacingBetType.ToString();

                    resultLink = "openHRresult('" + hrefRaceResult + "','Race_Result',300," + height + ")";
                }
            }
            else if (betlistHelper.IsVirtualHRBettype(ticketDetail.Ticket.BetTypeId))
            {
                if (isWonStatus || isLoseStatus)
                {
                    resultLink = "virtualHRPopup(" + ticketDetail.Ticket.SportTypeId + "," +
                        ticketDetail.Ticket.MatchId + "," +
                        ticketDetail.Ticket.Race + ")";
                }
            }
            else
            {
                if (isWonStatus ||
                   isLoseStatus ||
                   isDrawStatus)
                {
                    switch (ticketDetail.Ticket.SportTypeId)
                    {
                        case SportContants.NumberGame:
                            resultLink = "ViewResultNumberGame('Betlist/BetListNumberGameResult.aspx?" +
                                "matchId=" + ticketDetail.Ticket.MatchId.ToString() + "')";
                            break;
                        case SportContants.LiveCasino:
                            resultLink = "ViewResultLiveCasino('LiveCasino/Betlist/BetListLiveCasinoResult.aspx?" +
                                 "matchId=" + ticketDetail.Ticket.MatchId.ToString() +
                                 "&bettype=" + ticketDetail.Ticket.BetTypeId +
                                 "&league=" + CacheRef.GetLeagueName(ticketDetail.Ticket.LeagueId) +
                                 "')";
                            break;
                        case SportContants.Politics:
                        case SportContants.VirtualTennis:
                            break;
                        case SportContants.GoldDeluxe:
                            resultLink = "\"ViewGoldDeluxeResult(" + ticketDetail.Ticket.TransId + ", " + ticketDetail.Ticket.BetTypeId.ToString() + ")\"";
                            break;
                        case SportContants.MicroGamingLiveCasino:
                            resultLink = string.IsNullOrEmpty(ticketDetail.Ticket.TransDesc) ?
                                string.Empty :
                                "ViewMicroGamingPopup(" + AppSession.CustID + "," +
                                "'" + AppSession.SiteName + "'," +
                                ticketDetail.Ticket.BetCheck + ")";
                            break;
                        case SportContants.MicroGamingRNGCasino:
                            resultLink = string.IsNullOrEmpty(ticketDetail.Ticket.TransDesc) ?
                                string.Empty :
                                "ViewMicroGamingPopup(" + AppSession.CustID + "," +
                                "'" + AppSession.SiteName + "'," +
                                ticketDetail.Ticket.BetCheck + ")";
                            break;
                        case SportContants.AGCasino:
                            resultLink = string.IsNullOrEmpty(ticketDetail.Ticket.TransDesc) ?
                                string.Empty :
                                "ViewAGCasinoPopup(" + ticketDetail.Ticket.TransId + "," + ticketDetail.Ticket.BetTypeId + ")";
                            break;
                        default:
                            switch (ticketDetail.Ticket.BetTypeId)
                            {
                                case BetTypeContants.SystemParlay:
                                    resultLink = "ViewCombinationMixParlayResult(" + ticketDetail.Ticket.RefNo + "," +
                                        ticketDetail.Ticket.MatchId.ToString() + "," +
                                        ticketDetail.Ticket.BetTypeId + "," +
                                        ticketDetail.Ticket.BetId + ")";
                                    break;
                                case BetTypeContants.MixParlay:
                                    resultLink = "ViewMixParlayResult(" + ticketDetail.Ticket.RefNo + "," +
                                        ticketDetail.Ticket.MatchId.ToString() + "," +
                                        ticketDetail.Ticket.BetTypeId + "," +
                                        ticketDetail.Ticket.BetId + ")";
                                    break;
                                default:
                                    resultLink = string.Empty;
                                    break;
                            }

                            break;
                    }
                }
                else
                {
                    resultLink = string.Empty;
                }
            }

            var resultLink1 = string.IsNullOrEmpty(resultLink) ?
                string.Empty :
                "<br />" +
                "<a id=\"result_" + ticketDetail.Ticket.TransId + "\" onclick=" + resultLink + ">" + Labels.Result + "</a>" +
                "<br />";
            return statusHtml + resultLink1;
        }

        public bool IsShowOddsWithChoice(int betTypeId)
        {
            return betTypeId == BetTypeContants.MixParlay || betTypeId == BetTypeContants.SystemParlay;
        }
    }
}
