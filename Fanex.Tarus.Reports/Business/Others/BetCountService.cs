// -----------------------------------------------------------------------
// <copyright file="BetCountService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.Others
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Reflection;
    using DNAide.Core;
    using DNAide.Utils;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Reports.DataAccess.Others;
    using Fanex.Tarus.Reports.Entities.Others;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Core.Helper;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetCountService : IBetCountService
    {
        private readonly IBetCountDataAccess _betCountDataAccess;
        
        public BetCountService()
            : this(null)
        {
        }

        public BetCountService(IBetCountDataAccess betCountDataAccess)
        {
            _betCountDataAccess = betCountDataAccess ?? new BetCountDataAccess();            
        }

        public BetCount GetBetCountWithDateRange(
            string fromDate,
            string toDate,
            string products)
        {
            BetCount betCountObject = new BetCount();
            try
            {
                var dataTable = _betCountDataAccess.GetBetCountWithDateRange(fromDate, toDate, products);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    #region private variables
                    SiteTicketGroup depositTickets = new SiteTicketGroup();
                    depositTickets.SiteTickets = new List<SiteTicket>();
                    SiteTicketGroup creditTickets = new SiteTicketGroup();
                    creditTickets.SiteTickets = new List<SiteTicket>();
                    bool isDeposit = false;

                    double creditTurnover = 0, creditMemberWL = 0, creditAgent = 0, creditMaster = 0, creditSuper = 0, creditCompany = 0;
                    double depositTurnover = 0, depositMemberWL = 0, depositAgent = 0, depositMaster = 0, depositSuper = 0, depositCompany = 0;
                    int creditBetCount = 0, depositBetCount = 0;
                    betCountObject.HasData = true;
                    #endregion

                    foreach (DataRow row in dataTable.Rows)
                    {
                        var betcount = CastAide.AsInt32(row["BetCount"]);
                        var currentTurnover = CastAide.AsDouble(row["Turnover"]);
                        var memberWinLoss = CastAide.AsDouble(row["memberwl"]);
                        var agentWinLoss = CastAide.AsDouble(row["agentwl"]);
                        var masterWinLoss = CastAide.AsDouble(row["masterwl"]);
                        var superWinLoss = CastAide.AsDouble(row["superwl"]);
                        var companyWinLoss = CastAide.AsDouble(row["company"]);

                        var winloss = new SiteTicket
                        {
                            SiteName = CastAide.AsString(row["Site"]),
                            NumOfTicket = NumberFormat.DoubleFormat(betcount, 0),
                            TotalTurnover = NumberFormat.DoubleFormat(currentTurnover, 2),
                            MemberWinloss = NumberFormat.DoubleFormat(memberWinLoss, 2),
                            MarginPercent = NumberFormat.DoubleFormat((currentTurnover!= 0 ? (memberWinLoss / currentTurnover * 100) : 0), 2),
                            AgentWinLoss = NumberFormat.DoubleFormat(agentWinLoss, 2),
                            MasterWinLoss = NumberFormat.DoubleFormat(masterWinLoss, 2),
                            SuperWinLoss = NumberFormat.DoubleFormat(superWinLoss, 2),
                            Company = NumberFormat.DoubleFormat(companyWinLoss, 2)
                        };

                        isDeposit = CastAide.AsBoolean(row["IsDeposit"]);
                        #region deposit
                        if (isDeposit)
                        {
                            depositTickets.SiteTickets.Add(winloss);
                            depositBetCount += CastAide.AsInt32(betcount);
                            depositTurnover += currentTurnover;
                            depositMemberWL += memberWinLoss;
                            depositAgent += CastAide.AsDouble(agentWinLoss);
                            depositMaster += CastAide.AsDouble(masterWinLoss);
                            depositSuper += CastAide.AsDouble(superWinLoss);
                            depositCompany += CastAide.AsDouble(companyWinLoss);
                        }
                        #endregion
                        #region credit
                        else
                        {
                            creditTickets.SiteTickets.Add(winloss);
                            creditBetCount += CastAide.AsInt32(betcount);
                            creditTurnover += currentTurnover;
                            creditMemberWL += memberWinLoss;
                            creditAgent += CastAide.AsDouble(agentWinLoss);
                            creditMaster += CastAide.AsDouble(masterWinLoss);
                            creditSuper += CastAide.AsDouble(superWinLoss);
                            creditCompany += CastAide.AsDouble(companyWinLoss);
                        }
                        #endregion
                    }

                    betCountObject.SiteTicketGroups = new List<SiteTicketGroup>();

                    #region Add total for Credit Site and List Ticket
                    creditTickets.TotalSiteTicket = new SiteTicket
                    {
                        SiteName = "Credit",
                        NumOfTicket = NumberFormat.DoubleFormat(creditBetCount, 0),
                        TotalTurnover = NumberFormat.DoubleFormat(creditTurnover, 2),
                        MemberWinloss = NumberFormat.DoubleFormat(creditMemberWL, 2),
                        MarginPercent = NumberFormat.DoubleFormat((creditTurnover != 0 ? (creditMemberWL / creditTurnover * 100) : 0), 2),
                        AgentWinLoss = NumberFormat.DoubleFormat(creditAgent, 2),
                        MasterWinLoss = NumberFormat.DoubleFormat(creditMaster, 2),
                        SuperWinLoss = NumberFormat.DoubleFormat(creditSuper, 2),
                        Company = NumberFormat.DoubleFormat(creditCompany, 2)
                    };
                    betCountObject.SiteTicketGroups.Add(creditTickets);
                    #endregion

                    #region Add total for Deposit Site and List Ticket
                    depositTickets.TotalSiteTicket = new SiteTicket
                    {
                        SiteName = "Deposit",
                        NumOfTicket = NumberFormat.DoubleFormat(depositBetCount, 0),
                        TotalTurnover = NumberFormat.DoubleFormat(depositTurnover, 2),
                        MemberWinloss = NumberFormat.DoubleFormat(depositMemberWL, 2),
                        MarginPercent = NumberFormat.DoubleFormat((depositTurnover != 0 ? (depositMemberWL / depositTurnover * 100) : 0), 2),
                        AgentWinLoss = NumberFormat.DoubleFormat(depositAgent, 2),
                        MasterWinLoss = NumberFormat.DoubleFormat(depositMaster, 2),
                        SuperWinLoss = NumberFormat.DoubleFormat(depositSuper, 2),
                        Company = NumberFormat.DoubleFormat(depositCompany, 2)
                    };
                    betCountObject.SiteTicketGroups.Add(depositTickets);
                    #endregion

                    #region grand total
                    betCountObject.TotalBetCount = new SiteTicket
                    {
                        SiteName = "Total",
                        NumOfTicket = NumberFormat.DoubleFormat(depositBetCount + creditBetCount, 0),
                        TotalTurnover = NumberFormat.DoubleFormat(depositTurnover + creditTurnover, 2),
                        MemberWinloss = NumberFormat.DoubleFormat(depositMemberWL + creditMemberWL, 2),
                        MarginPercent = NumberFormat.DoubleFormat(((depositTurnover + creditTurnover) != 0 ? ((depositMemberWL + creditMemberWL) / (depositTurnover + creditTurnover) * 100) : 0), 2),
                        AgentWinLoss = NumberFormat.DoubleFormat(depositAgent + creditAgent, 2),
                        MasterWinLoss = NumberFormat.DoubleFormat(depositMaster + creditMaster, 2),
                        SuperWinLoss = NumberFormat.DoubleFormat(depositSuper + creditSuper, 2),
                        Company = NumberFormat.DoubleFormat(depositCompany + creditCompany, 2)
                    };
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return betCountObject;
        }

        #region export 2 excel
        public ExcelFile ExportBetCountWithExcelWithDateRange(
            string fromDate,
            string toDate, 
            string products,
            BetCount betCount)
        {
            if (betCount == null)
            {
                betCount = GetBetCountWithDateRange(fromDate, toDate, products);
            }

            // Export Excel File
            int startRow = 3;
            int startCol = 0;            
            var excelFile = new ExcelFile();
            var wsBetCount = excelFile.Worksheets.Add("BetCountReport");
            var bgHeader = new ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));

            #region header

            wsBetCount.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + 8).Merged = true;
            wsBetCount.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + 8).Value = string.Format("{0} from {1} to {2}", "Bet Count Report", fromDate, toDate);
            wsBetCount.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + 8).Style.Font.Weight = ExcelFont.BoldWeight;
            wsBetCount.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, startCol + 8).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center; 
            RenderExcelHeaderRow(wsBetCount, bgHeader, startRow, startCol, new SiteTicket
            {
                SiteName = "Site",
                NumOfTicket = "Bet Count",
                TotalTurnover = "Total Turnover",
                MemberWinloss = "Member Winloss",
                MarginPercent = "Margin %",
                AgentWinLoss = "Agent",
                MasterWinLoss = "Master",
                SuperWinLoss = "Super",
                Company = "Company"
            });
            startRow++;

            #endregion header

            #region Content

            if (betCount != null && betCount.HasData)
            {
                var normalBg = new ExcelGemboxUtils().GetStyleText(false);
                normalBg.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                var totalTicketTextCellStyle = GetExcelStyleAndFormat(true, true, false, true, true);
                totalTicketTextCellStyle.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                var totalTicketNumberCellStyle = GetExcelStyleAndFormat(true, true);
                foreach (SiteTicketGroup groupOfSiteTicket in betCount.SiteTicketGroups)
                {
                    // Render Sub Total Row for Each Customer Type
                    RenderExcelRow(wsBetCount, totalTicketTextCellStyle, totalTicketNumberCellStyle, startRow - 1, startCol, groupOfSiteTicket.TotalSiteTicket);
                    startRow++;

                    var siteTicketNumberCellStyle = GetExcelStyleAndFormat();
                    foreach (SiteTicket siteTicket in groupOfSiteTicket.SiteTickets)
                    {
                        // Render Betcount for each Site
                        RenderExcelRow(wsBetCount, normalBg, siteTicketNumberCellStyle, startRow - 1, startCol, siteTicket);
                        startRow++;
                    }
                }

                // Render Row Total
                var footerFormat = GetExcelStyleAndFormat(true, false, false, true);
                RenderExcelRow(wsBetCount, bgHeader, footerFormat, startRow - 1, startCol, betCount.TotalBetCount);
            }

            #endregion Content

            return excelFile;
        }

        private void RenderExcelHeaderRow(
            ExcelWorksheet excelWorksheet,
            CellStyle cellStyle,
            int startRow,
            int startColumn,
            SiteTicket siteTicket)
        {
            var nexcol = 0;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn, startRow - 1, startColumn).Merged = true;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn, startRow - 1, startColumn).Style = cellStyle;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn, startRow - 1, startColumn).Value = siteTicket.SiteName;
            excelWorksheet.Columns[startColumn].Width = 18 * 256;

            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol += 1), startRow - 1, startColumn + nexcol).Merged = true;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol), startRow - 1, startColumn + nexcol).Style = cellStyle;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol), startRow - 1, startColumn + nexcol).Value = siteTicket.NumOfTicket;
            excelWorksheet.Columns[startColumn + nexcol].Width = 18 * 256;

            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol += 1), startRow - 1, startColumn + nexcol).Merged = true;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol), startRow - 1, startColumn + nexcol).Style = cellStyle;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol), startRow - 1, startColumn + nexcol).Value = siteTicket.TotalTurnover;
            excelWorksheet.Columns[startColumn + nexcol].Width = 20 * 256;

            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol += 1), startRow - 1, startColumn + nexcol).Merged = true; ;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol), startRow - 1, startColumn + nexcol).Style = cellStyle;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol), startRow - 1, startColumn + nexcol).Value = siteTicket.MemberWinloss;
            excelWorksheet.Columns[startColumn + nexcol].Width = 20 * 256;

            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol += 1), startRow - 1, startColumn + nexcol).Merged = true;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol), startRow - 1, startColumn + nexcol).Style = cellStyle;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol), startRow - 1, startColumn + nexcol).Value = siteTicket.MarginPercent;
            excelWorksheet.Columns[startColumn + nexcol].Width = 12 * 256;

            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol + 1), startRow - 2, startColumn + nexcol + 3).Merged = true;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol + 1), startRow - 2, startColumn + nexcol + 3).Style = cellStyle;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol + 1), startRow - 2, startColumn + nexcol + 3).Value = "SMA Winloss";

            excelWorksheet.Cells[startRow - 1, startColumn + (nexcol += 1)].Style = cellStyle;
            excelWorksheet.Cells[startRow - 1, startColumn + (nexcol)].Value = siteTicket.AgentWinLoss;
            excelWorksheet.Columns[startColumn + nexcol].Width = 20 * 256;

            excelWorksheet.Cells[startRow - 1, startColumn + (nexcol += 1)].Style = cellStyle;
            excelWorksheet.Cells[startRow - 1, startColumn + (nexcol)].Value = siteTicket.MasterWinLoss;
            excelWorksheet.Columns[startColumn + nexcol].Width = 20 * 256;

            excelWorksheet.Cells[startRow - 1, startColumn + (nexcol += 1)].Style = cellStyle;
            excelWorksheet.Cells[startRow - 1, startColumn + (nexcol)].Value = siteTicket.SuperWinLoss;
            excelWorksheet.Columns[startColumn + nexcol].Width = 20 * 256;

            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol += 1), startRow - 1, startColumn + nexcol).Merged = true;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol), startRow - 1, startColumn + nexcol).Style = cellStyle;
            excelWorksheet.Cells.GetSubrangeAbsolute(startRow - 2, startColumn + (nexcol), startRow - 1, startColumn + nexcol).Value = siteTicket.Company;
            excelWorksheet.Columns[startColumn + nexcol].Width = 20 * 256;
        }

        private void RenderExcelRow(
            ExcelWorksheet excelWorksheet,
            CellStyle cellStyleForTextCell,
            CellStyle cellStyleForNumberCell,
            int startRow,
            int startColumn,
            SiteTicket siteTicket)
        {
            excelWorksheet.Cells[startRow, startColumn].Style = cellStyleForTextCell;
            excelWorksheet.Cells[startRow, startColumn].Value = siteTicket.SiteName;
            excelWorksheet.Cells[startRow, startColumn + 1].Style = cellStyleForNumberCell;
            excelWorksheet.Cells[startRow, startColumn + 1].Style.NumberFormat = "#,##0;(-#,##0)";
            excelWorksheet.Cells[startRow, startColumn + 1].Value = CastAide.AsDouble(siteTicket.NumOfTicket);
            excelWorksheet.Cells[startRow, startColumn + 2].Style = cellStyleForNumberCell;
            excelWorksheet.Cells[startRow, startColumn + 2].Value = CastAide.AsDouble(siteTicket.TotalTurnover);
            excelWorksheet.Cells[startRow, startColumn + 3].Style = cellStyleForNumberCell;
            excelWorksheet.Cells[startRow, startColumn + 3].Value = CastAide.AsDouble(siteTicket.MemberWinloss);
            excelWorksheet.Cells[startRow, startColumn + 4].Style = cellStyleForNumberCell;
            excelWorksheet.Cells[startRow, startColumn + 4].Value = CastAide.AsDouble(siteTicket.MarginPercent);
            excelWorksheet.Cells[startRow, startColumn + 5].Style = cellStyleForNumberCell;
            excelWorksheet.Cells[startRow, startColumn + 5].Value = CastAide.AsDouble(siteTicket.AgentWinLoss);
            excelWorksheet.Cells[startRow, startColumn + 6].Style = cellStyleForNumberCell;
            excelWorksheet.Cells[startRow, startColumn + 6].Value = CastAide.AsDouble(siteTicket.MasterWinLoss);
            excelWorksheet.Cells[startRow, startColumn + 7].Style = cellStyleForNumberCell;
            excelWorksheet.Cells[startRow, startColumn + 7].Value = CastAide.AsDouble(siteTicket.SuperWinLoss);

            excelWorksheet.Cells[startRow, startColumn + 8].Style = cellStyleForTextCell;
            excelWorksheet.Cells[startRow, startColumn + 8].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            excelWorksheet.Cells[startRow, startColumn + 8].Style.NumberFormat = "[RED][>0]#,##0.00;[<0]-#,##0.00;#,##0.00";
            excelWorksheet.Cells[startRow, startColumn + 8].Value = CastAide.AsDouble(siteTicket.Company);
        }

        private CellStyle GetExcelStyleAndFormat(
            bool isBold = false,
            bool isSubTotal = false,
            bool isHeader = false,
            bool isFooter = false,
            bool isFormatForText = false)
        {
            var style = new CellStyle();
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            if (!isFormatForText)
                style.NumberFormat = "#,##0.00;-#,##0.00";
            style.Font.Name = "Tahoma";
            style.Font.Size = 160;
            if (isFooter || isHeader)
            {
                style.FillPattern.SetSolid(Color.FromArgb(205, 222, 255));
            }
            if (isSubTotal)
            {
                style.FillPattern.SetSolid(Color.FromArgb(174, 174, 174));
            }
            if (isBold)
                style.Font.Weight = ExcelFont.BoldWeight;
            return style;
        }

        #endregion

        #region sorting
        public BetCount SortingBetCountReport(string fromDate, string toDate, string products, string sortCol, string sortType, BetCount betCount)
        {
            var result = new BetCount();
            try
            {
                if (betCount == null)
                {
                    betCount = GetBetCountWithDateRange(fromDate, toDate, products);                    
                }

                if (betCount != null && betCount.HasData)
                {
                    if (sortType.Length > 0)
                    {
                        SortList(betCount.SiteTicketGroups[0].SiteTickets, sortType, sortCol);
                        SortList(betCount.SiteTicketGroups[1].SiteTickets, sortType, sortCol);
                    }
                    else
                    {
                        result = betCount;
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return betCount;
        }

        void SortList<T>(List<T> dataSource, string sortType, string sortCol)
        {
            PropertyInfo propInfo = typeof(T).GetProperty(sortCol);
            if (propInfo == null)
                return;
            bool isAsc = false;
            if (sortType.ToLower().Equals("asc"))
                isAsc = true;
            else
                isAsc = false;

            Comparison<T> compare = delegate(T a, T b)
            {
                object valueA = isAsc ? CastAide.AsDecimal(propInfo.GetValue(a, null)) : CastAide.AsDecimal(propInfo.GetValue(b, null));
                object valueB = isAsc ? CastAide.AsDecimal(propInfo.GetValue(b, null)) : CastAide.AsDecimal(propInfo.GetValue(a, null));

                return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
            };
            
            dataSource.Sort(compare);
        }
        #endregion

        public List<Product> GetAllProducts()
        {
            var result = new List<Product>();
            try
            {
                var tbProducts = _betCountDataAccess.GetAllProducts();
                if (tbProducts != null && tbProducts.Rows.Count > 0)
                {
                    foreach (DataRow product in tbProducts.Rows)
                    {
                        result.Add(new Product()
                        {
                            Id = CastAide.AsInt32(product["sportid"]),
                            Name= CastAide.AsString(product["sportname"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }
    }
}