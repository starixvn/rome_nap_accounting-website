using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.Entities.Report;
using DNAide.Utils;
using IBCUtils;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Core.Cache;

namespace Sunplus.Accounting.Business.Report.Other
{
    public class TraderPerfomanceManager : ITraderPerformanceManager
    {
        private const string TraderPerformanceIsSoccerSessionKey = "traderperformanceissoccerkey";
        private const string TraderPerformanceIsNotSoccerSessionKey = "traderperformanceisnotsoccerkey";
        private const string TraderPerformanceDetailIsSoccerSessionKey = "traderperformancedetailissoccerkey";
        private const string TraderPerformanceDetailIsNoSoccerSessionKey = "traderperformancedetailisnosoccerkey";

        #region get traderperformance sum
        public TraderPerformanceDTO GetTraderPerformance(string fromDate, string toDate, int isSoccer)
        {
            #region local variables
            var result = new TraderPerformanceDTO();
            var traderData = new List<TraderPerformance>();
            var traderTotal = new List<TraderPerformance>();

            //result = (TraderPerformanceDTO)SessionExtension.GetSession(HttpContext.Current.Session, isSoccer == 1 ? TraderPerformanceIsSoccerSessionKey : TraderPerformanceIsNotSoccerSessionKey);
            //if (result != null)
            //    return result;
            #endregion

            var reportData = GetTraderPerformanceData(fromDate, toDate, isSoccer);
            try
            {
                if (reportData != null && reportData.Count > 0)
                {
                    #region private variables
                    result = new TraderPerformanceDTO();
                    var dt = reportData.ToList();
                    #region grand total

                    var totalMatch = 0;
                    #region wl
                    decimal wlHdpTotal = 0,
                            wl1stHdpTotal = 0,
                            wl1stOu = 0,
                            wlOuTotal = 0,
                            wl1st1x2 = 0,
                            wl1x2 = 0,
                            wl1stNoOfCornerTotal = 0,
                            wlNoOfCorner = 0,
                            wl15MinOu = 0,
                            wl1stNoOfYellowCard = 0,
                            wlNoOfYellowCard = 0,
                            wl1stOe = 0,
                            wlOe = 0,
                            wlNextCorner = 0,
                            wlOverall = 0;
                    #endregion
                    #region comm
                    decimal comHdpTotal = 0,
                            com1stHdpTotal = 0,
                            com1stOu = 0,
                            comOuTotal = 0,
                            com1st1x2 = 0,
                            com1x2 = 0,
                            com1stNoOfCornerTotal = 0,
                            comNoOfCorner = 0,
                            com15MinOu = 0,
                            com1stNoOfYellowCard = 0,
                            comNoOfYellowCard = 0,
                            com1stOe = 0,
                            comOe = 0,
                            comNextCorner = 0,
                            comOverall = 0;
                    #endregion
                    #region turn over
                    decimal toHdpTotal = 0,
                            to1stHdpTotal = 0,
                            to1stOu = 0,
                            toOuTotal = 0,
                            to1st1x2 = 0,
                            to1x2 = 0,
                            to1stNoOfCornerTotal = 0,
                            toNoOfCorner = 0,
                            to15MinOu = 0,
                            to1stNoOfYellowCard = 0,
                            toNoOfYellowCard = 0,
                            to1stOe = 0,
                            toOe = 0,
                            toNextCorner = 0,
                            toOverall = 0;
                    #endregion
                    #region margin
                    decimal mgHdpTotal = 0,
                            mg1stHdpTotal = 0,
                            mg1stOu = 0,
                            mgOuTotal = 0,
                            mg1st1x2 = 0,
                            mg1x2 = 0,
                            mg1stNoOfCornerTotal = 0,
                            mgNoOfCorner = 0,
                            mg15MinOu = 0,
                            mg1stNoOfYellowCard = 0,
                            mgNoOfYellowCard = 0,
                            mg1stOe = 0,
                            mgOe = 0,
                            mgNextCorner = 0,
                            mgOverall = 0;
                    #endregion
                    #region live betcount
                    decimal liHdpTotal = 0,
                            li1stHdpTotal = 0,
                            li1stOu = 0,
                            liOuTotal = 0,
                            li1st1x2 = 0,
                            li1x2 = 0,
                            li1stNoOfCornerTotal = 0,
                            liNoOfCorner = 0,
                            li15MinOu = 0,
                            li1stNoOfYellowCard = 0,
                            liNoOfYellowCard = 0,
                            li1stOe = 0,
                            liOe = 0,
                            liNextCorner = 0,
                            liOverall = 0;
                    #endregion
                    #endregion
                    #endregion

                    foreach (var dr in dt)
                    {
                        if (CastAide.AsInt32(dr["custid"]) > -1)
                        {
                            var traderDetail = new TraderPerformance
                            {
                                UserName = CastAide.AsString(dr["username"]),
                                MatchNo = CastAide.AsString(dr["matchno"]),

                                #region win-lost
                                WinLostHdp = NumberFormat.DecFormat(CastAide.AsDecimal(dr["winlost_hdp"], 0), 2),
                                WinLost1stHdp = NumberFormat.DecFormat(CastAide.AsDecimal(dr["winlost_1sthdp"]), 2),
                                WinLost1stOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["winlost_1stou"]), 2),
                                WinLostOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["winlost_ou"]), 2),
                                WinLost1st1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(dr["winlost_1st1x2"]), 2),
                                WinLost1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(dr["winlost_1x2"]), 2),
                                WinLost1stNoOfCorners =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["winlost_1stNoOfCorners"]), 2),
                                WinLostNoOfCorners =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["winlost_NoOfCorners"]), 2),
                                WinLost15minOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["winlost_15minOU"], 0), 2),
                                WinLost1stNoOfYellowCards =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["winlost_1stNoOfYellowCards"]), 2),
                                WinLostNoOfYellowCards =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["winlost_NoOfYellowCards"]), 2),
                                WinLost1stOe =
                                       NumberFormat.DecFormat(CastAide.AsDecimal(dr["winlost_1stoe"]), 2),
                                WinLostOe =
                                        NumberFormat.DecFormat(CastAide.AsDecimal(dr["winlost_oe"]), 2),
                                WinLostNextCorner =
                                    NumberFormat.DecFormat(dr.Table.Columns.Contains("winlost_NextCorner") ? CastAide.AsDecimal(dr["winlost_NextCorner"]) : 0, 2),
                                WinLostOverall = NumberFormat.DecFormat(CastAide.AsDecimal(dr["overall_wl"]), 2),
                                #endregion
                                #region comm
                                CommHdp = NumberFormat.DecFormat(CastAide.AsDecimal(dr["comm_hdp"], 0), 2),
                                Comm1stHdp = NumberFormat.DecFormat(CastAide.AsDecimal(dr["comm_1sthdp"], 0), 2),
                                Comm1stOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["comm_1stou"], 0), 2),
                                CommOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["comm_ou"], 0), 2),
                                Comm1st1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(dr["comm_1st1x2"], 0), 2),
                                Comm1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(dr["comm_1x2"], 0), 2),
                                Comm1stNoOfCorners =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["comm_1stNoOfCorners"], 0), 2),
                                CommNoOfCorners = NumberFormat.DecFormat(CastAide.AsDecimal(dr["comm_NoOfCorners"], 0), 2),
                                Comm15minOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["comm_15minOU"], 0), 2),
                                Comm1stNoOfYellowCards =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["comm_1stNoOfYellowCards"], 0), 2),
                                CommNoOfYellowCards =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["comm_NoOfYellowCards"], 0), 2),
                                Comm1stOe =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(
                                        dr.Table.Columns.Contains("comm_1stoe")
                                            ? dr["comm_1stoe"]
                                            : 0, 0), 2),
                                CommOe =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(
                                        dr.Table.Columns.Contains("comm_oe_value")
                                            ? dr["comm_oe_value"]
                                            : 0, 0), 2),
                                CommNextCorner =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(
                                        dr.Table.Columns.Contains("comm_NextCorner")
                                            ? dr["comm_NextCorner"]
                                            : 0, 0), 2),
                                CommOverall = NumberFormat.DecFormat(CastAide.AsDecimal(dr["overall_com"], 0), 2),
                                #endregion
                                #region turn-over
                                TurnOverHdp = NumberFormat.DecFormat(CastAide.AsDecimal(dr["stake_hdp"], 0), 2),
                                TurnOver1stHdp = NumberFormat.DecFormat(CastAide.AsDecimal(dr["stake_1sthdp"], 0), 2),
                                TurnOver1stOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["stake_1stou"], 0), 2),
                                TurnOverOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["stake_ou"], 0), 2),
                                TurnOver1st1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(dr["stake_1st1x2"], 0), 2),
                                TurnOver1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(dr["stake_1x2"], 0), 2),
                                TurnOver1stNoOfCorners =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["stake_1stNoOfCorners"], 0), 2),
                                TurnOverNoOfCorners = NumberFormat.DecFormat(CastAide.AsDecimal(dr["stake_NoOfCorners"], 0), 2),
                                TurnOver15minOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["stake_15minOU"], 0), 2),
                                TurnOver1stNoOfYellowCards =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["stake_1stNoOfYellowCards"], 0), 2),
                                TurnOverNoOfYellowCards =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["stake_NoOfYellowCards"], 0), 2),
                                TurnOver1stOe =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(
                                        dr.Table.Columns.Contains("stake_1stoe")
                                            ? dr["stake_1stoe"]
                                            : 0,
                                        0), 2),
                                TurnOverOe =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(
                                        dr.Table.Columns.Contains("stake_oe") ? dr["stake_oe"] : 0, 0), 2),
                                TurnOverNextCorner =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(
                                        dr.Table.Columns.Contains("stake_NextCorner") ? dr["stake_NextCorner"] : 0, 0), 2),
                                TurnOverOverall = NumberFormat.DecFormat(CastAide.AsDecimal(dr["overall_st"], 0), 2),
                                #endregion
                                #region margin
                                Margin1stHdp = NumberFormat.DecFormat(CastAide.AsDecimal(dr["margin_1sthdp"]) * 100, 2),
                                MarginHdp = NumberFormat.DecFormat(CastAide.AsDecimal(dr["margin_hdp"]) * 100, 2),
                                Margin1stOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["margin_1stou"]) * 100, 2),
                                MarginOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["margin_ou"]) * 100, 2),
                                Margin1st1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(dr["margin_1st1x2"]) * 100, 2),
                                Margin1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(dr["margin_1x2"]) * 100, 2),
                                Margin1stNoOfCorners =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["margin_1stNoOfCorners"]) * 100, 2),
                                MarginNoOfCorners = NumberFormat.DecFormat(CastAide.AsDecimal(dr["margin_NoOfCorners"]) * 100, 2),
                                Margin15minOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["margin_15minOU"]) * 100, 2),
                                Margin1stNoOfYellowCards =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["margin_1stNoOfYellowCards"]) * 100, 2),
                                MarginNoOfYellowCards =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["margin_NoOfYellowCards"]) * 100, 2),
                                Margin1stOe =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(
                                        dr.Table.Columns.Contains("margin_1stoe")
                                            ? dr["margin_1stoe"]
                                            : 0) * 100, 2),
                                MarginOe =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(
                                        dr.Table.Columns.Contains("margin_oe") ? dr["margin_oe"] : 0) * 100,
                                        2),
                                MarginNextCorner =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(
                                        dr.Table.Columns.Contains("margin_NextCorner") ? dr["margin_NextCorner"] : 0) * 100,
                                        2),
                                MarginOverall = NumberFormat.DecFormat(CastAide.AsDecimal(dr["overall_margin"]) * 100, 2),
                                #endregion
                                #region live bet count
                                LiveBetCountHdp = NumberFormat.DecFormat(CastAide.AsDecimal(dr["bet_hdp"], 0), 2),
                                LiveBetCount1stHdp = NumberFormat.DecFormat(CastAide.AsDecimal(dr["bet_1sthdp"], 0), 2),
                                LiveBetCount1stOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["bet_1stou"], 0), 2),
                                LiveBetCountOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["bet_ou"], 0), 2),
                                LiveBetCount1st1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(dr["bet_1st1x2"], 0), 2),
                                LiveBetCount1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(dr["bet_1x2"], 0), 2),
                                LiveBetCount1stNoOfCorners =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["bet_1stNoOfCorners"], 0), 2),
                                LiveBetCountNoOfCorners =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["bet_NoOfCorners"], 0), 2),
                                LiveBetCount15minOu = NumberFormat.DecFormat(CastAide.AsDecimal(dr["bet_15minOU"], 0), 2),
                                LiveBetCount1stNoOfYellowCards =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["bet_1stNoOfYellowCards"], 0), 2),
                                LiveBetCountNoOfYellowCards =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(dr["bet_NoOfYellowCards"], 0), 2),
                                LiveBetCount1stOe =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(
                                        dr.Table.Columns.Contains("bet_1stoe") ? dr["bet_1stoe"] : 0,
                                        0), 2),
                                LiveBetCountOe =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(
                                        dr.Table.Columns.Contains("bet_oe") ? dr["bet_oe"] : 0, 0), 2),
                                LiveBetCountNextCorner =
                                    NumberFormat.DecFormat(CastAide.AsDecimal(
                                        dr.Table.Columns.Contains("bet_NextCorner") ? dr["bet_NextCorner"] : 0, 0), 2),
                                LiveBetCountOverall = NumberFormat.DecFormat(CastAide.AsDecimal(dr["overall_livebet"], 0), 2)
                                #endregion
                            };

                            #region assign GrandTotal data
                            totalMatch += CastAide.AsInt16Invariant(dr["matchno"]);
                            #region win-lost
                            wlHdpTotal += CastAide.AsDecimal(dr["winlost_hdp"]);
                            wl1stHdpTotal += CastAide.AsDecimal(dr["winlost_1sthdp"]);
                            wl1stOu += CastAide.AsDecimal(dr["winlost_1stou"]);
                            wlOuTotal += CastAide.AsDecimal(dr["winlost_ou"]);
                            wl1st1x2 += CastAide.AsDecimal(dr["winlost_1st1x2"]);
                            wl1x2 += CastAide.AsDecimal(dr["winlost_1x2"]);
                            wl1stNoOfCornerTotal +=
                                CastAide.AsDecimal(dr["winlost_1stNoOfCorners"]);
                            wlNoOfCorner +=
                                CastAide.AsDecimal(dr["winlost_NoOfCorners"]);
                            wl15MinOu += CastAide.AsDecimal(dr["winlost_15minOU"]);
                            wl1stNoOfYellowCard +=
                                CastAide.AsDecimal(dr["winlost_1stNoOfYellowCards"]);
                            wlNoOfYellowCard +=
                                CastAide.AsDecimal(dr["winlost_NoOfYellowCards"]);
                            wl1stOe +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("winlost_1stoe")
                                        ? dr["winlost_1stoe"]
                                        : 0);
                            wlOe +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("winlost_oe")
                                        ? dr["winlost_oe"]
                                        : 0);
                            wlNextCorner +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("winlost_NextCorner")
                                        ? dr["winlost_NextCorner"]
                                        : 0);
                            wlOverall += CastAide.AsDecimal(dr["overall_wl"]);
                            #endregion
                            #region comm
                            comHdpTotal += CastAide.AsDecimal(dr["comm_hdp"]);
                            com1stHdpTotal += CastAide.AsDecimal(dr["comm_1sthdp"]);
                            com1stOu += CastAide.AsDecimal(dr["comm_1stou"]);
                            comOuTotal += CastAide.AsDecimal(dr["comm_ou"]);
                            com1st1x2 += CastAide.AsDecimal(dr["comm_1st1x2"]);
                            com1x2 += CastAide.AsDecimal(dr["comm_1x2"]);
                            com1stNoOfCornerTotal +=
                                CastAide.AsDecimal(dr["comm_1stNoOfCorners"]);
                            comNoOfCorner += CastAide.AsDecimal(dr["comm_NoOfCorners"]);
                            com15MinOu += CastAide.AsDecimal(dr["comm_15minOU"]);
                            com1stNoOfYellowCard +=
                                CastAide.AsDecimal(dr["comm_1stNoOfYellowCards"]);
                            comNoOfYellowCard +=
                                CastAide.AsDecimal(dr["comm_NoOfYellowCards"]);
                            com1stOe +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("comm_1stoe")
                                        ? dr["comm_1stoe"]
                                        : 0);
                            comOe +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("comm_oe_value")
                                        ? dr["comm_oe_value"]
                                        : 0);
                            comNextCorner +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("comm_NextCorner")
                                        ? dr["comm_NextCorner"]
                                        : 0);
                            comOverall += CastAide.AsDecimal(dr["overall_com"]);
                            #endregion
                            #region turn-over
                            toHdpTotal += CastAide.AsDecimal(dr["stake_hdp"]);
                            to1stHdpTotal += CastAide.AsDecimal(dr["stake_1sthdp"]);
                            to1stOu += CastAide.AsDecimal(dr["stake_1stou"]);
                            toOuTotal += CastAide.AsDecimal(dr["stake_ou"]);
                            to1st1x2 += CastAide.AsDecimal(dr["stake_1st1x2"]);
                            to1x2 += CastAide.AsDecimal(dr["stake_1x2"]);
                            to1stNoOfCornerTotal +=
                                CastAide.AsDecimal(dr["stake_1stNoOfCorners"]);
                            toNoOfCorner += CastAide.AsDecimal(dr["stake_NoOfCorners"]);
                            to15MinOu += CastAide.AsDecimal(dr["stake_15minOU"]);
                            to1stNoOfYellowCard +=
                                CastAide.AsDecimal(dr["stake_1stNoOfYellowCards"]);
                            toNoOfYellowCard +=
                                CastAide.AsDecimal(dr["stake_NoOfYellowCards"]);
                            to1stOe +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("stake_1stoe")
                                        ? dr["stake_1stoe"]
                                        : 0);
                            toOe +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("stake_oe") ? dr["stake_oe"] : 0);
                            toNextCorner +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("stake_NextCorner")
                                        ? dr["stake_NextCorner"]
                                        : 0);
                            toOverall += CastAide.AsDecimal(dr["overall_st"], 0);
                            #endregion
                            #region margin
                            mgHdpTotal += CastAide.AsDecimal(dr["margin_hdp"]);
                            mg1stHdpTotal += CastAide.AsDecimal(dr["margin_1sthdp"]);
                            mg1stOu += CastAide.AsDecimal(dr["margin_1stou"]);
                            mgOuTotal += CastAide.AsDecimal(dr["margin_ou"]);
                            mg1st1x2 += CastAide.AsDecimal(dr["margin_1st1x2"]);
                            mg1x2 += CastAide.AsDecimal(dr["margin_1x2"]);
                            mg1stNoOfCornerTotal +=
                                CastAide.AsDecimal(dr["margin_1stNoOfCorners"]);
                            mgNoOfCorner += CastAide.AsDecimal(dr["margin_NoOfCorners"]);
                            mg15MinOu += CastAide.AsDecimal(dr["margin_15minOU"]);
                            mg1stNoOfYellowCard +=
                                CastAide.AsDecimal(dr["margin_1stNoOfYellowCards"]);
                            mgNoOfYellowCard +=
                                CastAide.AsDecimal(dr["margin_NoOfYellowCards"]);
                            mg1stOe +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("margin_1stoe")
                                        ? dr["margin_1stoe"]
                                        : 0);
                            mgOe +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("margin_oe") ? dr["margin_oe"] : 0);
                            mgNextCorner +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("margin_NextCorner")
                                        ? dr["margin_NextCorner"]
                                        : 0);
                            mgOverall += CastAide.AsDecimal(dr["overall_margin"]);
                            #endregion
                            #region live bet count
                            liHdpTotal += CastAide.AsDecimal(dr["bet_hdp"]);
                            li1stHdpTotal += CastAide.AsDecimal(dr["bet_1sthdp"]);
                            li1stOu += CastAide.AsDecimal(dr["bet_1stou"]);
                            liOuTotal += CastAide.AsDecimal(dr["bet_ou"]);
                            li1st1x2 += CastAide.AsDecimal(dr["bet_1st1x2"]);
                            li1x2 += CastAide.AsDecimal(dr["bet_1x2"]);
                            li1stNoOfCornerTotal +=
                                CastAide.AsDecimal(dr["bet_1stNoOfCorners"]);
                            liNoOfCorner +=
                                CastAide.AsDecimal(dr["bet_NoOfCorners"]);
                            li15MinOu += CastAide.AsDecimal(dr["bet_15minOU"]);
                            li1stNoOfYellowCard +=
                                CastAide.AsDecimal(dr["bet_1stNoOfYellowCards"]);
                            liNoOfYellowCard +=
                                CastAide.AsDecimal(dr["bet_NoOfYellowCards"]);
                            li1stOe +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("bet_1stoe") ? dr["bet_1stoe"] : 0);
                            liOe +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("bet_oe") ? dr["bet_oe"] : 0);
                            liNextCorner +=
                                CastAide.AsDecimal(
                                    dr.Table.Columns.Contains("bet_NextCorner") ? dr["bet_NextCorner"] : 0);
                            liOverall += CastAide.AsDecimal(dr["overall_livebet"]);
                            #endregion
                            #endregion
                            if (CastAide.AsInt32(dr["custid"]) == 0 && CastAide.AsDecimal(dr["overall_st"], 0) == 0)
                                continue;
                            traderData.Add(traderDetail);
                        }
                    }

                    #region Grandtotal format
                    var traderGrandTotal = new TraderPerformance
                    {
                        MatchNo = totalMatch.ToString(),
                        UserName = string.Empty,
                        #region win-lost
                        WinLostHdp = NumberFormat.DecFormat(CastAide.AsDecimal(wlHdpTotal, 0), 2),
                        WinLost1stHdp = NumberFormat.DecFormat(CastAide.AsDecimal(wl1stHdpTotal, 0), 2),
                        WinLost1stOu = NumberFormat.DecFormat(CastAide.AsDecimal(wl1stOu, 0), 2),
                        WinLostOu = NumberFormat.DecFormat(CastAide.AsDecimal(wlOuTotal, 0), 2),
                        WinLost1st1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(wl1st1x2, 0), 2),
                        WinLost1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(wl1x2, 0), 2),
                        WinLost1stNoOfCorners =
                            NumberFormat.DecFormat(CastAide.AsDecimal(wl1stNoOfCornerTotal, 0), 2),
                        WinLostNoOfCorners =
                            NumberFormat.DecFormat(CastAide.AsDecimal(wlNoOfCorner, 0), 2),
                        WinLost15minOu = NumberFormat.DecFormat(CastAide.AsDecimal(wl15MinOu, 0), 2),
                        WinLost1stNoOfYellowCards =
                            NumberFormat.DecFormat(CastAide.AsDecimal(wl1stNoOfYellowCard, 0), 2),
                        WinLostNoOfYellowCards =
                            NumberFormat.DecFormat(CastAide.AsDecimal(wlNoOfYellowCard, 0), 2),
                        WinLost1stOe =
                            NumberFormat.DecFormat(CastAide.AsDecimal(wl1stOe, 0), 2),
                        WinLostOe =
                            NumberFormat.DecFormat(CastAide.AsDecimal(wlOe, 0), 2),
                        WinLostNextCorner =
                            NumberFormat.DecFormat(CastAide.AsDecimal(wlNextCorner, 0), 2),
                        WinLostOverall = NumberFormat.DecFormat(CastAide.AsDecimal(wlOverall, 0), 2),
                        #endregion
                        #region comm
                        CommHdp = NumberFormat.DecFormat(CastAide.AsDecimal(comHdpTotal, 0), 2),
                        Comm1stHdp = NumberFormat.DecFormat(CastAide.AsDecimal(com1stHdpTotal, 0), 2),
                        Comm1stOu = NumberFormat.DecFormat(CastAide.AsDecimal(com1stOu, 0), 2),
                        CommOu = NumberFormat.DecFormat(CastAide.AsDecimal(comOuTotal, 0), 2),
                        Comm1st1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(com1st1x2, 0), 2),
                        Comm1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(com1x2, 0), 2),
                        Comm1stNoOfCorners =
                            NumberFormat.DecFormat(CastAide.AsDecimal(com1stNoOfCornerTotal, 0), 2),
                        CommNoOfCorners = NumberFormat.DecFormat(CastAide.AsDecimal(comNoOfCorner, 0), 2),
                        Comm15minOu = NumberFormat.DecFormat(CastAide.AsDecimal(com15MinOu, 0), 2),
                        Comm1stNoOfYellowCards =
                            NumberFormat.DecFormat(CastAide.AsDecimal(com1stNoOfYellowCard, 0), 2),
                        CommNoOfYellowCards =
                            NumberFormat.DecFormat(CastAide.AsDecimal(comNoOfYellowCard, 0), 2),
                        Comm1stOe =
                            NumberFormat.DecFormat(CastAide.AsDecimal(com1stOe, 0), 2),
                        CommOe =
                            NumberFormat.DecFormat(CastAide.AsDecimal(comOe, 0), 2),
                        CommNextCorner =
                            NumberFormat.DecFormat(CastAide.AsDecimal(comNextCorner, 0), 2),
                        CommOverall = NumberFormat.DecFormat(CastAide.AsDecimal(comOverall, 0), 2),
                        #endregion
                        #region turn-over
                        TurnOverHdp = NumberFormat.DecFormat(CastAide.AsDecimal(toHdpTotal, 0), 2),
                        TurnOver1stHdp = NumberFormat.DecFormat(CastAide.AsDecimal(to1stHdpTotal, 0), 2),
                        TurnOver1stOu = NumberFormat.DecFormat(CastAide.AsDecimal(to1stOu, 0), 2),
                        TurnOverOu = NumberFormat.DecFormat(CastAide.AsDecimal(toOuTotal, 0), 2),
                        TurnOver1st1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(to1st1x2, 0), 2),
                        TurnOver1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(to1x2, 0), 2),
                        TurnOver1stNoOfCorners =
                            NumberFormat.DecFormat(CastAide.AsDecimal(to1stNoOfCornerTotal, 0), 2),
                        TurnOverNoOfCorners = NumberFormat.DecFormat(CastAide.AsDecimal(toNoOfCorner, 0), 2),
                        TurnOver15minOu = NumberFormat.DecFormat(CastAide.AsDecimal(to15MinOu, 0), 2),
                        TurnOver1stNoOfYellowCards =
                            NumberFormat.DecFormat(CastAide.AsDecimal(to1stNoOfYellowCard, 0), 2),
                        TurnOverNoOfYellowCards =
                            NumberFormat.DecFormat(CastAide.AsDecimal(toNoOfYellowCard, 0), 2),
                        TurnOver1stOe =
                            NumberFormat.DecFormat(CastAide.AsDecimal(to1stOe, 0), 2),
                        TurnOverOe =
                            NumberFormat.DecFormat(CastAide.AsDecimal(toOe, 0), 2),
                        TurnOverNextCorner =
                            NumberFormat.DecFormat(CastAide.AsDecimal(toNextCorner, 0), 2),
                        TurnOverOverall = NumberFormat.DecFormat(CastAide.AsDecimal(toOverall, 0), 2),
                        #endregion
                        #region margin

                        MarginHdp = NumberFormat.DecFormat((CastAide.AsDecimal(toHdpTotal, 0) != 0 ? CastAide.AsDecimal(wlHdpTotal, 0) * 100 / CastAide.AsDecimal(toHdpTotal, 0) : 0), 2),
                        Margin1stHdp = NumberFormat.DecFormat((CastAide.AsDecimal(to1stHdpTotal, 0) != 0 ? CastAide.AsDecimal(wl1stHdpTotal, 0) * 100 / CastAide.AsDecimal(to1stHdpTotal, 0) : 0), 2),
                        Margin1stOu = NumberFormat.DecFormat((CastAide.AsDecimal(to1stOu, 0) != 0 ? (CastAide.AsDecimal(wl1stOu, 0) * 100 / CastAide.AsDecimal(to1stOu, 0)) : 0), 2),
                        MarginOu = NumberFormat.DecFormat((CastAide.AsDecimal(toOuTotal, 0) != 0 ? (CastAide.AsDecimal(wlOuTotal, 0) * 100 / CastAide.AsDecimal(toOuTotal, 0)) : 0), 2),
                        Margin1st1x2 = NumberFormat.DecFormat((CastAide.AsDecimal(to1st1x2, 0) != 0 ? (CastAide.AsDecimal(wl1st1x2, 0) * 100 / CastAide.AsDecimal(to1st1x2, 0)) : 0), 2),
                        Margin1x2 = NumberFormat.DecFormat((CastAide.AsDecimal(to1x2, 0) != 0 ? (CastAide.AsDecimal(wl1x2, 0) * 100 / CastAide.AsDecimal(to1x2, 0)) : 0), 2),
                        Margin1stNoOfCorners =
                            NumberFormat.DecFormat((CastAide.AsDecimal(to1stNoOfCornerTotal, 0) != 0 ? (CastAide.AsDecimal(wl1stNoOfCornerTotal, 0) * 100 / CastAide.AsDecimal(to1stNoOfCornerTotal, 0)) : 0), 2),
                        MarginNoOfCorners = NumberFormat.DecFormat((CastAide.AsDecimal(toNoOfCorner, 0) > 0 ? (CastAide.AsDecimal(wlNoOfCorner, 0) * 100 / CastAide.AsDecimal(toNoOfCorner, 0)) : 0), 2),
                        Margin15minOu = NumberFormat.DecFormat((CastAide.AsDecimal(to15MinOu, 0) != 0 ? (CastAide.AsDecimal(wl15MinOu, 0) * 100 / CastAide.AsDecimal(to15MinOu, 0)) : 0), 2),
                        Margin1stNoOfYellowCards =
                            NumberFormat.DecFormat((CastAide.AsDecimal(to1stNoOfYellowCard, 0) != 0 ? (CastAide.AsDecimal(wl1stNoOfYellowCard, 0) * 100 / CastAide.AsDecimal(to1stNoOfYellowCard, 0)) : 0), 2),
                        MarginNoOfYellowCards =
                            NumberFormat.DecFormat((CastAide.AsDecimal(toNoOfYellowCard, 0) != 0 ? (CastAide.AsDecimal(wlNoOfYellowCard, 0) * 100 / CastAide.AsDecimal(toNoOfYellowCard, 0)) : 0), 2),
                        Margin1stOe =
                            NumberFormat.DecFormat((CastAide.AsDecimal(to1stOe, 0) != 0 ? (CastAide.AsDecimal(wl1stOe, 0) * 100 / CastAide.AsDecimal(to1stOe, 0)) : 0), 2),
                        MarginOe =
                            NumberFormat.DecFormat((CastAide.AsDecimal(toOe, 0) != 0 ? (CastAide.AsDecimal(wlOe, 0) * 100 / CastAide.AsDecimal(toOe, 0)) : 0), 2),
                        MarginNextCorner =
                            NumberFormat.DecFormat((CastAide.AsDecimal(toNextCorner, 0) != 0 ? (CastAide.AsDecimal(wlNextCorner, 0) * 100 / CastAide.AsDecimal(toNextCorner, 0)) : 0), 2),
                        MarginOverall = NumberFormat.DecFormat((CastAide.AsDecimal(toOverall, 0) != 0 ? (CastAide.AsDecimal(wlOverall, 0) * 100 / CastAide.AsDecimal(toOverall, 0)) : 0), 2),
                        #endregion
                        #region live bet count
                        LiveBetCountHdp = NumberFormat.DecFormat(CastAide.AsDecimal(liHdpTotal, 0), 2),
                        LiveBetCount1stHdp = NumberFormat.DecFormat(CastAide.AsDecimal(li1stHdpTotal, 0), 2),
                        LiveBetCount1stOu = NumberFormat.DecFormat(CastAide.AsDecimal(li1stOu, 0), 2),
                        LiveBetCountOu = NumberFormat.DecFormat(CastAide.AsDecimal(liOuTotal, 0), 2),
                        LiveBetCount1st1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(li1st1x2, 0), 2),
                        LiveBetCount1x2 = NumberFormat.DecFormat(CastAide.AsDecimal(li1x2, 0), 2),
                        LiveBetCount1stNoOfCorners =
                            NumberFormat.DecFormat(CastAide.AsDecimal(li1stNoOfCornerTotal, 0), 2),
                        LiveBetCountNoOfCorners =
                            NumberFormat.DecFormat(CastAide.AsDecimal(liNoOfCorner, 0), 2),
                        LiveBetCount15minOu = NumberFormat.DecFormat(CastAide.AsDecimal(li15MinOu, 0), 2),
                        LiveBetCount1stNoOfYellowCards =
                            NumberFormat.DecFormat(CastAide.AsDecimal(li1stNoOfYellowCard, 0), 2),
                        LiveBetCountNoOfYellowCards =
                            NumberFormat.DecFormat(CastAide.AsDecimal(liNoOfYellowCard, 0), 2),
                        LiveBetCount1stOe =
                            NumberFormat.DecFormat(CastAide.AsDecimal(li1stOe, 0), 2),
                        LiveBetCountOe =
                            NumberFormat.DecFormat(CastAide.AsDecimal(liOe, 0), 2),
                        LiveBetCountNextCorner =
                            NumberFormat.DecFormat(CastAide.AsDecimal(liNextCorner, 0), 2),
                        LiveBetCountOverall = NumberFormat.DecFormat(CastAide.AsDecimal(liOverall, 0), 2)
                        #endregion


                    };
                    traderTotal.Add(traderGrandTotal);
                    #endregion

                    result.rows = traderData;
                    result.footers = traderTotal;
                }
            }
            catch
            {
            }
            finally
            {
                switch (isSoccer)
                {
                    case 1:
                        SessionExtension.AddSession(HttpContext.Current.Session, TraderPerformanceIsSoccerSessionKey, result);
                        break;
                    case 0:
                        SessionExtension.AddSession(HttpContext.Current.Session, TraderPerformanceIsNotSoccerSessionKey, result);
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        private static List<DataRow> GetTraderPerformanceData(string fromDate, string toDate, int isSoccer)
        {
            var result = new DataTable();
            var hashTbl = new Hashtable { { "fromdate", fromDate }, { "todate", toDate }, { "issoccer", isSoccer } };

            #region get data
            var rs = AccountReport.Acc_TraderPerformanceSum(hashTbl);
            if (rs != null && rs.ResultSet.Tables.Count > 0)
            {
                result = rs.ResultSet.Tables[0];
            }
            #endregion

            var query = (from traderData in result.AsEnumerable() select traderData).ToList();
            return query;
        }
        #endregion

        #region Export 2 excel
        public void TraderPerformanceExport2Excel(string fromDate, string toDate, int isSoccer)
        {
            var trader = new TraderPerformanceDTO();
            trader = (TraderPerformanceDTO)SessionExtension.GetSession(HttpContext.Current.Session, isSoccer == 1 ? TraderPerformanceIsSoccerSessionKey : TraderPerformanceIsNotSoccerSessionKey) ??
                     GetTraderPerformance(fromDate, toDate, isSoccer);
            TraderPerformanceExport2ExcelFormat(trader, isSoccer, fromDate, toDate);
        }

        private void TraderPerformanceExport2ExcelFormat(TraderPerformanceDTO trader, int isSoccer, string fromDate, string toDate)
        {
            #region local variables
            const string firstOneXTwo = "1st 1x2";
            const string oneXTwo = "1x2";
            const string firstHdp = "1st HDP";
            const string hdp = "HDP";
            const string firstOu = "1st O/U";
            const string ou = "O/U";
            const string firstNoOfCorner = "1st No. of Corner";
            const string noOfCorner = "No. of Corner";
            const string firstNoOfYelowCard = "1st No. of Yellow Card";
            const string noOfYelowCard = "No. of Yellow Card";
            const string fifteenMinsOu = "15mins O/U";
            const string firstOE = "1st O/E";
            const string OE = "O/E";
            const string nextCorner = "Next Corner";
            const string overall = "Overall";

            var reportName = isSoccer == 1 ? "TraderPerformance_Soccer" : "TraderPerformance_OtherSports";
            var excelFile = new ExcelFile();
            const int startRowIndex = 3;
            const int startColumnIndex = 0;
            var nextRow = 0;
            var wsTraderPerformanceReport = excelFile.Worksheets.Add(reportName);
            var mergedTotalColumns = isSoccer == 1 ? 14 : 8;
            #endregion

            #region Header
            var bgHeaderFormat = ExcelFormat(Color.FromArgb(159, 183, 213), HorizontalAlignmentStyle.Center, ExcelFont.BoldWeight, false, false);
            var nextHeaderColumn = 0;

            #region first row
            wsTraderPerformanceReport.Cells[startRowIndex - 3, startColumnIndex].Style.Font.Weight = ExcelFont.BoldWeight;
            wsTraderPerformanceReport.Cells[startRowIndex - 3, startColumnIndex].Value =
                string.Format("Trader Performance For {0} From {1} To {2}", (isSoccer == 1 ? "SOCCER SPORT" : "OTHER SPORTS"), fromDate, toDate);

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex,
                                                    startRowIndex - 1,
                                                    startColumnIndex + (nextHeaderColumn)).Merged =
            true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                               startRowIndex - 1,
                                                               startColumnIndex + (nextHeaderColumn)).Style.VerticalAlignment =
               VerticalAlignmentStyle.Center;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                               startRowIndex - 1,
                                                               startColumnIndex + (nextHeaderColumn++)).Value = "No";


            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                    startRowIndex - 1,
                                                    startColumnIndex + (nextHeaderColumn)).Merged =
            true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                            startRowIndex - 1,
                                                            startColumnIndex + (nextHeaderColumn)).Style.VerticalAlignment =
                VerticalAlignmentStyle.Center;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn++)).Value =
                "Trader";


            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                    startRowIndex - 1,
                                                    startColumnIndex + (nextHeaderColumn)).Merged =
            true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn)).Style.VerticalAlignment =
                VerticalAlignmentStyle.Center;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn++)).Value =
                "Match";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex +
                                                                (nextHeaderColumn += mergedTotalColumns)).Merged =
                true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Value =
                "Winlost";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn += 1),
                                                                startRowIndex - 2,
                                                                startColumnIndex +
                                                                (nextHeaderColumn += mergedTotalColumns)).Merged =
                true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Value =
                "Turnover";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn += 1),
                                                                startRowIndex - 2,
                                                                startColumnIndex +
                                                                (nextHeaderColumn += mergedTotalColumns)).Merged =
                true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Value =
                "Live BetCount";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn += 1),
                                                                startRowIndex - 2,
                                                                startColumnIndex +
                                                                (nextHeaderColumn += mergedTotalColumns)).Merged =
                true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Value =
                "Margin%";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn += 1),
                                                                startRowIndex - 2,
                                                                startColumnIndex +
                                                                (nextHeaderColumn += mergedTotalColumns)).Merged =
                true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Value =
                "Commission";
            wsTraderPerformanceReport.Cells[startRowIndex - 2, startColumnIndex + (nextHeaderColumn++)].Style =
                    bgHeaderFormat;


            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                  startRowIndex - 1,
                                                  startColumnIndex + (nextHeaderColumn)).Merged =
          true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn)).Style.VerticalAlignment =
                VerticalAlignmentStyle.Center;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn)).Value =
                "Trader";

            #endregion
            //second row
            nextHeaderColumn = 0;
            wsTraderPerformanceReport.Rows[startRowIndex - 1].Height = 400;
            wsTraderPerformanceReport.Columns[nextHeaderColumn++].Width = 2000;
            wsTraderPerformanceReport.Columns[nextHeaderColumn++].Width = 4000;
            wsTraderPerformanceReport.Columns[nextHeaderColumn++].Width = 2000;

            #region group header
            for (var i = 0; i < 5; i++)
            {
                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    firstOneXTwo;

                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    oneXTwo;

                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    firstHdp;

                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    hdp;

                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    firstOu;

                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    ou;

                if (isSoccer == 1)
                {
                    wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style
                        =
                        bgHeaderFormat;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].
                        Value =
                        firstNoOfCorner;

                    wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style
                        =
                        bgHeaderFormat;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].
                        Value =
                        noOfCorner;

                    wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style
                        =
                        bgHeaderFormat;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].
                        Value =
                        firstNoOfYelowCard;

                    wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style
                        =
                        bgHeaderFormat;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].
                        Value =
                        noOfYelowCard;

                    wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style
                        =
                        bgHeaderFormat;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].
                        Value =
                        fifteenMinsOu;

                    wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style
                        =
                        bgHeaderFormat;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].
                        Value =
                        nextCorner;
                }
                //----
                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    firstOE;

                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    OE;

                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    overall;

            }

            wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;

            #endregion
            #endregion
            if (trader != null && trader.rows != null && trader.rows.Count() > 0)
            {
                #region Body

                foreach (var reportDetail in trader.rows)
                {
                    #region group1: No; Trader; Match

                    var nextColumn = 0;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        =
                        (nextRow % 2 == 0
                             ? ExcelFormat(Color.FromArgb(248, 248, 248),
                                           HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight, false, false)
                             : ExcelFormat(Color.FromArgb(217, 232, 235),
                                           HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight, false, false));
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value =
                        (nextRow + 1);

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        =
                        (nextRow % 2 == 0
                             ? ExcelFormat(Color.FromArgb(248, 248, 248),
                                           HorizontalAlignmentStyle.Left, ExcelFont.NormalWeight, false, false)
                             : ExcelFormat(Color.FromArgb(217, 232, 235),
                                           HorizontalAlignmentStyle.Left, ExcelFont.NormalWeight, false, false));
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value =
                        reportDetail.UserName;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        =
                        (nextRow % 2 == 0
                             ? ExcelFormat(Color.FromArgb(248, 248, 248),
                                           HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight, false, false)
                             : ExcelFormat(Color.FromArgb(217, 232, 235),
                                           HorizontalAlignmentStyle.Center, ExcelFont.NormalWeight, false, false));
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = reportDetail.MatchNo;
                    #endregion

                    #region win-lose

                    var bgFormat = ExcelFormat(Color.FromArgb(231, 237, 169),
                                           HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, true, false);

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.WinLost1st1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.WinLost1st1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.WinLost1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.WinLost1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.WinLost1stHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.WinLost1stHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.WinLostHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.WinLostHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.WinLost1stOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.WinLost1stOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.WinLostOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.WinLostOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;
                    if (isSoccer == 1)
                    {
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.WinLost1stNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.WinLost1stNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.WinLostNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.WinLostNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.WinLost1stNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.WinLost1stNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.WinLostNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.WinLostNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.WinLost15minOu) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.WinLost15minOu);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.WinLostNextCorner) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.WinLostNextCorner);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;
                    }
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.WinLost1stOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.WinLost1stOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.WinLostOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.WinLostOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.WinLostOverall) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.WinLostOverall);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    #endregion

                    #region TurnOver

                    bgFormat = ExcelFormat(Color.FromArgb(209, 227, 231),
                                           HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, true, false);

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.TurnOver1st1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.TurnOver1st1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.TurnOver1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.TurnOver1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.TurnOver1stHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.TurnOver1stHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.TurnOverHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.TurnOverHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.TurnOver1stOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.TurnOver1stOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.TurnOverOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.TurnOverOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;
                    if (isSoccer == 1)
                    {
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.TurnOver1stNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.TurnOver1stNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.TurnOverNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.TurnOverNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.TurnOver1stNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.TurnOver1stNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.TurnOverNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.TurnOverNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.TurnOver15minOu) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.TurnOver15minOu);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.TurnOverNextCorner) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.TurnOverNextCorner);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;
                    }
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.TurnOver1stOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.TurnOver1stOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.TurnOverOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.TurnOverOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.TurnOverOverall) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.TurnOverOverall);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    #endregion

                    #region Live BetCount

                    bgFormat = ExcelFormat(Color.FromArgb(253, 207, 246),
                                           HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, true, false);

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.LiveBetCount1st1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.LiveBetCount1st1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                       Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.LiveBetCount1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.LiveBetCount1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.LiveBetCount1stHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.LiveBetCount1stHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.LiveBetCountHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.LiveBetCountHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.LiveBetCount1stOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.LiveBetCount1stOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.LiveBetCountOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.LiveBetCountOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;
                    if (isSoccer == 1)
                    {
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.LiveBetCount1stNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.LiveBetCount1stNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.LiveBetCountNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.LiveBetCountNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.LiveBetCount1stNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.LiveBetCount1stNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.LiveBetCountNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.LiveBetCountNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.LiveBetCount15minOu) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.LiveBetCount15minOu);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.LiveBetCountNextCorner) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.LiveBetCountNextCorner);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;
                    }
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.LiveBetCount1stOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.LiveBetCount1stOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.LiveBetCountOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.LiveBetCountOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.LiveBetCountOverall) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.LiveBetCountOverall);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    #endregion

                    #region Margin

                    bgFormat = ExcelFormat(Color.FromArgb(202, 222, 230),
                                           HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, true, false);
                    var bgFormatPercent = ExcelFormat(Color.FromArgb(202, 222, 230),
                                  HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, false, true);

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormatPercent;
                    if (CastAide.AsDecimal(reportDetail.Margin1st1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.Margin1st1x2) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormatPercent;
                    if (CastAide.AsDecimal(reportDetail.Margin1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.Margin1x2) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormatPercent;
                    if (CastAide.AsDecimal(reportDetail.Margin1stHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.Margin1stHdp) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormatPercent;
                    if (CastAide.AsDecimal(reportDetail.MarginHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.MarginHdp) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormatPercent;
                    if (CastAide.AsDecimal(reportDetail.Margin1stOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.Margin1stOu) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormatPercent;
                    if (CastAide.AsDecimal(reportDetail.MarginOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.MarginOu) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;
                    if (isSoccer == 1)
                    {
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormatPercent;
                        if (CastAide.AsDecimal(reportDetail.Margin1stNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.Margin1stNoOfCorners) / 100;
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                           Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormatPercent;
                        if (CastAide.AsDecimal(reportDetail.MarginNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.MarginNoOfCorners) / 100;
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormatPercent;
                        if (CastAide.AsDecimal(reportDetail.Margin1stNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.Margin1stNoOfYellowCards) / 100;
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormatPercent;
                        if (CastAide.AsDecimal(reportDetail.MarginNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.MarginNoOfYellowCards) / 100;
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormatPercent;
                        if (CastAide.AsDecimal(reportDetail.Margin15minOu) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.Margin15minOu) / 100;
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormatPercent;
                        if (CastAide.AsDecimal(reportDetail.MarginNextCorner) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.MarginNextCorner) / 100;
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;
                    }
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormatPercent;
                    if (CastAide.AsDecimal(reportDetail.Margin1stOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.Margin1stOe) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormatPercent;
                    if (CastAide.AsDecimal(reportDetail.MarginOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.MarginOe) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                       Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormatPercent;
                    if (CastAide.AsDecimal(reportDetail.MarginOverall) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.MarginOverall) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                       Value = null;


                    #endregion

                    #region Commission

                    bgFormat = ExcelFormat(Color.FromArgb(255, 255, 238),
                                           HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, true, false);

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.Comm1st1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.Comm1st1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.Comm1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.Comm1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.Comm1stHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.Comm1stHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.CommHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.CommHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.Comm1stOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.Comm1stOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.CommOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.CommOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    if (isSoccer == 1)
                    {
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.Comm1stNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.Comm1stNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.CommNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.CommNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.Comm1stNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.Comm1stNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.CommNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.CommNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.Comm15minOu) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.Comm15minOu);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].
                            Style
                            = bgFormat;
                        if (CastAide.AsDecimal(reportDetail.CommNextCorner) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                                Value = CastAide.AsDecimal(reportDetail.CommNextCorner);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = null;
                    }
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.Comm1stOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.Comm1stOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.CommOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.CommOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        = bgFormat;
                    if (CastAide.AsDecimal(reportDetail.CommOverall) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                            Value = CastAide.AsDecimal(reportDetail.CommOverall);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn++)].
                        Value = null;


                    #endregion

                    #region last group

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style
                        =
                        (nextRow % 2 == 0
                             ? ExcelFormat(Color.FromArgb(248, 248, 248),
                                           HorizontalAlignmentStyle.Left, ExcelFont.NormalWeight, false, false)
                             : ExcelFormat(Color.FromArgb(217, 232, 235),
                                           HorizontalAlignmentStyle.Left, ExcelFont.NormalWeight, false, false));
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextColumn)].Value
                        =
                        reportDetail.UserName;

                    #endregion

                    nextRow++;
                }
                #endregion

                #region Footer
                var nextTotalColumn = 0;
                var footerTextStyleSheet = ExcelFormat(Color.FromArgb(159, 183, 213),
                                                            HorizontalAlignmentStyle.Center, ExcelFont.BoldWeight, false, false);
                var footerStyleSheet = ExcelFormat(Color.FromArgb(159, 183, 213),
                                                             HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, true, false);
                var footerStyleSheetPercent = ExcelFormat(Color.FromArgb(159, 183, 213),
                                                             HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, false, true);
                foreach (var reportFooter in trader.footers)
                {
                    #region group 1
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Style =
                        footerTextStyleSheet;
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerTextStyleSheet;
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                        "Total";
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerTextStyleSheet;
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                        reportFooter.MatchNo;
                    #endregion

                    #region win-lose
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.WinLost1st1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.WinLost1st1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.WinLost1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.WinLost1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.WinLost1stHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.WinLost1stHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.WinLostHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.WinLostHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.WinLost1stOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.WinLost1stOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.WinLostOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.WinLostOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    if (isSoccer == 1)
                    {
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.WinLost1stNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.WinLost1stNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.WinLostNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.WinLostNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.WinLost1stNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.WinLost1stNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.WinLostNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.WinLostNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.WinLost15minOu) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.WinLost15minOu);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.WinLostNextCorner) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.WinLostNextCorner);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;
                    }
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.WinLost1stOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.WinLost1stOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.WinLostOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.WinLostOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.WinLostOverall) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.WinLostOverall);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;
                    #endregion

                    #region TurnOver
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.TurnOver1st1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.TurnOver1st1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.TurnOver1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.TurnOver1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.TurnOver1stHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.TurnOver1stHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.TurnOverHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.TurnOverHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.TurnOver1stOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.TurnOver1stOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.TurnOverOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.TurnOverOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    if (isSoccer == 1)
                    {
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.TurnOver1stNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.TurnOver1stNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.TurnOverNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.TurnOverNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.TurnOver1stNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.TurnOver1stNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.TurnOverNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.TurnOverNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.TurnOver15minOu) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.TurnOver15minOu);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.TurnOverNextCorner) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.TurnOverNextCorner);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;
                    }
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.TurnOver1stOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.TurnOver1stOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.TurnOverOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.TurnOverOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.TurnOverOverall) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.TurnOverOverall);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;
                    #endregion

                    #region Live BetCount
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.LiveBetCount1st1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.LiveBetCount1st1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.LiveBetCount1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.LiveBetCount1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.LiveBetCount1stHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.LiveBetCount1stHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.LiveBetCountHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.LiveBetCountHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.LiveBetCount1stOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.LiveBetCount1stOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.LiveBetCountOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.LiveBetCountOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    if (isSoccer == 1)
                    {
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.LiveBetCount1stNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.LiveBetCount1stNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.LiveBetCountNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.LiveBetCountNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                    Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.LiveBetCount1stNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.LiveBetCount1stNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.LiveBetCountNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.LiveBetCountNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.LiveBetCount15minOu) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.LiveBetCount15minOu);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.LiveBetCountNextCorner) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.LiveBetCountNextCorner);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;
                    }
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.LiveBetCount1stOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.LiveBetCount1stOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.LiveBetCountOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.LiveBetCountOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.LiveBetCountOverall) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.LiveBetCountOverall);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;
                    #endregion

                    #region Margin
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheetPercent;
                    if (CastAide.AsDecimal(reportFooter.Margin1st1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.Margin1st1x2) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheetPercent;
                    if (CastAide.AsDecimal(reportFooter.Margin1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.Margin1x2) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheetPercent;
                    if (CastAide.AsDecimal(reportFooter.Margin1stHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.Margin1stHdp) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheetPercent;
                    if (CastAide.AsDecimal(reportFooter.MarginHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.MarginHdp) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheetPercent;
                    if (CastAide.AsDecimal(reportFooter.Margin1stOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.Margin1stOu) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheetPercent;
                    if (CastAide.AsDecimal(reportFooter.MarginOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.MarginOu) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    if (isSoccer == 1)
                    {
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheetPercent;
                        if (CastAide.AsDecimal(reportFooter.Margin1stNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.Margin1stNoOfCorners) / 100;
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheetPercent;
                        if (CastAide.AsDecimal(reportFooter.MarginNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.MarginNoOfCorners) / 100;
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheetPercent;
                        if (CastAide.AsDecimal(reportFooter.Margin1stNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.Margin1stNoOfYellowCards) / 100;
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheetPercent;
                        if (CastAide.AsDecimal(reportFooter.MarginNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.MarginNoOfYellowCards) / 100;
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheetPercent;
                        if (CastAide.AsDecimal(reportFooter.Margin15minOu) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.Margin15minOu) / 100;
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheetPercent;
                        if (CastAide.AsDecimal(reportFooter.MarginNextCorner) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.MarginNextCorner) / 100;
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = null;
                    }
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheetPercent;
                    if (CastAide.AsDecimal(reportFooter.Margin1stOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.Margin1stOe) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;
                  
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheetPercent;
                    if (CastAide.AsDecimal(reportFooter.MarginOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.MarginOe) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheetPercent;
                    if (CastAide.AsDecimal(reportFooter.MarginOverall) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.MarginOverall) / 100;
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;
                    #endregion

                    #region Comm
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.Comm1st1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.Comm1st1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.Comm1x2) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.Comm1x2);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.Comm1stHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.Comm1stHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.CommHdp) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.CommHdp);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.Comm1stOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.Comm1stOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.CommOu) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.CommOu);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    if (isSoccer == 1)
                    {
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.Comm1stNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.Comm1stNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.CommNoOfCorners) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.CommNoOfCorners);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.Comm1stNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.Comm1stNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.CommNoOfYellowCards) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.CommNoOfYellowCards);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.Comm15minOu) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.Comm15minOu);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;

                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style
                            = footerStyleSheet;
                        if (CastAide.AsDecimal(reportFooter.CommNextCorner) != 0)
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                                Value = CastAide.AsDecimal(reportFooter.CommNextCorner);
                        else
                            wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].
                            Value = null;
                    }
                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.Comm1stOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.Comm1stOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.CommOe) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.CommOe);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                    if (CastAide.AsDecimal(reportFooter.CommOverall) != 0)
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value =
                            CastAide.AsDecimal(reportFooter.CommOverall);
                    else
                        wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn++)].Value = null;
                    #endregion

                    wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (nextTotalColumn)].Style =
                        footerStyleSheet;
                }
                #endregion
            }
            else
            {
              //  var bgFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Left,
              //      ExcelFont.BoldWeight, false, false);

              //  wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(nextRow + startRowIndex,
              //      startColumnIndex, startRowIndex + nextRow,
              //      startColumnIndex + 2).Merged =
              //true;
              //  wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(nextRow + startRowIndex, startColumnIndex,
              //                                                      startRowIndex + nextRow,
              //                                                      startColumnIndex + 2).Style =
              //      bgFormat;

                wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(nextRow + startRowIndex, startColumnIndex + (3), nextRow + startRowIndex, startColumnIndex + (3)).Merged = true;
                wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (3)].Style.Font.Weight = ExcelFont.BoldWeight;
                wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (3)].Style.Font.Color = Color.Red;
                wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (3)].
                    Value = "There is no information available.";
            }
            #region response excel file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);
            #endregion
        }

        public void TraderPerformanceDetailExport2Excel(string fromDate, string toDate, int isSoccer)
        {
            var trader = new TraderPerformanceDetailDTO();
            trader = (TraderPerformanceDetailDTO)SessionExtension.GetSession(HttpContext.Current.Session, isSoccer == 1 ? TraderPerformanceDetailIsSoccerSessionKey : TraderPerformanceDetailIsNoSoccerSessionKey) ??
                     GetTraderPerformanceDetail(fromDate, toDate, isSoccer, true);
            TraderPerformanceDetailExport2ExcelFormat(trader, isSoccer, fromDate, toDate);
        }

        private void TraderPerformanceDetailExport2ExcelFormat(TraderPerformanceDetailDTO trader, int isSoccer, string fromDate, string toDate)
        {
            #region local variables
            const string firstOneXTwo = "1st 1x2";
            const string oneXTwo = "1x2";
            const string firstHdp = "1st HDP";
            const string hdp = "HDP";
            const string firstOu = "1st O/U";
            const string ou = "O/U";
            const string firstNoOfCorner = "1st No. of Corner";
            const string noOfCorner = "No. of Corner";
            const string firstNoOfYelowCard = "1st No. of Yellow Card";
            const string noOfYelowCard = "No. of Yellow Card";
            const string fifteenMinsOu = "15mins O/U";
            const string firstOE = "1st O/E";
            const string OE = "O/E";
            const string nextCorner = "Next Corner";
            const string overall = "Overall";

            var reportName = isSoccer == 1 ? "TraderPerformanceDetail_Soccer" : "TraderPerformanceDetail_OtherSports";
            var excelFile = new ExcelFile();
            const int startRowIndex = 3;
            const int startColumnIndex = 0;
            var nextRow = 0;
            var wsTraderPerformanceReport = excelFile.Worksheets.Add(reportName);
            var mergedTotalColumns = isSoccer == 1 ? 14 : 8;
            #endregion
            #region Header
            var bgHeaderFormat = ExcelFormat(Color.FromArgb(159, 183, 213), HorizontalAlignmentStyle.Center, ExcelFont.BoldWeight, false, false);
            var nextHeaderColumn = 0;

            //Title
            wsTraderPerformanceReport.Cells[startRowIndex - 3, startColumnIndex].Style.Font.Weight = ExcelFont.BoldWeight;
            wsTraderPerformanceReport.Cells[startRowIndex - 3, startColumnIndex].Value =
                string.Format("{0} RISK CONTROLLER REPORT BY MATCH FROM {1} TO {2}", (isSoccer == 1 ? "SOCCER SPORT" : "OTHER SPORTS"), fromDate, toDate);
            #region first row
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                       startRowIndex - 1,
                                       startColumnIndex + (nextHeaderColumn)).Merged =
            true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                               startRowIndex - 1,
                                                               startColumnIndex + (nextHeaderColumn)).Style.VerticalAlignment =
               VerticalAlignmentStyle.Center;
            wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value = "No";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                   startRowIndex - 1,
                                                   startColumnIndex + (nextHeaderColumn)).Merged =
                       true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                               startRowIndex - 1,
                                                               startColumnIndex + (nextHeaderColumn)).Style.VerticalAlignment =
               VerticalAlignmentStyle.Center;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                              startRowIndex - 1,
                                                              startColumnIndex + (nextHeaderColumn++)).Value =
                "Trader";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                   startRowIndex - 1,
                                                   startColumnIndex + (nextHeaderColumn)).Merged =
                       true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                               startRowIndex - 1,
                                                               startColumnIndex + (nextHeaderColumn)).Style.VerticalAlignment =
               VerticalAlignmentStyle.Center;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                             startRowIndex - 1,
                                                             startColumnIndex + (nextHeaderColumn++)).Value =
                "Date";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                   startRowIndex - 1,
                                                   startColumnIndex + (nextHeaderColumn)).Merged =
                       true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                               startRowIndex - 1,
                                                               startColumnIndex + (nextHeaderColumn)).Style.VerticalAlignment =
               VerticalAlignmentStyle.Center;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                             startRowIndex - 1,
                                                             startColumnIndex + (nextHeaderColumn++)).Value =
                "League";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                   startRowIndex - 1,
                                                   startColumnIndex + (nextHeaderColumn)).Merged =
                       true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                               startRowIndex - 1,
                                                               startColumnIndex + (nextHeaderColumn)).Style.VerticalAlignment =
               VerticalAlignmentStyle.Center;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                              startRowIndex - 1,
                                                              startColumnIndex + (nextHeaderColumn++)).Value =
                "Match";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex +
                                                                (nextHeaderColumn += 1)).Merged =
                true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn++)).Value =
                "Score";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex +
                                                                (nextHeaderColumn += mergedTotalColumns)).Merged =
                true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn++)).Value =
                "Winlost";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex +
                                                                (nextHeaderColumn += mergedTotalColumns)).Merged =
                true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn++)).Value =
                "Turnover";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex +
                                                                (nextHeaderColumn += mergedTotalColumns)).Merged =
                true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn++)).Value =
                "Live BetCount";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex +
                                                                (nextHeaderColumn += mergedTotalColumns)).Merged =
                true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn++)).Value =
                "Margin%";

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex +
                                                                (nextHeaderColumn += mergedTotalColumns)).Merged =
                true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 2,
                                                                startColumnIndex + (nextHeaderColumn)).Value =
                "Commission";
            wsTraderPerformanceReport.Cells[startRowIndex - 2, startColumnIndex + (nextHeaderColumn++)].Style =
                    bgHeaderFormat;

            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                       startRowIndex - 1,
                                       startColumnIndex + (nextHeaderColumn)).Merged =
            true;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                                startRowIndex - 1,
                                                                startColumnIndex + (nextHeaderColumn)).Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                               startRowIndex - 1,
                                                               startColumnIndex + (nextHeaderColumn)).Style.VerticalAlignment =
               VerticalAlignmentStyle.Center;
            wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(startRowIndex - 2, startColumnIndex + (nextHeaderColumn),
                                                               startRowIndex - 1,
                                                               startColumnIndex + (nextHeaderColumn)).Value =
                "Trader";
            #endregion
            #region second row
            nextHeaderColumn = 0;
            wsTraderPerformanceReport.Rows[startRowIndex - 1].Height = 400;
            wsTraderPerformanceReport.Columns[nextHeaderColumn++].Width = 2000;
            wsTraderPerformanceReport.Columns[nextHeaderColumn++].Width = 4000;
            wsTraderPerformanceReport.Columns[nextHeaderColumn++].Width = 2000;
            wsTraderPerformanceReport.Columns[nextHeaderColumn++].Width = 8000;
            wsTraderPerformanceReport.Columns[nextHeaderColumn++].Width = 8000;

            wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 2000;
            wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                "HT";

            wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 2000;
            wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                bgHeaderFormat;
            wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
            wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                "FT";
            #endregion
            #region group header
            for (var i = 0; i < 5; i++)
            {
                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    firstOneXTwo;

                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    oneXTwo;

                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    firstHdp;

                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    hdp;

                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    firstOu;

                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    ou;

                //remove
                if (isSoccer == 1)
                {
                    wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style
                        =
                        bgHeaderFormat;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].
                        Value =
                        firstNoOfCorner;

                    wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style
                        =
                        bgHeaderFormat;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].
                        Value =
                        noOfCorner;

                    wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style
                        =
                        bgHeaderFormat;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].
                        Value =
                        firstNoOfYelowCard;

                    wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style
                        =
                        bgHeaderFormat;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].
                        Value =
                        noOfYelowCard;

                    wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style
                        =
                        bgHeaderFormat;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].
                        Value =
                        fifteenMinsOu;

                    wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style
                        =
                        bgHeaderFormat;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].
                        Value =
                        nextCorner;
                }
                //----
                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    firstOE;

                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    OE;

                wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style =
                    bgHeaderFormat;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn)].Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsTraderPerformanceReport.Cells[startRowIndex - 1, startColumnIndex + (nextHeaderColumn++)].Value =
                    overall;

            }

            #endregion

            wsTraderPerformanceReport.Columns[nextHeaderColumn].Width = 4000;

            #endregion
            if (trader != null && trader.rows != null && trader.rows.Count() > 0)
            {
                #region Body
                var parentIndex = 1;

                foreach (var traderReportDetail in trader.rows)
                {
                    #region group1: No; Trader; Date; League; Match

                    var nextColumn = 0;
                    if (traderReportDetail.children.Count() > 0)
                    {
                        TraderPerformanceDetailRenderLeftRow(traderReportDetail, ref wsTraderPerformanceReport, ref nextRow,
                                                         ref nextColumn, startRowIndex, startColumnIndex, isSoccer, parentIndex);
                    }
                    nextColumn = 0;
                    TraderPerformanceDetaiRenderLeftTotal(traderReportDetail.total, ref wsTraderPerformanceReport, ref nextRow,
                                                          ref nextColumn, startRowIndex, startColumnIndex, false);
                    TraderPerformanceDetailRenderRightRow(traderReportDetail.total, ref wsTraderPerformanceReport, ref nextRow,
                                                          ref nextColumn, startRowIndex, startColumnIndex, isSoccer, false);
                    parentIndex++;

                    #endregion
                }

                #endregion

                #region Footer
                var nextColumnFooter = 0;
                TraderPerformanceDetaiRenderLeftTotal(trader.footer, ref wsTraderPerformanceReport, ref nextRow,
                                                      ref nextColumnFooter, startRowIndex, startColumnIndex, true);
                TraderPerformanceDetailRenderRightRow(trader.footer, ref wsTraderPerformanceReport, ref nextRow,
                                                      ref nextColumnFooter, startRowIndex, startColumnIndex, isSoccer, true);
                #endregion
            }
            else
            {
              //  var bgFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Left,
              //      ExcelFont.BoldWeight, false, false);

              //  wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(nextRow + startRowIndex,
              //      startColumnIndex, startRowIndex + nextRow,
              //      startColumnIndex + 4).Merged =
              //true;
              //  wsTraderPerformanceReport.Cells.GetSubrangeAbsolute(nextRow + startRowIndex, startColumnIndex,
              //                                                      startRowIndex + nextRow,
              //                                                      startColumnIndex + 4).Style =
              //      bgFormat;

                wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (5)].Style.Font.Weight = ExcelFont.BoldWeight;
                wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (5)].Style.Font.Color = Color.Red;
                wsTraderPerformanceReport.Cells[nextRow + startRowIndex, startColumnIndex + (5)].
                    Value = "There is no information available.";
            }

            #region response excel file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);
            #endregion
        }

        private static void TraderPerformanceDetailRenderLeftRow(TraderPerformanceDetailBO trader, ref ExcelWorksheet worksheet, ref int rowIndex,
            ref int colIndex, int startRow, int startCol, int isSoccer, int parentIndex)
        {
            var childIndex = 0;
            var childrenIndex = 0;
            int maxTextLengthPerLine = 39;
            int currentTextLength = 0;
            int currentRowHeight = 0;
            var totalChildren = (trader.children != null) ? trader.children.Count() : 0;
            foreach (var traderDetail in trader.children)
            {
                colIndex = 0;
                var bgFormat = ExcelFormat(childIndex % 2 == 0 ? Color.FromArgb(248, 248, 248) : Color.FromArgb(217, 232, 235), HorizontalAlignmentStyle.Center,
                                       ExcelFont.NormalWeight, true, false);
                var leftbgTextFormat = ExcelFormat(Color.FromArgb(248, 248, 248), HorizontalAlignmentStyle.Center,
                                    ExcelFont.NormalWeight, false, false);
                var bgTextFormat = ExcelFormat(childIndex % 2 == 0 ? Color.FromArgb(248, 248, 248) : Color.FromArgb(217, 232, 235), HorizontalAlignmentStyle.Center,
                                     ExcelFont.NormalWeight, false, false);
                var bgPercentFormat = ExcelFormat(childIndex % 2 == 0 ? Color.FromArgb(248, 248, 248) : Color.FromArgb(217, 232, 235), HorizontalAlignmentStyle.Right,
                                     ExcelFont.NormalWeight, false, true);
                #region leftcolumn
                currentTextLength = (traderDetail.League.Length > traderDetail.Match.Length) ? traderDetail.League.Length : traderDetail.Match.Length;
                if (maxTextLengthPerLine != 0 && currentTextLength > 0)
                {
                    currentRowHeight = ((int)Math.Ceiling((double)currentTextLength / maxTextLengthPerLine)) * 220;
                }
                else
                {
                    currentRowHeight = 220;
                }
                worksheet.Rows[rowIndex + startRow].Height = currentRowHeight;
                if (childIndex == 0)
                {
                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                        startRow + rowIndex + totalChildren - 1,
                                                        startCol + (colIndex)).Merged =
                        true;
                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                        startRow + rowIndex + totalChildren - 1,
                                                        startCol + (colIndex)).Style = leftbgTextFormat;
                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                         startRow + rowIndex + totalChildren - 1,
                                         startCol + (colIndex)).Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                        startRow + rowIndex + totalChildren - 1,
                                                        startCol + (colIndex)).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                        startRow + rowIndex + totalChildren - 1,
                                                        startCol + (colIndex++)).Value = parentIndex;

                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                        startRow + rowIndex + totalChildren - 1,
                                                        startCol + (colIndex)).Merged = true;
                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                         startRow + rowIndex + totalChildren - 1,
                                                         startCol + (colIndex)).Style = leftbgTextFormat;
                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                        startRow + rowIndex + totalChildren - 1,
                                                        startCol + (colIndex)).Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                         startRow + rowIndex + totalChildren - 1,
                                                         startCol + (colIndex)).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                        startRow + rowIndex + totalChildren - 1,
                                                        startCol + (colIndex++)).Value = traderDetail.UserName;
                }
                else
                {
                    colIndex += 2;
                }

                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                 startRow + rowIndex,
                                 startCol + (colIndex)).Merged =
                    true;
                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                    startRow + rowIndex,
                                                    startCol + (colIndex)).Style = bgTextFormat;
                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                    startRow + rowIndex,
                                                    startCol + (colIndex)).Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                    startRow + rowIndex,
                                                    startCol + (colIndex)).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                    startRow + rowIndex,
                                                    startCol + (colIndex++)).
                    Value = traderDetail.TransDate;

                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                    startRow + rowIndex,
                                                    startCol + (colIndex)).Merged =
                    true;
                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                    startRow + rowIndex,
                                                    startCol + (colIndex)).Style = bgTextFormat;
                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                startRow + rowIndex,
                                startCol + (colIndex)).Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                    startRow + rowIndex,
                                                    startCol + (colIndex)).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                    startRow + rowIndex,
                                                    startCol + (colIndex++)).
                    Value = traderDetail.League;

                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                    startRow + rowIndex,
                                                    startCol + (colIndex)).Merged =
                    true;
                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                    startRow + rowIndex,
                                                    startCol + (colIndex)).Style = bgTextFormat;
                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                              startRow + rowIndex,
                              startCol + (colIndex)).Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                    startRow + rowIndex,
                                                    startCol + (colIndex)).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                    startRow + rowIndex,
                                                    startCol + (colIndex++)).
                    Value = traderDetail.Match;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgTextFormat;
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = traderDetail.HT;
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgTextFormat;
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = traderDetail.FT;
                #endregion

                #region win-lose
                bgFormat = ExcelFormat(Color.FromArgb(231, 237, 169), HorizontalAlignmentStyle.Right,
                                       ExcelFont.NormalWeight, true, false);

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.WinLost1st1x2) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.WinLost1st1x2);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.WinLost1x2) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.WinLost1x2);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.WinLost1stHdp) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.WinLost1stHdp);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.WinLostHdp) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.WinLostHdp);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.WinLost1stOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.WinLost1stOu);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.WinLostOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.WinLostOu);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                if (isSoccer == 1)
                {
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.WinLost1stNoOfCorners) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.WinLost1stNoOfCorners);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.WinLostNoOfCorners) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.WinLostNoOfCorners);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.WinLost1stNoOfYellowCards) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.WinLost1stNoOfYellowCards);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.WinLostNoOfYellowCards) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.WinLostNoOfYellowCards);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.WinLost15minOu) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.WinLost15minOu);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.WinLostNextCorner) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.WinLostNextCorner);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;
                }
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.WinLost1stOe) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.WinLost1stOe);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.WinLostOe) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.WinLostOe);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.WinLostOverall) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.WinLostOverall);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;


                #endregion

                #region turnover
                bgFormat = ExcelFormat(Color.FromArgb(209, 227, 231), HorizontalAlignmentStyle.Right,
                                       ExcelFont.NormalWeight, true, false);
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.TurnOver1st1x2) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.TurnOver1st1x2);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.TurnOver1x2) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.TurnOver1x2);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.TurnOver1stHdp) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.TurnOver1stHdp);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.TurnOverHdp) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.TurnOverHdp);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.TurnOver1stOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.TurnOver1stOu);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.TurnOverOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.TurnOverOu);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                if (isSoccer == 1)
                {
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.TurnOver1stNoOfCorners) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.TurnOver1stNoOfCorners);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.TurnOverNoOfCorners) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.TurnOverNoOfCorners);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.TurnOver1stNoOfYellowCards) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.TurnOver1stNoOfYellowCards);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.TurnOverNoOfYellowCards) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.TurnOverNoOfYellowCards);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.TurnOver15minOu) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.TurnOver15minOu);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.TurnOverNextCorner) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.TurnOverNextCorner);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;
                }
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.TurnOver1stOe) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.TurnOver1stOe);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.TurnOverOe) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.TurnOverOe);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.TurnOverOverall) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.TurnOverOverall);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                #endregion

                #region live betcount
                bgFormat = ExcelFormat(Color.FromArgb(253, 207, 246),
                                       HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, true, false);
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.LiveBetCount1st1x2) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.LiveBetCount1st1x2);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.LiveBetCount1x2) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.LiveBetCount1x2);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.LiveBetCount1stHdp) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.LiveBetCount1stHdp);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.LiveBetCountHdp) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.LiveBetCountHdp);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.LiveBetCount1stOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.LiveBetCount1stOu);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.LiveBetCountOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.LiveBetCountOu);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                if (isSoccer == 1)
                {
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.LiveBetCount1stNoOfCorners) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.LiveBetCount1stNoOfCorners);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.LiveBetCountNoOfCorners) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.LiveBetCountNoOfCorners);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.LiveBetCount1stNoOfYellowCards) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.LiveBetCount1stNoOfYellowCards);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.LiveBetCountNoOfYellowCards) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.LiveBetCountNoOfYellowCards);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.LiveBetCount15minOu) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.LiveBetCount15minOu);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.LiveBetCountNextCorner) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.LiveBetCountNextCorner);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;
                }
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.LiveBetCount1stOe) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.LiveBetCount1stOe);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.LiveBetCountOe) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.LiveBetCountOe);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.LiveBetCountOverall) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.LiveBetCountOverall);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;


                #endregion

                #region margin
                bgFormat = ExcelFormat(Color.FromArgb(202, 222, 230),
                                       HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, true, false);
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(traderDetail.Margin1st1x2) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.Margin1st1x2) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(traderDetail.Margin1x2) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.Margin1x2) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(traderDetail.Margin1stHdp) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.Margin1stHdp) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(traderDetail.MarginHdp) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.MarginHdp) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(traderDetail.Margin1stOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.Margin1stOu) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(traderDetail.MarginOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.MarginOu) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                if (isSoccer == 1)
                {
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgPercentFormat;
                    if (CastAide.AsDecimal(traderDetail.Margin1stNoOfCorners) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.Margin1stNoOfCorners) / 100;
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgPercentFormat;
                    if (CastAide.AsDecimal(traderDetail.MarginNoOfCorners) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.MarginNoOfCorners) / 100;
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgPercentFormat;
                    if (CastAide.AsDecimal(traderDetail.Margin1stNoOfYellowCards) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.Margin1stNoOfYellowCards) / 100;
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgPercentFormat;
                    if (CastAide.AsDecimal(traderDetail.MarginNoOfYellowCards) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.MarginNoOfYellowCards) / 100;
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgPercentFormat;
                    if (CastAide.AsDecimal(traderDetail.Margin15minOu) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.Margin15minOu) / 100;
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgPercentFormat;
                    if (CastAide.AsDecimal(traderDetail.MarginNextCorner) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.MarginNextCorner) / 100;
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;
                }
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(traderDetail.Margin1stOe) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.Margin1stOe) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(traderDetail.MarginOe) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.MarginOe) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(traderDetail.MarginOverall) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.MarginOverall) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;


                #endregion

                #region comm
                bgFormat = ExcelFormat(Color.FromArgb(255, 255, 238),
                                       HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, true, false);
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.Comm1st1x2) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.Comm1st1x2);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.Comm1x2) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.Comm1x2);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.Comm1stHdp) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.Comm1stHdp);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.CommHdp) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.CommHdp);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                   Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.Comm1stOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.Comm1stOu);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.CommOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.CommOu);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;
                if (isSoccer == 1)
                {
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.Comm1stNoOfCorners) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.Comm1stNoOfCorners);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.CommNoOfCorners) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.CommNoOfCorners);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.Comm1stNoOfYellowCards) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.Comm1stNoOfYellowCards);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.CommNoOfYellowCards) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.CommNoOfYellowCards);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.Comm15minOu) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.Comm15minOu);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = null;

                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                        Style
                        = bgFormat;
                    if (CastAide.AsDecimal(traderDetail.CommNextCorner) != 0)
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                            Value = CastAide.AsDecimal(traderDetail.CommNextCorner);
                    else
                        worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = null;
                }
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.Comm1stOe) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.Comm1stOe);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.CommOe) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.CommOe);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                    = bgFormat;
                if (CastAide.AsDecimal(traderDetail.CommOverall) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(traderDetail.CommOverall);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;


                #endregion

                #region last column

                if (childIndex == 0)
                {
                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                        startRow + rowIndex + totalChildren - 1,
                                                        startCol + (colIndex)).Merged = true;
                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                        startRow + rowIndex + totalChildren - 1,
                                                        startCol + (colIndex)).Style = leftbgTextFormat;
                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                        startRow + rowIndex + totalChildren - 1,
                                                        startCol + (colIndex)).Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                        startRow + rowIndex + totalChildren - 1,
                                                        startCol + (colIndex)).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                    worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                        startRow + rowIndex + totalChildren - 1,
                                                        startCol + (colIndex++)).Value = traderDetail.UserName;
                }

                #endregion

                childIndex++;
                rowIndex++;
            }
        }

        private static void TraderPerformanceDetaiRenderLeftTotal(TraderPerformanceDetail trader, ref ExcelWorksheet worksheet,
            ref int rowIndex, ref int colIndex, int startRowIndex, int startColumnIndex, bool isGrandTotal)
        {
            var bgFormat = ExcelFormat(isGrandTotal ? Color.FromArgb(159, 183, 213) : Color.FromArgb(204, 255, 204), HorizontalAlignmentStyle.Left,
                                       ExcelFont.BoldWeight, false, false);


            worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRowIndex, startColumnIndex + (colIndex),
                                                                startRowIndex + rowIndex,
                                                                startColumnIndex + (colIndex + 4)).Merged =
                true;
            worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRowIndex, startColumnIndex,
                                                                startRowIndex + rowIndex,
                                                                startColumnIndex + (colIndex + 4)).Style =
                bgFormat;
            worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRowIndex, startColumnIndex,
                                                            startRowIndex + rowIndex,
                                                            startColumnIndex + (colIndex + 4)).Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            worksheet.Cells[rowIndex + startRowIndex, startColumnIndex + (colIndex + 4)].
                Value = isGrandTotal ? "Grand Total" : trader.UserName;
            colIndex += 5;

        }

        private static void TraderPerformanceDetailRenderRightRow(TraderPerformanceDetail trader, ref ExcelWorksheet worksheet,
            ref int rowIndex, ref int colIndex, int startRow, int startCol, int isSoccer, bool isGrandTotal)
        {
            var bgTextFormat = ExcelFormat(isGrandTotal ? Color.FromArgb(159, 183, 213) : Color.FromArgb(204, 255, 204), HorizontalAlignmentStyle.Right,
                                       ExcelFont.BoldWeight, false, false);
            var bgFormat = ExcelFormat(isGrandTotal ? Color.FromArgb(159, 183, 213) : Color.FromArgb(204, 255, 204), HorizontalAlignmentStyle.Right,
                                    ExcelFont.BoldWeight, true, false);
            var bgPercentFormat = ExcelFormat(isGrandTotal ? Color.FromArgb(159, 183, 213) : Color.FromArgb(204, 255, 204), HorizontalAlignmentStyle.Right,
                                     ExcelFont.BoldWeight, false, true);

            worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                          startRow + rowIndex,
                                                          startCol + (colIndex + 1)).Merged = true;
            worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                          startRow + rowIndex,
                                                          startCol + (colIndex + 1)).Style
                = bgTextFormat;
            worksheet.Cells.GetSubrangeAbsolute(rowIndex + startRow, startCol + (colIndex),
                                                          startRow + rowIndex,
                                                          startCol + (colIndex + 1)).
                Value = null;
            colIndex += 2;
            #region win-lose
            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.WinLost1st1x2) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.WinLost1st1x2);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.WinLost1x2) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.WinLost1x2);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.WinLost1stHdp) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.WinLost1stHdp);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.WinLostHdp) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.WinLostHdp);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.WinLost1stOu) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.WinLost1stOu);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.WinLostOu) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.WinLostOu);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;
            if (isSoccer == 1)
            {
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.WinLost1stNoOfCorners) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.WinLost1stNoOfCorners);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.WinLostNoOfCorners) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.WinLostNoOfCorners);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.WinLost1stNoOfYellowCards) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.WinLost1stNoOfYellowCards);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.WinLostNoOfYellowCards) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.WinLostNoOfYellowCards);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.WinLost15minOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.WinLost15minOu);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.WinLostNextCorner) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.WinLostNextCorner);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;
            }
            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.WinLost1stOe) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.WinLost1stOe);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.WinLostOe) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.WinLostOe);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.WinLostOverall) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.WinLostOverall);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            #endregion

            #region turnover
            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.TurnOver1st1x2) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.TurnOver1st1x2);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.TurnOver1x2) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.TurnOver1x2);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.TurnOver1stHdp) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.TurnOver1stHdp);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.TurnOverHdp) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.TurnOverHdp);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.TurnOver1stOu) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.TurnOver1stOu);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.TurnOverOu) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.TurnOverOu);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;
            if (isSoccer == 1)
            {
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.TurnOver1stNoOfCorners) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.TurnOver1stNoOfCorners);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.TurnOverNoOfCorners) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.TurnOverNoOfCorners);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.TurnOver1stNoOfYellowCards) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.TurnOver1stNoOfYellowCards);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.TurnOverNoOfYellowCards) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.TurnOverNoOfYellowCards);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.TurnOver15minOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.TurnOver15minOu);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.TurnOverNextCorner) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.TurnOverNextCorner);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;
            }
            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.TurnOver1stOe) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.TurnOver1stOe);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.TurnOverOe) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.TurnOverOe);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.TurnOverOverall) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.TurnOverOverall);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            #endregion

            #region live betcount
            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.LiveBetCount1st1x2) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.LiveBetCount1st1x2);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.LiveBetCount1x2) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.LiveBetCount1x2);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.LiveBetCount1stHdp) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.LiveBetCount1stHdp);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.LiveBetCountHdp) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.LiveBetCountHdp);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.LiveBetCount1stOu) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.LiveBetCount1stOu);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.LiveBetCountOu) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.LiveBetCountOu);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;
            if (isSoccer == 1)
            {
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.LiveBetCount1stNoOfCorners) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.LiveBetCount1stNoOfCorners);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.LiveBetCountNoOfCorners) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.LiveBetCountNoOfCorners);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.LiveBetCount1stNoOfYellowCards) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.LiveBetCount1stNoOfYellowCards);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.LiveBetCountNoOfYellowCards) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.LiveBetCountNoOfYellowCards);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.LiveBetCount15minOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.LiveBetCount15minOu);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.LiveBetCountNextCorner) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.LiveBetCountNextCorner);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;
            }
            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.LiveBetCount1stOe) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.LiveBetCount1stOe);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.LiveBetCountOe) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.LiveBetCountOe);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.LiveBetCountOverall) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.LiveBetCountOverall);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            #endregion

            #region margin
            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgPercentFormat;
            if (CastAide.AsDecimal(trader.Margin1st1x2) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.Margin1st1x2) / 100;
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgPercentFormat;
            if (CastAide.AsDecimal(trader.Margin1x2) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.Margin1x2) / 100;
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgPercentFormat;
            if (CastAide.AsDecimal(trader.Margin1stHdp) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.Margin1stHdp) / 100;
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgPercentFormat;
            if (CastAide.AsDecimal(trader.MarginHdp) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.MarginHdp) / 100;
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgPercentFormat;
            if (CastAide.AsDecimal(trader.Margin1stOu) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.Margin1stOu) / 100;
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgPercentFormat;
            if (CastAide.AsDecimal(trader.MarginOu) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.MarginOu) / 100;
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            if (isSoccer == 1)
            {
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(trader.Margin1stNoOfCorners) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.Margin1stNoOfCorners) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(trader.MarginNoOfCorners) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.MarginNoOfCorners) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(trader.Margin1stNoOfYellowCards) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.Margin1stNoOfYellowCards) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(trader.MarginNoOfYellowCards) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.MarginNoOfYellowCards) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                   Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(trader.Margin15minOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.Margin15minOu) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgPercentFormat;
                if (CastAide.AsDecimal(trader.MarginNextCorner) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.MarginNextCorner) / 100;
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;
            }
            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgPercentFormat;
            if (CastAide.AsDecimal(trader.Margin1stOe) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.Margin1stOe) / 100;
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgPercentFormat;
            if (CastAide.AsDecimal(trader.MarginOe) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.MarginOe) / 100;
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgPercentFormat;
            if (CastAide.AsDecimal(trader.MarginOverall) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.MarginOverall) / 100;
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            #endregion

            #region comm
            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.Comm1st1x2) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.Comm1st1x2);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.Comm1x2) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.Comm1x2);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.Comm1stHdp) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.Comm1stHdp);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
               Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.CommHdp) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.CommHdp);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.Comm1stOu) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.Comm1stOu);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.CommOu) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.CommOu);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;
            if (isSoccer == 1)
            {
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.Comm1stNoOfCorners) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.Comm1stNoOfCorners);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.CommNoOfCorners) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.CommNoOfCorners);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.Comm1stNoOfYellowCards) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.Comm1stNoOfYellowCards);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.CommNoOfYellowCards) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.CommNoOfYellowCards);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.Comm15minOu) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.Comm15minOu);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;

                worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].
                    Style
                    = bgFormat;
                if (CastAide.AsDecimal(trader.CommNextCorner) != 0)
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                        Value = CastAide.AsDecimal(trader.CommNextCorner);
                else
                    worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = null;
            }
            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.Comm1stOe) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.Comm1stOe);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.CommOe) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.CommOe);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgFormat;
            if (CastAide.AsDecimal(trader.CommOverall) != 0)
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                    Value = CastAide.AsDecimal(trader.CommOverall);
            else
                worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;


            #endregion

            worksheet.Cells[rowIndex + startRow, startCol + (colIndex)].Style
                = bgTextFormat;
            worksheet.Cells[rowIndex + startRow, startCol + (colIndex++)].
                Value = null;
            rowIndex++;
        }

        private static CellStyle ExcelFormat(Color backgroundColor, HorizontalAlignmentStyle alignmentStyle, int font, bool isDecFormat, bool isPercent)
        {
            var excelFormat = new CellStyle();

            excelFormat.FillPattern.SetSolid(backgroundColor);
            excelFormat.Borders.SetBorders(MultipleBorders.Outside, Color.FromArgb(65, 116, 177), LineStyle.Thin);
            excelFormat.Font.Weight = font;
            excelFormat.HorizontalAlignment = alignmentStyle;
            excelFormat.WrapText = true;
            excelFormat.Font.Name = "Tahoma";
            excelFormat.Font.Size = 160;

            if (isDecFormat)
                excelFormat.NumberFormat = "#,##0.00_);[RED](-#,##0.00); ";
            if (isPercent)
                excelFormat.NumberFormat = "#,##0.00%_);[RED](-#,##0.00%); ";
            return excelFormat;
        }
        #endregion

        #region get traderperformance report detail
        public TraderPerformanceDetailDTO GetTraderPerformanceDetail(string fromDate, string toDate, int isSoccer, bool isGetNewData)
        {
            #region get old data
            if (isGetNewData == false)
            {
                var getOldData = new TraderPerformanceDetailDTO();
                getOldData = (TraderPerformanceDetailDTO)SessionExtension.GetSession(HttpContext.Current.Session, isSoccer == 1 ? TraderPerformanceDetailIsSoccerSessionKey : TraderPerformanceDetailIsNoSoccerSessionKey) ??
                         GetTraderPerformanceDetail(fromDate, toDate, isSoccer, true);
                return getOldData;
            }
            #endregion

            #region get new data
            var result = new TraderPerformanceDetailDTO();
            DateTime fDate, tDate;
            if (!Validator.ValidDateMMDDYYYYNotNull(fromDate, out fDate).IsSuccess() ||
                !Validator.ValidDateMMDDYYYYNotNull(toDate, out tDate).IsSuccess())
            {
                return result;
            }
            tDate = new DateTime(tDate.Year, tDate.Month, tDate.Day, 23, 59, 59);
            #region get data
            DbResult rs = null;
            //if (tDate < ReportUtils.BalanceUp.MoveBetsDay)
            //    return result;
            rs = AccountReport.Acc_TraderPerformanceDetail(fDate, tDate, isSoccer);
            #endregion

            var traderDataBO = new TraderPerformanceDetailBO();
            try
            {
                if (rs != null && rs.ResultSet.Tables.Count > 0 && rs.ResultSet.Tables[0].Rows.Count > 0)
                {
                    var dt = rs.ResultSet.Tables[0];
                    var preCusId = 0;
                    var preUsername = string.Empty;

                    #region turnover
                    decimal to_hthdp = 0,
                            to_hdp = 0,
                            to_htou = 0,
                            to_ou = 0,
                            to_ht1x2 = 0,
                            to_1x2 = 0,
                            to_1stcorner = 0,
                            to_corner = 0,
                            to_15mou = 0,
                            to_1stYellowCard = 0,
                            to_YellowCard = 0,
                            to_overall = 0,
                            to_1stoe = 0,
                            to_NextCorner = 0,
                            to_oe = 0;
                    decimal Total_to_hthdp = 0,
                            Total_to_hdp = 0,
                            Total_to_htou = 0,
                            Total_to_ou = 0,
                            Total_to_ht1x2 = 0,
                            Total_to_1x2 = 0,
                            Total_to_1stcorner = 0,
                            Total_to_corner = 0,
                            Total_to_15mou = 0,
                            Total_to_1stYellowCard = 0,
                            Total_to_YellowCard = 0,
                            Total_to_overall = 0,
                            Total_to_1stoe = 0,
                            Total_to_NextCorner = 0,
                            Total_to_oe = 0;
                    decimal Grand_to_hthdp = 0,
                            Grand_to_hdp = 0,
                            Grand_to_htou = 0,
                            Grand_to_ou = 0,
                            Grand_to_ht1x2 = 0,
                            Grand_to_1x2 = 0,
                            Grand_to_1stcorner = 0,
                            Grand_to_corner = 0,
                            Grand_to_15mou = 0,
                            Grand_to_1stYellowCard = 0,
                            Grand_to_YellowCard = 0,
                            Grand_to_overall = 0,
                            Grand_to_1stoe = 0,
                            Grand_to_NextCorner = 0,
                            Grand_to_oe = 0;
                    #endregion

                    #region livebetcount
                    decimal livebetcount_hthdp = 0,
                            livebetcount_hdp = 0,
                            livebetcount_htou = 0,
                            livebetcount_ou = 0,
                            livebetcount_ht1x2 = 0,
                            livebetcount_1x2 = 0,
                            livebetcount_1stcorner = 0,
                            livebetcount_corner = 0,
                            livebetcount_15mou = 0,
                            livebetcount_1stYellowCard = 0,
                            livebetcount_YellowCard = 0,
                            livebetcount_overall = 0,
                            livebetcount_1stoe = 0,
                            livebetcount_NextCorner = 0,
                            livebetcount_oe = 0;
                    decimal Total_livebetcount_hthdp = 0,
                            Total_livebetcount_hdp = 0,
                            Total_livebetcount_htou = 0,
                            Total_livebetcount_ou = 0,
                            Total_livebetcount_ht1x2 = 0,
                            Total_livebetcount_1x2 = 0,
                            Total_livebetcount_1stcorner = 0,
                            Total_livebetcount_corner = 0,
                            Total_livebetcount_15mou = 0,
                            Total_livebetcount_1stYellowCard = 0,
                            Total_livebetcount_YellowCard = 0,
                            Total_livebetcount_overall = 0,
                            Total_livebetcount_1stoe = 0,
                            Total_livebetcount_NextCorner = 0,
                            Total_livebetcount_oe = 0;
                    decimal Grand_livebetcount_hthdp = 0,
                            Grand_livebetcount_hdp = 0,
                            Grand_livebetcount_htou = 0,
                            Grand_livebetcount_ou = 0,
                            Grand_livebetcount_ht1x2 = 0,
                            Grand_livebetcount_1x2 = 0,
                            Grand_livebetcount_1stcorner = 0,
                            Grand_livebetcount_corner = 0,
                            Grand_livebetcount_15mou = 0,
                            Grand_livebetcount_1stYellowCard = 0,
                            Grand_livebetcount_YellowCard = 0,
                            Grand_livebetcount_overall = 0,
                            Grand_livebetcount_1stoe = 0,
                            Grand_livebetcount_NextCorner = 0,
                            Grand_livebetcount_oe = 0;
                    #endregion

                    #region winlost
                    decimal wl_hthdp = 0,
                            wl_hdp = 0,
                            wl_htou = 0,
                            wl_ou = 0,
                            wl_ht1x2 = 0,
                            wl_1x2 = 0,
                            wl_1stcorner = 0,
                            wl_corner = 0,
                            wl_15mou = 0,
                            wl_1stYellowCard = 0,
                            wl_YellowCard = 0,
                            wl_overall = 0,
                            wl_1stoe = 0,
                            wl_NextCorner = 0,
                            wl_oe = 0;
                    decimal Total_wl_hthdp = 0,
                            Total_wl_hdp = 0,
                            Total_wl_htou = 0,
                            Total_wl_ou = 0,
                            Total_wl_ht1x2 = 0,
                            Total_wl_1x2 = 0,
                            Total_wl_1stcorner = 0,
                            Total_wl_corner = 0,
                            Total_wl_15mou = 0,
                            Total_wl_1stYellowCard = 0,
                            Total_wl_YellowCard = 0,
                            Total_wl_overall = 0,
                            Total_wl_1stoe = 0,
                            Total_wl_NextCorner = 0,
                            Total_wl_oe = 0;
                    decimal Grand_wl_hthdp = 0,
                            Grand_wl_hdp = 0,
                            Grand_wl_htou = 0,
                            Grand_wl_ou = 0,
                            Grand_wl_ht1x2 = 0,
                            Grand_wl_1x2 = 0,
                            Grand_wl_1stcorner = 0,
                            Grand_wl_corner = 0,
                            Grand_wl_15mou = 0,
                            Grand_wl_1stYellowCard = 0,
                            Grand_wl_YellowCard = 0,
                            Grand_wl_overall = 0,
                            Grand_wl_1stoe = 0,
                            Grand_wl_NextCorner = 0,
                            Grand_wl_oe = 0;
                    #endregion

                    #region comm
                    decimal comm_hthdp = 0,
                            comm_hdp = 0,
                            comm_htou = 0,
                            comm_ou = 0,
                            comm_ht1x2 = 0,
                            comm_1x2 = 0,
                            comm_1stcorner = 0,
                            comm_corner = 0,
                            comm_15mou = 0,
                            comm_1stYellowCard = 0,
                            comm_YellowCard = 0,
                            comm_overall = 0,
                            comm_1stoe = 0,
                            comm_NextCorner = 0,
                            comm_oe = 0;
                    decimal Total_comm_hthdp = 0,
                            Total_comm_hdp = 0,
                            Total_comm_htou = 0,
                            Total_comm_ou = 0,
                            Total_comm_ht1x2 = 0,
                            Total_comm_1x2 = 0,
                            Total_comm_1stcorner = 0,
                            Total_comm_corner = 0,
                            Total_comm_15mou = 0,
                            Total_comm_1stYellowCard = 0,
                            Total_comm_YellowCard = 0,
                            Total_comm_overall = 0,
                            Total_comm_1stoe = 0,
                            Total_comm_NextCorner = 0,
                            Total_comm_oe = 0;
                    decimal Grand_comm_hthdp = 0,
                            Grand_comm_hdp = 0,
                            Grand_comm_htou = 0,
                            Grand_comm_ou = 0,
                            Grand_comm_ht1x2 = 0,
                            Grand_comm_1x2 = 0,
                            Grand_comm_1stcorner = 0,
                            Grand_comm_corner = 0,
                            Grand_comm_15mou = 0,
                            Grand_comm_1stYellowCard = 0,
                            Grand_comm_YellowCard = 0,
                            Grand_comm_overall = 0,
                            Grand_comm_1stoe = 0,
                            Grand_comm_NextCorner = 0,
                            Grand_comm_oe = 0;
                    #endregion

                    #region margin
                    decimal m_hthdp = 0,
                           m_hdp = 0,
                           m_htou = 0,
                           m_ou = 0,
                           m_ht1x2 = 0,
                           m_1x2 = 0,
                           m_1stcorner = 0,
                           m_corner = 0,
                           m_15mou = 0,
                           m_1stYellowCard = 0,
                           m_YellowCard = 0,
                           m_overall = 0,
                           m_1stoe = 0,
                           m_NextCorner = 0,
                           m_oe = 0;
                    decimal Total_m_hthdp = 0,
                           Total_m_hdp = 0,
                           Total_m_htou = 0,
                           Total_m_ou = 0,
                           Total_m_ht1x2 = 0,
                           Total_m_1x2 = 0,
                           Total_m_1stcorner = 0,
                           Total_m_corner = 0,
                           Total_m_15mou = 0,
                           Total_m_1stYellowCard = 0,
                           Total_m_YellowCard = 0,
                           Total_m_overall = 0,
                           Total_m_1stoe = 0,
                           Total_m_NextCorner = 0,
                           Total_m_oe = 0;
                    decimal Grand_m_hthdp = 0,
                           Grand_m_hdp = 0,
                           Grand_m_htou = 0,
                           Grand_m_ou = 0,
                           Grand_m_ht1x2 = 0,
                           Grand_m_1x2 = 0,
                           Grand_m_1stcorner = 0,
                           Grand_m_corner = 0,
                           Grand_m_15mou = 0,
                           Grand_m_1stYellowCard = 0,
                           Grand_m_YellowCard = 0,
                           Grand_m_overall = 0,
                           Grand_m_1stoe = 0,
                           Grand_m_NextCorner = 0,
                           Grand_m_oe = 0;
                    #endregion

                    var subCount = 0;
                    var traderChildren = new List<TraderPerformanceDetail>();
                    var traderDataList = new List<TraderPerformanceDetailBO>();
                    int currentCustId = 0;
                    string currentUserName = "";
                    if (dt.Rows.Count > 0)
                    {
                        preCusId = CastAide.AsInt32(dt.Rows[0]["custid"]);
                        preUsername = dt.Rows[0]["username"].ToString();
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        currentCustId = CastAide.AsInt32(dr["custid"]);
                        currentUserName = dr["username"].ToString();
                        if (currentCustId != preCusId)
                        {
                            if (subCount > 0)
                            {
                                #region assign total row
                                var traderTotal = new TraderPerformanceDetail()
                                {
                                    Comm15minOu = NumberFormat.DecFormat(Total_comm_15mou, 0),
                                    Comm1st1x2 = NumberFormat.DecFormat(Total_comm_ht1x2, 0),
                                    Comm1stHdp = NumberFormat.DecFormat(Total_comm_hthdp, 0),
                                    Comm1stNoOfCorners = NumberFormat.DecFormat(Total_comm_1stcorner, 0),
                                    Comm1stNoOfYellowCards = NumberFormat.DecFormat(Total_comm_1stYellowCard, 0),
                                    Comm1stOe = NumberFormat.DecFormat(Total_comm_1stoe, 0),
                                    Comm1stOu = NumberFormat.DecFormat(Total_comm_htou, 0),
                                    Comm1x2 = NumberFormat.DecFormat(Total_comm_1x2, 0),
                                    CommHdp = NumberFormat.DecFormat(Total_comm_hdp, 0),
                                    CommNoOfCorners = NumberFormat.DecFormat(Total_comm_corner, 0),
                                    CommNoOfYellowCards = NumberFormat.DecFormat(Total_comm_YellowCard, 0),
                                    CommOe = NumberFormat.DecFormat(Total_comm_oe, 0),
                                    CommOu = NumberFormat.DecFormat(Total_comm_ou, 0),
                                    CommNextCorner = NumberFormat.DecFormat(Total_comm_NextCorner, 0),
                                    CommOverall = NumberFormat.DecFormat(Total_comm_overall, 0),

                                    WinLost15minOu = NumberFormat.DecFormat(Total_wl_15mou, 0),
                                    WinLost1st1x2 = NumberFormat.DecFormat(Total_wl_ht1x2, 0),
                                    WinLost1stHdp = NumberFormat.DecFormat(Total_wl_hthdp, 0),
                                    WinLost1stNoOfCorners = NumberFormat.DecFormat(Total_wl_1stcorner, 0),
                                    WinLost1stNoOfYellowCards = NumberFormat.DecFormat(Total_wl_1stYellowCard, 0),
                                    WinLost1stOe = NumberFormat.DecFormat(Total_wl_1stoe, 0),
                                    WinLost1stOu = NumberFormat.DecFormat(Total_wl_htou, 0),
                                    WinLost1x2 = NumberFormat.DecFormat(Total_wl_1x2, 0),
                                    WinLostHdp = NumberFormat.DecFormat(Total_wl_hdp, 0),
                                    WinLostNoOfCorners = NumberFormat.DecFormat(Total_wl_corner, 0),
                                    WinLostNoOfYellowCards = NumberFormat.DecFormat(Total_wl_YellowCard, 0),
                                    WinLostOe = NumberFormat.DecFormat(Total_wl_oe, 0),
                                    WinLostOu = NumberFormat.DecFormat(Total_wl_ou, 0),
                                    WinLostNextCorner = NumberFormat.DecFormat(Total_wl_NextCorner, 0),
                                    WinLostOverall = NumberFormat.DecFormat(Total_wl_overall, 0),

                                    TurnOver15minOu = NumberFormat.DecFormat(Total_to_15mou, 0),
                                    TurnOver1st1x2 = NumberFormat.DecFormat(Total_to_ht1x2, 0),
                                    TurnOver1stHdp = NumberFormat.DecFormat(Total_to_hthdp, 0),
                                    TurnOver1stNoOfCorners = NumberFormat.DecFormat(Total_to_1stcorner, 0),
                                    TurnOver1stNoOfYellowCards = NumberFormat.DecFormat(Total_to_1stYellowCard, 0),
                                    TurnOver1stOe = NumberFormat.DecFormat(Total_to_1stoe, 0),
                                    TurnOver1stOu = NumberFormat.DecFormat(Total_to_htou, 0),
                                    TurnOver1x2 = NumberFormat.DecFormat(Total_to_1x2, 0),
                                    TurnOverHdp = NumberFormat.DecFormat(Total_to_hdp, 0),
                                    TurnOverNoOfCorners = NumberFormat.DecFormat(Total_to_corner, 0),
                                    TurnOverNoOfYellowCards = NumberFormat.DecFormat(Total_to_YellowCard, 0),
                                    TurnOverOe = NumberFormat.DecFormat(Total_to_oe, 0),
                                    TurnOverOu = NumberFormat.DecFormat(Total_to_ou, 0),
                                    TurnOverNextCorner = NumberFormat.DecFormat(Total_to_NextCorner, 0),
                                    TurnOverOverall = NumberFormat.DecFormat(Total_to_overall, 0),

                                    LiveBetCount15minOu = NumberFormat.DecFormat(Total_livebetcount_15mou, 0),
                                    LiveBetCount1st1x2 = NumberFormat.DecFormat(Total_livebetcount_ht1x2, 0),
                                    LiveBetCount1stHdp = NumberFormat.DecFormat(Total_livebetcount_hthdp, 0),
                                    LiveBetCount1stNoOfCorners = NumberFormat.DecFormat(Total_livebetcount_1stcorner, 0),
                                    LiveBetCount1stNoOfYellowCards = NumberFormat.DecFormat(Total_livebetcount_1stYellowCard, 0),
                                    LiveBetCount1stOe = NumberFormat.DecFormat(Total_livebetcount_1stoe, 0),
                                    LiveBetCount1stOu = NumberFormat.DecFormat(Total_livebetcount_htou, 0),
                                    LiveBetCount1x2 = NumberFormat.DecFormat(Total_livebetcount_1x2, 0),
                                    LiveBetCountHdp = NumberFormat.DecFormat(Total_livebetcount_hdp, 0),
                                    LiveBetCountNoOfCorners = NumberFormat.DecFormat(Total_livebetcount_corner, 0),
                                    LiveBetCountNoOfYellowCards = NumberFormat.DecFormat(Total_livebetcount_YellowCard, 0),
                                    LiveBetCountOe = NumberFormat.DecFormat(Total_livebetcount_oe, 0),
                                    LiveBetCountOu = NumberFormat.DecFormat(Total_livebetcount_ou, 0),
                                    LiveBetCountNextCorner = NumberFormat.DecFormat(Total_livebetcount_NextCorner, 0),
                                    LiveBetCountOverall = NumberFormat.DecFormat(Total_livebetcount_overall, 0),

                                    Margin15minOu = NumberFormat.DecFormat(Total_m_15mou * 100, 2),
                                    Margin1st1x2 = NumberFormat.DecFormat(Total_m_ht1x2 * 100, 2),
                                    Margin1stHdp = NumberFormat.DecFormat(Total_m_hthdp * 100, 2),
                                    Margin1stNoOfCorners = NumberFormat.DecFormat(Total_m_1stcorner * 100, 2),
                                    Margin1stNoOfYellowCards = NumberFormat.DecFormat(Total_m_1stYellowCard * 100, 2),
                                    Margin1stOe = NumberFormat.DecFormat(Total_m_1stoe * 100, 2),
                                    Margin1stOu = NumberFormat.DecFormat(Total_m_htou * 100, 2),
                                    Margin1x2 = NumberFormat.DecFormat(Total_m_1x2 * 100, 2),
                                    MarginHdp = NumberFormat.DecFormat(Total_m_hdp * 100, 2),
                                    MarginNoOfCorners = NumberFormat.DecFormat(Total_m_corner * 100, 2),
                                    MarginNoOfYellowCards = NumberFormat.DecFormat(Total_m_YellowCard * 100, 2),
                                    MarginOe = NumberFormat.DecFormat(Total_m_oe * 100, 2),
                                    MarginOu = NumberFormat.DecFormat(Total_m_ou * 100, 2),
                                    MarginNextCorner = NumberFormat.DecFormat(Total_m_NextCorner * 100, 2),
                                    MarginOverall = NumberFormat.DecFormat(Total_m_overall * 100, 2),

                                    UserName = string.Format("{0} Total", preUsername),
                                    CustId = preCusId,
                                    League = string.Empty,
                                    Match = string.Empty,
                                    HT = string.Empty,
                                    FT = string.Empty

                                };
                                #endregion

                                traderDataBO.total = traderTotal;
                                traderDataBO.children = traderChildren;
                                traderDataList.Add(traderDataBO);
                                traderChildren = new List<TraderPerformanceDetail>();
                                traderDataBO = new TraderPerformanceDetailBO();
                                subCount = 0;
                                #region turnover

                                Total_to_hthdp = 0;
                                Total_to_hdp = 0;
                                Total_to_htou = 0;
                                Total_to_ou = 0;
                                Total_to_ht1x2 = 0;
                                Total_to_1x2 = 0;
                                Total_to_1stcorner = 0;
                                Total_to_corner = 0;
                                Total_to_15mou = 0;
                                Total_to_1stYellowCard = 0;
                                Total_to_YellowCard = 0;
                                Total_to_overall = 0;
                                Total_to_1stoe = 0;
                                Total_to_NextCorner = 0;
                                Total_to_oe = 0;

                                #endregion
                                #region livebetcount

                                Total_livebetcount_hthdp = 0;
                                Total_livebetcount_hdp = 0;
                                Total_livebetcount_htou = 0;
                                Total_livebetcount_ou = 0;
                                Total_livebetcount_ht1x2 = 0;
                                Total_livebetcount_1x2 = 0;
                                Total_livebetcount_1stcorner = 0;
                                Total_livebetcount_corner = 0;
                                Total_livebetcount_15mou = 0;
                                Total_livebetcount_1stYellowCard = 0;
                                Total_livebetcount_YellowCard = 0;
                                Total_livebetcount_overall = 0;
                                Total_livebetcount_1stoe = 0;
                                Total_livebetcount_NextCorner = 0;
                                Total_livebetcount_oe = 0;

                                #endregion
                                #region winlost

                                Total_wl_hthdp = 0;
                                Total_wl_hdp = 0;
                                Total_wl_htou = 0;
                                Total_wl_ou = 0;
                                Total_wl_ht1x2 = 0;
                                Total_wl_1x2 = 0;
                                Total_wl_1stcorner = 0;
                                Total_wl_corner = 0;
                                Total_wl_15mou = 0;
                                Total_wl_1stYellowCard = 0;
                                Total_wl_YellowCard = 0;
                                Total_wl_overall = 0;
                                Total_wl_1stoe = 0;
                                Total_wl_NextCorner = 0;
                                Total_wl_oe = 0;

                                #endregion
                                #region comm

                                Total_comm_hthdp = 0;
                                Total_comm_hdp = 0;
                                Total_comm_htou = 0;
                                Total_comm_ou = 0;
                                Total_comm_ht1x2 = 0;
                                Total_comm_1x2 = 0;
                                Total_comm_1stcorner = 0;
                                Total_comm_corner = 0;
                                Total_comm_15mou = 0;
                                Total_comm_1stYellowCard = 0;
                                Total_comm_YellowCard = 0;
                                Total_comm_overall = 0;
                                Total_comm_1stoe = 0;
                                Total_comm_NextCorner = 0;
                                Total_comm_oe = 0;

                                #endregion
                                #region margin

                                Total_m_hthdp = 0;
                                Total_m_hdp = 0;
                                Total_m_htou = 0;
                                Total_m_ou = 0;
                                Total_m_ht1x2 = 0;
                                Total_m_1x2 = 0;
                                Total_m_1stcorner = 0;
                                Total_m_corner = 0;
                                Total_m_15mou = 0;
                                Total_m_1stYellowCard = 0;
                                Total_m_YellowCard = 0;
                                Total_m_overall = 0;
                                Total_m_1stoe = 0;
                                Total_m_NextCorner = 0;
                                Total_m_oe = 0;

                                #endregion
                            }
                            preCusId = currentCustId;
                            preUsername = currentUserName;
                        }
                        subCount++;
                        #region for each row
                        #region turnover
                        to_hthdp = CastAide.AsDecimal(dr["stake_1sthdp"]);
                        to_hdp = CastAide.AsDecimal(dr["stake_hdp"]);
                        to_htou = CastAide.AsDecimal(dr["stake_1stou"]);
                        to_ou = CastAide.AsDecimal(dr["stake_ou"]);
                        to_ht1x2 = CastAide.AsDecimal(dr["stake_1st1x2"]);
                        to_1x2 = CastAide.AsDecimal(dr["stake_1x2"]);
                        to_1stcorner = CastAide.AsDecimal(dr["stake_1stnoofcorners"]);
                        to_corner = CastAide.AsDecimal(dr["stake_noofcorners"]);
                        to_15mou = CastAide.AsDecimal(dr["stake_15minou"]);
                        to_1stYellowCard = CastAide.AsDecimal(dr["stake_1stNoOfYellowCards"]);
                        to_YellowCard = CastAide.AsDecimal(dr["stake_NoOfYellowCards"]);
                        to_1stoe = CastAide.AsDecimal(dr["stake_1stoe"]);
                        to_oe = CastAide.AsDecimal(dr["stake_oe"]);
                        to_NextCorner = dr.Table.Columns.Contains("stake_NextCorner") ? CastAide.AsDecimal(dr["stake_NextCorner"]) : 0;
                        to_overall = to_hthdp + to_hdp + to_htou + to_ou + to_ht1x2 + to_1x2 + to_1stcorner + to_corner +
                                     to_15mou + to_1stYellowCard + to_YellowCard + to_1stoe + to_oe + to_NextCorner;
                        #endregion
                        #region livebetcount
                        livebetcount_hthdp = CastAide.AsDecimal(dr["bet_1sthdp"]);
                        livebetcount_hdp = CastAide.AsDecimal(dr["bet_hdp"]);
                        livebetcount_htou = CastAide.AsDecimal(dr["bet_1stou"]);
                        livebetcount_ou = CastAide.AsDecimal(dr["bet_ou"]);
                        livebetcount_ht1x2 = CastAide.AsDecimal(dr["bet_1st1x2"]);
                        livebetcount_1x2 = CastAide.AsDecimal(dr["bet_1x2"]);
                        livebetcount_1stcorner = CastAide.AsDecimal(dr["bet_1stnoofcorners"]);
                        livebetcount_corner = CastAide.AsDecimal(dr["bet_noofcorners"]);
                        livebetcount_15mou = CastAide.AsDecimal(dr["bet_15minou"]);
                        livebetcount_1stYellowCard = CastAide.AsDecimal(dr["bet_1stNoOfYellowCards"]);
                        livebetcount_YellowCard = CastAide.AsDecimal(dr["bet_NoOfYellowCards"]);
                        livebetcount_1stoe = CastAide.AsDecimal(dr["bet_1stoe"]);
                        livebetcount_NextCorner = dr.Table.Columns.Contains("bet_NextCorner") ? CastAide.AsDecimal(dr["bet_NextCorner"]) : 0;
                        livebetcount_oe = CastAide.AsDecimal(dr["bet_oe"]);

                        livebetcount_overall = livebetcount_hthdp + livebetcount_hdp + livebetcount_htou + livebetcount_ou + livebetcount_ht1x2 +
                            livebetcount_1x2 + livebetcount_1stcorner + livebetcount_corner +
                                     livebetcount_15mou + livebetcount_1stYellowCard + livebetcount_YellowCard + livebetcount_1stoe + livebetcount_oe + livebetcount_NextCorner;
                        #endregion
                        #region winlost
                        wl_hthdp = CastAide.AsDecimal(dr["winlost_1sthdp"]);
                        wl_hdp = CastAide.AsDecimal(dr["winlost_hdp"]);
                        wl_htou = CastAide.AsDecimal(dr["winlost_1stou"]);
                        wl_ou = CastAide.AsDecimal(dr["winlost_ou"]);
                        wl_ht1x2 = CastAide.AsDecimal(dr["winlost_1st1x2"]);
                        wl_1x2 = CastAide.AsDecimal(dr["winlost_1x2"]);
                        wl_1stcorner = CastAide.AsDecimal(dr["winlost_1stnoofcorners"]);
                        wl_corner = CastAide.AsDecimal(dr["winlost_noofcorners"]);
                        wl_15mou = CastAide.AsDecimal(dr["winlost_15minou"]);
                        wl_1stYellowCard = CastAide.AsDecimal(dr["winlost_1stNoOfYellowCards"]);
                        wl_YellowCard = CastAide.AsDecimal(dr["winlost_NoOfYellowCards"]);
                        wl_1stoe = CastAide.AsDecimal(dr["winlost_1stoe"]);
                        wl_NextCorner = dr.Table.Columns.Contains("winlost_NextCorner") ? CastAide.AsDecimal(dr["winlost_NextCorner"]) : 0;
                        wl_oe = CastAide.AsDecimal(dr["winlost_oe"]);

                        wl_overall = wl_hthdp + wl_hdp + wl_htou + wl_ou + wl_ht1x2 + wl_1x2 + wl_1stcorner + wl_corner +
                                     wl_15mou + wl_1stYellowCard + wl_YellowCard + wl_1stoe + wl_oe + wl_NextCorner;
                        #endregion
                        #region comm
                        comm_hthdp = CastAide.AsDecimal(dr["comm_1sthdp"]);
                        comm_hdp = CastAide.AsDecimal(dr["comm_hdp"]);
                        comm_htou = CastAide.AsDecimal(dr["comm_1stou"]);
                        comm_ou = CastAide.AsDecimal(dr["comm_ou"]);
                        comm_ht1x2 = CastAide.AsDecimal(dr["comm_1st1x2"]);
                        comm_1x2 = CastAide.AsDecimal(dr["comm_1x2"]);
                        comm_1stcorner = CastAide.AsDecimal(dr["comm_1stnoofcorners"]);
                        comm_corner = CastAide.AsDecimal(dr["comm_noofcorners"]);
                        comm_15mou = CastAide.AsDecimal(dr["comm_15minou"]);
                        comm_1stYellowCard = CastAide.AsDecimal(dr["comm_1stNoOfYellowCards"]);
                        comm_YellowCard = CastAide.AsDecimal(dr["comm_NoOfYellowCards"]);
                        comm_1stoe = CastAide.AsDecimal(dr["comm_1stoe"]);
                        comm_NextCorner = dr.Table.Columns.Contains("comm_NextCorner") ? CastAide.AsDecimal(dr["comm_NextCorner"]) : 0;
                        comm_oe = CastAide.AsDecimal(dr["comm_oe"]);

                        comm_overall = comm_hthdp + comm_hdp + comm_htou + comm_ou + comm_ht1x2 + comm_1x2 +
                                       comm_1stcorner + comm_corner + comm_15mou + comm_1stYellowCard + comm_YellowCard + comm_1stoe + comm_oe + comm_NextCorner;
                        #endregion
                        #region margin
                        m_hthdp = CastAide.AsDecimal(dr["margin_1sthdp"]);
                        m_hdp = CastAide.AsDecimal(dr["margin_hdp"]);
                        m_htou = CastAide.AsDecimal(dr["margin_1stou"]);
                        m_ou = CastAide.AsDecimal(dr["margin_ou"]);
                        m_ht1x2 = CastAide.AsDecimal(dr["margin_1st1x2"]);
                        m_1x2 = CastAide.AsDecimal(dr["margin_1x2"]);
                        m_1stcorner = CastAide.AsDecimal(dr["margin_1stnoofcorners"]);
                        m_corner = CastAide.AsDecimal(dr["margin_noofcorners"]);
                        m_15mou = CastAide.AsDecimal(dr["margin_15minou"]);
                        m_1stYellowCard = CastAide.AsDecimal(dr["margin_1stNoOfYellowCards"]);
                        m_YellowCard = CastAide.AsDecimal(dr["margin_NoOfYellowCards"]);
                        m_1stoe = CastAide.AsDecimal(dr["margin_1stoe"]);
                        m_NextCorner = CastAide.AsDecimal(dr["margin_NextCorner"]);
                        m_oe = CastAide.AsDecimal(dr["margin_oe"]);

                        m_overall = CastAide.AsDecimal(dr["overall_margin"]);
                        #endregion

                        #region assign data
                        var traderDataDetail = new TraderPerformanceDetail()
                        {
                            Comm15minOu = NumberFormat.DecFormat(comm_15mou, 0),
                            Comm1st1x2 = NumberFormat.DecFormat(comm_ht1x2, 0),
                            Comm1stHdp = NumberFormat.DecFormat(comm_hthdp, 0),
                            Comm1stNoOfCorners = NumberFormat.DecFormat(comm_1stcorner, 0),
                            Comm1stNoOfYellowCards = NumberFormat.DecFormat(comm_1stYellowCard, 0),
                            Comm1stOe = NumberFormat.DecFormat(comm_1stoe, 0),
                            Comm1stOu = NumberFormat.DecFormat(comm_htou, 0),
                            Comm1x2 = NumberFormat.DecFormat(comm_1x2, 0),
                            CommHdp = NumberFormat.DecFormat(comm_hdp, 0),
                            CommNoOfCorners = NumberFormat.DecFormat(comm_corner, 0),
                            CommNoOfYellowCards = NumberFormat.DecFormat(comm_YellowCard, 0),
                            CommOe = NumberFormat.DecFormat(comm_oe, 0),
                            CommOu = NumberFormat.DecFormat(comm_ou, 0),
                            CommNextCorner = NumberFormat.DecFormat(comm_NextCorner, 0),
                            CommOverall = NumberFormat.DecFormat(comm_overall, 0),

                            WinLost15minOu = NumberFormat.DecFormat(wl_15mou, 0),
                            WinLost1st1x2 = NumberFormat.DecFormat(wl_ht1x2, 0),
                            WinLost1stHdp = NumberFormat.DecFormat(wl_hthdp, 0),
                            WinLost1stNoOfCorners = NumberFormat.DecFormat(wl_1stcorner, 0),
                            WinLost1stNoOfYellowCards = NumberFormat.DecFormat(wl_1stYellowCard, 0),
                            WinLost1stOe = NumberFormat.DecFormat(wl_1stoe, 0),
                            WinLost1stOu = NumberFormat.DecFormat(wl_htou, 0),
                            WinLost1x2 = NumberFormat.DecFormat(wl_1x2, 0),
                            WinLostHdp = NumberFormat.DecFormat(wl_hdp, 0),
                            WinLostNoOfCorners = NumberFormat.DecFormat(wl_corner, 0),
                            WinLostNoOfYellowCards = NumberFormat.DecFormat(wl_YellowCard, 0),
                            WinLostOe = NumberFormat.DecFormat(wl_oe, 0),
                            WinLostOu = NumberFormat.DecFormat(wl_ou, 0),
                            WinLostNextCorner = NumberFormat.DecFormat(wl_NextCorner, 0),
                            WinLostOverall = NumberFormat.DecFormat(wl_overall, 0),

                            TurnOver15minOu = NumberFormat.DecFormat(to_15mou, 0),
                            TurnOver1st1x2 = NumberFormat.DecFormat(to_ht1x2, 0),
                            TurnOver1stHdp = NumberFormat.DecFormat(to_hthdp, 0),
                            TurnOver1stNoOfCorners = NumberFormat.DecFormat(to_1stcorner, 0),
                            TurnOver1stNoOfYellowCards = NumberFormat.DecFormat(to_1stYellowCard, 0),
                            TurnOver1stOe = NumberFormat.DecFormat(to_1stoe, 0),
                            TurnOver1stOu = NumberFormat.DecFormat(to_htou, 0),
                            TurnOver1x2 = NumberFormat.DecFormat(to_1x2, 0),
                            TurnOverHdp = NumberFormat.DecFormat(to_hdp, 0),
                            TurnOverNoOfCorners = NumberFormat.DecFormat(to_corner, 0),
                            TurnOverNoOfYellowCards = NumberFormat.DecFormat(to_YellowCard, 0),
                            TurnOverOe = NumberFormat.DecFormat(to_oe, 0),
                            TurnOverOu = NumberFormat.DecFormat(to_ou, 0),
                            TurnOverNextCorner = NumberFormat.DecFormat(to_NextCorner, 0),
                            TurnOverOverall = NumberFormat.DecFormat(to_overall, 0),

                            LiveBetCount15minOu = NumberFormat.DecFormat(livebetcount_15mou, 0),
                            LiveBetCount1st1x2 = NumberFormat.DecFormat(livebetcount_ht1x2, 0),
                            LiveBetCount1stHdp = NumberFormat.DecFormat(livebetcount_hthdp, 0),
                            LiveBetCount1stNoOfCorners = NumberFormat.DecFormat(livebetcount_1stcorner, 0),
                            LiveBetCount1stNoOfYellowCards = NumberFormat.DecFormat(livebetcount_1stYellowCard, 0),
                            LiveBetCount1stOe = NumberFormat.DecFormat(livebetcount_1stoe, 0),
                            LiveBetCount1stOu = NumberFormat.DecFormat(livebetcount_htou, 0),
                            LiveBetCount1x2 = NumberFormat.DecFormat(livebetcount_1x2, 0),
                            LiveBetCountHdp = NumberFormat.DecFormat(livebetcount_hdp, 0),
                            LiveBetCountNoOfCorners = NumberFormat.DecFormat(livebetcount_corner, 0),
                            LiveBetCountNoOfYellowCards = NumberFormat.DecFormat(livebetcount_YellowCard, 0),
                            LiveBetCountOe = NumberFormat.DecFormat(livebetcount_oe, 0),
                            LiveBetCountOu = NumberFormat.DecFormat(livebetcount_ou, 0),
                            LiveBetCountNextCorner = NumberFormat.DecFormat(livebetcount_NextCorner, 0),
                            LiveBetCountOverall = NumberFormat.DecFormat(livebetcount_overall, 0),

                            Margin15minOu = NumberFormat.DecFormat(m_15mou * 100, 2),
                            Margin1st1x2 = NumberFormat.DecFormat(m_ht1x2 * 100, 2),
                            Margin1stHdp = NumberFormat.DecFormat(m_hthdp * 100, 2),
                            Margin1stNoOfCorners = NumberFormat.DecFormat(m_1stcorner * 100, 2),
                            Margin1stNoOfYellowCards = NumberFormat.DecFormat(m_1stYellowCard * 100, 2),
                            Margin1stOe = NumberFormat.DecFormat(m_1stoe * 100, 2),
                            Margin1stOu = NumberFormat.DecFormat(m_htou * 100, 2),
                            Margin1x2 = NumberFormat.DecFormat(m_1x2 * 100, 2),
                            MarginHdp = NumberFormat.DecFormat(m_hdp * 100, 2),
                            MarginNoOfCorners = NumberFormat.DecFormat(m_corner * 100, 2),
                            MarginNoOfYellowCards = NumberFormat.DecFormat(m_YellowCard * 100, 2),
                            MarginOe = NumberFormat.DecFormat(m_oe * 100, 2),
                            MarginOu = NumberFormat.DecFormat(m_ou * 100, 2),
                            MarginNextCorner = NumberFormat.DecFormat(m_NextCorner * 100, 2),
                            MarginOverall = NumberFormat.DecFormat(m_overall * 100, 2),

                            CustId = currentCustId,
                            UserName = currentUserName,
                            TransDate = CastAide.AsDateTime(dr["eventdate"]).ToString("dd MMM"),
                            League = CacheRef.GetLeagueName(dr["LeagueID"]),
                            Match = CacheRef.GetTeamName(dr["homeid"]) +
                                    (CastAide.AsBoolean(dr["isNeutral"]) ?
                                                                             ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty) + " - " +
                                    CacheRef.GetTeamName(dr["awayid"]),
                            HT = string.Format("{0} -- {1}", dr["hthomescore"], dr["htawayscore"]),
                            FT = string.Format("{0} -- {1}", dr["fthomescore"], dr["ftawayscore"])
                        };
                        #endregion
                        traderChildren.Add(traderDataDetail);
                        #endregion

                        #region total
                        #region turnover
                        Total_to_hthdp += to_hthdp;
                        Total_to_hdp += to_hdp;
                        Total_to_htou += to_htou;
                        Total_to_ou += to_ou;
                        Total_to_ht1x2 += to_ht1x2;
                        Total_to_1x2 += to_1x2;
                        Total_to_1stcorner += to_1stcorner;
                        Total_to_corner += to_corner;
                        Total_to_15mou += to_15mou;
                        Total_to_1stYellowCard += to_1stYellowCard;
                        Total_to_YellowCard += to_YellowCard;
                        Total_to_overall += to_overall;
                        Total_to_1stoe += to_1stoe;
                        Total_to_NextCorner += to_NextCorner;
                        Total_to_oe += to_oe;
                        #endregion
                        #region livebetcount
                        Total_livebetcount_hthdp += livebetcount_hthdp;
                        Total_livebetcount_hdp += livebetcount_hdp;
                        Total_livebetcount_htou += livebetcount_htou;
                        Total_livebetcount_ou += livebetcount_ou;
                        Total_livebetcount_ht1x2 += livebetcount_ht1x2;
                        Total_livebetcount_1x2 += livebetcount_1x2;
                        Total_livebetcount_1stcorner += livebetcount_1stcorner;
                        Total_livebetcount_corner += livebetcount_corner;
                        Total_livebetcount_15mou += livebetcount_15mou;
                        Total_livebetcount_1stYellowCard += livebetcount_1stYellowCard;
                        Total_livebetcount_YellowCard += livebetcount_YellowCard;
                        Total_livebetcount_overall += livebetcount_overall;
                        Total_livebetcount_1stoe += livebetcount_1stoe;
                        Total_livebetcount_NextCorner += livebetcount_NextCorner;
                        Total_livebetcount_oe += livebetcount_oe;
                        #endregion
                        #region winlost
                        Total_wl_hthdp += wl_hthdp;
                        Total_wl_hdp += wl_hdp;
                        Total_wl_htou += wl_htou;
                        Total_wl_ou += wl_ou;
                        Total_wl_ht1x2 += wl_ht1x2;
                        Total_wl_1x2 += wl_1x2;
                        Total_wl_1stcorner += wl_1stcorner;
                        Total_wl_corner += wl_corner;
                        Total_wl_15mou += wl_15mou;
                        Total_wl_1stYellowCard += wl_1stYellowCard;
                        Total_wl_YellowCard += wl_YellowCard;
                        Total_wl_1stoe += wl_1stoe;
                        Total_wl_oe += wl_oe;
                        Total_wl_NextCorner += wl_NextCorner;
                        Total_wl_overall = Total_wl_hthdp + Total_wl_hdp + Total_wl_htou + Total_wl_ou + Total_wl_ht1x2 +
                                           Total_wl_1x2 + Total_wl_1stcorner + Total_wl_corner + Total_wl_15mou +
                                           Total_wl_1stYellowCard + Total_wl_YellowCard + Total_wl_1stoe + Total_wl_oe + Total_wl_NextCorner;
                        #endregion
                        #region comm
                        Total_comm_hthdp += comm_hthdp;
                        Total_comm_hdp += comm_hdp;
                        Total_comm_htou += comm_htou;
                        Total_comm_ou += comm_ou;
                        Total_comm_ht1x2 += comm_ht1x2;
                        Total_comm_1x2 += comm_1x2;
                        Total_comm_1stcorner += comm_1stcorner;
                        Total_comm_corner += comm_corner;
                        Total_comm_15mou += comm_15mou;
                        Total_comm_1stYellowCard += comm_1stYellowCard;
                        Total_comm_YellowCard += comm_YellowCard;
                        Total_comm_1stoe += comm_1stoe;
                        Total_comm_oe += comm_oe;
                        Total_comm_NextCorner += comm_NextCorner;
                        Total_comm_overall += comm_overall;
                        #endregion
                        #region margin
                        Total_m_hthdp = Total_to_hthdp == 0 ? Total_wl_hthdp : Total_wl_hthdp / Total_to_hthdp;
                        Total_m_hdp = Total_to_hdp == 0 ? Total_wl_hdp : Total_wl_hdp / Total_to_hdp;
                        Total_m_htou = Total_to_htou == 0 ? Total_wl_htou : Total_wl_htou / Total_to_htou;
                        Total_m_ou = Total_to_ou == 0 ? Total_wl_ou : Total_wl_ou / Total_to_ou;
                        Total_m_ht1x2 = Total_to_ht1x2 == 0 ? Total_wl_ht1x2 : Total_wl_ht1x2 / Total_to_ht1x2;
                        Total_m_1x2 = Total_to_1x2 == 0 ? Total_wl_1x2 : Total_wl_1x2 / Total_to_1x2;
                        Total_m_1stcorner = Total_to_1stcorner == 0
                                                ? Total_wl_1stcorner
                                                : Total_wl_1stcorner / Total_to_1stcorner;
                        Total_m_corner = Total_to_corner == 0 ? Total_wl_corner : Total_wl_corner / Total_to_corner;
                        Total_m_15mou = Total_to_15mou == 0 ? Total_wl_15mou : Total_wl_15mou / Total_to_15mou;
                        Total_m_1stYellowCard = Total_to_1stYellowCard == 0
                                                    ? Total_wl_1stYellowCard
                                                    : Total_wl_1stYellowCard / Total_to_1stYellowCard;
                        Total_m_YellowCard = Total_to_YellowCard == 0
                                                 ? Total_wl_YellowCard
                                                 : Total_wl_YellowCard / Total_to_YellowCard;
                        Total_m_1stoe = Total_to_1stoe == 0 ? Total_wl_1stoe : Total_wl_1stoe / Total_to_1stoe;
                        Total_m_oe = Total_to_oe == 0 ? Total_wl_oe : Total_wl_oe / Total_to_oe;
                        Total_m_NextCorner = Total_to_NextCorner == 0 ? Total_wl_NextCorner : Total_wl_NextCorner / Total_to_NextCorner;
                        Total_m_overall = Total_to_overall == 0 ? Total_wl_overall : Total_wl_overall / Total_to_overall;

                        #endregion
                        #endregion

                        #region grand total
                        #region turnover
                        Grand_to_hthdp += to_hthdp;
                        Grand_to_hdp += to_hdp;
                        Grand_to_htou += to_htou;
                        Grand_to_ou += to_ou;
                        Grand_to_ht1x2 += to_ht1x2;
                        Grand_to_1x2 += to_1x2;
                        Grand_to_1stcorner += to_1stcorner;
                        Grand_to_corner += to_corner;
                        Grand_to_15mou += to_15mou;
                        Grand_to_1stYellowCard += to_1stYellowCard;
                        Grand_to_YellowCard += to_YellowCard;
                        Grand_to_1stoe += to_1stoe;
                        Grand_to_oe += to_oe;
                        Grand_to_NextCorner += to_NextCorner;
                        Grand_to_overall += to_overall;
                        #endregion
                        #region livebetcount
                        Grand_livebetcount_hthdp += livebetcount_hthdp;
                        Grand_livebetcount_hdp += livebetcount_hdp;
                        Grand_livebetcount_htou += livebetcount_htou;
                        Grand_livebetcount_ou += livebetcount_ou;
                        Grand_livebetcount_ht1x2 += livebetcount_ht1x2;
                        Grand_livebetcount_1x2 += livebetcount_1x2;
                        Grand_livebetcount_1stcorner += livebetcount_1stcorner;
                        Grand_livebetcount_corner += livebetcount_corner;
                        Grand_livebetcount_15mou += livebetcount_15mou;
                        Grand_livebetcount_1stYellowCard += livebetcount_1stYellowCard;
                        Grand_livebetcount_YellowCard += livebetcount_YellowCard;
                        Grand_livebetcount_1stoe += livebetcount_1stoe;
                        Grand_livebetcount_oe += livebetcount_oe;
                        Grand_livebetcount_NextCorner += livebetcount_NextCorner;
                        Grand_livebetcount_overall += livebetcount_overall;
                        #endregion
                        #region winlost
                        Grand_wl_hthdp += wl_hthdp;
                        Grand_wl_hdp += wl_hdp;
                        Grand_wl_htou += wl_htou;
                        Grand_wl_ou += wl_ou;
                        Grand_wl_ht1x2 += wl_ht1x2;
                        Grand_wl_1x2 += wl_1x2;
                        Grand_wl_1stcorner += wl_1stcorner;
                        Grand_wl_corner += wl_corner;
                        Grand_wl_15mou += wl_15mou;
                        Grand_wl_1stYellowCard += wl_1stYellowCard;
                        Grand_wl_YellowCard += wl_YellowCard;
                        Grand_wl_1stoe += wl_1stoe;
                        Grand_wl_oe += wl_oe;
                        Grand_wl_NextCorner += wl_NextCorner;
                        Grand_wl_overall += wl_overall;
                        #endregion winlost
                        #region comm
                        Grand_comm_hthdp += comm_hthdp;
                        Grand_comm_hdp += comm_hdp;
                        Grand_comm_htou += comm_htou;
                        Grand_comm_ou += comm_ou;
                        Grand_comm_ht1x2 += comm_ht1x2;
                        Grand_comm_1x2 += comm_1x2;
                        Grand_comm_1stcorner += comm_1stcorner;
                        Grand_comm_corner += comm_corner;
                        Grand_comm_15mou += comm_15mou;
                        Grand_comm_1stYellowCard += comm_1stYellowCard;
                        Grand_comm_YellowCard += comm_YellowCard;
                        Grand_comm_1stoe += comm_1stoe;
                        Grand_comm_oe += comm_oe;
                        Grand_comm_NextCorner += comm_NextCorner;
                        Grand_comm_overall += comm_overall;
                        #endregion
                        #region margin
                        Grand_m_hthdp = Grand_to_hthdp != 0 ? Grand_wl_hthdp / Grand_to_hthdp : 0;
                        Grand_m_hdp = Grand_to_hdp != 0 ? Grand_wl_hdp / Grand_to_hdp : 0;
                        Grand_m_htou = Grand_to_htou != 0 ? Grand_wl_htou / Grand_to_htou : 0;
                        Grand_m_ou = Grand_to_ou != 0 ? Grand_wl_ou / Grand_to_ou : 0;
                        Grand_m_ht1x2 = Grand_to_ht1x2 != 0 ? Grand_wl_ht1x2 / Grand_to_ht1x2 : 0;
                        Grand_m_1x2 = Grand_to_1x2 != 0 ? Grand_wl_1x2 / Grand_to_1x2 : 0;
                        Grand_m_1stcorner = Grand_to_1stcorner != 0 ? Grand_wl_1stcorner / Grand_to_1stcorner : 0;
                        Grand_m_corner = Grand_to_corner != 0 ? Grand_wl_corner / Grand_to_corner : 0;
                        Grand_m_15mou = Grand_to_15mou != 0 ? Grand_wl_15mou / Grand_to_15mou : 0;
                        Grand_m_1stYellowCard = Grand_to_1stYellowCard != 0 ? Grand_wl_1stYellowCard / Grand_to_1stYellowCard : 0;
                        Grand_m_YellowCard = Grand_to_YellowCard != 0 ? Grand_wl_YellowCard / Grand_to_YellowCard : 0;
                        Grand_m_1stoe = Grand_to_1stoe != 0 ? Grand_wl_1stoe / Grand_to_1stoe : 0;
                        Grand_m_oe = Grand_to_oe != 0 ? Grand_wl_oe / Grand_to_oe : 0;
                        Grand_m_NextCorner = Grand_to_NextCorner != 0 ? Grand_wl_NextCorner / Grand_to_NextCorner : 0;
                        Grand_m_overall = Grand_to_overall != 0 ? Grand_wl_overall / Grand_to_overall : 0;
                        #endregion
                        #endregion
                    }
                    #region last user
                    #region assign total row
                    var traderTotalLastUser = new TraderPerformanceDetail()
                    {
                        Comm15minOu = NumberFormat.DecFormat(Total_comm_15mou, 0),
                        Comm1st1x2 = NumberFormat.DecFormat(Total_comm_ht1x2, 0),
                        Comm1stHdp = NumberFormat.DecFormat(Total_comm_hthdp, 0),
                        Comm1stNoOfCorners = NumberFormat.DecFormat(Total_comm_1stcorner, 0),
                        Comm1stNoOfYellowCards = NumberFormat.DecFormat(Total_comm_1stYellowCard, 0),
                        Comm1stOe = NumberFormat.DecFormat(Total_comm_1stoe, 0),
                        Comm1stOu = NumberFormat.DecFormat(Total_comm_htou, 0),
                        Comm1x2 = NumberFormat.DecFormat(Total_comm_1x2, 0),
                        CommHdp = NumberFormat.DecFormat(Total_comm_hdp, 0),
                        CommNoOfCorners = NumberFormat.DecFormat(Total_comm_corner, 0),
                        CommNoOfYellowCards = NumberFormat.DecFormat(Total_comm_YellowCard, 0),
                        CommOe = NumberFormat.DecFormat(Total_comm_oe, 0),
                        CommOu = NumberFormat.DecFormat(Total_comm_ou, 0),
                        CommNextCorner = NumberFormat.DecFormat(Total_comm_NextCorner, 0),
                        CommOverall = NumberFormat.DecFormat(Total_comm_overall, 0),

                        WinLost15minOu = NumberFormat.DecFormat(Total_wl_15mou, 0),
                        WinLost1st1x2 = NumberFormat.DecFormat(Total_wl_ht1x2, 0),
                        WinLost1stHdp = NumberFormat.DecFormat(Total_wl_hthdp, 0),
                        WinLost1stNoOfCorners = NumberFormat.DecFormat(Total_wl_1stcorner, 0),
                        WinLost1stNoOfYellowCards = NumberFormat.DecFormat(Total_wl_1stYellowCard, 0),
                        WinLost1stOe = NumberFormat.DecFormat(Total_wl_1stoe, 0),
                        WinLost1stOu = NumberFormat.DecFormat(Total_wl_htou, 0),
                        WinLost1x2 = NumberFormat.DecFormat(Total_wl_1x2, 0),
                        WinLostHdp = NumberFormat.DecFormat(Total_wl_hdp, 0),
                        WinLostNoOfCorners = NumberFormat.DecFormat(Total_wl_corner, 0),
                        WinLostNoOfYellowCards = NumberFormat.DecFormat(Total_wl_YellowCard, 0),
                        WinLostOe = NumberFormat.DecFormat(Total_wl_oe, 0),
                        WinLostOu = NumberFormat.DecFormat(Total_wl_ou, 0),
                        WinLostNextCorner = NumberFormat.DecFormat(Total_wl_NextCorner, 0),
                        WinLostOverall = NumberFormat.DecFormat(Total_wl_overall, 0),

                        TurnOver15minOu = NumberFormat.DecFormat(Total_to_15mou, 0),
                        TurnOver1st1x2 = NumberFormat.DecFormat(Total_to_ht1x2, 0),
                        TurnOver1stHdp = NumberFormat.DecFormat(Total_to_hthdp, 0),
                        TurnOver1stNoOfCorners = NumberFormat.DecFormat(Total_to_1stcorner, 0),
                        TurnOver1stNoOfYellowCards = NumberFormat.DecFormat(Total_to_1stYellowCard, 0),
                        TurnOver1stOe = NumberFormat.DecFormat(Total_to_1stoe, 0),
                        TurnOver1stOu = NumberFormat.DecFormat(Total_to_htou, 0),
                        TurnOver1x2 = NumberFormat.DecFormat(Total_to_1x2, 0),
                        TurnOverHdp = NumberFormat.DecFormat(Total_to_hdp, 0),
                        TurnOverNoOfCorners = NumberFormat.DecFormat(Total_to_corner, 0),
                        TurnOverNoOfYellowCards = NumberFormat.DecFormat(Total_to_YellowCard, 0),
                        TurnOverOe = NumberFormat.DecFormat(Total_to_oe, 0),
                        TurnOverOu = NumberFormat.DecFormat(Total_to_ou, 0),
                        TurnOverNextCorner = NumberFormat.DecFormat(Total_to_NextCorner, 0),
                        TurnOverOverall = NumberFormat.DecFormat(Total_to_overall, 0),

                        LiveBetCount15minOu = NumberFormat.DecFormat(Total_livebetcount_15mou, 0),
                        LiveBetCount1st1x2 = NumberFormat.DecFormat(Total_livebetcount_ht1x2, 0),
                        LiveBetCount1stHdp = NumberFormat.DecFormat(Total_livebetcount_hthdp, 0),
                        LiveBetCount1stNoOfCorners = NumberFormat.DecFormat(Total_livebetcount_1stcorner, 0),
                        LiveBetCount1stNoOfYellowCards = NumberFormat.DecFormat(Total_livebetcount_1stYellowCard, 0),
                        LiveBetCount1stOe = NumberFormat.DecFormat(Total_livebetcount_1stoe, 0),
                        LiveBetCount1stOu = NumberFormat.DecFormat(Total_livebetcount_htou, 0),
                        LiveBetCount1x2 = NumberFormat.DecFormat(Total_livebetcount_1x2, 0),
                        LiveBetCountHdp = NumberFormat.DecFormat(Total_livebetcount_hdp, 0),
                        LiveBetCountNoOfCorners = NumberFormat.DecFormat(Total_livebetcount_corner, 0),
                        LiveBetCountNoOfYellowCards = NumberFormat.DecFormat(Total_livebetcount_YellowCard, 0),
                        LiveBetCountOe = NumberFormat.DecFormat(Total_livebetcount_oe, 0),
                        LiveBetCountOu = NumberFormat.DecFormat(Total_livebetcount_ou, 0),
                        LiveBetCountNextCorner = NumberFormat.DecFormat(Total_livebetcount_NextCorner, 0),
                        LiveBetCountOverall = NumberFormat.DecFormat(Total_livebetcount_overall, 0),

                        Margin15minOu = NumberFormat.DecFormat(Total_m_15mou * 100, 2),
                        Margin1st1x2 = NumberFormat.DecFormat(Total_m_ht1x2 * 100, 2),
                        Margin1stHdp = NumberFormat.DecFormat(Total_m_hthdp * 100, 2),
                        Margin1stNoOfCorners = NumberFormat.DecFormat(Total_m_1stcorner * 100, 2),
                        Margin1stNoOfYellowCards = NumberFormat.DecFormat(Total_m_1stYellowCard * 100, 2),
                        Margin1stOe = NumberFormat.DecFormat(Total_m_1stoe * 100, 2),
                        Margin1stOu = NumberFormat.DecFormat(Total_m_htou * 100, 2),
                        Margin1x2 = NumberFormat.DecFormat(Total_m_1x2 * 100, 2),
                        MarginHdp = NumberFormat.DecFormat(Total_m_hdp * 100, 2),
                        MarginNoOfCorners = NumberFormat.DecFormat(Total_m_corner * 100, 2),
                        MarginNoOfYellowCards = NumberFormat.DecFormat(Total_m_YellowCard * 100, 2),
                        MarginOe = NumberFormat.DecFormat(Total_m_oe * 100, 2),
                        MarginOu = NumberFormat.DecFormat(Total_m_ou * 100, 2),
                        MarginNextCorner = NumberFormat.DecFormat(Total_m_NextCorner * 100, 2),
                        MarginOverall = NumberFormat.DecFormat(Total_m_overall * 100, 2),

                        UserName = string.Format("{0} Total", preUsername),
                        CustId = preCusId,
                        League = string.Empty,
                        Match = string.Empty,
                        HT = string.Empty,
                        FT = string.Empty

                    };
                    #endregion

                    traderDataBO.total = traderTotalLastUser;
                    traderDataBO.children = traderChildren;
                    traderDataList.Add(traderDataBO);
                    #endregion
                    result.rows = traderDataList;
                    #region footer
                    #region assign footer
                    var traderFooter = new TraderPerformanceDetail()
                    {
                        Comm15minOu = NumberFormat.DecFormat(Grand_comm_15mou, 0),
                        Comm1st1x2 = NumberFormat.DecFormat(Grand_comm_ht1x2, 0),
                        Comm1stHdp = NumberFormat.DecFormat(Grand_comm_hthdp, 0),
                        Comm1stNoOfCorners = NumberFormat.DecFormat(Grand_comm_1stcorner, 0),
                        Comm1stNoOfYellowCards = NumberFormat.DecFormat(Grand_comm_1stYellowCard, 0),
                        Comm1stOe = NumberFormat.DecFormat(Grand_comm_1stoe, 0),
                        Comm1stOu = NumberFormat.DecFormat(Grand_comm_htou, 0),
                        Comm1x2 = NumberFormat.DecFormat(Grand_comm_1x2, 0),
                        CommHdp = NumberFormat.DecFormat(Grand_comm_hdp, 0),
                        CommNoOfCorners = NumberFormat.DecFormat(Grand_comm_corner, 0),
                        CommNoOfYellowCards = NumberFormat.DecFormat(Grand_comm_YellowCard, 0),
                        CommOe = NumberFormat.DecFormat(Grand_comm_oe, 0),
                        CommOu = NumberFormat.DecFormat(Grand_comm_ou, 0),
                        CommNextCorner = NumberFormat.DecFormat(Grand_comm_NextCorner, 0),
                        CommOverall = NumberFormat.DecFormat(Grand_comm_overall, 0),

                        WinLost15minOu = NumberFormat.DecFormat(Grand_wl_15mou, 0),
                        WinLost1st1x2 = NumberFormat.DecFormat(Grand_wl_ht1x2, 0),
                        WinLost1stHdp = NumberFormat.DecFormat(Grand_wl_hthdp, 0),
                        WinLost1stNoOfCorners = NumberFormat.DecFormat(Grand_wl_1stcorner, 0),
                        WinLost1stNoOfYellowCards = NumberFormat.DecFormat(Grand_wl_1stYellowCard, 0),
                        WinLost1stOe = NumberFormat.DecFormat(Grand_wl_1stoe, 0),
                        WinLost1stOu = NumberFormat.DecFormat(Grand_wl_htou, 0),
                        WinLost1x2 = NumberFormat.DecFormat(Grand_wl_1x2, 0),
                        WinLostHdp = NumberFormat.DecFormat(Grand_wl_hdp, 0),
                        WinLostNoOfCorners = NumberFormat.DecFormat(Grand_wl_corner, 0),
                        WinLostNoOfYellowCards = NumberFormat.DecFormat(Grand_wl_YellowCard, 0),
                        WinLostOe = NumberFormat.DecFormat(Grand_wl_oe, 0),
                        WinLostOu = NumberFormat.DecFormat(Grand_wl_ou, 0),
                        WinLostNextCorner = NumberFormat.DecFormat(Grand_wl_NextCorner, 0),
                        WinLostOverall = NumberFormat.DecFormat(Grand_wl_overall, 0),

                        TurnOver15minOu = NumberFormat.DecFormat(Grand_to_15mou, 0),
                        TurnOver1st1x2 = NumberFormat.DecFormat(Grand_to_ht1x2, 0),
                        TurnOver1stHdp = NumberFormat.DecFormat(Grand_to_hthdp, 0),
                        TurnOver1stNoOfCorners = NumberFormat.DecFormat(Grand_to_1stcorner, 0),
                        TurnOver1stNoOfYellowCards = NumberFormat.DecFormat(Grand_to_1stYellowCard, 0),
                        TurnOver1stOe = NumberFormat.DecFormat(Grand_to_1stoe, 0),
                        TurnOver1stOu = NumberFormat.DecFormat(Grand_to_htou, 0),
                        TurnOver1x2 = NumberFormat.DecFormat(Grand_to_1x2, 0),
                        TurnOverHdp = NumberFormat.DecFormat(Grand_to_hdp, 0),
                        TurnOverNoOfCorners = NumberFormat.DecFormat(Grand_to_corner, 0),
                        TurnOverNoOfYellowCards = NumberFormat.DecFormat(Grand_to_YellowCard, 0),
                        TurnOverOe = NumberFormat.DecFormat(Grand_to_oe, 0),
                        TurnOverOu = NumberFormat.DecFormat(Grand_to_ou, 0),
                        TurnOverNextCorner = NumberFormat.DecFormat(Grand_to_NextCorner, 0),
                        TurnOverOverall = NumberFormat.DecFormat(Grand_to_overall, 0),

                        LiveBetCount15minOu = NumberFormat.DecFormat(Grand_livebetcount_15mou, 0),
                        LiveBetCount1st1x2 = NumberFormat.DecFormat(Grand_livebetcount_ht1x2, 0),
                        LiveBetCount1stHdp = NumberFormat.DecFormat(Grand_livebetcount_hthdp, 0),
                        LiveBetCount1stNoOfCorners = NumberFormat.DecFormat(Grand_livebetcount_1stcorner, 0),
                        LiveBetCount1stNoOfYellowCards = NumberFormat.DecFormat(Grand_livebetcount_1stYellowCard, 0),
                        LiveBetCount1stOe = NumberFormat.DecFormat(Grand_livebetcount_1stoe, 0),
                        LiveBetCount1stOu = NumberFormat.DecFormat(Grand_livebetcount_htou, 0),
                        LiveBetCount1x2 = NumberFormat.DecFormat(Grand_livebetcount_1x2, 0),
                        LiveBetCountHdp = NumberFormat.DecFormat(Grand_livebetcount_hdp, 0),
                        LiveBetCountNoOfCorners = NumberFormat.DecFormat(Grand_livebetcount_corner, 0),
                        LiveBetCountNoOfYellowCards = NumberFormat.DecFormat(Grand_livebetcount_YellowCard, 0),
                        LiveBetCountOe = NumberFormat.DecFormat(Grand_livebetcount_oe, 0),
                        LiveBetCountOu = NumberFormat.DecFormat(Grand_livebetcount_ou, 0),
                        LiveBetCountNextCorner = NumberFormat.DecFormat(Grand_livebetcount_NextCorner, 0),
                        LiveBetCountOverall = NumberFormat.DecFormat(Grand_livebetcount_overall, 0),

                        Margin15minOu = NumberFormat.DecFormat(Grand_m_15mou * 100, 2),
                        Margin1st1x2 = NumberFormat.DecFormat(Grand_m_ht1x2 * 100, 2),
                        Margin1stHdp = NumberFormat.DecFormat(Grand_m_hthdp * 100, 2),
                        Margin1stNoOfCorners = NumberFormat.DecFormat(Grand_m_1stcorner * 100, 2),
                        Margin1stNoOfYellowCards = NumberFormat.DecFormat(Grand_m_1stYellowCard * 100, 2),
                        Margin1stOe = NumberFormat.DecFormat(Grand_m_1stoe * 100, 2),
                        Margin1stOu = NumberFormat.DecFormat(Grand_m_htou * 100, 2),
                        Margin1x2 = NumberFormat.DecFormat(Grand_m_1x2 * 100, 2),
                        MarginHdp = NumberFormat.DecFormat(Grand_m_hdp * 100, 2),
                        MarginNoOfCorners = NumberFormat.DecFormat(Grand_m_corner * 100, 2),
                        MarginNoOfYellowCards = NumberFormat.DecFormat(Grand_m_YellowCard * 100, 0),
                        MarginOe = NumberFormat.DecFormat(Grand_m_oe * 100, 2),
                        MarginOu = NumberFormat.DecFormat(Grand_m_ou * 100, 2),
                        MarginNextCorner = NumberFormat.DecFormat(Grand_m_NextCorner * 100, 2),
                        MarginOverall = NumberFormat.DecFormat(Grand_m_overall * 100, 2),

                        UserName = string.Empty,
                        League = string.Empty,
                        Match = string.Empty
                    };
                    #endregion
                    result.footer = traderFooter;
                    #endregion
                }
            }
            catch
            {
            }
            finally
            {
                #region assign session
                switch (isSoccer)
                {
                    case 1:
                        SessionExtension.AddSession(HttpContext.Current.Session, TraderPerformanceDetailIsSoccerSessionKey, result);
                        break;
                    case 0:
                        SessionExtension.AddSession(HttpContext.Current.Session, TraderPerformanceDetailIsNoSoccerSessionKey, result);
                        break;
                    default:
                        break;
                }
                #endregion
            }
            return result;
            #endregion
        }

        #endregion
    }
}
