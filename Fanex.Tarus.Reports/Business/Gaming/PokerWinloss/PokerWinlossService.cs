// -----------------------------------------------------------------------
// <copyright file="PokerWinlossService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.Gaming.PokerWinloss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Reports.DataAccess.Gaming.PokerWinloss;
    using Fanex.Tarus.Reports.Entities.Gaming.PokerWinloss;
    using System.Data;
    using Fanex.Tarus.Infrastructure.Extensions;
    using DNAide.Core;
    using Fanex.Tarus.Business.Implement;
    using Fanex.Tarus.Infrastructure.Caching;
    using DNAide.Utils;
    using Fanex.Tarus.Infrastructure.Common;
    using System.Web;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Infrastructure.Helper;
    using System.Drawing;
    
    public class PokerWinlossService: IPokerWinlossService
    {
        public readonly IPokerWinlossAccess _pokerWlAccess;
        public const string _keyPokerWinlossSession = "PokerWinloss";
        public const string _keyPokerWinlossDetailSession = "PokerWinlossDetail";
        public PokerWinlossService()
            : this(null)
        { }

        public PokerWinlossService(IPokerWinlossAccess pokerWlAccess)
        {
            this._pokerWlAccess = pokerWlAccess ?? new PokerWinlossAccess();
        }

        public Entities.Gaming.PokerWinloss.PokerWinloss GetPokerWinloss(DateTime fromDate, DateTime toDate, bool isViewWH)
        {
            var pokerWinloss = new PokerWinloss();
            var pokerWLTotal = new List<PokerWinlossDetail>();
            var pokerWLByDateList = new List<PokerWinlossByDate>();
            double grossCommBaseGrand = 0, companyCommBaseGrand = 0;
            DataTable datatable = null;
            try
            {
                if (!isViewWH)
                    datatable = _pokerWlAccess.GetPokerWinloss(fromDate, toDate);
                else
                    datatable = _pokerWlAccess.GetPokerWinloss_WH(fromDate, toDate);
                if (datatable != null && datatable.Rows.Count > 0)
                {
                    var groupbyDate = from s in datatable.AsEnumerable()
                                      group s by s.Field<DateTime>("Winlossdate")
                                          into dtb
                                          select dtb;
                    foreach (var grp in groupbyDate)
                    {
                        var pokerWinlossByDate = new PokerWinlossByDate();
                        var pokerWLDetailList = new List<PokerWinlossDetail>();
                        pokerWinlossByDate.Date = String.Format("{0:MM/dd/yyyy}", grp.Key);
                        var query = from d in datatable.AsEnumerable()
                                    where (d.Field<DateTime>("Winlossdate")) == CastAide.AsDateTime(grp.Key)
                                    select d;

                        foreach (DataRow row in query)
                        {
                            var pokerWinlossDetail = new PokerWinlossDetail();
                            var currencyId = row["currency"].ToString();
                            var currencyName = CacheUtils.GetExchangeName(CastAide.AsInt32(currencyId));
                            var winloss = NumberFormat.DecFormat(row["winloss"], 2);
                            var playerComm = NumberFormat.DecFormat(row["commplayer"],2);
                            var agentComm = NumberFormat.DecFormat(row["commagent"], 2);
                            var masterComm = NumberFormat.DecFormat(row["commmaster"], 2);
                            var superComm = NumberFormat.DecFormat(row["commsuper"], 2);
                            var companyComm = NumberFormat.DecFormat(row["commcompany"], 2);
                            var grossComm = NumberFormat.DecFormat(row["commgross"], 2);
                            var grossCommVSRM = NumberFormat.DecFormat(row["commgrossvsrm"], 2);
                            var companyCommVSRM = NumberFormat.DecFormat(row["commcompanyvsrm"], 2);
                            pokerWLDetailList.Add(new PokerWinlossDetail()
                            {
                                CurrencyId = currencyId,
                                Name = currencyName,
                                MemberWL = winloss,
                                MemberComm = playerComm,
                                TotalMember = NumberFormat.DecFormat(CastAide.AsDouble(winloss) + CastAide.AsDouble(playerComm), 2),
                                AgentComm = agentComm,
                                MasterComm = masterComm,
                                SuperComm = superComm,
                                CompanyComm = companyComm,
                                GrossComm = grossComm,
                                GrossCommBase = grossCommVSRM,
                                CompanyCommBase = companyCommVSRM
                            });

                            var curr = pokerWLTotal.Where(s => s.CurrencyId == currencyId).ToList();
                            if (curr.Count > 0)
                            {
                                var memberWLTotal = CastAide.AsDouble(curr[0].MemberWL);
                                var memberCommTotal = CastAide.AsDouble(curr[0].MemberComm);
                                var totalMemberTotal = CastAide.AsDouble(curr[0].TotalMember);
                                var agentCommTotal = CastAide.AsDouble(curr[0].AgentComm);
                                var masterCommTotal = CastAide.AsDouble(curr[0].MasterComm);
                                var superCommTotal = CastAide.AsDouble(curr[0].SuperComm);
                                var companyCommTotal = CastAide.AsDouble(curr[0].CompanyComm);
                                var grossCommTotal = CastAide.AsDouble(curr[0].GrossComm);
                                var grossCommBaseTotal = CastAide.AsDouble(curr[0].GrossCommBase);
                                var CompanyCommBaseTotal = CastAide.AsDouble(curr[0].CompanyCommBase);

                                curr[0].MemberWL = NumberFormat.DecFormat(CastAide.AsDouble(winloss) + memberWLTotal, 2);
                                curr[0].MemberComm = NumberFormat.DecFormat(CastAide.AsDouble(playerComm) + memberCommTotal, 2);
                                curr[0].TotalMember = NumberFormat.DecFormat(CastAide.AsDouble(winloss) + CastAide.AsDouble(playerComm) + totalMemberTotal, 2);
                                curr[0].AgentComm = NumberFormat.DecFormat(CastAide.AsDouble(agentComm) + agentCommTotal, 2);
                                curr[0].MasterComm = NumberFormat.DecFormat(CastAide.AsDouble(masterComm) + masterCommTotal, 2);
                                curr[0].SuperComm = NumberFormat.DecFormat(CastAide.AsDouble(superComm) + superCommTotal, 2);
                                curr[0].CompanyComm = NumberFormat.DecFormat(CastAide.AsDouble(companyComm) + companyCommTotal, 2);
                                curr[0].GrossComm = NumberFormat.DecFormat(CastAide.AsDouble(grossComm) + grossCommTotal, 2);
                                curr[0].GrossCommBase = NumberFormat.DecFormat(CastAide.AsDouble(grossCommVSRM) + grossCommBaseTotal, 2);
                                curr[0].CompanyCommBase = NumberFormat.DecFormat(CastAide.AsDouble(companyCommVSRM) + CompanyCommBaseTotal, 2);

                            }
                            else
                            {
                                var pokerTotalByCurrency = new PokerWinlossDetail() { 
                                    CurrencyId = currencyId,
                                    Name = currencyName,
                                    MemberWL = NumberFormat.DecFormat(winloss, 2),
                                    MemberComm = NumberFormat.DecFormat(playerComm, 2),
                                    TotalMember = NumberFormat.DecFormat(CastAide.AsDouble(winloss) + CastAide.AsDouble(playerComm), 2),
                                    AgentComm = NumberFormat.DecFormat(agentComm,2),
                                    MasterComm = NumberFormat.DecFormat(masterComm, 2),
                                    SuperComm = NumberFormat.DecFormat(superComm, 2),
                                    CompanyComm = NumberFormat.DecFormat(companyComm, 2),
                                    GrossComm = NumberFormat.DecFormat(grossComm, 2),
                                    GrossCommBase = NumberFormat.DecFormat(grossCommVSRM, 2),
                                    CompanyCommBase = NumberFormat.DecFormat(companyCommVSRM, 2)
                                };
                               
                                pokerWLTotal.Add(pokerTotalByCurrency);
                            }
                            grossCommBaseGrand += CastAide.AsDouble(grossCommVSRM);
                            companyCommBaseGrand += CastAide.AsDouble(companyCommVSRM);
                        }
                        pokerWinlossByDate.ListPokerWLDetail = pokerWLDetailList;
                        pokerWLByDateList.Add(pokerWinlossByDate);
                    }
                    pokerWinloss.Total = pokerWLTotal;
                    pokerWinloss.GrossCommBaseGrand = NumberFormat.DecFormat(grossCommBaseGrand,2);
                    pokerWinloss.CompanyCommBaseGrand = NumberFormat.DecFormat(companyCommBaseGrand, 2);
                    pokerWinloss.ListPokerWLDate = pokerWLByDateList;
                   
                }
                AppSession.AddSession(HttpContext.Current.Session, _keyPokerWinlossSession, pokerWinloss);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
           
            return pokerWinloss;
        }


        public Entities.Gaming.PokerWinloss.PokerWinlossByCurrency GetPokerWinlossDetail(DateTime wlDate, int currency, bool isViewWH)
        {
            var pokerWinlossCurr = new PokerWinlossByCurrency();
            pokerWinlossCurr.PokerWinlossDetailList = new List<PokerWinlossDetail>();
            double grossCommBaseGrand = 0, companyCommBaseGrand = 0;
            DataTable datatable = null;
            try
            {
                if (!isViewWH)
                    datatable = _pokerWlAccess.GetPokerWinlossDetail(wlDate, currency);
                else
                    datatable = _pokerWlAccess.GetPokerWinlossDetail_WH(wlDate, currency);
                if (datatable != null && datatable.Rows.Count > 0)
                {
                    foreach (DataRow row in datatable.Rows)
                    {
                        var username = row["username"].ToString();
                        var memberWL = NumberFormat.DecFormat(row["winloss"], 2);
                        var memberComm = NumberFormat.DecFormat(row["commplayer"], 2);
                        var agentComm = NumberFormat.DecFormat(row["commagent"], 2);
                        var masterComm = NumberFormat.DecFormat(row["commmaster"], 2);
                        var superComm = NumberFormat.DecFormat(row["commsuper"], 2);
                        var companyComm = NumberFormat.DecFormat(row["commcompany"], 2);
                        var grossComm = NumberFormat.DecFormat(row["commgross"], 2);
                        var grossCommBase = NumberFormat.DecFormat(row["commgrossVSRM"], 2);
                        var companyCommBase = NumberFormat.DecFormat(row["commcompanyVSRM"], 2);
                        pokerWinlossCurr.PokerWinlossDetailList.Add(new PokerWinlossDetail() { 
                            Name = username,
                            MemberWL = memberWL,
                            MemberComm = memberComm,
                            TotalMember = NumberFormat.DecFormat(CastAide.AsDouble(memberWL) + CastAide.AsDouble(memberComm), 2),
                            AgentComm = agentComm,
                            MasterComm = masterComm,
                            SuperComm = superComm,
                            CompanyComm = companyComm,
                            GrossComm = grossComm,
                            GrossCommBase = grossCommBase,
                            CompanyCommBase = companyCommBase
                        });
                        grossCommBaseGrand += CastAide.AsDouble(grossCommBase);
                        companyCommBaseGrand += CastAide.AsDouble(companyCommBase);
                    }
                    pokerWinlossCurr.CompanyCommBaseGrand = NumberFormat.DecFormat(companyCommBaseGrand, 2);
                    pokerWinlossCurr.GrossCommBaseGrand = NumberFormat.DecFormat(grossCommBaseGrand, 2);
                }
                AppSession.AddSession(HttpContext.Current.Session, _keyPokerWinlossDetailSession, pokerWinlossCurr);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return pokerWinlossCurr;
        }




        public ExcelUtil.Spreadsheet.ExcelFile PokerWinlossExportExcel(DateTime fromDate, DateTime toDate, bool isViewWH)
        {
            var excelFile = new ExcelFile();
            var pokerWinloss = new PokerWinloss();
            try
            {
                
                var objWinloss = AppSession.GetSession(HttpContext.Current.Session, _keyPokerWinlossSession);

                if (objWinloss != null)
                {
                    pokerWinloss = (PokerWinloss)objWinloss;
                }
                else
                {
                    pokerWinloss = GetPokerWinloss(fromDate, toDate, isViewWH);
                }
                #region Export
                var reportName = string.Format("PokerWinlossFrom{0}To{1}", fromDate.ToShortDateString().Replace("/", "_"), toDate.ToShortDateString().Replace("/", "_"));
                int startRowIndex = 0;
                int startColIndex = 0;
                var wsWinlossPoker = excelFile.Worksheets.Add(reportName);
                #region Header
                var gembox = new ExcelGemboxUtils();
                var bgFormat = gembox.GetStyleText(159, 183, 213, HorizontalAlignmentStyle.Center, true);
                var titleCell = wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex,
                                                     startColIndex + 11);
                titleCell.Merged = true;
                titleCell.Style = bgFormat;
                titleCell.Value = string.Format("Poker Win Loss - From {0} to {1}", fromDate.ToShortDateString(), toDate.ToShortDateString());
                startRowIndex += 1;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Date";
                startColIndex++;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Currency";
                startColIndex++;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Gross Comm";
                startColIndex++;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 2).Merged = true;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 2).Style = bgFormat;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 2).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 2).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 2).Value = "Member";

                wsWinlossPoker.Cells[startRowIndex + 1, startColIndex].Value = "Win/Loss";
                wsWinlossPoker.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossPoker.Cells[startRowIndex + 1, startColIndex].Value = "Comm";
                wsWinlossPoker.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossPoker.Cells[startRowIndex + 1, startColIndex].Value = "Total";
                wsWinlossPoker.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 3).Merged = true;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 3).Style = bgFormat;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 3).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 3).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 3).Value = "Comm";
                wsWinlossPoker.Cells[startRowIndex + 1, startColIndex].Value = "Agent";
                wsWinlossPoker.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossPoker.Cells[startRowIndex + 1, startColIndex].Value = "Master";
                wsWinlossPoker.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossPoker.Cells[startRowIndex + 1, startColIndex].Value = "Super";
                wsWinlossPoker.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossPoker.Cells[startRowIndex + 1, startColIndex].Value = "Company";
                wsWinlossPoker.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                //wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                //wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Gross Comm Base (RM)";
                startColIndex++;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                //wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Company Comm Base (RM)";

                #endregion
                #region Body
                var numberFormat = gembox.GetStyleIntNumber(".00", Color.FromArgb(255, 255, 255), false, false, false);
                numberFormat.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                var numberFormatNeg = gembox.GetStyleIntNumber(".00", Color.FromArgb(255, 255, 255), false, false, true);
                numberFormatNeg.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                var intNumberFormat = gembox.GetStyleIntNumber("", Color.FromArgb(255, 255, 255), false, false, false);
                intNumberFormat.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsWinlossPoker.Columns[0].Width = 255 * 15;
                wsWinlossPoker.Columns[1].Width = 255 * 10;
                wsWinlossPoker.Columns[2].Width = 255 * 10;
                wsWinlossPoker.Columns[3].Width = 255 * 10;
                wsWinlossPoker.Columns[4].Width = 255 * 10;
                wsWinlossPoker.Columns[5].Width = 255 * 10;
                wsWinlossPoker.Columns[6].Width = 255 * 10;
                wsWinlossPoker.Columns[7].Width = 255 * 10;
                wsWinlossPoker.Columns[8].Width = 255 * 10;
                wsWinlossPoker.Columns[9].Width = 255 * 10;
                wsWinlossPoker.Columns[10].Width = 255 * 15;
                wsWinlossPoker.Columns[11].Width = 255 * 15;
                startRowIndex = 3;
                if (pokerWinloss.ListPokerWLDate != null && pokerWinloss.ListPokerWLDate.Count > 0)
                {
                    foreach (PokerWinlossByDate wlbyDate in pokerWinloss.ListPokerWLDate)
                    {
                        startColIndex = 0;
                        int count = wlbyDate.ListPokerWLDetail.Count;
                        wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + count - 1, startColIndex).Merged = true;
                        wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + count - 1, startColIndex).Style = numberFormat;
                        wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + count - 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + count - 1, startColIndex).Value = wlbyDate.Date;
                        for (int i = 0; i < count; i++)
                        {
                            startColIndex++;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = numberFormat;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = wlbyDate.ListPokerWLDetail[i].Name;
                            startColIndex++;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = numberFormat;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.ListPokerWLDetail[i].GrossComm);
                            startColIndex++;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = numberFormat;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.ListPokerWLDetail[i].MemberWL);
                            startColIndex++;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = numberFormat;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.ListPokerWLDetail[i].MemberComm);
                            startColIndex++;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = numberFormat;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.ListPokerWLDetail[i].TotalMember);
                            startColIndex++;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = numberFormatNeg;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.ListPokerWLDetail[i].AgentComm);
                            startColIndex++;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = numberFormatNeg;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.ListPokerWLDetail[i].MasterComm);
                            startColIndex++;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = numberFormatNeg;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.ListPokerWLDetail[i].SuperComm);
                            startColIndex++;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = numberFormatNeg;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.ListPokerWLDetail[i].CompanyComm);
                            startColIndex++;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = numberFormatNeg;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.ListPokerWLDetail[i].GrossCommBase);
                            startColIndex++;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = numberFormatNeg;
                            wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wlbyDate.ListPokerWLDetail[i].CompanyCommBase);
                            startRowIndex++;
                            startColIndex = 0;
                        }

                    }
                    #region Export Total
                    var bgTotal = gembox.GetStyleIntNumber(".00", Color.FromArgb(221, 217, 196), false, false, false);
                    var bgIntNumberTotal = gembox.GetStyleIntNumber("", Color.FromArgb(221, 217, 196), false, false, false);
                    var bgTotalNumbeNegative = gembox.GetStyleIntNumber(".00", Color.FromArgb(221, 217, 196), false, false, true);
                    int countTotal = pokerWinloss.Total.Count;
                    wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + countTotal - 1, startColIndex).Merged = true;
                    wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + countTotal - 1, startColIndex).Style = bgTotal;
                    wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + countTotal - 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + countTotal - 1, startColIndex).Value = "Total";
                    foreach (PokerWinlossDetail total in pokerWinloss.Total)
                    {
                        startColIndex++;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = bgTotal;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = total.Name;
                        startColIndex++;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = bgTotal;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.GrossComm);
                        startColIndex++;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = bgTotal;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.MemberWL);
                        startColIndex++;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = bgTotal;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.MemberComm);
                        startColIndex++;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = bgTotal;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.TotalMember);
                        startColIndex++;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = bgTotalNumbeNegative;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.AgentComm);
                        startColIndex++;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = bgTotalNumbeNegative;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.MasterComm);
                        startColIndex++;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = bgTotalNumbeNegative;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.SuperComm);
                        startColIndex++;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = bgTotalNumbeNegative;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.CompanyComm);
                        startColIndex++;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = bgTotalNumbeNegative;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.GrossCommBase);
                        startColIndex++;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = bgTotalNumbeNegative;
                        wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(total.CompanyCommBase);
                        startRowIndex++;
                        startColIndex = 0;
                    }
                    #endregion

                    #region GrandTotal
                    //var bgGrandTotal = gembox.GetStyleIntNumber(".00", Color.FromArgb(196, 215, 155), true, false, false);
                    var bgIntNumberGrandTotal = gembox.GetStyleIntNumber("", Color.FromArgb(159, 183, 213), true, false, false);
                    var bgGrandTotalFormatNegative = gembox.GetStyleIntNumber(".00", Color.FromArgb(159, 183, 213), true, false, true);
                    wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 9).Merged = true;
                    wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 9).Style = bgIntNumberGrandTotal;
                    wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 9).Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 9).Value = "Grand Total";
                    startColIndex += 10;
                    wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = bgGrandTotalFormatNegative;
                    wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(pokerWinloss.GrossCommBaseGrand);
                    startColIndex++;
                    wsWinlossPoker.Cells[startRowIndex, startColIndex].Style = bgGrandTotalFormatNegative;
                    wsWinlossPoker.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(pokerWinloss.CompanyCommBaseGrand);
                    #endregion
                }
                else
                {
                    startRowIndex = 3;
                    startColIndex = 0;
                    var groupCell = wsWinlossPoker.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex,
                                                    startColIndex + 11);
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

        public ExcelUtil.Spreadsheet.ExcelFile PokerWinlossDetailExportExcel(DateTime wlDate, int currency, bool isViewWH)
        {
            var excelFile = new ExcelFile();
            var pokerWinlossDetail = new PokerWinlossByCurrency();
            try
            {
                var objWinloss = AppSession.GetSession(HttpContext.Current.Session, _keyPokerWinlossDetailSession);

                if (objWinloss != null)
                {
                    pokerWinlossDetail = (PokerWinlossByCurrency)objWinloss;
                }
                else
                {
                    pokerWinlossDetail = GetPokerWinlossDetail(wlDate, currency, isViewWH);
                }
                #region Export
                string currencyName = CacheUtils.GetExchangeName(currency);
                var reportName = string.Format("PokerWinlossDetails_{0}_{1}", wlDate.ToShortDateString().Replace("/", "_"), currencyName);
                int startRowIndex = 0;
                int startColIndex = 0;
                var wsWinlossPokerDetail = excelFile.Worksheets.Add(reportName);
                #region Header
                var gembox = new ExcelGemboxUtils();
                var bgFormat = gembox.GetStyleText(159, 183, 213, HorizontalAlignmentStyle.Center, true);
                var titleCell = wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex,
                                                     startColIndex + 10);
                titleCell.Merged = true;
                titleCell.Style = bgFormat;
                titleCell.Value = string.Format("Poker Win Loss Details - {0} - {1}", wlDate.ToShortDateString(), currencyName);
                startRowIndex += 1;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Username";

                startColIndex++;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Gross Comm";
                startColIndex++;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 2).Merged = true;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 2).Style = bgFormat;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 2).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 2).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 2).Value = "Member";

                wsWinlossPokerDetail.Cells[startRowIndex + 1, startColIndex].Value = "Win/Loss";
                wsWinlossPokerDetail.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossPokerDetail.Cells[startRowIndex + 1, startColIndex].Value = "Comm";
                wsWinlossPokerDetail.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossPokerDetail.Cells[startRowIndex + 1, startColIndex].Value = "Total";
                wsWinlossPokerDetail.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 3).Merged = true;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 3).Style = bgFormat;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 3).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 3).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 3).Value = "Comm";
                wsWinlossPokerDetail.Cells[startRowIndex + 1, startColIndex].Value = "Agent";
                wsWinlossPokerDetail.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossPokerDetail.Cells[startRowIndex + 1, startColIndex].Value = "Master";
                wsWinlossPokerDetail.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossPokerDetail.Cells[startRowIndex + 1, startColIndex].Value = "Super";
                wsWinlossPokerDetail.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossPokerDetail.Cells[startRowIndex + 1, startColIndex].Value = "Company";
                wsWinlossPokerDetail.Cells[startRowIndex + 1, startColIndex].Style = bgFormat;
                startColIndex++;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.VerticalAlignment = VerticalAlignmentStyle.Center;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Gross Comm Base (RM)";
                startColIndex++;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Merged = true;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style = bgFormat;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex + 1, startColIndex).Value = "Company Comm Base (RM)";

                #endregion
                #region Body
               
                var numberFormat = gembox.GetStyleIntNumber(".00", Color.FromArgb(255, 255, 255), false, false, false);
                var intNumberFormat = gembox.GetStyleIntNumber("", Color.FromArgb(255, 255, 255), false, false, false);
                var numberFormatNegative = gembox.GetStyleIntNumber(".00", Color.FromArgb(255, 255, 255), false, false, true);
                wsWinlossPokerDetail.Columns[0].Width = 255 * 15;
                wsWinlossPokerDetail.Columns[1].Width = 255 * 10;
                wsWinlossPokerDetail.Columns[2].Width = 255 * 10;
                wsWinlossPokerDetail.Columns[3].Width = 255 * 10;
                wsWinlossPokerDetail.Columns[4].Width = 255 * 10;
                wsWinlossPokerDetail.Columns[5].Width = 255 * 10;
                wsWinlossPokerDetail.Columns[6].Width = 255 * 10;
                wsWinlossPokerDetail.Columns[7].Width = 255 * 10;
                wsWinlossPokerDetail.Columns[8].Width = 255 * 10;
                wsWinlossPokerDetail.Columns[9].Width = 255 * 15;
                wsWinlossPokerDetail.Columns[10].Width = 255 * 15;
                startRowIndex = 3;
                startColIndex = 0;
                if (pokerWinlossDetail.PokerWinlossDetailList != null && pokerWinlossDetail.PokerWinlossDetailList.Count > 0)
                {
                    foreach (PokerWinlossDetail wldetail in pokerWinlossDetail.PokerWinlossDetailList)
                    {
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Style = numberFormat;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Value = wldetail.Name;
                        startColIndex++;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Style = numberFormat;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.GrossComm);
                        startColIndex++;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Style = numberFormat;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.MemberWL);
                        startColIndex++;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Style = numberFormat;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.MemberComm);
                        startColIndex++;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Style = numberFormat;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.TotalMember);
                        startColIndex++;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Style = numberFormatNegative;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.AgentComm);
                        startColIndex++;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Style = numberFormatNegative;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.MasterComm);
                        startColIndex++;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Style = numberFormatNegative;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.SuperComm);
                        startColIndex++;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Style = numberFormatNegative;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.CompanyComm);
                        startColIndex++;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Style = numberFormatNegative;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.GrossCommBase);
                        startColIndex++;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Style = numberFormatNegative;
                        wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(wldetail.CompanyCommBase);
                        startRowIndex++;
                        startColIndex = 0;

                    }
                    
                    var bgIntNumberTotal = gembox.GetStyleIntNumber("", Color.FromArgb(159, 183, 213), true, false, false);
                    var bgFormatNegative = gembox.GetStyleIntNumber(".00", Color.FromArgb(159, 183, 213), true, false, true);
                    wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 8).Merged = true;
                    wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 8).Style = bgIntNumberTotal;
                    wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 8).Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                    wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex, startColIndex + 8).Value = "Grand Total";
                    startColIndex += 9;
                    wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Style = bgFormatNegative;
                    wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(pokerWinlossDetail.GrossCommBaseGrand);
                    startColIndex++;
                    wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Style = bgFormatNegative;
                    wsWinlossPokerDetail.Cells[startRowIndex, startColIndex].Value = CastAide.AsDouble(pokerWinlossDetail.CompanyCommBaseGrand);

                }
                else
                {
                    startRowIndex = 3;
                    startColIndex = 0;
                    var groupCell = wsWinlossPokerDetail.Cells.GetSubrangeAbsolute(startRowIndex, startColIndex, startRowIndex,
                                                    startColIndex + 10);
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
