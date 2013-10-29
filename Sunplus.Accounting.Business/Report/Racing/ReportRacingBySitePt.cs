using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DNAide.Core;
using DNAide.Utils;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities.Report.Racebook;

namespace Sunplus.Accounting.Business.Report.Racing
{
    public partial class ReportRacing
    {
        #region Licensee having Pt

        public RacingDailyBySiteBO RacingWinLossByLicenseeHavingPt(string winlossdate, string site)
        {
            var wl = new RacingDailyBySiteBO { Items = new List<RacingDailyBySiteCurrency>(), Total = new RacingDailyBySiteRow() };

            var sites = WinlostRacingLicensee_SiteSel();
            int count = sites.SiteItems.Where(s => s.Name == site).Count();

            if (count == 0)
            {
                return wl;
            }

            try
            {
                DataTable dataTable = _reportRacingDao.WinlostRacingLicensee_Details(winlossdate, site);
                if (dataTable == null)
                    return wl;

                List<int> currencies = dataTable.AsEnumerable().Select(s => CastAide.AsInt32(s["currency"])).Distinct().ToList();

                List<RacingDailyBySiteRow> allRowsByBaseCurr = new List<RacingDailyBySiteRow>();

                foreach (int c in currencies)
                {
                    RacingDailyBySiteCurrency Currency = new RacingDailyBySiteCurrency();
                    Currency.Items = new List<RacingDailyBySiteMeeting>();
                    Currency.Totals = new List<RacingDailyBySiteRow>();

                    List<int> MeetingIds =
                        dataTable.AsEnumerable().Where(s => CastAide.AsInt32(s["currency"]) == c).Select(
                            s => CastAide.AsInt32(s["leagueid"])).Distinct().ToList();

                    foreach (int MeetingId in MeetingIds)
                    {
                        RacingDailyBySiteMeeting meeting = new RacingDailyBySiteMeeting();
                        meeting.Items = new List<RacingDailyBySiteRow>();
                        meeting.Total = new RacingDailyBySiteRow();

                        DataRow[] rows = dataTable.Select("currency = " + c + " and leagueid = " + MeetingId);

                        foreach (DataRow row in rows)
                        {
                            #region AssignRow

                            meeting.Items.Add(new RacingDailyBySiteRow()
                            {
                                CurrencyName = CastAide.AsString(row["currencyName"]),
                                CurrencyId = CastAide.AsInt32(row["currency"]),

                                Name = CacheRef.GetLeagueName(row["leagueid"]),
                                Id = CastAide.AsInt32(row["leagueid"]),

                                Race = CastAide.AsString(row["groupmatch"]),

                                WinTO = NumberFormat.DecFormat(row["winTOMem_Origin"], 2),
                                PlaceTO = NumberFormat.DecFormat(row["placeTOMem_Origin"], 2),
                                WinPlaceTO = NumberFormat.DecFormat(row["winplaceTOMem_Origin"], 2),
                                TotalTO = NumberFormat.DecFormat(row["totalTOMem_Origin"], 2),

                                DividendWon = NumberFormat.DecFormat(row["DividendWonMem_Origin"], 2),
                                AgentWinTO = NumberFormat.DecFormat(row["winTOLicensee_Origin"], 2),
                                AgentPlaceTO = NumberFormat.DecFormat(row["placeTOLicensee_Origin"], 2),
                                AgentWinPlaceTO = NumberFormat.DecFormat(row["winplaceTOLicensee_Origin"], 2),
                                AgentTotalTO = NumberFormat.DecFormat(row["totalTOLicensee_Origin"], 2),
                                AgentPtWinLoss = NumberFormat.DecFormat(row["PTWLLicensee_Origin"], 2),
                                CommIncome = NumberFormat.DecFormat(row["commIncomeLicensee_Origin"], 2),
                                Amountdueto = NumberFormat.DecFormat(row["AmountDue2IBC_Origin"], 2),

                                IBCPTWL = NumberFormat.DecFormat(row["PTWLIBC_Origin"], 2),
                                IBCCommIncomm = NumberFormat.DecFormat(row["CommIncomeIBC_Origin"], 2),
                            });

                            #endregion AssignRow

                            #region Get All Rows by Base Currency

                            allRowsByBaseCurr.Add(new RacingDailyBySiteRow()
                            {
                                CurrencyName = CastAide.AsString(row["currencyName"]),
                                CurrencyId = CastAide.AsInt32(row["currency"]),

                                Name = CacheRef.GetLeagueName(row["leagueid"]),
                                Id = CastAide.AsInt32(row["leagueid"]),

                                WinTO = NumberFormat.DecFormat(row["winTOMem"], 2),
                                PlaceTO = NumberFormat.DecFormat(row["placeTOMem"], 2),
                                WinPlaceTO = NumberFormat.DecFormat(row["winplaceTOMem"], 2),
                                TotalTO = NumberFormat.DecFormat(row["totalTOMem"], 2),

                                DividendWon = NumberFormat.DecFormat(row["DividendWonMem"], 2),
                                AgentWinTO = NumberFormat.DecFormat(row["winTOLicensee"], 2),
                                AgentPlaceTO = NumberFormat.DecFormat(row["placeTOLicensee"], 2),
                                AgentTotalTO = NumberFormat.DecFormat(row["totalTOLicensee"], 2),
                                AgentPtWinLoss = NumberFormat.DecFormat(row["PTWLLicensee"], 2),
                                AgentWinPlaceTO = NumberFormat.DecFormat(row["winplaceTOLicensee"], 2),
                                CommIncome = NumberFormat.DecFormat(row["commIncomeLicensee"], 2),
                                Amountdueto = NumberFormat.DecFormat(row["AmountDue2IBC"], 2),

                                IBCPTWL = NumberFormat.DecFormat(row["PTWLIBC"], 2),
                                IBCCommIncomm = NumberFormat.DecFormat(row["CommIncomeIBC"], 2)
                            });

                            #endregion Get All Rows by Base Currency
                        }

                        #region Total Meeting

                        meeting.Total = new RacingDailyBySiteRow()
                        {
                            Name = CacheRef.GetLeagueName(MeetingId) + " - " + CacheUtils.GetExchangeName(c),

                            WinTO = NumberFormat.DecFormat(meeting.Items.Sum(s => CastAide.AsDecimal(s.WinTO)), 2),
                            PlaceTO = NumberFormat.DecFormat(meeting.Items.Sum(s => CastAide.AsDecimal(s.PlaceTO)), 2),
                            WinPlaceTO = NumberFormat.DecFormat(meeting.Items.Sum(s => CastAide.AsDecimal(s.WinPlaceTO)), 2),
                            TotalTO = NumberFormat.DecFormat(meeting.Items.Sum(s => CastAide.AsDecimal(s.TotalTO)), 2),

                            DividendWon = NumberFormat.DecFormat(meeting.Items.Sum(s => CastAide.AsDecimal(s.DividendWon)), 2),
                            AgentWinTO = NumberFormat.DecFormat(meeting.Items.Sum(s => CastAide.AsDecimal(s.AgentWinTO)), 2),
                            AgentPlaceTO = NumberFormat.DecFormat(meeting.Items.Sum(s => CastAide.AsDecimal(s.AgentPlaceTO)), 2),
                            AgentWinPlaceTO = NumberFormat.DecFormat(meeting.Items.Sum(s => CastAide.AsDecimal(s.AgentWinPlaceTO)), 2),
                            AgentPtWinLoss = NumberFormat.DecFormat(meeting.Items.Sum(s => CastAide.AsDecimal(s.AgentPtWinLoss)), 2),
                            AgentTotalTO = NumberFormat.DecFormat(meeting.Items.Sum(s => CastAide.AsDecimal(s.AgentTotalTO)), 2),
                            CommIncome = NumberFormat.DecFormat(meeting.Items.Sum(s => CastAide.AsDecimal(s.CommIncome)), 2),
                            Amountdueto = NumberFormat.DecFormat(meeting.Items.Sum(s => CastAide.AsDecimal(s.Amountdueto)), 2),

                            IBCPTWL = NumberFormat.DecFormat(meeting.Items.Sum(s => CastAide.AsDecimal(s.IBCPTWL)), 2),
                            IBCCommIncomm = NumberFormat.DecFormat(meeting.Items.Sum(s => CastAide.AsDecimal(s.IBCCommIncomm)), 2),
                        };

                        #endregion Total Meeting

                        Currency.Items.Add(meeting);
                    }

                    #region Total Currency

                    #region//Origin
                    Currency.Totals.Add(new RacingDailyBySiteRow()
                    {
                        Name = CacheUtils.GetExchangeName(c) + " Total",
                        CurrencyId = c,
                        CurrencyName = CacheUtils.GetExchangeName(c),

                        WinTO = NumberFormat.DecFormat(Currency.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(t => CastAide.AsDecimal(t.WinTO)))), 2),
                        PlaceTO = NumberFormat.DecFormat(Currency.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(t => CastAide.AsDecimal(t.PlaceTO)))), 2),
                        WinPlaceTO = NumberFormat.DecFormat(Currency.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(t => CastAide.AsDecimal(t.WinPlaceTO)))), 2),
                        TotalTO = NumberFormat.DecFormat(Currency.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(t => CastAide.AsDecimal(t.TotalTO)))), 2),

                        DividendWon = NumberFormat.DecFormat(Currency.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(t => CastAide.AsDecimal(t.DividendWon)))), 2),
                        AgentWinTO = NumberFormat.DecFormat(Currency.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(t => CastAide.AsDecimal(t.AgentWinTO)))), 2),
                        AgentPlaceTO = NumberFormat.DecFormat(Currency.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(t => CastAide.AsDecimal(t.AgentPlaceTO)))), 2),
                        AgentWinPlaceTO = NumberFormat.DecFormat(Currency.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(t => CastAide.AsDecimal(t.AgentWinPlaceTO)))), 2),
                        AgentPtWinLoss = NumberFormat.DecFormat(Currency.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(t => CastAide.AsDecimal(t.AgentPtWinLoss)))), 2),
                        AgentTotalTO = NumberFormat.DecFormat(Currency.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(t => CastAide.AsDecimal(t.AgentTotalTO)))), 2),
                        CommIncome = NumberFormat.DecFormat(Currency.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(t => CastAide.AsDecimal(t.CommIncome)))), 2),
                        Amountdueto = NumberFormat.DecFormat(Currency.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(t => CastAide.AsDecimal(t.Amountdueto)))), 2),

                        IBCPTWL = NumberFormat.DecFormat(Currency.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(t => CastAide.AsDecimal(t.IBCPTWL)))), 2),
                        IBCCommIncomm = NumberFormat.DecFormat(Currency.Items.Sum(s => CastAide.AsDecimal(s.Items.Sum(t => CastAide.AsDecimal(t.IBCCommIncomm)))), 2),
                    });

                    #endregion Total Currency

                    #region// Rows by Base Currency
                    if (c != 2)
                    {
                        Currency.Totals.Add(new RacingDailyBySiteRow()
                        {
                            Name = CacheUtils.GetExchangeName(c) + " Total",
                            CurrencyId = c,
                            CurrencyName = "Base (RM)",

                            WinTO = NumberFormat.DecFormat(allRowsByBaseCurr.Where(s => s.CurrencyId == c).Sum(s => CastAide.AsDecimal(s.WinTO)), 2),
                            PlaceTO = NumberFormat.DecFormat(allRowsByBaseCurr.Where(s => s.CurrencyId == c).Sum(s => CastAide.AsDecimal(s.PlaceTO)), 2),
                            WinPlaceTO = NumberFormat.DecFormat(allRowsByBaseCurr.Where(s => s.CurrencyId == c).Sum(s => CastAide.AsDecimal(s.WinPlaceTO)), 2),
                            TotalTO = NumberFormat.DecFormat(allRowsByBaseCurr.Where(s => s.CurrencyId == c).Sum(s => CastAide.AsDecimal(s.TotalTO)), 2),

                            DividendWon = NumberFormat.DecFormat(allRowsByBaseCurr.Where(s => s.CurrencyId == c).Sum(s => CastAide.AsDecimal(s.DividendWon)), 2),
                            AgentWinTO = NumberFormat.DecFormat(allRowsByBaseCurr.Where(s => s.CurrencyId == c).Sum(s => CastAide.AsDecimal(s.AgentWinTO)), 2),
                            AgentPlaceTO = NumberFormat.DecFormat(allRowsByBaseCurr.Where(s => s.CurrencyId == c).Sum(s => CastAide.AsDecimal(s.AgentPlaceTO)), 2),
                            AgentWinPlaceTO = NumberFormat.DecFormat(allRowsByBaseCurr.Where(s => s.CurrencyId == c).Sum(s => CastAide.AsDecimal(s.AgentWinPlaceTO)), 2),
                            AgentPtWinLoss = NumberFormat.DecFormat(allRowsByBaseCurr.Where(s => s.CurrencyId == c).Sum(s => CastAide.AsDecimal(s.AgentPtWinLoss)), 2),
                            AgentTotalTO = NumberFormat.DecFormat(allRowsByBaseCurr.Where(s => s.CurrencyId == c).Sum(s => CastAide.AsDecimal(s.AgentTotalTO)), 2),
                            CommIncome = NumberFormat.DecFormat(allRowsByBaseCurr.Where(s => s.CurrencyId == c).Sum(s => CastAide.AsDecimal(s.CommIncome)), 2),
                            Amountdueto = NumberFormat.DecFormat(allRowsByBaseCurr.Where(s => s.CurrencyId == c).Sum(s => CastAide.AsDecimal(s.Amountdueto)), 2),

                            IBCPTWL = NumberFormat.DecFormat(allRowsByBaseCurr.Where(s => s.CurrencyId == c).Sum(s => CastAide.AsDecimal(s.IBCPTWL)), 2),
                            IBCCommIncomm = NumberFormat.DecFormat(allRowsByBaseCurr.Where(s => s.CurrencyId == c).Sum(s => CastAide.AsDecimal(s.IBCCommIncomm)), 2),
                        });
                    }

                    #endregion Licensee having Pt

                    #endregion

                    wl.Items.Add(Currency);
                }

                #region Grand Total by Base currency
                wl.Total = new RacingDailyBySiteRow()
                {
                    Name = "Grand Total",
                    CurrencyName = "Base (RM)",

                    WinTO = NumberFormat.DecFormat(allRowsByBaseCurr.Sum(s => CastAide.AsDecimal(s.WinTO)), 2),
                    PlaceTO = NumberFormat.DecFormat(allRowsByBaseCurr.Sum(s => CastAide.AsDecimal(s.PlaceTO)), 2),
                    WinPlaceTO = NumberFormat.DecFormat(allRowsByBaseCurr.Sum(s => CastAide.AsDecimal(s.WinPlaceTO)), 2),
                    TotalTO = NumberFormat.DecFormat(allRowsByBaseCurr.Sum(s => CastAide.AsDecimal(s.TotalTO)), 2),

                    DividendWon = NumberFormat.DecFormat(allRowsByBaseCurr.Sum(s => CastAide.AsDecimal(s.DividendWon)), 2),
                    AgentWinTO = NumberFormat.DecFormat(allRowsByBaseCurr.Sum(s => CastAide.AsDecimal(s.AgentWinTO)), 2),
                    AgentPlaceTO = NumberFormat.DecFormat(allRowsByBaseCurr.Sum(s => CastAide.AsDecimal(s.AgentPlaceTO)), 2),
                    AgentWinPlaceTO = NumberFormat.DecFormat(allRowsByBaseCurr.Sum(s => CastAide.AsDecimal(s.AgentWinPlaceTO)), 2),
                    AgentPtWinLoss = NumberFormat.DecFormat(allRowsByBaseCurr.Sum(s => CastAide.AsDecimal(s.AgentPtWinLoss)), 2),
                    AgentTotalTO = NumberFormat.DecFormat(allRowsByBaseCurr.Sum(s => CastAide.AsDecimal(s.AgentTotalTO)), 2),
                    CommIncome = NumberFormat.DecFormat(allRowsByBaseCurr.Sum(s => CastAide.AsDecimal(s.CommIncome)), 2),
                    Amountdueto = NumberFormat.DecFormat(allRowsByBaseCurr.Sum(s => CastAide.AsDecimal(s.Amountdueto)), 2),

                    IBCPTWL = NumberFormat.DecFormat(allRowsByBaseCurr.Sum(s => CastAide.AsDecimal(s.IBCPTWL)), 2),
                    IBCCommIncomm = NumberFormat.DecFormat(allRowsByBaseCurr.Sum(s => CastAide.AsDecimal(s.IBCCommIncomm)), 2),
                };
                #endregion

                SessionExtension.AddSession(HttpContext.Current.Session,
                                            string.Format(_winLossRacingReportLicensseSessionKey, site), wl);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return wl;
        }

        #endregion
    }
}