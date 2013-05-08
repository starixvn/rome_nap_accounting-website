

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using DNAide.Core;
using DNAide.Utils;
using ExcelUtil.Spreadsheet;
using Fanex.Tarus.Reports.Entities.MatchWinLoss;
using Fanex.Tarus.Reports.Entities.Sport;
using Fanex.Tarus.Reports.Helpers;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;

namespace Fanex.Tarus.Reports.Business.MatchWinLoss
{
    public partial class MatchWinLossService : IMatchWinLossService
    {
        public WLAllDetailsReport SelMatchWinLossReportAllDetails(DateTime reportDate)
        {
            WLAllDetailsReport wlAllDetailsReport = new WLAllDetailsReport
            {
                NonThirdPartyDetails = new List<WLBySportsGroupByBetType>(),
                ThirdPartyDetails = new WLDetailBySportReport
                {
                    ReportDetails = new List<WLDetailByBetTypeModel>()
                },
                NonThirdPartyGrandTotal = new WLDetailByBetTypeModel
                {
                    Name = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.GrandTotal
                },
                Total = new WLDetailByBetTypeModel
                {
                    Name = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.GrandTotal
                }
            };

            try
            {
                Hashtable outParam = new Hashtable();
                DataSet dbResult = _matchWinLossAccess.SelMatchWinLossReportAllDetail(reportDate, ref outParam);
                decimal eightRMTotal = _eightRMUtils.IsActive ? CastAide.AsDecimal(outParam["winlost_8rm"]) : 0;
                decimal barmTotal = _barmUtils.IsActive ? CastAide.AsDecimal(outParam["winlost_barm"]) : 0;
                decimal eightRMTurnover = _eightRMUtils.IsActive ? CastAide.AsDecimal(outParam["ibcturnover_8rm"]) : 0;
                decimal barmTurnover = _barmUtils.IsActive ? CastAide.AsDecimal(outParam["ibcturnover_barm"]) : 0;
                Dictionary<int, string> teamNameList = new Dictionary<int, string>();
                Dictionary<int, string> betTypeList = new Dictionary<int, string>();

                if (dbResult != null)
                {
                    var totalTable = dbResult.Tables.Count;
                    #region Entities

                    WLDetailByBetType wlEntity = new WLDetailByBetType();
                    WLDetailByBetType sportTotalEntity = new WLDetailByBetType();
                    WLDetailByBetType betTypeGroupTotalEntity = new WLDetailByBetType();
                    WLDetailByBetType nonThirdPartyGrandTotalEntity = new WLDetailByBetType();
                    WLDetailByBetType wl3rdPartyEntity = new WLDetailByBetType();
                    WLDetailByBetType _3rdPartyTotalEntity = new WLDetailByBetType();

                    #endregion Entities
                    #region GetNonThirdPartyData

                    if (totalTable >= 1)
                    {
                        var nonThirdPartyData = dbResult.Tables[0];
                        var wlDetailByBetType = new WLDetailByBetTypeModel();
                        var wlgroupByBetTypeReport = new WLDetailBySportReport
                        {
                            ReportDetails = new List<WLDetailByBetTypeModel>()
                        };
                        var wlNon3rdPartyBySportReport = new WLBySportsGroupByBetType
                        {
                            BetTypeGroup = new List<WLDetailBySportReport>()
                        };

                        string previousSport = null;
                        string previousBetType = null;
                        string currentBetType = null;
                        if (nonThirdPartyData.Rows.Count > 0)
                        {
                            previousSport = CastAide.AsString(nonThirdPartyData.Rows[0]["leaguenametc"]);
                            var initBetTypeId = CastAide.AsInt32(nonThirdPartyData.Rows[0]["bettype"]);
                            betTypeList.Add(initBetTypeId, GetBetTypeName(initBetTypeId));
                            previousBetType = GetLiveTypeName(CastAide.AsInt32(nonThirdPartyData.Rows[0]["liveindicator"]), true) +
                                betTypeList[initBetTypeId];

                        }

                        foreach (DataRow row in nonThirdPartyData.Rows)
                        {
                            #region GetData

                            wlEntity = new WLDetailByBetType
                            {
                                Commission = CastAide.AsDecimal(row["comm"]),
                                VirtualComm = CastAide.AsDecimal(row["VirtualComm"]),
                                LineTotal = CastAide.AsDecimal(row["LineTotal"]),
                                VirtualLineTotal = CastAide.AsDecimal(row["VirtualLineTotal"]),
                                HomeTeamId = CastAide.AsInt32(row["homeid"]),
                                AwayTeamId = CastAide.AsInt32(row["awayid"]),
                                Position = CastAide.AsDecimal(row["pos"]),
                                WinLost = CastAide.AsDecimal(row["winlost"]),
                                Turnover = CastAide.AsDecimal(row["IBCTurnover"])
                            };

                            wlEntity.Variance = wlEntity.LineTotal - wlEntity.VirtualLineTotal;

                            wlDetailByBetType = new WLDetailByBetTypeModel
                            {
                                BetType = new BaseModel
                                {
                                    Id = CastAide.AsInt32(row["bettype"])
                                },
                                SportType = new Sporttype
                                {
                                    Name = CastAide.AsString(row["leaguenametc"])
                                },
                                LiveIndicator = new BaseModel
                                {
                                    Id = CastAide.AsInt32(row["liveindicator"])
                                },
                                Commission = NumberFormat.DecFormat(wlEntity.Commission, 2),
                                VirtualComm = NumberFormat.DecFormat(wlEntity.VirtualComm, 2),
                                LineTotal = NumberFormat.DecFormat(wlEntity.LineTotal, 2),
                                VirtualLineTotal = NumberFormat.DecFormat(wlEntity.VirtualLineTotal, 2),
                                Variance = NumberFormat.DecFormat(wlEntity.Variance, 2),
                                Turnover = NumberFormat.DecFormat(wlEntity.Turnover, 2),
                                Margin = this.CalculateMargin(wlEntity.LineTotal, wlEntity.Turnover),

                                WinLost = NumberFormat.DecFormat(wlEntity.WinLost, 2),
                                Position = NumberFormat.DecFormat(wlEntity.Position, 2),
                                MatchCode = CastAide.AsString(row["matchcode"]),
                                EventDate = CastAide.AsDateTime(row["eventdate"]).ToShortDateString(),
                                KickOffTime = CastAide.AsDateTime(row["kickofftime"]).ToShortTimeString(),
                                IsNeutral = CastAide.AsBoolean(row["IsNeutral"])
                            };

                            if (!teamNameList.ContainsKey(wlEntity.HomeTeamId))
                            {
                                teamNameList.Add(wlEntity.HomeTeamId, CacheRef.GetTeamName(row["homeid"]));
                            }
                            if (!teamNameList.ContainsKey(wlEntity.AwayTeamId))
                            {
                                teamNameList.Add(wlEntity.AwayTeamId, CacheRef.GetTeamName(row["awayid"]));
                            }
                            wlDetailByBetType.HomeName = teamNameList[wlEntity.HomeTeamId];
                            wlDetailByBetType.AwayName = teamNameList[wlEntity.AwayTeamId];

                            if (!betTypeList.ContainsKey(wlDetailByBetType.BetType.Id))
                            {
                                betTypeList.Add(wlDetailByBetType.BetType.Id, GetBetTypeName(wlDetailByBetType.BetType.Id));
                            }
                            wlDetailByBetType.BetType.Name = betTypeList[wlDetailByBetType.BetType.Id];
                            currentBetType = GetLiveTypeName(wlDetailByBetType.LiveIndicator.Id, true) + wlDetailByBetType.BetType.Name;

                            #endregion GetData

                            if (wlDetailByBetType.SportType.Name != previousSport)
                            {
                                #region InsertPreviousBetType

                                wlgroupByBetTypeReport.Total = new WLDetailByBetTypeModel
                                {
                                    Name = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.BetTypeTotal,
                                    Commission = NumberFormat.DecFormat(betTypeGroupTotalEntity.Commission, 2),
                                    VirtualComm = NumberFormat.DecFormat(betTypeGroupTotalEntity.VirtualComm, 2),
                                    LineTotal = NumberFormat.DecFormat(betTypeGroupTotalEntity.LineTotal, 2),
                                    VirtualLineTotal = NumberFormat.DecFormat(betTypeGroupTotalEntity.VirtualLineTotal, 2),
                                    Variance = NumberFormat.DecFormat(betTypeGroupTotalEntity.Variance, 2),
                                    Turnover = NumberFormat.DecFormat(betTypeGroupTotalEntity.Turnover, 2),
                                    Margin = this.CalculateMargin(betTypeGroupTotalEntity.LineTotal, betTypeGroupTotalEntity.Turnover)
                                };

                                wlNon3rdPartyBySportReport.BetTypeGroup.Add(wlgroupByBetTypeReport);
                                sportTotalEntity.Commission += betTypeGroupTotalEntity.Commission;
                                sportTotalEntity.VirtualComm += betTypeGroupTotalEntity.VirtualComm;
                                sportTotalEntity.LineTotal += betTypeGroupTotalEntity.LineTotal;
                                sportTotalEntity.VirtualLineTotal += betTypeGroupTotalEntity.VirtualLineTotal;
                                sportTotalEntity.Variance += betTypeGroupTotalEntity.Variance;
                                sportTotalEntity.Turnover += betTypeGroupTotalEntity.Turnover;

                                wlgroupByBetTypeReport = new WLDetailBySportReport
                                {
                                    ReportDetails = new List<WLDetailByBetTypeModel>()
                                };

                                betTypeGroupTotalEntity.Commission = wlEntity.Commission;
                                betTypeGroupTotalEntity.VirtualComm = wlEntity.VirtualComm;
                                betTypeGroupTotalEntity.LineTotal = wlEntity.LineTotal;
                                betTypeGroupTotalEntity.VirtualLineTotal = wlEntity.VirtualLineTotal;
                                betTypeGroupTotalEntity.Variance = wlEntity.Variance;
                                betTypeGroupTotalEntity.Turnover = wlEntity.Turnover;

                                previousBetType = currentBetType;

                                #endregion InsertPreviousBetType
                                #region InsertPreviousSport

                                wlNon3rdPartyBySportReport.Total = new WLDetailByBetTypeModel
                                {
                                    Name = previousSport + " " + Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.SportTotal,
                                    Commission = NumberFormat.DecFormat(sportTotalEntity.Commission, 2),
                                    VirtualComm = NumberFormat.DecFormat(sportTotalEntity.VirtualComm, 2),
                                    LineTotal = NumberFormat.DecFormat(sportTotalEntity.LineTotal, 2),
                                    VirtualLineTotal = NumberFormat.DecFormat(sportTotalEntity.VirtualLineTotal, 2),
                                    Variance = NumberFormat.DecFormat(sportTotalEntity.Variance, 2),
                                    Turnover = NumberFormat.DecFormat(sportTotalEntity.Turnover, 2),
                                    Margin = this.CalculateMargin(sportTotalEntity.LineTotal, sportTotalEntity.Turnover)
                                };

                                wlAllDetailsReport.NonThirdPartyDetails.Add(wlNon3rdPartyBySportReport);
                                wlNon3rdPartyBySportReport = new WLBySportsGroupByBetType
                                {
                                    BetTypeGroup = new List<WLDetailBySportReport>()
                                };

                                nonThirdPartyGrandTotalEntity.Commission += sportTotalEntity.Commission;
                                nonThirdPartyGrandTotalEntity.VirtualComm += sportTotalEntity.VirtualComm;
                                nonThirdPartyGrandTotalEntity.LineTotal += sportTotalEntity.LineTotal;
                                nonThirdPartyGrandTotalEntity.VirtualLineTotal += sportTotalEntity.VirtualLineTotal;
                                nonThirdPartyGrandTotalEntity.Variance += sportTotalEntity.Variance;
                                nonThirdPartyGrandTotalEntity.Turnover += sportTotalEntity.Turnover;

                                #endregion InsertPreviousSport
                                #region SaveCurrentDataToEntities

                                sportTotalEntity = new WLDetailByBetType();
                                previousSport = wlDetailByBetType.SportType.Name;
                                previousBetType = GetLiveTypeName(wlDetailByBetType.LiveIndicator.Id, true) +
                                    wlDetailByBetType.BetType.Name;

                                #endregion SaveCurrentDataToEntities
                            }
                            else if (currentBetType != previousBetType)
                            {
                                #region InsertPreviousBetType

                                wlgroupByBetTypeReport.Total = new WLDetailByBetTypeModel
                                {
                                    Name = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.BetTypeTotal,
                                    Commission = NumberFormat.DecFormat(betTypeGroupTotalEntity.Commission, 2),
                                    VirtualComm = NumberFormat.DecFormat(betTypeGroupTotalEntity.VirtualComm, 2),
                                    LineTotal = NumberFormat.DecFormat(betTypeGroupTotalEntity.LineTotal, 2),
                                    VirtualLineTotal = NumberFormat.DecFormat(betTypeGroupTotalEntity.VirtualLineTotal, 2),
                                    Variance = NumberFormat.DecFormat(betTypeGroupTotalEntity.Variance, 2),
                                    Turnover = NumberFormat.DecFormat(betTypeGroupTotalEntity.Turnover, 2),
                                    Margin = this.CalculateMargin(betTypeGroupTotalEntity.LineTotal, betTypeGroupTotalEntity.Turnover)
                                };

                                wlNon3rdPartyBySportReport.BetTypeGroup.Add(wlgroupByBetTypeReport);
                                sportTotalEntity.Commission += betTypeGroupTotalEntity.Commission;
                                sportTotalEntity.VirtualComm += betTypeGroupTotalEntity.VirtualComm;
                                sportTotalEntity.LineTotal += betTypeGroupTotalEntity.LineTotal;
                                sportTotalEntity.VirtualLineTotal += betTypeGroupTotalEntity.VirtualLineTotal;
                                sportTotalEntity.Variance += betTypeGroupTotalEntity.Variance;
                                sportTotalEntity.Turnover += betTypeGroupTotalEntity.Turnover;

                                wlgroupByBetTypeReport = new WLDetailBySportReport
                                {
                                    ReportDetails = new List<WLDetailByBetTypeModel>()
                                };

                                betTypeGroupTotalEntity.Commission = wlEntity.Commission;
                                betTypeGroupTotalEntity.VirtualComm = wlEntity.VirtualComm;
                                betTypeGroupTotalEntity.LineTotal = wlEntity.LineTotal;
                                betTypeGroupTotalEntity.VirtualLineTotal = wlEntity.VirtualLineTotal;
                                betTypeGroupTotalEntity.Variance = wlEntity.Variance;
                                betTypeGroupTotalEntity.Turnover = wlEntity.Turnover;

                                #endregion InsertPreviousBetType

                                previousBetType = currentBetType;
                            }
                            else
                            {
                                #region AddValueToTotal

                                betTypeGroupTotalEntity.Commission += wlEntity.Commission;
                                betTypeGroupTotalEntity.VirtualComm += wlEntity.VirtualComm;
                                betTypeGroupTotalEntity.LineTotal += wlEntity.LineTotal;
                                betTypeGroupTotalEntity.VirtualLineTotal += wlEntity.VirtualLineTotal;
                                betTypeGroupTotalEntity.Variance += wlEntity.Variance;
                                betTypeGroupTotalEntity.Turnover += wlEntity.Turnover;

                                #endregion AddValueToTotal
                            }


                            wlDetailByBetType.Name = GetLiveTypeName(wlDetailByBetType.LiveIndicator.Id, true) +
                                previousBetType +
                                Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.NewLine +
                                wlDetailByBetType.MatchCode;
                            wlgroupByBetTypeReport.ReportDetails.Add(wlDetailByBetType);
                        }

                        // AddLastSport   
                        if (previousSport != null)
                        {
                            #region InsertPreviousBetType

                            wlgroupByBetTypeReport.Total = new WLDetailByBetTypeModel
                            {
                                Name = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.BetTypeTotal,
                                Commission = NumberFormat.DecFormat(betTypeGroupTotalEntity.Commission, 2),
                                VirtualComm = NumberFormat.DecFormat(betTypeGroupTotalEntity.VirtualComm, 2),
                                LineTotal = NumberFormat.DecFormat(betTypeGroupTotalEntity.LineTotal, 2),
                                VirtualLineTotal = NumberFormat.DecFormat(betTypeGroupTotalEntity.VirtualLineTotal, 2),
                                Variance = NumberFormat.DecFormat(betTypeGroupTotalEntity.Variance, 2),
                                Turnover = NumberFormat.DecFormat(betTypeGroupTotalEntity.Turnover, 2),
                                Margin = this.CalculateMargin(betTypeGroupTotalEntity.LineTotal, betTypeGroupTotalEntity.Turnover)
                            };

                            wlNon3rdPartyBySportReport.BetTypeGroup.Add(wlgroupByBetTypeReport);
                            sportTotalEntity.Commission += betTypeGroupTotalEntity.Commission;
                            sportTotalEntity.VirtualComm += betTypeGroupTotalEntity.VirtualComm;
                            sportTotalEntity.LineTotal += betTypeGroupTotalEntity.LineTotal;
                            sportTotalEntity.VirtualLineTotal += betTypeGroupTotalEntity.VirtualLineTotal;
                            sportTotalEntity.Variance += betTypeGroupTotalEntity.Variance;
                            sportTotalEntity.Turnover += betTypeGroupTotalEntity.Turnover;

                            previousBetType = currentBetType;

                            #endregion InsertPreviousBetType
                            #region InsertPreviousSport

                            wlNon3rdPartyBySportReport.Total = new WLDetailByBetTypeModel
                            {
                                Name = previousSport + " " + Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.SportTotal,
                                Commission = NumberFormat.DecFormat(sportTotalEntity.Commission, 2),
                                VirtualComm = NumberFormat.DecFormat(sportTotalEntity.VirtualComm, 2),
                                LineTotal = NumberFormat.DecFormat(sportTotalEntity.LineTotal, 2),
                                VirtualLineTotal = NumberFormat.DecFormat(sportTotalEntity.VirtualLineTotal, 2),
                                Variance = NumberFormat.DecFormat(sportTotalEntity.Variance, 2),
                                Turnover = NumberFormat.DecFormat(sportTotalEntity.Turnover, 2),
                                Margin = this.CalculateMargin(sportTotalEntity.LineTotal, sportTotalEntity.Turnover)
                            };

                            wlAllDetailsReport.NonThirdPartyDetails.Add(wlNon3rdPartyBySportReport);

                            nonThirdPartyGrandTotalEntity.Commission += sportTotalEntity.Commission;
                            nonThirdPartyGrandTotalEntity.VirtualComm += sportTotalEntity.VirtualComm;
                            nonThirdPartyGrandTotalEntity.LineTotal += sportTotalEntity.LineTotal;
                            nonThirdPartyGrandTotalEntity.VirtualLineTotal += sportTotalEntity.VirtualLineTotal;
                            nonThirdPartyGrandTotalEntity.Variance += sportTotalEntity.Variance;
                            nonThirdPartyGrandTotalEntity.Turnover += sportTotalEntity.Turnover;

                            #endregion InsertPreviousSport
                        }

                        wlAllDetailsReport.NonThirdPartyGrandTotal = new WLDetailByBetTypeModel
                        {
                            Name = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.GrandTotal,
                            Commission = NumberFormat.DecFormat(nonThirdPartyGrandTotalEntity.Commission, 2),
                            VirtualComm = NumberFormat.DecFormat(nonThirdPartyGrandTotalEntity.VirtualComm, 2),
                            LineTotal = NumberFormat.DecFormat(nonThirdPartyGrandTotalEntity.LineTotal, 2),
                            VirtualLineTotal = NumberFormat.DecFormat(nonThirdPartyGrandTotalEntity.VirtualLineTotal, 2),
                            Variance = NumberFormat.DecFormat(nonThirdPartyGrandTotalEntity.Variance, 2),
                            Turnover = NumberFormat.DecFormat(nonThirdPartyGrandTotalEntity.Turnover, 2),
                            Margin = this.CalculateMargin(nonThirdPartyGrandTotalEntity.LineTotal, nonThirdPartyGrandTotalEntity.Turnover)
                        };
                    }

                    #endregion GetNonThirdPartyData
                    #region GetThirdPartyData

                    if (totalTable >= 2)
                    {
                        var thirdPartyData = dbResult.Tables[1];
                        var wl3rdPartyByBetType = new WLDetailByBetTypeModel();
                        foreach (DataRow row in thirdPartyData.Rows)
                        {
                            wl3rdPartyEntity = new WLDetailByBetType
                            {
                                Name = CastAide.AsString(row["bettype"]),
                                Commission = CastAide.AsDecimal(row["comm"]),
                                VirtualComm = CastAide.AsDecimal(row["VirtualComm"]),
                                LineTotal = CastAide.AsDecimal(row["LineTotal"]),
                                VirtualLineTotal = CastAide.AsDecimal(row["VirtualLineTotal"]),
                                Position = CastAide.AsDecimal(row["pos"]),
                                WinLost = CastAide.AsDecimal(row["winlost"]),
                                Turnover = CastAide.AsDecimal(row["IBCTurnover"])
                            };

                            wl3rdPartyByBetType = new WLDetailByBetTypeModel
                            {
                                Name = wl3rdPartyEntity.Name,
                                Commission = NumberFormat.DecFormat(wl3rdPartyEntity.Commission, 2),
                                VirtualComm = NumberFormat.DecFormat(wl3rdPartyEntity.VirtualComm, 2),
                                LineTotal = NumberFormat.DecFormat(wl3rdPartyEntity.LineTotal, 2),
                                VirtualLineTotal = NumberFormat.DecFormat(wl3rdPartyEntity.VirtualLineTotal, 2),
                                Position = NumberFormat.DecFormat(wl3rdPartyEntity.Position, 2),
                                WinLost = NumberFormat.DecFormat(wl3rdPartyEntity.WinLost, 2),
                                Variance = NumberFormat.DecFormat(wl3rdPartyEntity.Variance, 2),
                                EventDate = CastAide.AsDateTime(row["winlostdate"]).ToShortDateString(),
                                Turnover = NumberFormat.DecFormat(wl3rdPartyEntity.Turnover, 2),
                                Margin = this.CalculateMargin(wl3rdPartyEntity.LineTotal, wl3rdPartyEntity.Turnover)
                            };

                            var betTypeLowerCase = wl3rdPartyEntity.Name.ToLower();
                            if (betTypeLowerCase.Equals(Fanex.Tarus.Reports.Resource.BetType.Casino.ToLower()))
                            {
                                wl3rdPartyByBetType.ThirdPartyType = RowType.DisplayLink;
                                wl3rdPartyByBetType.VirtualComm = _notAvailableString;
                                wl3rdPartyByBetType.VirtualLineTotal = _notAvailableString;
                                wl3rdPartyByBetType.Variance = _notAvailableString;
                                wl3rdPartyEntity.Variance = 0;
                            }
                            else if (betTypeLowerCase.Equals(Fanex.Tarus.Reports.Resource.BetType.Racing.ToLower()) ||
                                     betTypeLowerCase.Equals(Fanex.Tarus.Reports.Resource.BetType.LiveCasino.ToLower()) ||
                                     betTypeLowerCase.Equals(Fanex.Tarus.Reports.Resource.BetType.VirtualSports.ToLower()) ||
                                betTypeLowerCase.Equals(Fanex.Tarus.Reports.Resource.BetType.P2P.ToLower()))
                            {
                                wl3rdPartyByBetType.VirtualComm = _notAvailableString;
                                wl3rdPartyByBetType.VirtualLineTotal = _notAvailableString;
                                wl3rdPartyByBetType.Variance = _notAvailableString;
                                wl3rdPartyEntity.Variance = 0;
                            }
                            else
                            {
                                wl3rdPartyEntity.Variance = wl3rdPartyEntity.LineTotal - wl3rdPartyEntity.VirtualLineTotal;
                                wl3rdPartyByBetType.Variance = NumberFormat.DecFormat(wl3rdPartyEntity.Variance, 2);
                            }

                            if (string.Equals(wl3rdPartyEntity.Name, "Casino", StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(wl3rdPartyEntity.Name, "Bingo", StringComparison.OrdinalIgnoreCase))
                            {
                                wl3rdPartyEntity.Turnover = 0;
                                wl3rdPartyByBetType.Turnover = _notAvailableString;
                                wl3rdPartyByBetType.Margin = _notAvailableString;
                            }

                            wlAllDetailsReport.ThirdPartyDetails.ReportDetails.Add(wl3rdPartyByBetType);
                            #region AddValueToTotal

                            _3rdPartyTotalEntity.Commission += wl3rdPartyEntity.Commission;
                            _3rdPartyTotalEntity.VirtualComm += wl3rdPartyEntity.VirtualComm;
                            _3rdPartyTotalEntity.LineTotal += wl3rdPartyEntity.LineTotal;
                            _3rdPartyTotalEntity.VirtualLineTotal += wl3rdPartyEntity.VirtualLineTotal;
                            _3rdPartyTotalEntity.Variance += wl3rdPartyEntity.Variance;
                            _3rdPartyTotalEntity.Turnover += wl3rdPartyEntity.Turnover;

                            #endregion AddValueToTotal

                        }

                    }

                    #endregion GetThirdPartyData
                    #region CalculateTotal

                    wlAllDetailsReport.IsBARMActive = _barmUtils.IsActive;
                    wlAllDetailsReport.BARMName = Fanex.Tarus.Reports.Resource.MatchWinLoss.Total + " " + _barmUtils.DisplayName + ":";
                    wlAllDetailsReport.BARMTotal = NumberFormat.DecFormat(barmTotal, 2);
                    wlAllDetailsReport.BARMTurnover = NumberFormat.DecFormat(barmTurnover, 2);
                    wlAllDetailsReport.BARMMargin = this.CalculateMargin(barmTotal, barmTurnover);

                    wlAllDetailsReport.IsEightRMActive = _eightRMUtils.IsActive;
                    wlAllDetailsReport.EightRMName = Fanex.Tarus.Reports.Resource.MatchWinLoss.Total + " " + _eightRMUtils.DisplayName + ":";
                    wlAllDetailsReport.EightRMTotal = NumberFormat.DecFormat(eightRMTotal, 2);
                    wlAllDetailsReport.EightRMTurnover = NumberFormat.DecFormat(eightRMTurnover, 2);
                    wlAllDetailsReport.EightRMMargin = this.CalculateMargin(eightRMTotal, eightRMTurnover);

                    wlAllDetailsReport.Total = new WLDetailByBetTypeModel
                    {
                        Name = Fanex.Tarus.Reports.Resource.MatchWinLoss.GrandTotal,
                        Commission = NumberFormat.DecFormat(nonThirdPartyGrandTotalEntity.Commission + _3rdPartyTotalEntity.Commission, 2),
                        VirtualComm = NumberFormat.DecFormat(nonThirdPartyGrandTotalEntity.VirtualComm + _3rdPartyTotalEntity.VirtualComm, 2),
                        LineTotal = NumberFormat.DecFormat(nonThirdPartyGrandTotalEntity.LineTotal + _3rdPartyTotalEntity.LineTotal + eightRMTotal + barmTotal, 2),
                        VirtualLineTotal = NumberFormat.DecFormat(nonThirdPartyGrandTotalEntity.VirtualLineTotal + _3rdPartyTotalEntity.VirtualLineTotal + eightRMTotal + barmTotal, 2),
                        Variance = NumberFormat.DecFormat(nonThirdPartyGrandTotalEntity.Variance + _3rdPartyTotalEntity.Variance, 2),
                        Turnover = NumberFormat.DecFormat(nonThirdPartyGrandTotalEntity.Turnover + _3rdPartyTotalEntity.Turnover + eightRMTurnover + barmTurnover, 2),
                        Margin = this.CalculateMargin(nonThirdPartyGrandTotalEntity.LineTotal + _3rdPartyTotalEntity.LineTotal + eightRMTotal + barmTotal, nonThirdPartyGrandTotalEntity.Turnover + _3rdPartyTotalEntity.Turnover + eightRMTurnover + barmTurnover)
                    };

                    #endregion CalculateTotal
                }

                SessionExtension.AddSession(HttpContext.Current.Session, _wlAllDetails, wlAllDetailsReport);
            }
            catch (Exception ex)
            {
                wlAllDetailsReport = null;
                LogExtensions.ExceptionHandler(ex);
            }

            return wlAllDetailsReport;
        }

        public void ExportWLSummaryMainReports(DateTime reportDate, string reportName)
        {
            try
            {
                #region ExcelTypes

                ExcelFile excelFile = new ExcelFile();
                var gembox = new ExcelGemboxUtils();
                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                ExcelWorksheet ws = null;
                string reportSheetName = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.ReportSheetName;
                ws = worksheets.Add(reportSheetName);

                int startColumn = 0;
                int startRow = 0;
                var currentColumn = startColumn;
                var currentRow = startRow;

                #endregion ExcelTypes
                #region ExportSummaryByBetTypeReport
                var wlSummaryByBetTypeService = new WLSummaryByBetTypeService();
                var wLSummaryByBetTypeReport = (WLSummaryByBetTypeReport)SessionExtension.GetSession(HttpContext.Current.Session, wlSummaryByBetTypeService.WLSummaryReportByBetType);
                if (wLSummaryByBetTypeReport == null)
                {
                    wLSummaryByBetTypeReport = wlSummaryByBetTypeService.SelSummaryReportByBetType(reportDate);
                }

                wlSummaryByBetTypeService.ExportWLSummaryByBetTypeReport(ref ws, startRow, startColumn,
                  ref currentRow, wLSummaryByBetTypeReport, reportDate, reportName);


                #endregion ExportSummaryByBetTypeReport

                currentRow += 2;

                #region ExportWLAllDetails

                var wlAllDetails = SelMatchWinLossReportAllDetails(reportDate);
                ExportWLAllDetails(ref ws, startRow, startColumn,
                    ref currentRow, wlAllDetails, reportDate, reportName);

                #endregion ExportWLAllDetails
                (new ReportHelpers()).ReponseExcelFile(excelFile, reportName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        public void ExportWLAllDetails(DateTime reportDate, string reportName)
        {
            try
            {
                #region GetCurrentReport

                var wlAllDetails = SelMatchWinLossReportAllDetails(reportDate);

                #endregion GetCurrentReport
                #region ExcelTypes

                ExcelFile excelFile = new ExcelFile();
                var gembox = new ExcelGemboxUtils();
                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                ExcelWorksheet ws = null;
                string reportSheetName = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.ReportSheetName;
                ws = worksheets.Add(reportSheetName);

                int startColumn = 0;
                int startRow = 0;
                var currentColumn = startColumn;
                var currentRow = startRow;

                #endregion ExcelTypes

                ExportWLAllDetails(ref ws, startRow, startColumn,
                    ref currentRow, wlAllDetails, reportDate, reportName);
                (new ReportHelpers()).ReponseExcelFile(excelFile, reportName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        public void ExportWLSummaryBySportTypeReport(DateTime reportDate, string reportName)
        {
            try
            {
                #region GetCurrentReport

                var wLSummaryBySportTypeReport = (WLSummaryBySportReport)SessionExtension.GetSession(HttpContext.Current.Session, _wlSummaryReportBySport);
                if (wLSummaryBySportTypeReport == null)
                {
                    wLSummaryBySportTypeReport = SelSummaryReportBySportType(reportDate);
                }

                #endregion GetCurrentReport
                #region ExcelTypes

                ExcelFile excelFile = new ExcelFile();
                var gembox = new ExcelGemboxUtils();
                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                ExcelWorksheet ws = null;
                string reportSheetName = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.ReportSheetName;
                ws = worksheets.Add(reportSheetName);

                int startColumn = 0;
                int startRow = 0;
                var currentColumn = startColumn;
                var currentRow = startRow;

                #endregion ExcelTypes

                ExportWLSummaryBySportTypeReport(ref ws, startRow, startColumn,
                     ref currentRow, wLSummaryBySportTypeReport, reportDate, reportName);
                (new ReportHelpers()).ReponseExcelFile(excelFile, reportName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        public void ExportWLSummaryBySportTypeReport(ref ExcelWorksheet ws, int startRow, int startColumn,
            ref int currentRow, WLSummaryBySportReport wLSummaryBySportTypeReport,
            DateTime reportDate, string reportName)
        {
            #region ExcelTypes

            ExcelFile excelFile = new ExcelFile();
            var gembox = new ExcelGemboxUtils();
            var currentColumn = startColumn;
            var endRow = currentRow;
            var endColumn = currentColumn;

            var headerTextStyle = gembox.GetStyleTitle(181, 196, 208);
            var textStyle = gembox.GetStyleTitle(255, 255, 255);

            #endregion ExcelTypes
            #region WLSummaryBySportTypeReportHeader

            currentColumn = startColumn;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.BetType;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 50;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Comm;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.VirtualComm;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.LineTotal;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.VirtualLineTotal;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Variance;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Turnover;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Margin;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;

            #endregion WLSummaryBySportTypeReportHeader
            #region WLSummaryBySportTypeReportContent

            if (wLSummaryBySportTypeReport != null && (wLSummaryBySportTypeReport.NonThirdPartyDetails.Count > 0 ||
                    wLSummaryBySportTypeReport.ThirdPartyDetails.ReportDetails.Count > 0))
            {
                Color bgColor = Color.FromArgb(255, 255, 255);
                var rightTextFormat = gembox.GetStyleText(bgColor.R, bgColor.G, bgColor.B,
                            HorizontalAlignmentStyle.Right, false);
                var leftTextFormat = gembox.GetStyleText(bgColor.R, bgColor.G, bgColor.B,
                            HorizontalAlignmentStyle.Left, false);
                var centerTextFormat = gembox.GetStyleText(bgColor.R, bgColor.G, bgColor.B,
                            HorizontalAlignmentStyle.Center, false);
                var numberFormart = gembox.GetStyleNumber(0, string.Empty, bgColor.R, bgColor.G, bgColor.B, false);
                var boldNumberFormart = gembox.GetStyleNumber(0, string.Empty, bgColor.R, bgColor.G, bgColor.B, true);
                var boldRightTextFormat = gembox.GetStyleText(bgColor.R, bgColor.G, bgColor.B,
                            HorizontalAlignmentStyle.Right, true);

                Color totalBgColor = Color.FromArgb(211, 225, 200);
                var totalRightTextFormat = gembox.GetStyleText(totalBgColor.R, totalBgColor.G, totalBgColor.B,
                           HorizontalAlignmentStyle.Right, true);
                var totalNumberFormart = gembox.GetStyleNumber(0, string.Empty, totalBgColor.R, totalBgColor.G, totalBgColor.B, false);
                var totalBoldNumberFormart = gembox.GetStyleNumber(0, string.Empty, totalBgColor.R, totalBgColor.G, totalBgColor.B, true);

                var nonThirdPartyLength = wLSummaryBySportTypeReport.NonThirdPartyDetails.Count;
                for (var i = 0; i < nonThirdPartyLength; i++)
                {
                    var sportDetail = wLSummaryBySportTypeReport.NonThirdPartyDetails[i];
                    var sportDetailLength = sportDetail.ReportDetails.Count;
                    var sportTotalDetail = sportDetail.Total;
                    for (var j = 0; j < sportDetailLength; j++)
                    {
                        var reportDetail = sportDetail.ReportDetails[j];
                        #region SportDetails

                        currentRow++;
                        currentColumn = startColumn;
                        ws.Cells[currentRow, currentColumn].Value = GetLiveTypeName(reportDetail.LiveIndicator.Id, true) + reportDetail.BetType.Name;
                        ws.Cells[currentRow, currentColumn].Style = leftTextFormat;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.Commission);
                        ws.Cells[currentRow, currentColumn].Style = numberFormart;

                        currentColumn++;
                        if (reportDetail.VirtualComm == _notAvailableString)
                        {
                            ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                            ws.Cells[currentRow, currentColumn].Style = rightTextFormat;
                        }
                        else
                        {
                            ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.VirtualComm);
                            ws.Cells[currentRow, currentColumn].Style = numberFormart;
                        }

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.LineTotal);
                        ws.Cells[currentRow, currentColumn].Style = boldNumberFormart;

                        currentColumn++;
                        if (reportDetail.VirtualLineTotal == _notAvailableString)
                        {
                            ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                            ws.Cells[currentRow, currentColumn].Style = boldRightTextFormat;
                        }
                        else
                        {
                            ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.VirtualLineTotal);
                            ws.Cells[currentRow, currentColumn].Style = boldNumberFormart;
                        }

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.Variance);
                        ws.Cells[currentRow, currentColumn].Style = numberFormart;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.Turnover);
                        ws.Cells[currentRow, currentColumn].Style = numberFormart;

                        currentColumn++;
                        if (reportDetail.Margin == _notAvailableString)
                        {
                            ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                            ws.Cells[currentRow, currentColumn].Style = rightTextFormat;
                        }
                        else
                        {
                            ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(reportDetail.Margin);
                            ws.Cells[currentRow, currentColumn].Style = numberFormart;
                        }

                        #endregion SportDetails
                    }

                    #region SportTotal

                    currentRow++;
                    currentColumn = startColumn;
                    ws.Cells[currentRow, currentColumn].Value = sportTotalDetail.Name;
                    ws.Cells[currentRow, currentColumn].Style = totalRightTextFormat;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(sportTotalDetail.Commission);
                    ws.Cells[currentRow, currentColumn].Style = totalNumberFormart;

                    currentColumn++;
                    if (sportTotalDetail.VirtualComm == _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                        ws.Cells[currentRow, currentColumn].Style = totalRightTextFormat;
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(sportTotalDetail.VirtualComm);
                        ws.Cells[currentRow, currentColumn].Style = totalNumberFormart;
                    }

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(sportTotalDetail.LineTotal);
                    ws.Cells[currentRow, currentColumn].Style = totalBoldNumberFormart;

                    currentColumn++;
                    if (sportTotalDetail.VirtualLineTotal == _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                        ws.Cells[currentRow, currentColumn].Style = totalRightTextFormat;
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(sportTotalDetail.VirtualLineTotal);
                        ws.Cells[currentRow, currentColumn].Style = totalBoldNumberFormart;
                    }

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(sportTotalDetail.Variance);
                    ws.Cells[currentRow, currentColumn].Style = totalNumberFormart;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(sportTotalDetail.Turnover);
                    ws.Cells[currentRow, currentColumn].Style = totalNumberFormart;

                    currentColumn++;
                    if (sportTotalDetail.Margin == _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                        ws.Cells[currentRow, currentColumn].Style = totalRightTextFormat;
                        ws.Cells[currentRow, currentColumn].Style.Font.Weight = ExcelFont.NormalWeight;
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(sportTotalDetail.Margin);
                        ws.Cells[currentRow, currentColumn].Style = totalNumberFormart;
                    }

                    #endregion SportTotal
                }

                #region GrandTotalNon3rdParty

                Color grandTotalBgColor = Color.FromArgb(136, 170, 255);
                var grandTotalRightTextFormat = gembox.GetStyleText(grandTotalBgColor.R, grandTotalBgColor.G, grandTotalBgColor.B,
                           HorizontalAlignmentStyle.Right, true);
                var grandTotalNumberFormart = gembox.GetStyleNumber(0, string.Empty, grandTotalBgColor.R, grandTotalBgColor.G, grandTotalBgColor.B, false);
                var grandTotalBoldNumberFormart = gembox.GetStyleNumber(0, string.Empty, grandTotalBgColor.R, grandTotalBgColor.G, grandTotalBgColor.B, true);
                var non3rdTotalDetail = wLSummaryBySportTypeReport.NonThirdPartyGrandTotal;

                currentRow++;
                currentColumn = startColumn;
                ws.Cells[currentRow, currentColumn].Value = non3rdTotalDetail.Name;
                ws.Cells[currentRow, currentColumn].Style = grandTotalRightTextFormat;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(non3rdTotalDetail.Commission);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumberFormart;

                currentColumn++;
                if (non3rdTotalDetail.VirtualComm == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = grandTotalRightTextFormat;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(non3rdTotalDetail.VirtualComm);
                    ws.Cells[currentRow, currentColumn].Style = grandTotalNumberFormart;
                }

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(non3rdTotalDetail.LineTotal);
                ws.Cells[currentRow, currentColumn].Style = grandTotalBoldNumberFormart;

                currentColumn++;
                if (non3rdTotalDetail.VirtualLineTotal == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = grandTotalRightTextFormat;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(non3rdTotalDetail.VirtualLineTotal);
                    ws.Cells[currentRow, currentColumn].Style = grandTotalBoldNumberFormart;
                }

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(non3rdTotalDetail.Variance);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumberFormart;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(non3rdTotalDetail.Turnover);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumberFormart;

                currentColumn++;
                if (non3rdTotalDetail.Margin == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = grandTotalRightTextFormat;
                    ws.Cells[currentRow, currentColumn].Style.Font.Weight = ExcelFont.NormalWeight;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(non3rdTotalDetail.Margin);
                    ws.Cells[currentRow, currentColumn].Style = grandTotalNumberFormart;
                }

                #endregion GrandTotalNon3rdParty
                #region EightRM_BARM

                Color rmBgColor = Color.FromArgb(230, 233, 240);
                var rmRightTextFormat = gembox.GetStyleText(rmBgColor.R, rmBgColor.G, rmBgColor.B,
                           HorizontalAlignmentStyle.Right, true);
                var rmNumberFormart = gembox.GetStyleNumber(0, string.Empty, rmBgColor.R, rmBgColor.G, rmBgColor.B, false);
                var rmBoldNumberFormart = gembox.GetStyleNumber(0, string.Empty, rmBgColor.R, rmBgColor.G, rmBgColor.B, true);

                if (_eightRMUtils.IsActive)
                {
                    currentRow++;
                    currentColumn = startColumn;
                    endColumn = currentColumn + 2;
                    endRow = currentRow;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = wLSummaryBySportTypeReport.EightRMName;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = rmRightTextFormat;

                    currentColumn += 3;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wLSummaryBySportTypeReport.EightRMTotal);
                    ws.Cells[currentRow, currentColumn].Style = rmBoldNumberFormart;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wLSummaryBySportTypeReport.EightRMTotal);
                    ws.Cells[currentRow, currentColumn].Style = rmBoldNumberFormart;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal("0.00");
                    ws.Cells[currentRow, currentColumn].Style = rmNumberFormart;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = wLSummaryBySportTypeReport.EightRMTurnover;
                    ws.Cells[currentRow, currentColumn].Style = rmNumberFormart;
                    ws.Cells[currentRow, currentColumn].Style.Font.Weight = ExcelFont.NormalWeight;
                    ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    currentColumn++;
                    if (wLSummaryBySportTypeReport.EightRMMargin == _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                        ws.Cells[currentRow, currentColumn].Style = rmRightTextFormat;
                        ws.Cells[currentRow, currentColumn].Style.Font.Weight = ExcelFont.NormalWeight;
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wLSummaryBySportTypeReport.EightRMMargin);
                        ws.Cells[currentRow, currentColumn].Style = rmNumberFormart;
                    }
                }

                if (_barmUtils.IsActive)
                {
                    currentRow++;
                    currentColumn = startColumn;
                    endColumn = currentColumn + 2;
                    endRow = currentRow;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = wLSummaryBySportTypeReport.BARMName;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = rmRightTextFormat;

                    currentColumn += 3;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wLSummaryBySportTypeReport.BARMTotal);
                    ws.Cells[currentRow, currentColumn].Style = rmBoldNumberFormart;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wLSummaryBySportTypeReport.BARMTotal);
                    ws.Cells[currentRow, currentColumn].Style = rmBoldNumberFormart;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal("0.00");
                    ws.Cells[currentRow, currentColumn].Style = rmNumberFormart;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = wLSummaryBySportTypeReport.BARMTurnover;
                    ws.Cells[currentRow, currentColumn].Style = rmNumberFormart;
                    ws.Cells[currentRow, currentColumn].Style.Font.Weight = ExcelFont.NormalWeight;
                    ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    currentColumn++;
                    if (wLSummaryBySportTypeReport.BARMMargin == _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                        ws.Cells[currentRow, currentColumn].Style = rmRightTextFormat;
                        ws.Cells[currentRow, currentColumn].Style.Font.Weight = ExcelFont.NormalWeight;
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wLSummaryBySportTypeReport.BARMMargin);
                        ws.Cells[currentRow, currentColumn].Style = rmNumberFormart;
                    }
                }

                #endregion EightRM_BARM
                #region ThirdPartyReport

                var thirdPartyLength = wLSummaryBySportTypeReport.ThirdPartyDetails.ReportDetails.Count;
                for (var k = 0; k < thirdPartyLength; k++)
                {
                    var thirdDetail = wLSummaryBySportTypeReport.ThirdPartyDetails.ReportDetails[k];
                    currentRow++;
                    currentColumn = startColumn;
                    ws.Cells[currentRow, currentColumn].Value = thirdDetail.Name;
                    ws.Cells[currentRow, currentColumn].Style = leftTextFormat;
                    ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(thirdDetail.Commission);
                    ws.Cells[currentRow, currentColumn].Style = numberFormart;

                    currentColumn++;
                    if (thirdDetail.VirtualComm == _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                        ws.Cells[currentRow, currentColumn].Style = rightTextFormat;
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(thirdDetail.VirtualComm);
                        ws.Cells[currentRow, currentColumn].Style = numberFormart;
                    }

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(thirdDetail.LineTotal);
                    ws.Cells[currentRow, currentColumn].Style = boldNumberFormart;

                    currentColumn++;
                    if (thirdDetail.VirtualLineTotal == _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                        ws.Cells[currentRow, currentColumn].Style = boldRightTextFormat;
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(thirdDetail.VirtualLineTotal);
                        ws.Cells[currentRow, currentColumn].Style = boldNumberFormart;
                    }

                    currentColumn++;
                    if (thirdDetail.Variance == _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                        ws.Cells[currentRow, currentColumn].Style = rightTextFormat;
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(thirdDetail.Variance);
                        ws.Cells[currentRow, currentColumn].Style = numberFormart;
                    }

                    currentColumn++;
                    if (thirdDetail.Turnover == _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                        ws.Cells[currentRow, currentColumn].Style = rightTextFormat;
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(thirdDetail.Turnover);
                        ws.Cells[currentRow, currentColumn].Style = numberFormart;
                    }

                    currentColumn++;
                    if (thirdDetail.Margin == _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                        ws.Cells[currentRow, currentColumn].Style = rightTextFormat;
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(thirdDetail.Margin);
                        ws.Cells[currentRow, currentColumn].Style = numberFormart;
                    }
                }

                #endregion ThirdPartyReport
                #region ReprotGrandTotal

                var grandTotalDetail = wLSummaryBySportTypeReport.Total;

                currentRow++;
                currentColumn = startColumn;
                ws.Cells[currentRow, currentColumn].Value = grandTotalDetail.Name;
                ws.Cells[currentRow, currentColumn].Style = grandTotalRightTextFormat;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(grandTotalDetail.Commission);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumberFormart;

                currentColumn++;
                if (non3rdTotalDetail.VirtualComm == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = grandTotalRightTextFormat;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(grandTotalDetail.VirtualComm);
                    ws.Cells[currentRow, currentColumn].Style = grandTotalNumberFormart;
                }

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(grandTotalDetail.LineTotal);
                ws.Cells[currentRow, currentColumn].Style = grandTotalBoldNumberFormart;

                currentColumn++;
                if (grandTotalDetail.VirtualLineTotal == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = grandTotalRightTextFormat;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(grandTotalDetail.VirtualLineTotal);
                    ws.Cells[currentRow, currentColumn].Style = grandTotalBoldNumberFormart;
                }

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(grandTotalDetail.Variance);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumberFormart;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(grandTotalDetail.Turnover);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumberFormart;

                currentColumn++;
                if (grandTotalDetail.Margin == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = grandTotalRightTextFormat;
                    ws.Cells[currentRow, currentColumn].Style.Font.Weight = ExcelFont.NormalWeight;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(grandTotalDetail.Margin);
                    ws.Cells[currentRow, currentColumn].Style = grandTotalNumberFormart;
                }

                #endregion ReprotGrandTotal
            }
            else
            {
                currentRow++;
                currentColumn = startColumn;
                endColumn = currentColumn + 7;
                endRow = currentRow;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.Language.EmptyReportMessage;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = textStyle;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style.Font.Weight = ExcelFont.BoldWeight;
            }

            #endregion WLSummaryBySportTypeReportContent
        }

        public void ExportSportTypeReportDetail(DateTime reportDate, string reportName, string sportName)
        {
            try
            {
                #region ExcelTypes

                ExcelFile excelFile = new ExcelFile();
                var gembox = new ExcelGemboxUtils();
                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                ExcelWorksheet ws = null;
                string reportSheetName = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.ReportSheetName;
                ws = worksheets.Add(reportSheetName);

                int startColumn = 0;
                int startRow = 0;
                var currentColumn = startColumn;
                var currentRow = startRow;

                #endregion ExcelTypes
                #region GetSportTypeReportDetail

                var wlSportTypeReport = (WLBySportsGroupByBetType)SessionExtension.GetSession(HttpContext.Current.Session, _sportTypeReportDetail);
                if (wlSportTypeReport == null)
                {
                    wlSportTypeReport = SelSportTypeReportDetail(reportDate, sportName);
                }

                #endregion GetSportTypeReportDetail
                ExportSportTypeReportDetail(ref ws, startRow, startColumn,
                    ref currentRow, wlSportTypeReport, reportDate, reportName, sportName);

                (new ReportHelpers()).ReponseExcelFile(excelFile, reportName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        public void ExportBetTypeReportDetail(DateTime reportDate, string reportName,
            string sportName, int betType, string betTypeName, int liveType)
        {
            try
            {
                #region ExcelTypes

                ExcelFile excelFile = new ExcelFile();
                var gembox = new ExcelGemboxUtils();
                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                ExcelWorksheet ws = null;
                string reportSheetName = Fanex.Tarus.Reports.Resource.WLSummaryByBetType.ReportSheetName;
                ws = worksheets.Add(reportSheetName);

                int startColumn = 0;
                int startRow = 0;
                var currentColumn = startColumn;
                var currentRow = startRow;

                #endregion ExcelTypes
                #region GetBetTypeReportDetail

                var wlBetTypeReport = (WLDetailBySportReport)SessionExtension.GetSession(HttpContext.Current.Session, _betTypeReportDetail);
                if (wlBetTypeReport == null)
                {
                    wlBetTypeReport = SelBetTypeReportDetail(reportDate, sportName, betType, liveType);
                }

                #endregion GetBetTypeReportDetail

                ExportBetTypeReportDetail(ref ws, startRow, startColumn,
                    ref currentRow, wlBetTypeReport, reportDate, reportName, sportName,
                    betType, betTypeName, liveType);
                (new ReportHelpers()).ReponseExcelFile(excelFile, reportName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private void ExportSportTypeReportDetail(ref ExcelWorksheet ws, int startRow, int startColumn,
            ref int currentRow, WLBySportsGroupByBetType wlSportTypeReport,
            DateTime reportDate, string reportName, string sportName)
        {
            #region ExcelTypes

            ExcelFile excelFile = new ExcelFile();
            var gembox = new ExcelGemboxUtils();
            var currentColumn = startColumn;
            var endRow = currentRow;
            var endColumn = currentColumn;

            var headerTextStyle = gembox.GetStyleTitle(181, 196, 208);
            var textStyle = gembox.GetStyleTitle(255, 255, 255);

            #endregion ExcelTypes
            #region HeaderTitle

            currentColumn = startColumn;
            endColumn = currentColumn + 12;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = sportName.ToUpper() + "  PRODUCT WIN/LOSS REPORT FOR " + string.Format("{0:MM/dd/yyyy}", reportDate);
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style.Font.Weight = ExcelFont.BoldWeight;

            #endregion HeaderTitle
            #region WLAllDetailsHeader

            currentRow++;
            currentColumn = startColumn;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.BetType;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 50;

            currentColumn++;
            endColumn = currentColumn + 2;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Match;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 90;

            currentColumn += 3;
            endColumn = currentColumn;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Winloss;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Position;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Comm;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.VirtualComm;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.LineTotal;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.VirtualLineTotal;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Variance;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Turnover;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Margin;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            #endregion WLAllDetailsHeader
            #region BetTypeReportDetail

            if (wlSportTypeReport != null && wlSportTypeReport.BetTypeGroup != null &&
                wlSportTypeReport.BetTypeGroup.Count > 0)
            {
                #region Styles

                Color bgColor = Color.FromArgb(230, 233, 240);
                var bgText = gembox.GetStyleText(bgColor.R, bgColor.G, bgColor.B);
                var bgNumber = gembox.GetStyleNumber(0, string.Empty, bgColor.R, bgColor.G, bgColor.B);

                Color betTotalColor = Color.FromArgb(255, 238, 211);
                var betTotalText = gembox.GetStyleText(betTotalColor.R, betTotalColor.G, betTotalColor.B,
                    HorizontalAlignmentStyle.Right, true);
                var betTotalNumber = gembox.GetStyleNumber(0, string.Empty, betTotalColor.R, betTotalColor.G, betTotalColor.B, false);

                Color grandTotalColor = Color.FromArgb(136, 170, 255);
                var grandTotalText = gembox.GetStyleText(grandTotalColor.R, grandTotalColor.G, grandTotalColor.B,
                    HorizontalAlignmentStyle.Right, true);
                var grandTotalNumber = gembox.GetStyleNumber(0, string.Empty, grandTotalColor.R, grandTotalColor.G, grandTotalColor.B, false);

                #endregion Styles

                foreach (var group in wlSportTypeReport.BetTypeGroup)
                {
                    foreach (var detail in group.ReportDetails)
                    {
                        #region ExportNormalRow

                        currentRow++;
                        currentColumn = startColumn;

                        ws.Cells[currentRow, currentColumn].Value =
                            GetLiveTypeName(detail.LiveIndicator.Id, true) + detail.BetType.Name + " " + detail.MatchCode;
                        ws.Cells[currentRow, currentColumn].Style = bgText;

                        currentColumn++;
                        endColumn = currentColumn + 2;
                        endRow = currentRow;
                        ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                        ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = bgText;
                        ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value =
                           GetMatchEvent(detail.SportType.Id, detail.BetType.Id, detail.HomeName, detail.AwayName);

                        currentColumn += 3;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.WinLost);
                        ws.Cells[currentRow, currentColumn].Style = bgNumber;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Position);
                        ws.Cells[currentRow, currentColumn].Style = bgNumber;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Commission);
                        ws.Cells[currentRow, currentColumn].Style = bgNumber;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.VirtualComm);
                        ws.Cells[currentRow, currentColumn].Style = bgNumber;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.LineTotal);
                        ws.Cells[currentRow, currentColumn].Style = bgNumber;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.VirtualLineTotal);
                        ws.Cells[currentRow, currentColumn].Style = bgNumber;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Variance);
                        ws.Cells[currentRow, currentColumn].Style = bgNumber;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Turnover);
                        ws.Cells[currentRow, currentColumn].Style = bgNumber;

                        currentColumn++;
                        if (detail.Margin == _notAvailableString)
                        {
                            ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                            ws.Cells[currentRow, currentColumn].Style = bgText;
                            ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        }
                        else
                        {
                            ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Margin);
                            ws.Cells[currentRow, currentColumn].Style = bgNumber;
                        }

                        #endregion ExportNormalRow
                    }
                    #region TotalOfEachBetType-LiveIndicator

                    currentRow++;
                    currentColumn = startColumn;
                    endColumn = currentColumn + 5;
                    endRow = currentRow;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = group.Total.Name;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = betTotalText;

                    currentColumn += 6;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(group.Total.Commission);
                    ws.Cells[currentRow, currentColumn].Style = betTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(group.Total.VirtualComm);
                    ws.Cells[currentRow, currentColumn].Style = betTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(group.Total.LineTotal);
                    ws.Cells[currentRow, currentColumn].Style = betTotalNumber;
                    ws.Cells[currentRow, currentColumn].Style.Font.Weight = ExcelFont.BoldWeight;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(group.Total.VirtualLineTotal);
                    ws.Cells[currentRow, currentColumn].Style = betTotalNumber;
                    ws.Cells[currentRow, currentColumn].Style.Font.Weight = ExcelFont.BoldWeight;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(group.Total.Variance);
                    ws.Cells[currentRow, currentColumn].Style = betTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(group.Total.Turnover);
                    ws.Cells[currentRow, currentColumn].Style = betTotalNumber;

                    currentColumn++;
                    if (group.Total.Margin == _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                        ws.Cells[currentRow, currentColumn].Style = betTotalText;
                        ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(group.Total.Margin);
                        ws.Cells[currentRow, currentColumn].Style = betTotalNumber;
                    }

                    #endregion TotalOfEachBetType-LiveIndicator
                }

                #region TotalOfEachSport

                currentRow++;
                currentColumn = startColumn;
                endColumn = currentColumn + 5;
                endRow = currentRow;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow, endColumn).Merged = true;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow, endColumn).Value = wlSportTypeReport.Total.Name;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow, endColumn).Style = grandTotalText;

                currentColumn += 6;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlSportTypeReport.Total.Commission);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlSportTypeReport.Total.VirtualComm);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlSportTypeReport.Total.LineTotal);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;
                ws.Cells[currentRow, currentColumn].Style.Font.Weight = ExcelFont.BoldWeight;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlSportTypeReport.Total.VirtualLineTotal);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;
                ws.Cells[currentRow, currentColumn].Style.Font.Weight = ExcelFont.BoldWeight;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlSportTypeReport.Total.Variance);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlSportTypeReport.Total.Turnover);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                if (wlSportTypeReport.Total.Margin == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = grandTotalText;
                    ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlSportTypeReport.Total.Margin);
                    ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;
                }

                #endregion TotalOfEachSport
            }
            else
            {
                #region EmptyReport

                currentRow++;
                currentColumn = startColumn;
                endColumn = currentColumn + 12;
                endRow = currentRow;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.Language.EmptyReportMessage;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = textStyle;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style.Font.Weight = ExcelFont.BoldWeight;

                #endregion EmptyReport
            }

            #endregion BetTypeReportDetail
        }

        private void ExportBetTypeReportDetail(ref ExcelWorksheet ws, int startRow, int startColumn,
            ref int currentRow, WLDetailBySportReport wlBetTypeReport,
            DateTime reportDate, string reportName, string sportName,
            int betType, string betTypeName, int liveType)
        {
            #region ExcelTypes

            ExcelFile excelFile = new ExcelFile();
            var gembox = new ExcelGemboxUtils();
            var currentColumn = startColumn;
            var endRow = currentRow;
            var endColumn = currentColumn;

            var headerTextStyle = gembox.GetStyleTitle(181, 196, 208);
            var textStyle = gembox.GetStyleTitle(255, 255, 255);

            #endregion ExcelTypes
            #region HeaderTitle

            currentColumn = startColumn;
            endColumn = currentColumn + 12;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = sportName.ToUpper() + "  PRODUCT " + GetLiveTypeName(liveType, false).ToUpper() + betTypeName.ToUpper() + " BETTYPE WIN/LOSS REPORT FOR " + string.Format("{0:MM/dd/yyyy}", reportDate);
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style.Font.Weight = ExcelFont.BoldWeight;

            #endregion HeaderTitle
            #region WLAllDetailsHeader

            currentRow++;
            currentColumn = startColumn;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.BetType;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 50;

            currentColumn++;
            endColumn = currentColumn + 2;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Match;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 90;

            currentColumn += 3;
            endColumn = currentColumn;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Winloss;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Position;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Comm;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.VirtualComm;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.LineTotal;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.VirtualLineTotal;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Variance;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Turnover;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Margin;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            #endregion WLAllDetailsHeader
            #region BetTypeReportDetail

            if (wlBetTypeReport != null && wlBetTypeReport.ReportDetails != null &&
                wlBetTypeReport.ReportDetails.Count > 0)
            {
                #region Styles

                Color bgColor = Color.FromArgb(230, 233, 240);
                var bgText = gembox.GetStyleText(bgColor.R, bgColor.G, bgColor.B);
                var bgNumber = gembox.GetStyleNumber(0, string.Empty, bgColor.R, bgColor.G, bgColor.B);

                Color grandTotalColor = Color.FromArgb(136, 170, 255);
                var grandTotalText = gembox.GetStyleText(grandTotalColor.R, grandTotalColor.G, grandTotalColor.B,
                    HorizontalAlignmentStyle.Right, true);
                var grandTotalNumber = gembox.GetStyleNumber(0, string.Empty, grandTotalColor.R, grandTotalColor.G, grandTotalColor.B, false);

                #endregion Styles
                foreach (var detail in wlBetTypeReport.ReportDetails)
                {
                    #region ExportNormalRow

                    currentRow++;
                    currentColumn = startColumn;

                    ws.Cells[currentRow, currentColumn].Value =
                        GetLiveTypeName(detail.LiveIndicator.Id, true) + detail.BetType.Name + " " + detail.MatchCode;
                    ws.Cells[currentRow, currentColumn].Style = bgText;

                    currentColumn++;
                    endColumn = currentColumn + 2;
                    endRow = currentRow;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = bgText;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value =
                       GetMatchEvent(detail.SportType.Id, detail.BetType.Id, detail.HomeName, detail.AwayName);

                    currentColumn += 3;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.WinLost);
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Position);
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Commission);
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.VirtualComm);
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.LineTotal);
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.VirtualLineTotal);
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Variance);
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Turnover);
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;

                    currentColumn++;
                    if (detail.Margin == _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                        ws.Cells[currentRow, currentColumn].Style = bgText;
                        ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Margin);
                        ws.Cells[currentRow, currentColumn].Style = bgNumber;
                    }

                    #endregion ExportNormalRow
                }
                #region TotalOfEachBetType-LiveIndicator

                currentRow++;
                currentColumn = startColumn;
                endColumn = currentColumn + 5;
                endRow = currentRow;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = wlBetTypeReport.Total.Name;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = grandTotalText;

                currentColumn += 6;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlBetTypeReport.Total.Commission);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlBetTypeReport.Total.VirtualComm);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlBetTypeReport.Total.LineTotal);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;
                ws.Cells[currentRow, currentColumn].Style.Font.Weight = ExcelFont.BoldWeight;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlBetTypeReport.Total.VirtualLineTotal);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;
                ws.Cells[currentRow, currentColumn].Style.Font.Weight = ExcelFont.BoldWeight;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlBetTypeReport.Total.Variance);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlBetTypeReport.Total.Turnover);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                if (wlBetTypeReport.Total.Margin == _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                    ws.Cells[currentRow, currentColumn].Style = grandTotalText;
                    ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlBetTypeReport.Total.Margin);
                    ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;
                }

                #endregion TotalOfEachBetType-LiveIndicator
            }
            else
            {
                #region EmptyReport

                currentRow++;
                currentColumn = startColumn;
                endColumn = currentColumn + 12;
                endRow = currentRow;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.Language.EmptyReportMessage;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = textStyle;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style.Font.Weight = ExcelFont.BoldWeight;

                #endregion EmptyReport
            }

            #endregion BetTypeReportDetail
        }

        private void ExportWLAllDetails(ref ExcelWorksheet ws, int startRow, int startColumn,
                                        ref int currentRow, WLAllDetailsReport wlAllDetails,
                                        DateTime reportDate, string reportName)
        {
            #region ExcelTypes

            ExcelFile excelFile = new ExcelFile();
            var gembox = new ExcelGemboxUtils();
            var currentColumn = startColumn;
            var endRow = currentRow;
            var endColumn = currentColumn;

            var headerTextStyle = gembox.GetStyleTitle(181, 196, 208);
            var textStyle = gembox.GetStyleTitle(255, 255, 255);

            #endregion ExcelTypes
            #region WLAllDetailsHeader

            currentColumn = startColumn;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.BetType;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 50;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Date;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn + 2;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Match;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;

            currentColumn += 3;
            endColumn = currentColumn;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Winloss;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Position;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Comm;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.VirtualComm;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.LineTotal;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.VirtualLineTotal;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Variance;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Turnover;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            currentColumn++;
            endColumn = currentColumn;
            endRow = currentRow;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.Margin;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = headerTextStyle;
            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
            ws.Columns[currentColumn].Width = 120 * 30;

            #endregion WLAllDetailsHeader
            #region WLSummaryByBetTypeReportContent

            if (wlAllDetails != null && (wlAllDetails.NonThirdPartyDetails.Count > 0 ||
                                        (wlAllDetails.ThirdPartyDetails != null &&
                                         wlAllDetails.ThirdPartyDetails.ReportDetails.Count > 0)))
            {
                #region Styles

                Color bgColor = Color.FromArgb(230, 233, 240);
                var bgText = gembox.GetStyleText(bgColor.R, bgColor.G, bgColor.B);
                var bgNumber = gembox.GetStyleNumber(0, string.Empty, bgColor.R, bgColor.G, bgColor.B);

                Color betTotalColor = Color.FromArgb(255, 238, 211);
                var betTotalText = gembox.GetStyleText(betTotalColor.R, betTotalColor.G, betTotalColor.B,
                    HorizontalAlignmentStyle.Right, true);
                var betTotalNumber = gembox.GetStyleNumber(0, string.Empty, betTotalColor.R, betTotalColor.G, betTotalColor.B, true);

                Color sportTotalColor = Color.FromArgb(211, 255, 200);
                var sportTotalText = gembox.GetStyleText(sportTotalColor.R, sportTotalColor.G, sportTotalColor.B,
                    HorizontalAlignmentStyle.Right, true);
                var sportTotalNumber = gembox.GetStyleNumber(0, string.Empty, sportTotalColor.R, sportTotalColor.G, sportTotalColor.B, true);

                Color grandTotalColor = Color.FromArgb(136, 170, 255);
                var grandTotalText = gembox.GetStyleText(grandTotalColor.R, grandTotalColor.G, grandTotalColor.B,
                    HorizontalAlignmentStyle.Right, true);
                var grandTotalNumber = gembox.GetStyleNumber(0, string.Empty, grandTotalColor.R, grandTotalColor.G, grandTotalColor.B, true);

                #endregion Styles
                #region ExportNon3rdParty

                foreach (var non3rdDetail in wlAllDetails.NonThirdPartyDetails)
                {
                    foreach (var group in non3rdDetail.BetTypeGroup)
                    {
                        foreach (var detail in group.ReportDetails)
                        {
                            #region ExportNormalRow

                            currentRow++;
                            currentColumn = startColumn;

                            ws.Cells[currentRow, currentColumn].Value =
                                GetLiveTypeName(detail.LiveIndicator.Id, true) + detail.BetType.Name + " " + detail.MatchCode;
                            ws.Cells[currentRow, currentColumn].Style = bgText;

                            currentColumn++;
                            ws.Cells[currentRow, currentColumn].Value = detail.EventDate + " " + detail.KickOffTime;
                            ws.Cells[currentRow, currentColumn].Style = bgText;

                            currentColumn++;
                            endColumn = currentColumn + 2;
                            endRow = currentRow;
                            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = bgText;
                            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                            ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value =
                               GetMatchEvent(detail.SportType.Id, detail.BetType.Id, detail.HomeName, detail.AwayName);

                            currentColumn += 3;
                            ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.WinLost);
                            ws.Cells[currentRow, currentColumn].Style = bgNumber;

                            currentColumn++;
                            ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Position);
                            ws.Cells[currentRow, currentColumn].Style = bgNumber;

                            currentColumn++;
                            ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Commission);
                            ws.Cells[currentRow, currentColumn].Style = bgNumber;

                            currentColumn++;
                            ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.VirtualComm);
                            ws.Cells[currentRow, currentColumn].Style = bgNumber;

                            currentColumn++;
                            ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.LineTotal);
                            ws.Cells[currentRow, currentColumn].Style = bgNumber;

                            currentColumn++;
                            ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.VirtualLineTotal);
                            ws.Cells[currentRow, currentColumn].Style = bgNumber;

                            currentColumn++;
                            ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Variance);
                            ws.Cells[currentRow, currentColumn].Style = bgNumber;

                            currentColumn++;
                            ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Turnover);
                            ws.Cells[currentRow, currentColumn].Style = bgNumber;

                            currentColumn++;
                            ws.Cells[currentRow, currentColumn].Style = bgNumber;
                            if (detail.Margin != _notAvailableString)
                            {
                                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Margin);
                            }
                            else
                            {
                                ws.Cells[currentRow, currentColumn].Value = detail.Margin;
                                ws.Cells[currentRow, currentColumn].Style = bgText;
                                ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                            }
                            #endregion ExportNormalRow
                        }

                        #region TotalOfEachBetType-LiveIndicator

                        currentRow++;
                        currentColumn = startColumn;
                        endColumn = currentColumn + 6;
                        endRow = currentRow;
                        ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                        ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = group.Total.Name;
                        ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = betTotalText;

                        currentColumn += 7;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(group.Total.Commission);
                        ws.Cells[currentRow, currentColumn].Style = betTotalNumber;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(group.Total.VirtualComm);
                        ws.Cells[currentRow, currentColumn].Style = betTotalNumber;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(group.Total.LineTotal);
                        ws.Cells[currentRow, currentColumn].Style = betTotalNumber;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(group.Total.VirtualLineTotal);
                        ws.Cells[currentRow, currentColumn].Style = betTotalNumber;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(group.Total.Variance);
                        ws.Cells[currentRow, currentColumn].Style = betTotalNumber;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(group.Total.Turnover);
                        ws.Cells[currentRow, currentColumn].Style = betTotalNumber;

                        currentColumn++;
                        ws.Cells[currentRow, currentColumn].Style = betTotalNumber;
                        if (group.Total.Margin != _notAvailableString)
                        {
                            ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(group.Total.Margin);
                        }
                        else
                        {
                            ws.Cells[currentRow, currentColumn].Value = group.Total.Margin;
                            ws.Cells[currentRow, currentColumn].Style = betTotalText;
                            ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        }

                        #endregion TotalOfEachBetType-LiveIndicator
                    }

                    #region TotalOfEachSport

                    currentRow++;
                    currentColumn = startColumn;
                    endColumn = currentColumn + 6;
                    endRow = currentRow;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow, endColumn).Merged = true;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow, endColumn).Value = non3rdDetail.Total.Name;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow, endColumn).Style = sportTotalText;

                    currentColumn += 7;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(non3rdDetail.Total.Commission);
                    ws.Cells[currentRow, currentColumn].Style = sportTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(non3rdDetail.Total.VirtualComm);
                    ws.Cells[currentRow, currentColumn].Style = sportTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(non3rdDetail.Total.LineTotal);
                    ws.Cells[currentRow, currentColumn].Style = sportTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(non3rdDetail.Total.VirtualLineTotal);
                    ws.Cells[currentRow, currentColumn].Style = sportTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(non3rdDetail.Total.Variance);
                    ws.Cells[currentRow, currentColumn].Style = sportTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(non3rdDetail.Total.Turnover);
                    ws.Cells[currentRow, currentColumn].Style = sportTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Style = sportTotalNumber;
                    if (non3rdDetail.Total.Margin != _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(non3rdDetail.Total.Margin);
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = non3rdDetail.Total.Margin;
                        ws.Cells[currentRow, currentColumn].Style = sportTotalText;
                        ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    }

                    #endregion TotalOfEachSport
                }

                #region GrandTotalOfNon3rdParty

                currentRow++;
                currentColumn = startColumn;
                endColumn = currentColumn + 6;
                endRow = currentRow;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow, endColumn).Merged = true;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow, endColumn).Value = wlAllDetails.NonThirdPartyGrandTotal.Name;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow, endColumn).Style = grandTotalText;

                currentColumn += 7;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.NonThirdPartyGrandTotal.Commission);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.NonThirdPartyGrandTotal.VirtualComm);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.NonThirdPartyGrandTotal.LineTotal);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.NonThirdPartyGrandTotal.VirtualLineTotal);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.NonThirdPartyGrandTotal.Variance);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.NonThirdPartyGrandTotal.Turnover);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;
                if (wlAllDetails.NonThirdPartyGrandTotal.Margin != _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.NonThirdPartyGrandTotal.Margin);
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = wlAllDetails.NonThirdPartyGrandTotal.Margin;
                    ws.Cells[currentRow, currentColumn].Style = grandTotalText;
                    ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                }

                #endregion GrandTotalOfNon3rdParty
                #endregion ExportNon3rdParty
                #region EightRM

                Color rmTotalColor = Color.FromArgb(124, 152, 164);
                var rmTotalText = gembox.GetStyleText(rmTotalColor.R, rmTotalColor.G, rmTotalColor.B,
                    HorizontalAlignmentStyle.Right, true);
                var rmTotalNumber = gembox.GetStyleNumber(0, string.Empty, rmTotalColor.R, rmTotalColor.G, rmTotalColor.B, true);

                if (wlAllDetails.IsEightRMActive)
                {
                    currentRow++;
                    currentColumn = startColumn;
                    endRow = currentRow;
                    endColumn = currentColumn + 8;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = wlAllDetails.EightRMName;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = rmTotalText;

                    currentColumn += 9;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.EightRMTotal);
                    ws.Cells[currentRow, currentColumn].Style = rmTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.EightRMTotal);
                    ws.Cells[currentRow, currentColumn].Style = rmTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = 0;
                    ws.Cells[currentRow, currentColumn].Style = rmTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = wlAllDetails.EightRMTurnover;
                    ws.Cells[currentRow, currentColumn].Style = rmTotalNumber;
                    ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    currentColumn++;
                    if (wlAllDetails.EightRMMargin == _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                        ws.Cells[currentRow, currentColumn].Style = rmTotalText;
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.EightRMMargin);
                        ws.Cells[currentRow, currentColumn].Style = rmTotalNumber;
                    }
                }

                #endregion EightRM
                #region BARM

                if (wlAllDetails.IsBARMActive)
                {
                    currentRow++;
                    currentColumn = startColumn;
                    endRow = currentRow;
                    endColumn = currentColumn + 8;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = wlAllDetails.BARMName;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = rmTotalText;

                    currentColumn += 9;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.BARMTotal);
                    ws.Cells[currentRow, currentColumn].Style = rmTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.BARMTotal);
                    ws.Cells[currentRow, currentColumn].Style = rmTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal("0.00");
                    ws.Cells[currentRow, currentColumn].Style = rmTotalNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = wlAllDetails.BARMTurnover;
                    ws.Cells[currentRow, currentColumn].Style = rmTotalNumber;
                    ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    currentColumn++;
                    if (wlAllDetails.BARMMargin == _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = _notAvailableString;
                        ws.Cells[currentRow, currentColumn].Style = rmTotalText;
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.BARMMargin);
                        ws.Cells[currentRow, currentColumn].Style = rmTotalNumber;
                    }
                }

                #endregion BARM
                #region ExportThirdParty

                foreach (var detail in wlAllDetails.ThirdPartyDetails.ReportDetails)
                {
                    currentRow++;
                    currentColumn = startColumn;
                    ws.Cells[currentRow, currentColumn].Value = detail.Name;
                    ws.Cells[currentRow, currentColumn].Style = bgText;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = detail.EventDate;
                    ws.Cells[currentRow, currentColumn].Style = bgText;


                    currentColumn++;
                    endColumn = currentColumn + 2;
                    endRow = currentRow;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = string.Empty;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = bgText;
                    ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;

                    currentColumn += 3;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.WinLost);
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Position);
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Commission);
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;
                    if (detail.VirtualComm != _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.VirtualComm);
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = detail.VirtualComm;
                        ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    }

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.LineTotal);
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;
                    if (detail.VirtualLineTotal != _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.VirtualLineTotal);
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = detail.VirtualLineTotal;
                        ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    }

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;
                    if (detail.Variance != _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Variance);
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = detail.Variance;
                        ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    }

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;
                    if (detail.Turnover != _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Turnover);
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = detail.Turnover;
                        ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    }

                    currentColumn++;
                    ws.Cells[currentRow, currentColumn].Style = bgNumber;
                    if (detail.Margin != _notAvailableString)
                    {
                        ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(detail.Margin);
                    }
                    else
                    {
                        ws.Cells[currentRow, currentColumn].Value = detail.Margin;
                        ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    }
                }

                #endregion ExportThirdParty
                #region GrandTotal

                currentRow++;
                currentColumn = startColumn;
                endColumn = currentColumn + 6;
                endRow = currentRow;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow, endColumn).Merged = true;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow, endColumn).Value = wlAllDetails.Total.Name;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, currentRow, endColumn).Style = grandTotalText;

                currentColumn += 7;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.Total.Commission);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.Total.VirtualComm);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.Total.LineTotal);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.Total.VirtualLineTotal);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.Total.Variance);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.Total.Turnover);
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;

                currentColumn++;
                ws.Cells[currentRow, currentColumn].Style = grandTotalNumber;
                if (wlAllDetails.Total.Margin != _notAvailableString)
                {
                    ws.Cells[currentRow, currentColumn].Value = CastAide.AsDecimal(wlAllDetails.Total.Margin);
                }
                else
                {
                    ws.Cells[currentRow, currentColumn].Value = wlAllDetails.Total.Margin;
                    ws.Cells[currentRow, currentColumn].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                }

                #endregion GrandTotal
            }
            else
            {
                #region EmptyReport

                currentRow++;
                currentColumn = startColumn;
                endColumn = currentColumn + 13;
                endRow = currentRow;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Merged = true;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Value = Fanex.Tarus.Reports.Resource.Language.EmptyReportMessage;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style = textStyle;
                ws.Cells.GetSubrangeAbsolute(currentRow, currentColumn, endRow, endColumn).Style.Font.Weight = ExcelFont.BoldWeight;

                #endregion EmptyReport
            }

            #endregion WLSummaryByBetTypeReportContent
        }

        private string GetMatchEvent(int sportType, int betType, string homeTeam, string awayTeam)
        {
            string result = string.Empty;
            result = homeTeam + Fanex.Tarus.Reports.Resource.MatchWinLossAllDetails.VersusString + awayTeam;
            return result;
        }
    }
}
