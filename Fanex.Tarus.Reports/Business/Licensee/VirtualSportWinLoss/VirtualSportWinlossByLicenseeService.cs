
namespace Fanex.Tarus.Reports.Business.Licensee.VirtualSportWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Web;
    using DNAide.Core;
    using DNAide.Utils;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Fanex.Tarus.Infrastructure.Helper;
    using Fanex.Tarus.Reports.Core.Configurations;
    using Fanex.Tarus.Reports.DataAccess.Licensee.VirtualSportWinLoss;
    using Fanex.Tarus.Reports.Entities.Licensee.VirtualSportWinLoss;

    public class VirtualSportWinlossByLicenseeService: IVirtualSportWinlossByLicenseeService
    {
        IVirtualSportsWinlossByLicenseeAccess _virtualSportsWinLossAccess = null;
        protected string _licenseeVirtualSportsWinLossSessionKey = "licensee.virtualsportswinloss.sessionkey";

        public VirtualSportWinlossByLicenseeService(): this(null)
        { 
        }

        public VirtualSportWinlossByLicenseeService(IVirtualSportsWinlossByLicenseeAccess virtualSportsWinLossAccess)
        {
            _virtualSportsWinLossAccess = virtualSportsWinLossAccess ?? new VirtualSportWinlossByLicenseeAccess();
        }

        public Licensees SelAllLicensees(string productCode, bool isExcludeBA, bool isExcludeTest)
        {
            var result = new Licensees();

            try
            {
                var dtbLicensee = _virtualSportsWinLossAccess.GetReseller(productCode, isExcludeBA, isExcludeTest);
                if (dtbLicensee != null  && dtbLicensee.Rows.Count > 0)
                {
                    result.Licensee = new List<LicenseeDetail>();
                    foreach (DataRow item in dtbLicensee.Rows)
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

        public List<Sport> SelSportType()
        {
            var sportTypeList = new List<Sport>();
            DataTable dtb = _virtualSportsWinLossAccess.GetSportType();
            foreach (DataRow row in dtb.Rows)
            {
                sportTypeList.Add(new Sport()
                {
                    SportId = (row["sporttype"]).ToString(),
                    SportName = row["sportname"].ToString()
                });
            }
            return sportTypeList;
        }

        public VirtualSportsWinLoss SelVirtualSportsWinLoss(string normalLicensee, string operators, DateTime fromDate, DateTime toDate, string sportList)
        {
            var result = new VirtualSportsWinLoss();

            try
            {
                AppSession.AddSession(HttpContext.Current.Session, _licenseeVirtualSportsWinLossSessionKey, null);
                var dtbLicenseesWinLoss = _virtualSportsWinLossAccess.GetVirtualSportWinloss(normalLicensee, operators, fromDate, toDate, sportList);
                if (dtbLicenseesWinLoss != null  && dtbLicenseesWinLoss.Rows.Count > 0)
                {
                    var licenseeConfigSection = LicenseeReportSection.Instance.LicenseesGroup;
                    if (licenseeConfigSection != null && licenseeConfigSection.Count > 0)
                    {
                        result.LicenseesWinLoss = new List<VirtualSportsWinLossByGroupLicensees>();
                        double grandTotalTransaction = 0, grandTotalTurnover = 0, grandTotalCustomerWL = 0, grandTotalGrossIncome = 0, 
                            grandTotalAmountDueToInspired = 0, grandTotalAmountDueToOneWorks = 0, grandTotalDistributorRevenue = 0;

                        foreach (LicenseeReportElement resellerConfig in licenseeConfigSection)
                        {
                            var resellerIds = resellerConfig.ResellerIds.Split(',');
                            var licenseeWinLoss = new VirtualSportsWinLossByGroupLicensees();
                            licenseeWinLoss.Licensees = new List<VirtualSportsWinLossByLicensee>();
                            licenseeWinLoss.RevenueInfo = new VirtualSportsWinLossDetail();
                            var grossIncomeInfo = string.Empty;
                            var InspiredInfo = string.Empty;
                            var distributorInfo = string.Empty;
                            double currentGrossIncomeChargeRate = 0, currentInspiredChargeRate = 0, currentOneWorksChargeRate = 0,
                                currentDistributorChargeRate = 0;
                            foreach (string resellerId in resellerIds)
                            {
                                var licenseeWinLossDetail = new VirtualSportsWinLossByLicensee();
                                licenseeWinLossDetail.LicenseeWinLossDetail = new List<VirtualSportsWinLossDetail>();
                                licenseeWinLossDetail.WinLossTotal = new VirtualSportsWinLossDetail();
                                double totalGrossIncome = 0, totalAmountDueToInspired = 0, totalAmountDueToOneWorks = 0, 
                                    totalTurnover = 0, totalCustomerWL = 0, totalDistributorRevenue = 0;
                                var totalTransaction = 0;
                                double oldGrossIncomeChargeRate = 0, oldInspiredChargeRate = 0, oldOneWorksChargeRate = 0,
                                    oldDistributorChargeRate = 0;
                                var selResellerData = (from q in dtbLicenseesWinLoss.AsEnumerable().Cast<DataRow>() where q.Field<int>("resellerid") == CastAide.AsInt32(resellerId) select q).ToList();
                                if (selResellerData != null && selResellerData.Count > 0)
                                {
                                    var currentResellerType = 0;
                                    var currentResellerName = string.Empty;

                                    foreach (DataRow item in selResellerData)
                                    {
                                        currentResellerType = CastAide.AsInt32(item["resellertype"]);
                                        currentResellerName = CastAide.AsString(item["resellername"]);
                                        currentOneWorksChargeRate = CastAide.AsDouble(item["oneworkschargerate"]);
                                        currentInspiredChargeRate = CastAide.AsDouble(item["inspiredchargerate"]);
                                        currentDistributorChargeRate = CastAide.AsDouble(item["distributorschargerate"]);

                                        if ((currentInspiredChargeRate != 0 && oldOneWorksChargeRate != 0) &&
                                            (oldInspiredChargeRate != currentInspiredChargeRate | 
                                            oldOneWorksChargeRate != currentOneWorksChargeRate |
                                            oldDistributorChargeRate != currentDistributorChargeRate))
                                        {
                                            #region assign data for each reseller
                                            var turnover = NumberFormat.DecFormat(totalTurnover, 2);
                                            var totalTrans = NumberFormat.DecFormat(totalTransaction, 0);
                                            var grossWin = NumberFormat.DecFormat(totalCustomerWL, 2);
                                            var distributorRevenue = NumberFormat.DecFormat(totalDistributorRevenue, 2);
                                            InspiredInfo = string.Format("{0}% gross win", (oldInspiredChargeRate * 100));
                                            var _totalGrossIncome = NumberFormat.DecFormat(totalGrossIncome, 2);
                                            grossIncomeInfo = string.Format("{0}% gross win", (oldOneWorksChargeRate * 100));
                                            distributorInfo = currentResellerType > 1 ? "" : string.Format("{0}% gross win", (oldDistributorChargeRate * 100));
                                            
                                            licenseeWinLossDetail.WinLossTotal = new VirtualSportsWinLossDetail()
                                            {
                                                Licensee = currentResellerName,
                                                Turnover = turnover,
                                                GrossWin = grossWin,
                                                DistributorRevenue = currentResellerType > 1? "-" : NumberFormat.DecFormat(totalDistributorRevenue, 2),
                                                ONEWorksIncome = NumberFormat.DecFormat(totalGrossIncome, 2),
                                                AmountDueToInspired = NumberFormat.DecFormat(totalAmountDueToInspired, 2),
                                                AmountDueToOneWorks = NumberFormat.DecFormat(totalAmountDueToOneWorks, 2)
                                            };

                                            licenseeWinLoss.Licensees.Add(licenseeWinLossDetail);
                                            #endregion

                                            #region revenue info - group reseller type
                                            licenseeWinLoss.RevenueInfo = new VirtualSportsWinLossDetail()
                                            {
                                                ONEWorksIncome = grossIncomeInfo,
                                                AmountDueToInspired = InspiredInfo,
                                                DistributorRevenue = distributorInfo
                                            };

                                            result.LicenseesWinLoss.Add(licenseeWinLoss);
                                            #endregion

                                            #region reset values
                                            licenseeWinLossDetail = new VirtualSportsWinLossByLicensee();
                                            licenseeWinLossDetail.LicenseeWinLossDetail = new List<VirtualSportsWinLossDetail>();
                                            licenseeWinLossDetail.WinLossTotal = new VirtualSportsWinLossDetail();
                                            licenseeWinLoss = new VirtualSportsWinLossByGroupLicensees();
                                            licenseeWinLoss.Licensees = new List<VirtualSportsWinLossByLicensee>();
                                            licenseeWinLoss.RevenueInfo = new VirtualSportsWinLossDetail();

                                            totalGrossIncome = 0;
                                            totalAmountDueToInspired = 0;
                                            totalAmountDueToOneWorks = 0;
                                            totalTurnover = 0;
                                            totalTransaction = 0;
                                            totalCustomerWL = 0;
                                            oldGrossIncomeChargeRate = 0;
                                            oldInspiredChargeRate = 0;
                                            oldOneWorksChargeRate = 0;
                                            oldDistributorChargeRate = 0;
                                            #endregion
                                        }

                                        #region assign - distributor licensees detail
                                        if (currentResellerType == 1)
                                        {
                                            licenseeWinLossDetail.LicenseeWinLossDetail.Add(new VirtualSportsWinLossDetail()
                                            {
                                                Licensee = CastAide.AsString(item["licenseename"]),
                                                Turnover = NumberFormat.DecFormat(item["turnover"], 2),
                                                TotalTransaction = NumberFormat.DecFormat(item["betcount"], 0),
                                                GrossWin = NumberFormat.DecFormat(item["grosswinloss"], 2),
                                                DistributorRevenue = NumberFormat.DecFormat(item["distributorrevenue"], 2),
                                                ONEWorksIncome = NumberFormat.DecFormat(item["oneworksincome"], 2),
                                                AmountDueToInspired = NumberFormat.DecFormat(item["amtduetoinspired"], 2),
                                                AmountDueToOneWorks = NumberFormat.DecFormat(CastAide.AsDouble(NumberFormat.DecFormat(item["oneworksincome"], 2)) - CastAide.AsDouble(NumberFormat.DecFormat(item["amtduetoinspired"], 2)), 2)

                                            });
                                        }
                                        
                                        
                                        #endregion

                                        #region assign - Total & grand total
                                        totalGrossIncome += CastAide.AsDouble(item["oneworksincome"]);
                                        totalAmountDueToInspired += CastAide.AsDouble(item["amtduetoinspired"]);
                                        totalAmountDueToOneWorks += totalGrossIncome - totalAmountDueToInspired;
                                        totalTurnover += CastAide.AsDouble(item["turnover"]);
                                        totalTransaction += CastAide.AsInt32(item["betcount"]);
                                        totalCustomerWL += CastAide.AsDouble(item["grosswinloss"]);
                                        totalDistributorRevenue += CastAide.AsDouble(item["distributorrevenue"]);

                                        grandTotalTurnover += CastAide.AsDouble(item["turnover"]);
                                        grandTotalTransaction += CastAide.AsInt32(item["betcount"]);
                                        grandTotalCustomerWL += CastAide.AsDouble(item["grosswinloss"]);
                                        grandTotalGrossIncome += CastAide.AsDouble(item["oneworksincome"]);
                                        grandTotalAmountDueToInspired += CastAide.AsDouble(item["amtduetoinspired"]);
                                        grandTotalAmountDueToOneWorks += CastAide.AsDouble(item["oneworksincome"]) - CastAide.AsDouble(item["amtduetoinspired"]);
                                        grandTotalDistributorRevenue += CastAide.AsDouble(item["distributorrevenue"]);
                                        #endregion

                                        oldGrossIncomeChargeRate = currentGrossIncomeChargeRate;
                                        oldInspiredChargeRate = currentInspiredChargeRate;
                                        oldDistributorChargeRate = currentDistributorChargeRate;
                                       
                                    }

                                    #region assign data for each reseller
                                    var currentTurnover = NumberFormat.DecFormat(totalTurnover, 2);
                                    var currentTotalTrans = NumberFormat.DecFormat(totalTransaction, 0);
                                    var currentCustmerWL = NumberFormat.DecFormat(totalCustomerWL, 2);
                                    InspiredInfo = string.Format("{0}% gross win", (currentInspiredChargeRate * 100));
                                    var currentTotalGrossIncome = NumberFormat.DecFormat(totalGrossIncome, 2);
                                    grossIncomeInfo = string.Format("{0}% gross win", (currentOneWorksChargeRate * 100));
                                    distributorInfo = currentResellerType > 1 ? "" : string.Format("{0}% gross win", (oldDistributorChargeRate * 100));
                                    licenseeWinLossDetail.WinLossTotal = new VirtualSportsWinLossDetail()
                                    {
                                       
                                        Licensee = currentResellerName,
                                        Turnover = currentTurnover,
                                        TotalTransaction = currentTotalTrans,
                                        GrossWin = currentCustmerWL,
                                        DistributorRevenue = currentResellerType > 1 ? "-" : NumberFormat.DecFormat(totalDistributorRevenue, 2),
                                        ONEWorksIncome = currentTotalGrossIncome,
                                        AmountDueToInspired = NumberFormat.DecFormat(totalAmountDueToInspired, 2),
                                        AmountDueToOneWorks = NumberFormat.DecFormat(totalAmountDueToOneWorks, 2)
                                    };

                                    licenseeWinLoss.Licensees.Add(licenseeWinLossDetail);
                                    #endregion
                                }
                            }

                            #region revenue info - group reseller type
                            if (licenseeWinLoss.Licensees != null && licenseeWinLoss.Licensees.Count > 0)
                            {
                                licenseeWinLoss.RevenueInfo = new VirtualSportsWinLossDetail()
                                {
                                    ONEWorksIncome = grossIncomeInfo,
                                    AmountDueToInspired = InspiredInfo,
                                    DistributorRevenue = distributorInfo
                                };

                                result.LicenseesWinLoss.Add(licenseeWinLoss);
                            }
                            #endregion

                        }

                        #region grand total
                        result.GrandTotal = new VirtualSportsWinLossDetail()
                        {
                            TotalTransaction = NumberFormat.DecFormat(grandTotalTransaction, 0),
                            Turnover = NumberFormat.DecFormat(grandTotalTurnover, 2),
                            GrossWin = NumberFormat.DecFormat(grandTotalCustomerWL, 2),
                            DistributorRevenue = NumberFormat.DecFormat(grandTotalDistributorRevenue, 2),
                            ONEWorksIncome = NumberFormat.DecFormat(grandTotalGrossIncome, 2),
                            AmountDueToInspired = NumberFormat.DecFormat(grandTotalAmountDueToInspired, 2),
                            AmountDueToOneWorks = NumberFormat.DecFormat(grandTotalAmountDueToOneWorks, 2)

                        };
                        #endregion
                        AppSession.AddSession(HttpContext.Current.Session, _licenseeVirtualSportsWinLossSessionKey, result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }


        public void ExportExcelVirtualSportsWinLossReport(string normalLicensee, string operators, string fromDate, string toDate, string sportList, string reportTitle)
        {
             try
            {
                #region local varialbles
                var startCol = 0;
                var startRow = 2;
                var reportName = "Licensee-VirtualSportWinLoss";
                var excelFile = new ExcelFile();
                var getSession = (VirtualSportsWinLoss)AppSession.GetSession(HttpContext.Current.Session, _licenseeVirtualSportsWinLossSessionKey);
                getSession = getSession ?? SelVirtualSportsWinLoss(normalLicensee, operators, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), sportList);
                var wsReport = excelFile.Worksheets.Add(string.Format("VirtualSportWinLossByLicensee"));
                #endregion

                #region header
                ExportExcelVirtualSportWinLossReportAssignHeader(startRow, startCol, ref wsReport, reportTitle);
                #endregion

                #region content
                if (getSession != null && getSession.LicenseesWinLoss != null && getSession.LicenseesWinLoss.Count > 0)
                {
                    int nextRow = 0;
                    var licenseesWinLoss = getSession.LicenseesWinLoss;
                    var licenseeIndex = 0;
                    var totalLicenseeGroup = licenseesWinLoss.Count;
                    foreach (VirtualSportsWinLossByGroupLicensees group in licenseesWinLoss)
                    {
                        ExportExcelVirtualSportWinLossReportAssignRevenueInfoRow(startRow, startCol, ref nextRow, ref wsReport, group.RevenueInfo);
                        foreach (VirtualSportsWinLossByLicensee licensee in group.Licensees)
                        {
                            ExportExcelVirtualSportWinLossReportAssignTotalRow(startRow, startCol, ref nextRow, ref wsReport, licensee.WinLossTotal);
                            foreach (VirtualSportsWinLossDetail item in licensee.LicenseeWinLossDetail)
                            {
                                ExportExcelVirtualSportWinLossReportAssignBody(startRow, startCol, ref nextRow, ref wsReport, item);
                            }
                        }

                        licenseeIndex++;
                    }
                    ExportExcelVirtualSportWinLossReportAssignGrandTotalRow(startRow, startCol, ref nextRow, ref wsReport, getSession.GrandTotal);

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
        private void ExportExcelVirtualSportWinLossReportAssignHeader(int startRow, int startCol, ref ExcelWorksheet wsReport, string title)
        {
            var bgHeaderFormat = new ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            wsReport.Rows[startRow -1].Height = 255 * 2;
            wsReport.Cells[startRow - 1, startCol].Style = bgHeaderFormat;            
            wsReport.Cells[startRow - 1, startCol].Value = "Licensee";
            wsReport.Columns[startCol].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;            
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Bet count";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 10;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Turnover";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Gross win";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Distributor's revenue";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "ONEWorks's income";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Amount due to Inspired";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Amount Due To ONEWorks";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Value = title;
        }

        private void ExportExcelVirtualSportWinLossReportAssignBody(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport, 
            VirtualSportsWinLossDetail winloss)
        {
            var cellStyle = GetDefaultTextFormat(Color.FromArgb(255, 255, 255));
            cellStyle.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = cellStyle;            
            wsReport.Cells[startRow + nextRow, startCol].Value = winloss.Licensee;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.TotalTransaction;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.Turnover;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.GrossWin;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.DistributorRevenue;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.ONEWorksIncome;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.AmountDueToInspired;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.AmountDueToOneWorks;
            nextRow++;
        }

        private void ExportExcelVirtualSportWinLossReportAssignTotalRow(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport,
            VirtualSportsWinLossDetail winloss)
        {
            var cellStyle = GetDefaultTextFormat(Color.FromArgb(250, 227, 190));
            cellStyle.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow + nextRow, startCol].Style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            wsReport.Cells[startRow + nextRow, startCol].Value = winloss.Licensee;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.TotalTransaction;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.Turnover;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.GrossWin;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.DistributorRevenue;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.ONEWorksIncome;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.AmountDueToInspired;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.AmountDueToOneWorks;
            nextRow++;
        }

        private void ExportExcelVirtualSportWinLossReportAssignRevenueInfoRow(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport,
            VirtualSportsWinLossDetail winloss)
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
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.ONEWorksIncome;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.AmountDueToInspired;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = string.Empty;
            nextRow++;
        }

        private void ExportExcelVirtualSportWinLossReportAssignGrandTotalRow(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport,
            VirtualSportsWinLossDetail winloss)
        {
            var cellStyle = GetDefaultTextFormat(Color.FromArgb(216, 228, 188));
            cellStyle.Font.Weight = ExcelFont.BoldWeight;
            cellStyle.Font.Color = Color.FromArgb(0, 0, 0);
            cellStyle.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            var nextCol = 0;
            wsReport.Cells[startRow + nextRow, startCol].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol].Value = "Total";
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.TotalTransaction;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.Turnover;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.GrossWin;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.DistributorRevenue;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.ONEWorksIncome;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.AmountDueToInspired;
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
