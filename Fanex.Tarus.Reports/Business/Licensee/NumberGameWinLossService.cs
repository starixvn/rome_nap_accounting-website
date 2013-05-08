// -----------------------------------------------------------------------
// <copyright file="NumberGameService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.Licensee
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web;
    using DNAide.Core;
    using DNAide.Utils;
    using Entities.Licensee.NumberGameWinLoss;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Fanex.Tarus.Reports.DataAccess.Licensee;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Infrastructure.Helper;
    using System.Drawing;
    using Fanex.Tarus.Reports.Core.Configurations;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NumberGameWinLossService: INumberGameWinLossService
    {
        INumberGameWinLossAccess _numberGameWinLossAccess = null;
        protected string _licenseeNumberGameWinLossSessionKey = "licensee.numbergamewinloss.sessionkey";
        public NumberGameWinLossService() : this(null)
        { 
        }
        public NumberGameWinLossService(INumberGameWinLossAccess numberGameWinLossAccesss) 
        {
            _numberGameWinLossAccess = numberGameWinLossAccesss ?? new NumberGameWinLossAccess();
        }

        public Licensees SelAllLicensees()
        {
            var result = new Licensees();

            try
            {
                var allLicensee = _numberGameWinLossAccess.SelAllLicensees();
                if (allLicensee != null && allLicensee.Tables != null && allLicensee.Tables.Count > 0 && allLicensee.Tables[0].Rows.Count > 0)
                {
                    result.Licensee = new List<LicenseeDetail>();
                    var tbLicensee = allLicensee.Tables[0];
                    foreach (DataRow item in tbLicensee.Rows)
                    {
                        var licenseeId = CastAide.AsInt32(item["licenseeid"]);
                        if (licenseeId != 5) //not include demo licensee
                        {
                            var licenseeDetail = new LicenseeDetail()
                            {
                                ResellerId = CastAide.AsInt32(item["resellerid"]),
                                ResellerName = CastAide.AsString(item["resellername"]),
                                ResellerType = CastAide.AsInt32(item["resellertype"]),
                                LicenseeId = licenseeId,
                                LicenseeName = CastAide.AsString(item["licenseename"])
                            };

                            result.Licensee.Add(licenseeDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public NumberGameWinLoss SelNumberGameWinLoss(string normalLicensee, string operators, string fromDate, string toDate)
        {
            var result = new NumberGameWinLoss();

            try
            {                
                AppSession.AddSession(HttpContext.Current.Session, _licenseeNumberGameWinLossSessionKey, null);
                var dsLicenseesWinLoss = _numberGameWinLossAccess.SelNumberGameWinLoss(normalLicensee, operators, fromDate, toDate);
                if (dsLicenseesWinLoss != null && dsLicenseesWinLoss.Tables.Count > 0 && dsLicenseesWinLoss.Tables[0].Rows.Count > 0)
                {
                    var licenseeConfigSection = LicenseeReportSection.Instance.LicenseesGroup;
                    if (licenseeConfigSection != null && licenseeConfigSection.Count > 0)
                    {
                        var winloss = dsLicenseesWinLoss.Tables[0];
                        result.LicenseesWinLoss = new List<NumberGameWinLossByGroupLicensees>();
                        double grandTotalLupineRevenue = 0, grandTotalAmountDueTo = 0;
                        
                        foreach (LicenseeReportElement resellerConfig in licenseeConfigSection)
                        {
                            var resellerIds = resellerConfig.ResellerIds.Split(',');
                            var licenseeWinLoss = new NumberGameWinLossByGroupLicensees();
                            licenseeWinLoss.Licensees = new List<NumberGameWinLossByLicensee>();
                            licenseeWinLoss.RevenueInfo = new NumberGameWinLossDetail();
                            var distributorInfo = string.Empty;
                            var lupineInfo = string.Empty;
                            double currentDistributorChargeRate = 0, currentLupineChargeRate = 0, currentOneWorksChargeRate = 0;
                            foreach (string resellerId in resellerIds)
                            {
                                var licenseeWinLossDetail = new NumberGameWinLossByLicensee();
                                licenseeWinLossDetail.LicenseeWinLossDetail = new List<NumberGameWinLossDetail>();
                                licenseeWinLossDetail.WinLossTotal = new NumberGameWinLossDetail();
                                double totalDistributorRevenue = 0, totalLupineRevenue = 0, totalAmountDueTo = 0, totalTurnover = 0, totalGrosWin = 0;
                                var totalTransaction = 0;
                                double oldDistributorChargeRate = 0, oldLupineChargeRate = 0, oldOneWorksChargeRate = 0;
                                var selResellerData = (from q in winloss.AsEnumerable().Cast<DataRow>() where q.Field<int>("resellerid") == CastAide.AsInt32(resellerId) select q).ToList();
                                if (selResellerData != null && selResellerData.Count > 0)
                                {
                                    var currentResellerType = 0;
                                    var currentResellerName = string.Empty;
                                    
                                    foreach (DataRow item in selResellerData)
                                    {                                        
                                        currentResellerType = CastAide.AsInt32(item["resellertype"]);
                                        currentResellerName = CastAide.AsString(item["resellername"]);
                                        currentOneWorksChargeRate = CastAide.AsDouble(item["oneworkschargerate"]);
                                        currentDistributorChargeRate = CastAide.AsDouble(item["distributorschargerate"]);
                                        currentLupineChargeRate = CastAide.AsDouble(item["lupinechargerate"]);

                                        if ((oldDistributorChargeRate != 0 && oldLupineChargeRate !=0 && oldOneWorksChargeRate != 0) && 
                                            (oldDistributorChargeRate != currentDistributorChargeRate | oldLupineChargeRate != currentLupineChargeRate | oldOneWorksChargeRate != currentOneWorksChargeRate))
                                        {
                                            #region assign data for each reseller
                                            var turnover = NumberFormat.DecFormat(totalTurnover, 2);
                                            var totalTrans = NumberFormat.DecFormat(totalTransaction, 0);
                                            var grosswin = NumberFormat.DecFormat(totalGrosWin, 2);
                                            lupineInfo = string.Format("{0}% gross win", (oldLupineChargeRate * 100));
                                            var totalDistributor = "-";
                                            if (currentResellerType == 1)
                                            {

                                                totalDistributor = NumberFormat.DecFormat(totalDistributorRevenue, 2);
                                                turnover = string.Empty;
                                                totalTrans = string.Empty;
                                                grosswin = string.Empty;

                                                distributorInfo = string.Format("{0}% gross win", (oldDistributorChargeRate * 100));
                                                lupineInfo = string.Format("{0}% distributor's revenue", (oldLupineChargeRate * 100));
                                            }
                                            licenseeWinLossDetail.WinLossTotal = new NumberGameWinLossDetail()
                                            {
                                                ResellerType = currentResellerType,
                                                Licensee = currentResellerName,
                                                Turnover = turnover,
                                                TotalTransaction = totalTrans,
                                                GrossWin = grosswin,
                                                DistributorRevenue = totalDistributor,
                                                LupineRevenue = NumberFormat.DecFormat(totalLupineRevenue, 2),
                                                AmountDueToOneWorks = NumberFormat.DecFormat(totalAmountDueTo, 2)
                                            };

                                            licenseeWinLoss.Licensees.Add(licenseeWinLossDetail);
                                            #endregion

                                            #region revenue info - group reseller type
                                            licenseeWinLoss.RevenueInfo = new NumberGameWinLossDetail()
                                            {
                                                DistributorRevenue = distributorInfo,
                                                LupineRevenue = lupineInfo,
                                                AmountDueToOneWorks = string.Format("{0}% Lupine's revenue", (oldOneWorksChargeRate * 100))
                                            };

                                            result.LicenseesWinLoss.Add(licenseeWinLoss);
                                            #endregion

                                            #region reset values
                                            licenseeWinLossDetail = new NumberGameWinLossByLicensee();
                                            licenseeWinLossDetail.LicenseeWinLossDetail = new List<NumberGameWinLossDetail>();
                                            licenseeWinLossDetail.WinLossTotal = new NumberGameWinLossDetail();
                                            licenseeWinLoss = new NumberGameWinLossByGroupLicensees();
                                            licenseeWinLoss.Licensees = new List<NumberGameWinLossByLicensee>();
                                            licenseeWinLoss.RevenueInfo = new NumberGameWinLossDetail();

                                            totalDistributorRevenue = 0;
                                            totalLupineRevenue = 0;
                                            totalAmountDueTo = 0;
                                            totalTurnover = 0;
                                            totalTransaction = 0;
                                            totalGrosWin = 0;
                                            oldDistributorChargeRate = 0;
                                            oldLupineChargeRate = 0;
                                            oldOneWorksChargeRate = 0;
                                            #endregion
                                        }

                                        #region assign - distributor licensees detail
                                        if (currentResellerType == 1)
                                        {
                                            licenseeWinLossDetail.LicenseeWinLossDetail.Add(new NumberGameWinLossDetail()
                                            {
                                                Licensee = CastAide.AsString(item["licenseename"]),
                                                Turnover = NumberFormat.DecFormat(item["turnover"], 2),
                                                TotalTransaction = NumberFormat.DecFormat(item["betcount"], 0),
                                                GrossWin = NumberFormat.DecFormat(item["grosswinloss"], 2),
                                                DistributorRevenue = NumberFormat.DecFormat(item["distributorrevenue"], 2),
                                                LupineRevenue = NumberFormat.DecFormat(item["lupinerevenue"], 2),
                                                AmountDueToOneWorks = NumberFormat.DecFormat(item["oneworksrevenue"], 2)

                                            });
                                        }
                                        #endregion

                                        #region assign - Total & grand total
                                        totalDistributorRevenue += CastAide.AsDouble(item["distributorrevenue"]);
                                        totalLupineRevenue += CastAide.AsDouble(item["lupinerevenue"]);
                                        totalAmountDueTo += CastAide.AsDouble(item["oneworksrevenue"]);
                                        totalTurnover += CastAide.AsDouble(item["turnover"]);
                                        totalTransaction += CastAide.AsInt32(item["betcount"]);
                                        totalGrosWin += CastAide.AsDouble(item["grosswinloss"]);

                                        grandTotalLupineRevenue += CastAide.AsDouble(item["lupinerevenue"]);
                                        grandTotalAmountDueTo += CastAide.AsDouble(item["oneworksrevenue"]);
                                        #endregion

                                        oldDistributorChargeRate = currentDistributorChargeRate;
                                        oldLupineChargeRate = currentLupineChargeRate;
                                        oldOneWorksChargeRate = currentOneWorksChargeRate;
                                    }

                                    #region assign data for each reseller
                                    var currentTurnover = NumberFormat.DecFormat(totalTurnover, 2);
                                    var currentTotalTrans = NumberFormat.DecFormat(totalTransaction, 0);
                                    var currentGrosswin = NumberFormat.DecFormat(totalGrosWin, 2);
                                    lupineInfo = string.Format("{0}% gross win", (currentLupineChargeRate * 100));
                                    var currentTotalDistributor = "-";
                                    if (currentResellerType == 1)
                                    {
                                        currentTotalDistributor = NumberFormat.DecFormat(totalDistributorRevenue, 2);
                                        currentTurnover = string.Empty;
                                        currentTotalTrans = string.Empty;
                                        currentGrosswin = string.Empty;

                                        distributorInfo = string.Format("{0}% gross win", (currentDistributorChargeRate * 100));
                                        lupineInfo = string.Format("{0}% distributor's revenue", (currentLupineChargeRate * 100));
                                    }
                                    licenseeWinLossDetail.WinLossTotal = new NumberGameWinLossDetail()
                                    {
                                        ResellerType = currentResellerType,
                                        Licensee = currentResellerName,
                                        Turnover = currentTurnover,
                                        TotalTransaction = currentTotalTrans,
                                        GrossWin = currentGrosswin,
                                        DistributorRevenue = currentTotalDistributor,
                                        LupineRevenue = NumberFormat.DecFormat(totalLupineRevenue, 2),
                                        AmountDueToOneWorks = NumberFormat.DecFormat(totalAmountDueTo, 2)
                                    };

                                    licenseeWinLoss.Licensees.Add(licenseeWinLossDetail);
                                    #endregion
                                }
                            }

                            #region revenue info - group reseller type
                            if (licenseeWinLoss.Licensees != null && licenseeWinLoss.Licensees.Count > 0)
                            {
                                licenseeWinLoss.RevenueInfo = new NumberGameWinLossDetail()
                                {
                                    DistributorRevenue = distributorInfo,
                                    LupineRevenue = lupineInfo,
                                    AmountDueToOneWorks = string.Format("{0}% Lupine's revenue", (currentOneWorksChargeRate * 100))
                                };

                                result.LicenseesWinLoss.Add(licenseeWinLoss);
                            }
                            #endregion

                        }

                        #region grand total
                        result.GrandTotal = new NumberGameWinLossDetail()
                        {
                            LupineRevenue = NumberFormat.DecFormat(grandTotalLupineRevenue, 2),
                            AmountDueToOneWorks = NumberFormat.DecFormat(grandTotalAmountDueTo, 2)
                        };
                        #endregion
                        AppSession.AddSession(HttpContext.Current.Session, _licenseeNumberGameWinLossSessionKey, result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }
        
        public void ExportExcelNumberGameWinLossReport(string normalLicensee, string operators, string fromDate, string toDate, string reportTitle)
        {
            try
            {
                #region local varialbles
                var startCol = 0;
                var startRow = 2;
                var reportName = "Licensee-NumberGameWinLoss";
                var excelFile = new ExcelFile();
                var getSession = (NumberGameWinLoss)AppSession.GetSession(HttpContext.Current.Session, _licenseeNumberGameWinLossSessionKey);
                getSession = getSession ?? SelNumberGameWinLoss(normalLicensee, operators, fromDate, toDate);
                var wsReport = excelFile.Worksheets.Add(string.Format("NumberGameWinLoss"));
                #endregion

                #region header
                ExportExcelNumberGameWinLossReportAssignHeader(startRow, startCol, ref wsReport, reportTitle);
                #endregion

                #region content
                if (getSession != null && getSession.LicenseesWinLoss != null && getSession.LicenseesWinLoss.Count > 0)
                {
                    int nextRow = 0;
                    var licenseesWinLoss = getSession.LicenseesWinLoss;
                    var licenseeIndex = 0;
                    var totalLicenseeGroup = licenseesWinLoss.Count;
                    foreach (NumberGameWinLossByGroupLicensees group in licenseesWinLoss)
                    {
                        ExportExcelNumberGameWinLossReportAssignRevenueInfoRow(startRow, startCol, ref nextRow, ref wsReport, group.RevenueInfo);
                        foreach (NumberGameWinLossByLicensee licensee in group.Licensees)
                        {
                            ExportExcelNumberGameWinLossReportAssignTotalRow(startRow, startCol, ref nextRow, ref wsReport, licensee.WinLossTotal);
                            foreach (NumberGameWinLossDetail item in licensee.LicenseeWinLossDetail)
                            {
                                ExportExcelNumberGameWinLossReportAssignBody(startRow, startCol, ref nextRow, ref wsReport, item);
                            }
                        }

                        if (licenseeIndex < totalLicenseeGroup -1)
                        {
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).Merged = true;
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).Style.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).Value = string.Empty;
                            nextRow++;
                        }
                        licenseeIndex++;
                    }
                    ExportExcelNumberGameWinLossReportAssignGrandTotalRow(startRow, startCol, ref nextRow, ref wsReport, getSession.GrandTotal);

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
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }
        #region export excel - private functions
        private void ExportExcelNumberGameWinLossReportAssignHeader(int startRow, int startCol, ref ExcelWorksheet wsReport, string title)
        {
            var bgHeaderFormat = new ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            wsReport.Rows[startRow -1].Height = 255 * 2;
            wsReport.Cells[startRow - 1, startCol].Style = bgHeaderFormat;            
            wsReport.Cells[startRow - 1, startCol].Value = "Licensee";
            wsReport.Columns[startCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;            
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Turnover";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Total Transaction";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Gross Win";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Distributor's Revenue";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Lupine's Revenue";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 25;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Amount Due To ONEWorks";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 25;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Value = title;
        }

        private void ExportExcelNumberGameWinLossReportAssignBody(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport, 
            NumberGameWinLossDetail winloss)
        {
            var cellStyle = GetDefaultTextFormat(Color.FromArgb(250, 227, 190));
            cellStyle.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = cellStyle;            
            wsReport.Cells[startRow + nextRow, startCol].Value = winloss.Licensee;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.Turnover;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.TotalTransaction;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.GrossWin;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.DistributorRevenue;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.LupineRevenue;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.AmountDueToOneWorks;
            nextRow++;
        }

        private void ExportExcelNumberGameWinLossReportAssignTotalRow(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport,
            NumberGameWinLossDetail winloss)
        {
            var cellStyle = GetDefaultTextFormat(Color.FromArgb(255, 255, 255));
            cellStyle.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            wsReport.Cells[startRow + nextRow, startCol].Value = winloss.Licensee;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.Turnover;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.TotalTransaction;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.GrossWin;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.DistributorRevenue;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.LupineRevenue;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.AmountDueToOneWorks;
            nextRow++;
        }

        private void ExportExcelNumberGameWinLossReportAssignRevenueInfoRow(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport,
            NumberGameWinLossDetail winloss)
        {
            var cellStyle = GetDefaultTextFormat(Color.FromArgb(255, 255, 255));
            cellStyle.Font.Weight = ExcelFont.BoldWeight;
            cellStyle.Font.Italic = true;
            cellStyle.Font.Color = Color.FromArgb(224, 83, 7);
            cellStyle.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            var nextCol = 0;            
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol += 3)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol)).Style = cellStyle;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol)).Value = string.Empty;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.DistributorRevenue;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.LupineRevenue;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.AmountDueToOneWorks;
            nextRow++;
        }

        private void ExportExcelNumberGameWinLossReportAssignGrandTotalRow(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport,
            NumberGameWinLossDetail winloss)
        {
            var cellStyle = GetDefaultTextFormat(Color.FromArgb(184, 120, 1));
            cellStyle.Font.Weight = ExcelFont.BoldWeight;
            cellStyle.Font.Color = Color.FromArgb(255, 255, 255);
            cellStyle.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            var nextCol = 0;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol += 4)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol)).Style = cellStyle;
            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol)).Value = "Total";
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.LupineRevenue;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.AmountDueToOneWorks;
            nextRow++;
        }

        public CellStyle GetDefaultTextFormat(Color color)
        {
            var celltitle = new CellStyle();            
            celltitle.FillPattern.SetSolid(color);
            celltitle.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            celltitle.Font.Color = Color.Black;
            celltitle.Font.Name = "Tahoma";
            celltitle.Font.Size = 160;
            celltitle.WrapText = true;
            return celltitle;
        }
        #endregion
    }
}
