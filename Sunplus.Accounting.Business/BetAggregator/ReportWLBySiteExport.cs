using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using DNAide.Core;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Entities.BetAggregator;


namespace Sunplus.Accounting.Business.BetAggregator
{
    public partial class ReportWlbaBySiteService
    {
        #region Export 2 excel
        #region licensee report
        public void Export2ExcelReportByLicensee(string reportTitle, int licenseeId, int currency, int BaseCurrency, string fromDate, string toDate, int showSb, int showRb,int showNg, int reportType, int drawFilter, int excludeTest, int pageIndex, int pageSize, int reportSelected, int custId)
        {
            #region excel format
            const int startCol = 0;
            const int startRow = 2;
            var nextRow = 0;

            var reportName = reportTitle.Replace(" ", "");
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(string.Format("{0}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDepositReport")));

            #endregion

            #region header
            wsReport.Cells[startRow - 2, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow - 2, startCol].Value = reportTitle;
            Export2ExcelReportByLicenseeAssignHeader(ref wsReport, startRow, startCol, reportType, reportSelected);
            #endregion

            #region content
            var MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();
            var getReportData = new Report_LicenseeWLBA_DTO();
            switch (reportSelected)
            {
                case 0:
                    getReportData = (Report_LicenseeWLBA_DTO)AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                           _keySelReportByLicensee);
                    getReportData = getReportData ??
                                    SelReportByLicenseeFromDB(licenseeId, currency, BaseCurrency, fromDate, toDate, showSb, showRb, showNg,
                                                              reportType, MoveBetsDay,
                                                              drawFilter,excludeTest,1, pageIndex,pageSize);


                    break;
                case 1:
                    getReportData = (Report_LicenseeWLBA_DTO)AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                           _keySelReportByLicenseeDetail_Ag);
                    getReportData = getReportData ??
                                    SelReportByLicenseeDetail_AgFromDB(licenseeId, currency, BaseCurrency, fromDate, toDate, showSb,
                                                                       showRb,showNg,
                                                                       reportType, drawFilter, excludeTest,1, pageIndex,pageSize);
                    break;
                case 2:
                    getReportData = (Report_LicenseeWLBA_DTO)AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                           _keySelReportByLicenseeDetail);
                    getReportData = getReportData ??
                                    SelReportByLicenseeDetailFromDB(licenseeId, currency, BaseCurrency, custId, fromDate, toDate,
                                                                    showSb, showRb, showNg,
                                                                    reportType, MoveBetsDay, drawFilter, excludeTest,1, pageIndex,pageSize);
                    break;
                default:
                    break;
            }


            if (getReportData != null && getReportData.Items != null)
            {
                Export2ExcelReportByLicenseeAssignContent("",ref wsReport, startRow, startCol, ref nextRow, false, false,
                                                          getReportData.Items, reportSelected, getReportData.DateGroup);
                Export2ExcelReportByLicenseeAssignContent("Total ", ref wsReport, startRow, startCol, ref nextRow, true, false,
                                                          getReportData.Totals, reportSelected);
                Export2ExcelReportByLicenseeAssignContent("Grand Total ", ref wsReport, startRow, startCol, ref nextRow, false, true,
                                                          getReportData.TotalsBaseCurrency, reportSelected);

            }

            #endregion

            #region response file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);
            #endregion
        }

        private static void Export2ExcelReportByLicenseeAssignHeader(ref ExcelWorksheet wsReport, int startRow, int startCol, int reportType, int selectedReport)
        {
            #region private variables
            var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;
            var dateTitle = selectedReport == 0 ? (reportType != 3
                                ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Date")
                                : ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "sport"))
                                : ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "username");

            var currencyTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Currency");
            var turnoverTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TurnOver");
            var totalTransTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTotalTrans");
           // var memberTilte = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Member");
           // var agentTilte = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent");
            //var companyTilte = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Company");
            //var winlossTilte = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "WinLost");
            //var commTilte = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
           // var totalTilte = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Total");
            var customerTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CustomerWL");
            var licenseeCommTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "licenseeComm");
            var AmtDuetoBATitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "AmtDuetoBA");

            #endregion

            
            wsReport.Cells[startRow - 1, startCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol].Value = dateTitle;

            nextCol++;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = currencyTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = currencyTitle;

            nextCol++;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = turnoverTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = turnoverTitle;

            nextCol++;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = totalTransTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = totalTransTitle;


            #region member
            nextCol++;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = customerTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = customerTitle;
            #endregion

            #region agent
            nextCol++;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = licenseeCommTitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = licenseeCommTitle;

            #endregion
            nextCol++;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            //wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = AmtDuetoBATitle;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = AmtDuetoBATitle;
        }

        private static void Export2ExcelReportByLicenseeAssignContent(string preCurrency, ref ExcelWorksheet wsReport, int startRow, int startCol, ref int nextRow, bool isTotal, bool isTotalBaseCurr, List<Report_LicenseeWLBA_Row> licenseeRows, int reportSelected, List<int> dateGroup )
        {

            var bgColor = isTotal
                              ? Color.FromArgb(255, 238, 221)
                              : (isTotalBaseCurr ? Color.FromArgb(211, 225, 200) : Color.FromArgb(255, 255, 255));
            var numberFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", bgColor,
                                                                                    isTotal || isTotalBaseCurr
                                                                                        ? true
                                                                                        : false, false);
            var numberNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", bgColor,
                                                                                    isTotal || isTotalBaseCurr
                                                                                        ? true
                                                                                        : false, false);
            var index = 0;
            if (dateGroup != null && dateGroup.Count > 0)
            {
                var items = 0;
                for (int i = 0; i < dateGroup.Count; i++)
                {
                    var totalMergeRows = CastAide.AsInt32(dateGroup[i]);
                    for (int j = 0; j < totalMergeRows; j++)
                    {
                        var licenseeRow = licenseeRows[index];
                        var nextCol = 0;
                        if (j == 0 && !isTotal && !isTotalBaseCurr)
                        {
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = numberFormat;
                            if (reportSelected == 0)
                            {
                                //wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = licenseeRow.ReportTypeId;
                                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow + totalMergeRows - 1, startCol + nextCol).Merged = true;
                                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow + totalMergeRows - 1, startCol + nextCol).Style = numberFormat;
                                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow + totalMergeRows - 1, startCol + nextCol).Value = licenseeRow.ReportTypeId;


                            }
                            else
                            {
                                wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = licenseeRow.Username;
                            }

                            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = licenseeRow.CurrencyName;

                        }
                        else if (isTotalBaseCurr || isTotal)
                        {
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol + 1)).
                                Merged = true;
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol + 1)).
                                Style = numberFormat;
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol += 1)).
                                Value = preCurrency + licenseeRow.CurrencyName;
                        }
                        else
                        {
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = licenseeRow.CurrencyName;
                        }
                        


                        #region Turnover
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.Turnover);
                        #endregion

                        #region betcount (total trans)
                        if (items == 0 && isTotalBaseCurr)
                        {
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol += 1),
                                                               startRow + nextRow + licenseeRows.Count - 1, startCol + nextCol).
                                Merged = true;
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol),
                                                               startRow + nextRow + licenseeRows.Count - 1, startCol + nextCol).
                                Style = numberNormalFormat;
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol),
                                                               startRow + nextRow + licenseeRows.Count - 1, startCol + nextCol).
                                Value = CastAide.AsDouble(licenseeRow.BetCount);
                        }
                        else if (isTotalBaseCurr && items > 0)
                            nextCol += 1;
                        else if (!isTotalBaseCurr)
                        {
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberNormalFormat;
                            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.BetCount);
                        }

                        #endregion

                        #region member
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.Winlost);
                        #endregion

                        #region agent

                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.AComm);
                        #endregion
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.Company);

                        items++;
                        nextRow++;
                        index++;
                    }
                }
            }
            else
            {
                var items = 0;
                foreach (Report_LicenseeWLBA_Row licenseeRow in licenseeRows)
                {
                    var nextCol = 0;
                    #region current row is total or not
                    if (isTotal || isTotalBaseCurr)
                    {
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol + 1)).
                            Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol + 1)).
                            Style = numberFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol += 1)).
                            Value = preCurrency + licenseeRow.CurrencyName;
                    }
                    else
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = numberFormat;
                        if (reportSelected == 0)
                        {
                            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = licenseeRow.ReportTypeId;
                        }
                        else
                        {
                            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = licenseeRow.Username;
                        }

                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = licenseeRow.CurrencyName;
                    }


                    #endregion

                    #region Turnover
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.Turnover);
                    #endregion

                    #region betcount (total trans)
                    if (items == 0 && isTotalBaseCurr)
                    {
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol += 1),
                                                           startRow + nextRow + licenseeRows.Count - 1, startCol + nextCol).
                            Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol),
                                                           startRow + nextRow + licenseeRows.Count - 1, startCol + nextCol).
                            Style = numberNormalFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol),
                                                           startRow + nextRow + licenseeRows.Count - 1, startCol + nextCol).
                            Value = CastAide.AsDouble(licenseeRow.BetCount);
                    }
                    else if (isTotalBaseCurr && items > 0)
                        nextCol += 1;
                    else if (!isTotalBaseCurr)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberNormalFormat;
                        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.BetCount);
                    }

                    #endregion

                    #region member
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.Winlost);

                    //wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    //wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.Comm);

                    //wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    //wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.Total);
                    #endregion

                    #region agent
                    //wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    //wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.AWinlost);

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.AComm);

                    //wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    //wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.ATotal);
                    #endregion
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.Company);
                    nextRow++;
                    items++;
                }
            }
            
        }

        private static void Export2ExcelReportByLicenseeAssignContent(string preCurrency, ref ExcelWorksheet wsReport, int startRow, int startCol, ref int nextRow, bool isTotal, bool isTotalBaseCurr, List<Report_LicenseeWLBA_Row> licenseeRows, int reportSelected)
        {

            var bgColor = isTotal
                              ? Color.FromArgb(255, 238, 221)
                              : (isTotalBaseCurr ? Color.FromArgb(211, 225, 200) : Color.FromArgb(255, 255, 255));
            var numberFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", bgColor,
                                                                                    isTotal || isTotalBaseCurr
                                                                                        ? true
                                                                                        : false, false);
            var numberNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", bgColor,
                                                                                    isTotal || isTotalBaseCurr
                                                                                        ? true
                                                                                        : false, false);
            var index = 0;
            
                var items = 0;
                foreach (Report_LicenseeWLBA_Row licenseeRow in licenseeRows)
                {
                    var nextCol = 0;
                    #region current row is total or not
                    if (isTotal || isTotalBaseCurr)
                    {
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol + 1)).
                            Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol + 1)).
                            Style = numberFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol += 1)).
                            Value = preCurrency + licenseeRow.CurrencyName;
                    }
                    else
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = numberFormat;
                        if (reportSelected == 0)
                        {
                            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = licenseeRow.ReportTypeId;
                        }
                        else
                        {
                            wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = licenseeRow.Username;
                        }

                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = licenseeRow.CurrencyName;
                    }


                    #endregion

                    #region Turnover
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.Turnover);
                    #endregion

                    #region betcount (total trans)
                    if (items == 0 && isTotalBaseCurr)
                    {
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol += 1),
                                                           startRow + nextRow + licenseeRows.Count - 1, startCol + nextCol).
                            Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol),
                                                           startRow + nextRow + licenseeRows.Count - 1, startCol + nextCol).
                            Style = numberNormalFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol),
                                                           startRow + nextRow + licenseeRows.Count - 1, startCol + nextCol).
                            Value = CastAide.AsDouble(licenseeRow.BetCount);
                    }
                    else if (isTotalBaseCurr && items > 0)
                        nextCol += 1;
                    else if (!isTotalBaseCurr)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberNormalFormat;
                        wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.BetCount);
                    }

                    #endregion

                    #region member
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.Winlost);

                    //wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    //wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.Comm);

                    //wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    //wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.Total);
                    #endregion

                    #region agent
                    //wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    //wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.AWinlost);

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.AComm);

                    //wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    //wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.ATotal);
                    #endregion
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + nextCol].Value = CastAide.AsDouble(licenseeRow.Company);
                    nextRow++;
                    items++;
                }
            

        }

        #endregion 

        #region BA win loss by licensee
        public void ExportExcelBAWinLossReportByLicensee(int licenseeId, int currency, int baseCurrency, string fromDate, string toDate, int reportType, int drawfilter, int excludeTest, string title, int reportSelected)
        {
            #region excel format
            const int startCol = 0;
            const int startRow = 2;
            var nextRow = 0;
            var reportName = title.Replace(" ", "");
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(string.Format("{0}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDepositReport")));
            #endregion
            
            #region header
            wsReport.Cells.GetSubrangeAbsolute(startRow -2, startCol, startRow -2, startCol+ 7).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 7).Style.
                HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 7).Style.Font.Weight =
                ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 7).Value = title;
            ExportExcelBAWinLossReportByLicenseeAssignHeader(ref wsReport, startRow, startCol, reportType, reportSelected);
            #endregion

            #region content

            var getData = SessionExtension.GetSession(HttpContext.Current.Session, _keySelBAReportByLicensee) != null
                              ? (LicenseesWLBA)
                                SessionExtension.GetSession(HttpContext.Current.Session, _keySelBAReportByLicensee)
                              : SelReportByLicensees(licenseeId, currency, baseCurrency, fromDate, toDate, reportType,
                                                     drawfilter, excludeTest, 1, 100, 0);
            if (getData != null && getData.Items.Count > 0)
            {
                ExportExcelBAWinLossReportByLicenseeAssignContent(ref wsReport, startRow, startCol, ref nextRow,
                                                                  getData.Items);


                #region grand total
                var numberBaseTotalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(250, 191, 143), true, false);

                var nextCol = 0;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol,
                                                   startRow + nextRow + 2,
                                                   startCol + nextCol + 1).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol,
                                                   startRow + nextRow + 2,
                                                   startCol + nextCol + 1).Style = numberBaseTotalFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol,
                                                   startRow + nextRow + 2,
                                                   startCol + nextCol + 1).Style.HorizontalAlignment =
                    HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol,
                                                   startRow + nextRow + 2,
                                                   startCol + nextCol + 1).Value = "Grand Total";

                var baseIndex = 0;
                foreach (Report_LicenseeWLBA_Row total in getData.TotalsBaseCurrency)
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 2)].Style = numberBaseTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Center;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = total.CurrencyName;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberBaseTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(total.Turnover);

                    if (baseIndex == 0)
                    {
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol + 1), startRow + nextRow + 2, startCol + nextCol + 1).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol + 1), startRow + nextRow + 2, startCol + nextCol + 1).Style = numberBaseTotalFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol + 1), startRow + nextRow + 2, startCol + nextCol + 1).Style.NumberFormat = "#,##0";
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol + 1), startRow + nextRow + 2, startCol + nextCol + 1).Value = CastAide.AsDouble(total.BetCount);

                        nextCol++;

                    }
                    else
                        nextCol++;

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberBaseTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(total.Winlost);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberBaseTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(total.AComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberBaseTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(total.Company);

                    baseIndex++;
                    nextRow++;
                    nextCol = 0;
                }
                #endregion

            }

            #endregion

            #region response file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);
            #endregion
        }
        
        private void ExportExcelBAWinLossReportByLicenseeAssignHeader(ref ExcelWorksheet wsReport, int startRow, int startCol, int reportType, int selectedReport)
        {
            #region private variables
            var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;
            var dateTitle = selectedReport == 0
                                ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Date")
                                : ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "username");

            var currencyTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Currency");
            var turnoverTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "TurnOver");
            var totalTransTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTotalTrans");
            var customerTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CustomerWL");
            var licenseeCommTitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "licenseeComm");
            var AmtDuetoBATitle = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "AmtDuetoBA");

            #endregion

            wsReport.Cells[startRow - 1, startCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol].Value = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblLicensee");

            nextCol++;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = dateTitle;

            nextCol++;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = currencyTitle;

            nextCol++;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = turnoverTitle;

            nextCol++;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = totalTransTitle;


            #region member
            nextCol++;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = customerTitle;
            #endregion

            #region agent
            nextCol++;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = licenseeCommTitle;

            #endregion
            nextCol++;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = AmtDuetoBATitle;
        }

        private void ExportExcelBAWinLossReportByLicenseeAssignContent(ref ExcelWorksheet wsReport, int startRow, int startCol, ref int nextRow, List<LicenseeWLBA> data)
        {
            var numberFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(255, 255, 255), false, false);
            var numberTotalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(255, 238, 221), true, false);
            var numberBaseTotalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(211, 225, 200), true, false);

            foreach (LicenseeWLBA licenseeWlba in data)
            {
                var totalItems = licenseeWlba.Items.Count + licenseeWlba.Totals.Count + licenseeWlba.TotalsBaseCurrency.Count;
                var nextCol = 0;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + (totalItems-1),
                                                   startCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + (totalItems - 1),
                                                   startCol).Style = numberFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + (totalItems - 1),
                                                   startCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow + (totalItems - 1),
                                                   startCol).Value = licenseeWlba.Licensee;
                #region items                
                var index = 0;
                for (int i = 0; i < licenseeWlba.DateGroup.Count; i++)
                {
                    var totalMergeRows = CastAide.AsInt32(licenseeWlba.DateGroup[i]);
                    for (int j = 0; j < totalMergeRows; j++)
                    {
                        var item = licenseeWlba.Items[index];

                        if (j == 0)
                        {
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol + 1, startRow + nextRow + totalMergeRows - 1, startCol + nextCol + 1).Merged = true;
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol + 1, startRow + nextRow + totalMergeRows - 1, startCol + nextCol + 1).Style = numberFormat;
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol + 1, startRow + nextRow + totalMergeRows - 1, startCol + nextCol + 1).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol + 1, startRow + nextRow + totalMergeRows - 1, startCol + nextCol + 1).Value = item.ReportTypeId;

                            nextCol += 1;
                        }
                        else
                            nextCol += 1;
                       

                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment =
                            HorizontalAlignmentStyle.Center;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = item.CurrencyName;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.Turnover);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.NumberFormat = "#,##0";
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.BetCount);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.Winlost);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.AComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.Company);

                        index++;
                        nextRow++;
                        nextCol = 0;
                    }
                }
                #endregion

                #region total

                nextCol = 0;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol + 1,
                                                   startRow + nextRow + licenseeWlba.Totals.Count - 1,
                                                   startCol + nextCol + 1).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol + 1,
                                                   startRow + nextRow + licenseeWlba.Totals.Count - 1,
                                                   startCol + nextCol + 1).Style = numberTotalFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol + 1,
                                                   startRow + nextRow + licenseeWlba.Totals.Count - 1,
                                                   startCol + nextCol + 1).Style.HorizontalAlignment =
                    HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol + 1,
                                                   startRow + nextRow + licenseeWlba.Totals.Count - 1,
                                                   startCol + nextCol + 1).Value = "Total";
                foreach (Report_LicenseeWLBA_Row total in licenseeWlba.Totals)
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 2)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Center;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = total.CurrencyName;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(total.Turnover);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.NumberFormat = "#,##0";
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(total.BetCount);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(total.Winlost);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(total.AComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(total.Company);
                    nextRow++;
                    nextCol = 0;
                }
                #endregion

                #region base total

                nextCol = 0;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol + 1,
                                                   startRow + nextRow + 2,
                                                   startCol + nextCol + 1).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol + 1,
                                                   startRow + nextRow + 2,
                                                   startCol + nextCol + 1).Style = numberBaseTotalFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol + 1,
                                                   startRow + nextRow + 2,
                                                   startCol + nextCol + 1).Style.HorizontalAlignment =
                    HorizontalAlignmentStyle.Center;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol + 1,
                                                   startRow + nextRow + 2,
                                                   startCol + nextCol + 1).Value = "Total";

                var baseIndex = 0;
                foreach (Report_LicenseeWLBA_Row total in licenseeWlba.TotalsBaseCurrency)
                {
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 2)].Style = numberBaseTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.HorizontalAlignment =
                        HorizontalAlignmentStyle.Center;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = total.CurrencyName;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberBaseTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(total.Turnover);

                    if (baseIndex == 0)
                    {
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol + 1), startRow + nextRow + 2, startCol + nextCol + 1).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol + 1), startRow + nextRow + 2, startCol + nextCol + 1).Style = numberBaseTotalFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol + 1), startRow + nextRow + 2, startCol + nextCol + 1).Style.NumberFormat = "#,##0";
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + (nextCol + 1), startRow + nextRow + 2, startCol + nextCol + 1).Value = CastAide.AsDouble(total.BetCount);

                        nextCol++;

                    }
                    else
                        nextCol++;

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberBaseTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(total.Winlost);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberBaseTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(total.AComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberBaseTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(total.Company);
                    
                    baseIndex++;
                    nextRow++;
                    nextCol = 0;
                }
                #endregion

            }

        }
        #endregion

        public void ExportExcelBAWinLoss12BetDetail(int licenseeId, int currency, int baseCurrency, string fromDate, string toDate, int drawfilter, int excludeTest, string title, int reportSeleted)
        {
            #region excel format
            const int startCol = 0;
            const int startRow = 2;
            var nextRow = 0;

            var reportName = title.Replace(" ", "");
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(string.Format("{0}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDepositReport")));

            #endregion

            #region header
            wsReport.Cells[startRow - 2, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow - 2, startCol].Value = title;
            Export2ExcelReportByLicenseeAssignHeader(ref wsReport, startRow, startCol, 0, 1);
            #endregion

            #region content
            var getReportData = new Report_LicenseeWLBA_DTO();

            getReportData = (Report_LicenseeWLBA_DTO)AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                           _keySelReportByLicenseeDetail_AgV2);
            getReportData = getReportData ??
                            SelWinLossReportByLicenseeDetailAgent(licenseeId, currency, baseCurrency, fromDate, toDate,
                                                                  drawfilter, excludeTest, 0, 100, 0);

            if (getReportData != null && getReportData.Items != null)
            {
                Export2ExcelReportByLicenseeAssignContent("", ref wsReport, startRow, startCol, ref nextRow, false, false,
                                                          getReportData.Items, 1);
                Export2ExcelReportByLicenseeAssignContent("Total ", ref wsReport, startRow, startCol, ref nextRow, true, false,
                                                          getReportData.Totals, 1);
                Export2ExcelReportByLicenseeAssignContent("Grand Total ", ref wsReport, startRow, startCol, ref nextRow, false, true,
                                                          getReportData.TotalsBaseCurrency, 1);

            }

            #endregion

            #region response file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);
            #endregion
        }

        public void ExportExcelBAWinLossDetail(int licenseeId, int currency, int baseCurrency, string fromDate, int custId, string toDate, int drawfilter, int excludeTest, string title, int reportSeleted, int reportType)
        {
            #region excel format
            const int startCol = 0;
            const int startRow = 2;
            var nextRow = 0;

            var reportName = title.Replace(" ", "");
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add(string.Format("{0}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblDepositReport")));

            #endregion

            #region header
            wsReport.Cells[startRow - 2, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow - 2, startCol].Value = title;
            Export2ExcelReportByLicenseeAssignHeader(ref wsReport, startRow, startCol, 0, 1);
            #endregion

            #region content
            var getReportData = new Report_LicenseeWLBA_DTO();

            getReportData = (Report_LicenseeWLBA_DTO)AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                           _keySelReportByLicenseeDetailV2);
            getReportData = getReportData ??
                            SelWinLossReportByLicenseeDetail(licenseeId, currency, baseCurrency, custId, fromDate,
                                                             toDate, drawfilter, excludeTest, reportType, 0, 100, 0);

            if (getReportData != null && getReportData.Items != null)
            {
                Export2ExcelReportByLicenseeAssignContent("", ref wsReport, startRow, startCol, ref nextRow, false, false,
                                                          getReportData.Items, 1);
                Export2ExcelReportByLicenseeAssignContent("Total ", ref wsReport, startRow, startCol, ref nextRow, true, false,
                                                          getReportData.Totals, 1);
                Export2ExcelReportByLicenseeAssignContent("Grand Total ", ref wsReport, startRow, startCol, ref nextRow, false, true,
                                                          getReportData.TotalsBaseCurrency, 1);

            }

            #endregion

            #region response file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);
            #endregion
        }

        #endregion


        public void Export2ExcelReportByCommissionLevel(string reportTitle, int licenseeId, int currencyId, string fromDate, string toDate, int drawFilter, bool excludeTest)
        {
            const int startCol = 0;
            const int startRow = 2;
            var nextRow = 1;
            var nextCol = 0;
            var excelFile = new ExcelFile();
            var wsReport = excelFile.Worksheets.Add("WinLossByCommLevel");
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 13).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 13).Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 13).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + 13).Value = reportTitle;
            //wsReport.Cells[startRow - 2, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
            //wsReport.Cells[startRow - 2, startCol].Value = reportTitle;
            ExportToExcelHeaderCommissionLevel(ref wsReport, startRow, startCol);

            var getReportData = (LicenseeWinLossCommDTO)SessionExtension.GetSession(HttpContext.Current.Session, _baWinLossByCommLevelSessionKey);
            getReportData = getReportData ?? LicenseeWinLossByCommLevel(licenseeId, currencyId, excludeTest, fromDate, toDate, drawFilter);
            if (getReportData != null && getReportData.Items.Count > 0)
            {
                ExportToExcelContentCommissionLevel(ref wsReport, startRow, startCol,ref nextRow, getReportData);
            }
            #region response file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "Application/x-msexcel";
            response.AddHeader("Content-Disposition", "attachment; filename=WLByCommLevel-" + fromDate + "-" + toDate + ".xls");
            excelFile.SaveXls(response.OutputStream);

            #endregion
                                                                                       
        }

        private static void ExportToExcelHeaderCommissionLevel(ref ExcelWorksheet wsReport, int startRow, int startCol)
        {
            var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var bgHeaderNormalFormatMember = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(224, 244, 170));
            var bgHeaderNormalFormatDate = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(204, 255, 255));
            var dateText = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Date");
            var currencyName = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Currency");
            var Discount = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CommLevel");
            var betType = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "typebet");
            var totalTrans = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTotalTrans");
            var Turnover = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Turnover");
            var customerWL = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CustomerWL");
            var licenseeComm = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "licenseeComm");
            var AmtDueto = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "AmtDue"); ;

            var nextCol = 0;
            var nextRow = 0;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Value = dateText;

            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = currencyName;

            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = Discount;

            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = betType;


            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = totalTrans;

            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow - 1, startCol + (nextCol + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow - 1, startCol + (nextCol + 3)).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow - 1, startCol + (nextCol + 3)).Value = "Base EURO";

            wsReport.Cells[startRow, startCol + nextCol].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + nextCol].Value = Turnover;
            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + nextCol].Value = customerWL;
            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + nextCol].Value = licenseeComm;
            wsReport.Cells[startRow, startCol + (nextCol += 1)].Style = bgHeaderNormalFormat;
            wsReport.Cells[startRow, startCol + nextCol].Value = AmtDueto;

            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = Turnover;

            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = customerWL;


            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = licenseeComm;

            nextCol++;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Style = bgHeaderNormalFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol), startRow, startCol + (nextCol)).Value = AmtDueto;
           

        }

        private static void ExportToExcelContentCommissionLevel(ref ExcelWorksheet wsReport, int startRow, int startCol, ref int nextRow, LicenseeWinLossCommDTO listCommLevel) 
        {
            var bgColorComm = new Core.Helper.ExcelGemboxUtils().GetStyleText(255, 255, 255);
            var bgColorTrans = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(255, 255, 255), false, false);
            var bgColorNormal = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(255, 255, 255), false, false);
            var bgColorSubTotal = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(221, 246, 207), true, false);
            var bgColorTotal = new Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(250, 191, 143),
                                                                                    true, false); 

            foreach (LicenseeWinLossCommBO item in listCommLevel.Items)
            {
                var nextCol = 0;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol , startRow + nextRow + item.TotalChilds - 1, startCol + nextCol).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow + item.TotalChilds - 1, startCol + nextCol).Style = bgColorNormal;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow + item.TotalChilds - 1, startCol + nextCol).Value = item.ReportDate;

                var listItemCurr = item.Items;
                nextCol++;
                foreach (LicenseeWinLossCurrency itemcurr in listItemCurr)
                {
                  
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow + itemcurr.Items.Count - 1, startCol + nextCol).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow + itemcurr.Items.Count - 1, startCol + nextCol).Style = bgColorNormal;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow + itemcurr.Items.Count - 1, startCol + nextCol).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol + nextCol, startRow + nextRow + itemcurr.Items.Count - 1, startCol + nextCol).Value = itemcurr.Currency;
                    
                        var listitemComm = itemcurr.Items;
                        foreach (LicenseeWinLossCommLevel itemComm in listitemComm)
                        {
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorComm;
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value =  CastAide.AsDecimal(itemComm.Comm);
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorNormal;
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = itemComm.Bettype;
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorTrans;
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(itemComm.TotalTrans);
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorNormal;
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDecimal(itemComm.BaseTurnover);
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorNormal;
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDecimal(itemComm.BaseCustWinLoss);
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorNormal;
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDecimal(itemComm.BaseLinceseeComm);
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorNormal;
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDecimal(itemComm.BaseAmountDue);
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorNormal;
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDecimal(itemComm.Turnover);
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorNormal;
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDecimal(itemComm.CustWinLoss);
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorNormal;
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDecimal(itemComm.LicenseeComm);
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorNormal;
                            wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Value = CastAide.AsDecimal(itemComm.AmountDue);
                            nextRow++;
                            nextCol = 1;
                        }
                       
                    
                }
                nextCol = 0;
                //assign Sub total
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol + 3)).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol + 3)).Style = bgColorSubTotal;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol + 3)).Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol + 3)).Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol += 3)).Value = "Sub Total";

                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorTrans;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style.FillPattern.SetSolid(Color.FromArgb(221, 246, 207));
                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style.Font.Weight = ExcelFont.BoldWeight;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDouble(item.SubTotal.TotalTrans);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorSubTotal;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDecimal(item.SubTotal.BaseTurnover);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorSubTotal;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDecimal(item.SubTotal.BaseCustWinLoss);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorSubTotal;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Value = CastAide.AsDecimal(item.SubTotal.BaseLinceseeComm);

                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Style = bgColorSubTotal;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol + 1)].Value = CastAide.AsDecimal(item.SubTotal.BaseAmountDue);

                nextRow++;

            }
            //Assign Total
            var nextColTotal = 0;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextColTotal + 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextColTotal + 3)).Style = bgColorTotal;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextColTotal + 3)).Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextColTotal += 3)).Value = "Total";

            wsReport.Cells[startRow + nextRow, startCol + (nextColTotal + 1)].Style = bgColorTrans;
            wsReport.Cells[startRow + nextRow, startCol + (nextColTotal + 1)].Style.FillPattern.SetSolid(Color.FromArgb(250, 191, 143));
            wsReport.Cells[startRow + nextRow, startCol + (nextColTotal + 1)].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow + nextRow, startCol + (nextColTotal += 1)].Value = CastAide.AsDouble(listCommLevel.Total.TotalTrans);

            wsReport.Cells[startRow + nextRow, startCol + (nextColTotal + 1)].Style = bgColorTotal;
            wsReport.Cells[startRow + nextRow, startCol + (nextColTotal += 1)].Value = CastAide.AsDecimal(listCommLevel.Total.BaseTurnover);

            wsReport.Cells[startRow + nextRow, startCol + (nextColTotal + 1)].Style = bgColorTotal;
            wsReport.Cells[startRow + nextRow, startCol + (nextColTotal += 1)].Value = CastAide.AsDecimal(listCommLevel.Total.BaseCustWinLoss);

            wsReport.Cells[startRow + nextRow, startCol + (nextColTotal + 1)].Style = bgColorTotal;
            wsReport.Cells[startRow + nextRow, startCol + (nextColTotal += 1)].Value = CastAide.AsDecimal(listCommLevel.Total.BaseLinceseeComm);

            wsReport.Cells[startRow + nextRow, startCol + (nextColTotal + 1)].Style = bgColorTotal;
            wsReport.Cells[startRow + nextRow, startCol + (nextColTotal + 1)].Value = CastAide.AsDecimal(listCommLevel.Total.BaseAmountDue);

        }
   
    }
}

