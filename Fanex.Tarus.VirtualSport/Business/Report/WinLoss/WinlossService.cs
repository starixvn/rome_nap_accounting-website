
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using DNAide.Core;
using DNAide.Utils;
using ExcelUtil.Spreadsheet;
using Fanex.Tarus.Business.Implement;
using Fanex.Tarus.Entities.Sport;
using Fanex.Tarus.Infrastructure.Common;
using Fanex.Tarus.Infrastructure.Extensions;
using Fanex.Tarus.Infrastructure.Helper;
using Fanex.Tarus.VirtualSport.DataAccess.Report.Winloss;
using Fanex.Tarus.VirtualSport.Entities.Report.Winloss;

namespace Fanex.Tarus.VirtualSport.Business.Report.WinLoss
{
    public class WinlossService : IWinlossService
    {
        public readonly IWinlossAccess _winlossAccess;
        private string _keyWinlossDaily = "winlossDaily";
        private string _keyWinlossDetail = "winlossDetailByCurr";

        public WinlossService()
            : this(null)
        {
        }

        public WinlossService(IWinlossAccess winlossAccess)
        {
            this._winlossAccess = winlossAccess ?? new WinlossAccess();
        }

        public Winloss SelDailyWinloss(DateTime fromDate, DateTime toDate, bool isExclude, int drawFilter, string sportType)
        {
            var vsWinloss = new Winloss();
            var winlossByDateList = new List<WinlossByDate>();
            var winlossTotal = new List<WinlossDetail>();
            try
            {
                DataSet ds = _winlossAccess.GetDailyWinloss(fromDate, toDate, isExclude, drawFilter, sportType);
                if (ds != null && ds.Tables.Count >= 2)
                {
                    DataTable datatable = ds.Tables[0];
                    var groupbyDate = from s in datatable.AsEnumerable()
                                      group s by s.Field<DateTime>("rptDate")
                                          into dtb
                                          select dtb;

                    foreach (var grp in groupbyDate)
                    {
                        var vsWinlossByDate = new WinlossByDate();

                        var query = from d in datatable.AsEnumerable()
                                    where (d.Field<DateTime>("rptDate")) == CastAide.AsDateTime(grp.Key)
                                    select d;
                        vsWinlossByDate.Date = String.Format("{0:MM/dd/yyyy}", grp.Key);
                        var winlossDetailList = new List<WinlossDetail>();
                        foreach (DataRow row in query)
                        {
                            var currency = CacheUtils.GetExchangeName(CastAide.AsInt32(row["currency"]));
                            var betcount = NumberFormat.DecFormat(row["betcount"], 0);
                            var turnover = NumberFormat.DecFormat(row["turnover"], 2);
                            var position = NumberFormat.DecFormat(row["positionwl"], 2);
                            var wlMember = NumberFormat.DecFormat(row["memberwl"], 2);
                            var wlCompany = NumberFormat.DecFormat(row["Companywl"], 2);
                            var amountDueto = NumberFormat.DecFormat(row["amtDuePartner"], 2);
                            winlossDetailList.Add(new WinlossDetail()
                            {
                                Name = currency,
                                Betcount = betcount,
                                Turnover = turnover,
                                WLMember = wlMember,
                                Position = position,
                                WLCompany = wlCompany,
                                AmountDueto = amountDueto,
                            });
                            var curr = winlossTotal.Where(s => s.Name == currency).ToList();
                            if (curr.Count > 0)
                            {
                                var betcountCur = CastAide.AsDouble(curr[0].Betcount);
                                var turnoverCur = CastAide.AsDouble(curr[0].Turnover);
                                var positionCur = CastAide.AsDouble(curr[0].Turnover);
                                var wlMemberCur = CastAide.AsDouble(curr[0].WLMember);
                                var wlCompanyCur = CastAide.AsDouble(curr[0].WLCompany);
                                var amtDuePartnerCur = CastAide.AsDouble(curr[0].AmountDueto);
                                curr[0].Betcount = NumberFormat.DecFormat(CastAide.AsDouble(betcount) + betcountCur, 0);
                                curr[0].Turnover = NumberFormat.DecFormat(CastAide.AsDouble(turnover) + turnoverCur, 2);
                                curr[0].Position = NumberFormat.DecFormat(CastAide.AsDouble(position) + positionCur, 2);
                                curr[0].WLMember = NumberFormat.DecFormat(CastAide.AsDouble(wlMember) + wlMemberCur, 2);
                                curr[0].WLCompany = NumberFormat.DecFormat(CastAide.AsDouble(wlCompany) + wlCompanyCur, 2);
                                curr[0].AmountDueto = NumberFormat.DecFormat(CastAide.AsDouble(amountDueto) + amtDuePartnerCur, 2);
                            }
                            else
                            {
                                var wsTotalByCur = new WinlossDetail()
                                {
                                    Name = currency,
                                    Betcount = NumberFormat.DecFormat(betcount, 0),
                                    Turnover = NumberFormat.DecFormat(turnover, 2),
                                    Position = NumberFormat.DecFormat(position, 2),
                                    WLMember = NumberFormat.DecFormat(wlMember, 2),
                                    WLCompany = NumberFormat.DecFormat(wlCompany, 2),
                                    AmountDueto = NumberFormat.DecFormat(amountDueto, 2)
                                };
                                winlossTotal.Add(wsTotalByCur);
                            }
                        }
                        vsWinlossByDate.WLDetailList = winlossDetailList;
                        winlossByDateList.Add(vsWinlossByDate);

                    }
                    #region Total by Currency

                    vsWinloss.Total = winlossTotal;

                    #endregion

                    #region GrandTotal BaseCurr

                    DataTable dtb2 = ds.Tables[1];
                    DataRow row2 = dtb2.Rows[0];
                    var winlossGrandTotal = new List<WinlossDetail>();
                    winlossGrandTotal.Add(new WinlossDetail()
                    {
                        Name = "Grand Total in RM",
                        Betcount = NumberFormat.DecFormat(row2["Betcount"].ToString(), 0),
                        Turnover = NumberFormat.DecFormat(row2["turnoverrm"], 2),
                        WLMember = NumberFormat.DecFormat(row2["memberwlrm"], 2),
                        Position = NumberFormat.DecFormat(row2["positionwlrm"], 2),
                        WLCompany = NumberFormat.DecFormat(row2["companywlrm"], 2),
                        AmountDueto = NumberFormat.DecFormat(row2["amtduepartnerrm"], 2)
                    });
                    winlossGrandTotal.Add(new WinlossDetail()
                    {
                        Name = "Grand Total in EUR",
                        Betcount = NumberFormat.DecFormat(row2["Betcount"].ToString(), 0),
                        Turnover = NumberFormat.DecFormat(row2["turnovereur"], 2),
                        WLMember = NumberFormat.DecFormat(row2["memberwleur"], 2),
                        Position = NumberFormat.DecFormat(row2["positionwleur"], 2),
                        WLCompany = NumberFormat.DecFormat(row2["companywleur"], 2),
                        AmountDueto = NumberFormat.DecFormat(row2["amtduepartnereur"], 2)
                    });
                    vsWinloss.GrandTotal = winlossGrandTotal;
                    vsWinloss.WLByDateList = winlossByDateList;
                    AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyWinlossDaily, vsWinloss);

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return vsWinloss;
        }


        public List<BaseModel> SelSportType()
        {
            var sportTypeList = new List<BaseModel>();
            DataTable dtb = _winlossAccess.GetSportType();
            foreach (DataRow row in dtb.Rows)
            {
                sportTypeList.Add(new BaseModel()
                {
                    Id = CastAide.AsInt32(row["sporttype"]),
                    Name = row["sportname"].ToString()
                });
            }
            return sportTypeList;
        }


        public WinlossByCurrency SelDailyWinlossByCurrency(DateTime date, bool isExclude, int drawFilter, string sportType, string currency)
        {
            var winlossCurr = new WinlossByCurrency();
            double TotalBetCount = 0;
            double turnoverTotal = 0, positionTotal = 0, wlmemberTotal = 0, wlcompanyTotal = 0, amtDuePartnerTotal = 0;
            try
            {
                DataTable dtb = _winlossAccess.GetDailyWinlossByCurrency(date, sportType, isExclude, drawFilter, currency);
                var winlossDetailList = new List<WinlossDetail>();
                foreach (DataRow row in dtb.Rows)
                {
                    var username = CastAide.AsString(row["username"]);
                    var betcount = NumberFormat.DecFormat(row["betcount"], 0);
                    var turnover = NumberFormat.DecFormat(row["turnover"], 2);
                    var wlmember = NumberFormat.DecFormat(row["memberwl"], 2);
                    var wlcompany = NumberFormat.DecFormat(row["company"], 2);
                    var amtDuePartner = NumberFormat.DecFormat(row["amtDuePartner"], 2);
                    var position = NumberFormat.DecFormat(row["positionwl"], 2);
                    winlossDetailList.Add(new WinlossDetail()
                    {
                        Name = username,
                        Betcount = betcount,
                        Turnover = turnover,
                        WLMember = wlmember,
                        WLCompany = wlcompany,
                        Position = position,
                        AmountDueto = amtDuePartner

                    });
                    TotalBetCount += CastAide.AsDouble(betcount);
                    turnoverTotal += CastAide.AsDouble(turnover);
                    positionTotal += CastAide.AsDouble(position);
                    wlmemberTotal += CastAide.AsDouble(wlmember);
                    wlcompanyTotal += CastAide.AsDouble(wlcompany);
                    amtDuePartnerTotal += CastAide.AsDouble(amtDuePartner);
                }
                winlossCurr.WinlossDetailList = winlossDetailList;
                var winlossTotal = new WinlossDetail()
                {
                    Name = "Total",
                    Betcount = NumberFormat.DecFormat(TotalBetCount, 0),
                    Turnover = NumberFormat.DecFormat(turnoverTotal, 2),
                    Position = NumberFormat.DecFormat(positionTotal, 2),
                    WLMember = NumberFormat.DecFormat(wlmemberTotal, 2),
                    WLCompany = NumberFormat.DecFormat(wlcompanyTotal, 2),
                    AmountDueto = NumberFormat.DecFormat(amtDuePartnerTotal, 2)
                };
                winlossCurr.Total = winlossTotal;
                AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyWinlossDetail, winlossCurr);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return winlossCurr;
        }

        public ExcelFile ExportExcelWinlossDaily(DateTime fromDate, DateTime toDate, bool isExclude, int drawFilter, string sportType)
        {
            var excelFile = new ExcelFile();
            var winlossDaily = new Winloss();
            try
            {
                var objWinloss = AppSession.GetSession(HttpContext.Current.Session, _keyWinlossDaily);

                if (objWinloss != null)
                {
                    winlossDaily = (Winloss)objWinloss;
                }
                else
                {
                    winlossDaily = SelDailyWinloss(fromDate, toDate, isExclude, drawFilter, sportType);
                }
                #region Export
                var reportName = string.Format("VirtualSportWinlossFrom{0}To{1}", fromDate.ToShortDateString().Replace("/", "_"), toDate.ToShortDateString().Replace("/", "_"));
                int startRowIndex = 0;
                int startColIndex = 0;
                var wsWinlossDaily = excelFile.Worksheets.Add(reportName);
                #region Header
                var gembox = new ExcelGemboxUtils();
                var bgFormat = gembox.GetStyleText(159, 183, 213, HorizontalAlignmentStyle.Center, true);
                var titleCell = wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex,
                                                     startColIndex + 7);
                titleCell.Merged = true;
                titleCell.Style = bgFormat;
                titleCell.Value = string.Format("Virtual Sports Win Loss - From {0} to {1}", fromDate.ToShortDateString(), toDate.ToShortDateString());
                startRowIndex += 1;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Date";
                startColIndex++;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Currency";
                startColIndex++;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Bet count";
                startColIndex++;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Turnover";
                startColIndex++;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Member win loss";
                startColIndex++;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 1).Merged = true;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 1).Style = bgFormat;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 1).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 1).Value = "Winloss";

                wsWinlossDaily.Cells[startRowIndex + 1, startColIndex].Value = "Position";
                wsWinlossDaily.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossDaily.Cells[startRowIndex + 1, startColIndex].Value = "Company";
                wsWinlossDaily.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Amount due to Inspired";

                #endregion
                #region Body
                var numberFormat = gembox.GetStyleIntNumber(".00", Color.FromArgb(255, 255, 255), false, false, false);
                numberFormat.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                var intNumberFormat = gembox.GetStyleIntNumber("", Color.FromArgb(255, 255, 255), false, false, false);
                intNumberFormat.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsWinlossDaily.Columns[0].Width = 255 * 15;
                wsWinlossDaily.Columns[1].Width = 255 * 10;
                wsWinlossDaily.Columns[2].Width = 255 * 15;
                wsWinlossDaily.Columns[3].Width = 255 * 15;
                wsWinlossDaily.Columns[4].Width = 255 * 15;
                wsWinlossDaily.Columns[5].Width = 255 * 15;
                wsWinlossDaily.Columns[6].Width = 255 * 15;
                wsWinlossDaily.Columns[7].Width = 255 * 20;
                startRowIndex = 3;
                if (winlossDaily.WLByDateList.Count > 0)
                {
                    foreach (WinlossByDate wlbyDate in winlossDaily.WLByDateList)
                    {
                        startColIndex = 0;
                        int count = wlbyDate.WLDetailList.Count;
                        wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + count - 1, startColIndex).Merged = true;
                        wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + count - 1, startColIndex).Style = numberFormat;
                        wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + count - 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + count - 1, startColIndex).Value = wlbyDate.Date;
                        for (int i = 0; i < count; i++)
                        {
                            startColIndex++;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = numberFormat;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = wlbyDate.WLDetailList[i].Name;
                            startColIndex++;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = intNumberFormat;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.WLDetailList[i].Betcount);
                            startColIndex++;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = numberFormat;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.WLDetailList[i].Turnover);
                            startColIndex++;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = numberFormat;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.WLDetailList[i].WLMember);
                            startColIndex++;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = numberFormat;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.WLDetailList[i].Position);
                            startColIndex++;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = numberFormat;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.WLDetailList[i].WLCompany);
                            startColIndex++;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = numberFormat;
                            wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.WLDetailList[i].AmountDueto);
                            startRowIndex++;
                            startColIndex = 0;
                        }

                    }
                    #region Export Total
                    var bgTotal = gembox.GetStyleIntNumber(".00", Color.FromArgb(221, 217, 196), false, false, false);
                    var bgIntNumberTotal = gembox.GetStyleIntNumber("", Color.FromArgb(221, 217, 196), false, false, false);
                    int countTotal = winlossDaily.Total.Count;
                    wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + countTotal - 1, startColIndex).Merged = true;
                    wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + countTotal - 1, startColIndex).Style = bgTotal;
                    wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + countTotal - 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + countTotal - 1, startColIndex).Value = "Total";
                    foreach (WinlossDetail total in winlossDaily.Total)
                    {
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgTotal;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = total.Name;
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgIntNumberTotal;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.Betcount);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgTotal;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.Turnover);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgTotal;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.WLMember);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgTotal;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.Position);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgTotal;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.WLCompany);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgTotal;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.AmountDueto);
                        startRowIndex++;
                        startColIndex = 0;
                    }
                    #endregion

                    #region GrandTotal
                    var bgGrandTotal = gembox.GetStyleIntNumber(".00", Color.FromArgb(196, 215, 155), true, false, false);
                    var bgIntNumberGrandTotal = gembox.GetStyleIntNumber("", Color.FromArgb(196, 215, 155), true, false, false);
                    foreach (WinlossDetail grandtotal in winlossDaily.GrandTotal)
                    {
                        wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 1).Merged = true;
                        wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 1).Style = bgGrandTotal;
                        wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 1).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 1).Value = grandtotal.Name;
                        startColIndex = 2;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgIntNumberGrandTotal;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(grandtotal.Betcount);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgGrandTotal;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(grandtotal.Turnover);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgGrandTotal;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(grandtotal.WLMember);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgGrandTotal;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(grandtotal.Position);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgGrandTotal;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(grandtotal.WLCompany);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgGrandTotal;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(grandtotal.AmountDueto);
                        startRowIndex++;
                        startColIndex = 0;
                    }
                    #endregion
                }
                else
                {
                    startRowIndex = 3;
                    startColIndex = 0;
                    var groupCell = wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex,
                                                    startColIndex + 7);
                    bgFormat = gembox.GetStyleText(255, 255, 255, HorizontalAlignmentStyle.Center, true);
                    groupCell.Style = bgFormat;
                    groupCell.Merged = true;
                    groupCell.Value = "There is no data.";
                }


                #endregion
                #region response
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return excelFile;
        }

        public ExcelFile ExportExcelWinlossDetailByCurrency(DateTime reportDate, bool isExclude, int drawFilter, string sportType, string currency)
        {

            var excelFile = new ExcelFile();
            var winlossDetail = new WinlossByCurrency();
            try
            {
                var objWinloss = AppSession.GetSession(HttpContext.Current.Session, _keyWinlossDetail);

                if (objWinloss != null)
                {
                    winlossDetail = (WinlossByCurrency)objWinloss;
                }
                else
                {
                    winlossDetail = SelDailyWinlossByCurrency(reportDate, isExclude, drawFilter, sportType, currency);
                }
                #region Export
                var reportName = string.Format("VirtualSportWinlossDetails_{0}_{1}", reportDate.ToShortDateString().Replace("/", "_"), currency);
                int startRowIndex = 0;
                int startColIndex = 0;
                var wsWinlossDaily = excelFile.Worksheets.Add(reportName);
                #region Header
                var gembox = new ExcelGemboxUtils();
                var bgFormat = gembox.GetStyleText(159, 183, 213, HorizontalAlignmentStyle.Center, true);
                var titleCell = wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex,
                                                     startColIndex + 6);
                titleCell.Merged = true;
                titleCell.Style = bgFormat;
                titleCell.Value = string.Format("Virtual Sports Win Loss Details - {0} - {1}", reportDate.ToShortDateString(), currency);
                startRowIndex += 1;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Username";

                startColIndex++;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Bet count";
                startColIndex++;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Turnover";
                startColIndex++;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Member win loss";
                startColIndex++;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 1).Merged = true;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 1).Style = bgFormat;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 1).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 1).Value = "Winloss";

                wsWinlossDaily.Cells[startRowIndex + 1, startColIndex].Value = "Position";
                wsWinlossDaily.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossDaily.Cells[startRowIndex + 1, startColIndex].Value = "Company";
                wsWinlossDaily.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Amount due to Inspired";

                #endregion
                #region Body

                var numberFormat = gembox.GetStyleIntNumber(".00", Color.FromArgb(255, 255, 255), false, false, false);
                var intNumberFormat = gembox.GetStyleIntNumber("", Color.FromArgb(255, 255, 255), false, false, false);
                wsWinlossDaily.Columns[0].Width = 255 * 20;
                wsWinlossDaily.Columns[1].Width = 255 * 20;
                wsWinlossDaily.Columns[2].Width = 255 * 20;
                wsWinlossDaily.Columns[3].Width = 255 * 20;
                wsWinlossDaily.Columns[4].Width = 255 * 20;
                wsWinlossDaily.Columns[5].Width = 255 * 20;
                wsWinlossDaily.Columns[6].Width = 255 * 25;
                startRowIndex = 3;
                startColIndex = 0;
                if (winlossDetail.WinlossDetailList.Count > 0)
                {
                    foreach (WinlossDetail wldetail in winlossDetail.WinlossDetailList)
                    {
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = numberFormat;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = wldetail.Name;
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = intNumberFormat;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.Betcount);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = numberFormat;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.Turnover);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = numberFormat;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.WLMember);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = numberFormat;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.Position);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = numberFormat;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.WLCompany);
                        startColIndex++;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = numberFormat;
                        wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.AmountDueto);
                        startRowIndex++;
                        startColIndex = 0;

                    }
                    var bgTotal = gembox.GetStyleIntNumber(".00", Color.FromArgb(221, 217, 196), true, false, false);
                    var bgIntNumberTotal = gembox.GetStyleIntNumber("", Color.FromArgb(221, 217, 196), true, false, false);
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgTotal;
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = winlossDetail.Total.Name;
                    startColIndex++;
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgIntNumberTotal;
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(winlossDetail.Total.Betcount);
                    startColIndex++;
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgTotal;
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(winlossDetail.Total.Turnover);
                    startColIndex++;
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgTotal;
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(winlossDetail.Total.WLMember);
                    startColIndex++;
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgTotal;
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(winlossDetail.Total.Position);
                    startColIndex++;
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgTotal;
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(winlossDetail.Total.WLCompany);
                    startColIndex++;
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Style = bgTotal;
                    wsWinlossDaily.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(winlossDetail.Total.AmountDueto);

                }
                else
                {
                    startRowIndex = 3;
                    startColIndex = 0;
                    var groupCell = wsWinlossDaily.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex,
                                                    startColIndex + 6);
                    bgFormat = gembox.GetStyleText(255, 255, 255, HorizontalAlignmentStyle.Center, true);
                    groupCell.Style = bgFormat;
                    groupCell.Merged = true;
                    groupCell.Value = "There is no data.";
                }


                #endregion
                #region response
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return excelFile;
        }
    }

}
