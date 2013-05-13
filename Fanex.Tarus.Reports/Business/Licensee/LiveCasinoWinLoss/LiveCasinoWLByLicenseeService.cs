// -----------------------------------------------------------------------
// <copyright file="LiveCasinoWLByLicenseeService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.Licensee.LiveCasinoWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Reports.Entities.Licensee.LiveCasinoWinLoss;
    using Fanex.Tarus.Infrastructure.Extensions;
    using System.Data;
using Fanex.Tarus.Reports.DataAccess.Licensee.LiveCasinoWinLoss;
    using DNAide.Core;
    using Fanex.Tarus.Infrastructure.Common;
    using System.Web;
    using DNAide.Utils;
    using Fanex.Tarus.Reports.Core.Configurations;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Infrastructure.Helper;
    using System.Drawing;
using Fanex.Tarus.Reports.Entities.Sport;

    
    public class LiveCasinoWLByLicenseeService: ILiveCasinoWLByLicenseeService
    {
        public  ILiveCasinoWLByLicenseeAccess _liveCasinoWinlossAccess;
        protected string _licenseeLiveCasinoWinLossSessionKey = "licensee.livecasinowinloss.sessionkey";
        public LiveCasinoWLByLicenseeService()
            : this(null)
        { }

        public LiveCasinoWLByLicenseeService(ILiveCasinoWLByLicenseeAccess livecasinoWinlossAccess)
        {
            _liveCasinoWinlossAccess = livecasinoWinlossAccess ?? new LiveCasinoWLByLicenseeAccess();
        }
        public Licensees SelAllLicensees(string productCode)
        {
            var licensees = new Licensees();
            try
            {
                DataTable dtb = _liveCasinoWinlossAccess.SelAllLicensees(productCode);
                if (dtb != null & dtb.Rows.Count > 0)
                {
                    licensees.Licensee = new List<LicenseeDetail>();
                    foreach (DataRow item in dtb.Rows)
                    {
                        var licenseeDetail = new LicenseeDetail()
                        {
                            ResellerId = CastAide.AsInt32(item["resellerid"]),
                            ResellerName = CastAide.AsString(item["resellername"]),
                            ResellerType = CastAide.AsInt32(item["resellertype"]),
                            LicenseeId = CastAide.AsInt32(item["licenseeid"]),
                            LicenseeName = CastAide.AsString(item["licenseename"])
                        };

                        licensees.Licensee.Add(licenseeDetail);
                    }
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return licensees;
        }

        public List<BaseModel> SelAllGameTypes()
        {
            var gameTypeList = new List<BaseModel>();
            try
            {
                DataTable dtb = _liveCasinoWinlossAccess.SelAllGameTypes();
                if (dtb != null & dtb.Rows.Count > 0)
                {
                    foreach (DataRow row in dtb.Rows)
                    {
                        var gameType = new BaseModel()
                        {
                            Id = CastAide.AsInt32(row["bettype"]),
                            Name = CastAide.AsString(row["gametypename"])
                        };
                        gameTypeList.Add(gameType);
                    }
                }

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return gameTypeList;
        }

        public LiveCasinoWinLoss SelLiveCasinoWinLoss(string normalLicensee, string operators, DateTime fromDate, DateTime toDate, int gameType)
        {
            var result = new LiveCasinoWinLoss();
            try
            {
                AppSession.AddSession(HttpContext.Current.Session, _licenseeLiveCasinoWinLossSessionKey, null);
                var dtLicenseesWinLoss = _liveCasinoWinlossAccess.SelLiveCasinoWLByLicensees(normalLicensee, operators, fromDate, toDate, gameType);
                if (dtLicenseesWinLoss != null && dtLicenseesWinLoss.Rows.Count > 0)
                {
                    var licenseeConfigSection = LicenseeReportSection.Instance.LicenseesGroup;
                    if (licenseeConfigSection != null && licenseeConfigSection.Count > 0)
                    {
                        result.LicenseesWinLoss = new List<LiveCasinoWinLossByGroupLicensees>();
                        double grandTotalLupineRevenue = 0, grandTotalAmountDueTo = 0;
                        foreach (LicenseeReportElement resellerConfig in licenseeConfigSection)
                        {
                            var resellerIds = resellerConfig.ResellerIds.Split(',');
                            var licenseeWinLoss = new LiveCasinoWinLossByGroupLicensees();
                            licenseeWinLoss.Licensees = new List<LiveCasinoWinLossByLicensee>();
                            licenseeWinLoss.RevenueInfo = new LiveCasinoWinLossDetail();
                            var distributorInfo = string.Empty;
                            var starExpressInfo = string.Empty;
                            double currentDistributorChargeRate = 0, currentLupineChargeRate = 0, currentOneWorksChargeRate = 0;
                            foreach (string resellerId in resellerIds)
                            {
                                var licenseeWinLossDetail = new LiveCasinoWinLossByLicensee();
                                licenseeWinLossDetail.LicenseeWinLossDetail = new List<LiveCasinoWinLossDetail>();
                                licenseeWinLossDetail.WinLossTotal = new LiveCasinoWinLossDetail();
                                double totalDistributorRevenue = 0, totalStarExpressRevenue = 0, totalAmountDueTo = 0, totalTurnover = 0, totalGrosWin = 0;
                                var totalTransaction = 0;
                                double oldDistributorChargeRate = 0, oldStarExpressChargeRate = 0, oldOneWorksChargeRate = 0;
                                var selResellerData = (from q in dtLicenseesWinLoss.AsEnumerable().Cast<DataRow>() where q.Field<int>("resellerid") == CastAide.AsInt32(resellerId) select q).ToList();
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
                                        currentLupineChargeRate = CastAide.AsDouble(item["starexpresschargerate"]);

                                        if (((oldDistributorChargeRate + oldStarExpressChargeRate + oldOneWorksChargeRate) != 0) &&
                                            (oldDistributorChargeRate != currentDistributorChargeRate | 
                                            oldStarExpressChargeRate != currentLupineChargeRate | 
                                            oldOneWorksChargeRate != currentOneWorksChargeRate))
                                        {
                                            #region assign data for each reseller
                                            var turnover = NumberFormat.DecFormat(totalTurnover, 2);
                                            var totalTrans = NumberFormat.DecFormat(totalTransaction, 0);
                                            var grosswin = NumberFormat.DecFormat(totalGrosWin, 2);
                                            starExpressInfo = string.Format("{0}% gross win", (oldStarExpressChargeRate * 100));
                                            var totalDistributor = "-";
                                            if (currentResellerType == 1)
                                            {
                                                totalDistributor = NumberFormat.DecFormat(totalDistributorRevenue, 2);
                                                turnover = string.Empty;
                                                totalTrans = string.Empty;
                                                grosswin = string.Empty;

                                                distributorInfo = string.Format("{0}% gross win", (oldDistributorChargeRate * 100));
                                                starExpressInfo = string.Format("{0}% distributor's revenue", (oldStarExpressChargeRate * 100));
                                            }
                                            licenseeWinLossDetail.WinLossTotal = new LiveCasinoWinLossDetail()
                                            {
                                                ResellerType = currentResellerType,
                                                Licensee = currentResellerName,
                                                Turnover = turnover,
                                                TotalTransaction = totalTrans,
                                                GrossWin = grosswin,
                                                DistributorRevenue = totalDistributor,
                                                StarExpressRevenue = NumberFormat.DecFormat(totalStarExpressRevenue, 2),
                                                AmountDueToOneWorks = NumberFormat.DecFormat(totalAmountDueTo, 2)
                                            };

                                            licenseeWinLoss.Licensees.Add(licenseeWinLossDetail);
                                            #endregion

                                            #region revenue info - group reseller type
                                            licenseeWinLoss.RevenueInfo = new LiveCasinoWinLossDetail()
                                            {
                                                DistributorRevenue = distributorInfo,
                                                StarExpressRevenue = starExpressInfo,
                                                AmountDueToOneWorks = string.Format("{0}% Star Express's revenue", (oldOneWorksChargeRate * 100))
                                            };

                                            result.LicenseesWinLoss.Add(licenseeWinLoss);
                                            #endregion

                                            #region reset values
                                            licenseeWinLossDetail = new LiveCasinoWinLossByLicensee();
                                            licenseeWinLossDetail.LicenseeWinLossDetail = new List<LiveCasinoWinLossDetail>();
                                            licenseeWinLossDetail.WinLossTotal = new LiveCasinoWinLossDetail();
                                            licenseeWinLoss = new LiveCasinoWinLossByGroupLicensees();
                                            licenseeWinLoss.Licensees = new List<LiveCasinoWinLossByLicensee>();
                                            licenseeWinLoss.RevenueInfo = new LiveCasinoWinLossDetail();

                                            totalDistributorRevenue = 0;
                                            totalStarExpressRevenue = 0;
                                            totalAmountDueTo = 0;
                                            totalTurnover = 0;
                                            totalTransaction = 0;
                                            totalGrosWin = 0;
                                            oldDistributorChargeRate = 0;
                                            oldStarExpressChargeRate = 0;
                                            oldOneWorksChargeRate = 0;
                                            #endregion
                                        }

                                        #region assign - distributor licensees detail
                                        if (currentResellerType == 1)
                                        {
                                            licenseeWinLossDetail.LicenseeWinLossDetail.Add(new LiveCasinoWinLossDetail()
                                            {
                                                Licensee = CastAide.AsString(item["licenseename"]),
                                                Turnover = NumberFormat.DecFormat(item["turnover"], 2),
                                                TotalTransaction = NumberFormat.DecFormat(item["betcount"], 0),
                                                GrossWin = NumberFormat.DecFormat(item["grosswinloss"], 2),
                                                DistributorRevenue = NumberFormat.DecFormat(item["distributorrevenue"], 2),
                                                StarExpressRevenue = NumberFormat.DecFormat(item["starexpressrevenue"], 2),
                                                AmountDueToOneWorks = NumberFormat.DecFormat(item["oneworksrevenue"], 2)

                                            });
                                        }
                                        #endregion

                                        #region assign - Total & grand total
                                        totalDistributorRevenue += CastAide.AsDouble(item["distributorrevenue"]);
                                        totalStarExpressRevenue += CastAide.AsDouble(item["starexpressrevenue"]);
                                        totalAmountDueTo += CastAide.AsDouble(item["oneworksrevenue"]);
                                        totalTurnover += CastAide.AsDouble(item["turnover"]);
                                        totalTransaction += CastAide.AsInt32(item["betcount"]);
                                        totalGrosWin += CastAide.AsDouble(item["grosswinloss"]);

                                        grandTotalLupineRevenue += CastAide.AsDouble(item["starexpressrevenue"]);
                                        grandTotalAmountDueTo += CastAide.AsDouble(item["oneworksrevenue"]);
                                        #endregion

                                        oldDistributorChargeRate = currentDistributorChargeRate;
                                        oldStarExpressChargeRate = currentLupineChargeRate;
                                        oldOneWorksChargeRate = currentOneWorksChargeRate;
                                        
                                    }

                                    #region assign data for each reseller
                                    var currentTurnover = NumberFormat.DecFormat(totalTurnover, 2);
                                    var currentTotalTrans = NumberFormat.DecFormat(totalTransaction, 0);
                                    var currentGrosswin = NumberFormat.DecFormat(totalGrosWin, 2);
                                    starExpressInfo = string.Format("{0}% gross win", (currentLupineChargeRate * 100));
                                    var currentTotalDistributor = "-";
                                    if (currentResellerType == 1)
                                    {
                                        currentTotalDistributor = NumberFormat.DecFormat(totalDistributorRevenue, 2);
                                        currentTurnover = string.Empty;
                                        currentTotalTrans = string.Empty;
                                        currentGrosswin = string.Empty;

                                        distributorInfo = string.Format("{0}% gross win", (currentDistributorChargeRate * 100));
                                        starExpressInfo = string.Format("{0}% distributor's revenue", (currentLupineChargeRate * 100));
                                    }
                                    licenseeWinLossDetail.WinLossTotal = new LiveCasinoWinLossDetail()
                                    {
                                        ResellerType = currentResellerType,
                                        Licensee = currentResellerName,
                                        Turnover = currentTurnover,
                                        TotalTransaction = currentTotalTrans,
                                        GrossWin = currentGrosswin,
                                        DistributorRevenue = currentTotalDistributor,
                                        StarExpressRevenue = NumberFormat.DecFormat(totalStarExpressRevenue, 2),
                                        AmountDueToOneWorks = NumberFormat.DecFormat(totalAmountDueTo, 2)
                                    };

                                    licenseeWinLoss.Licensees.Add(licenseeWinLossDetail);
                                    #endregion
                                }
                            }

                            #region revenue info - group reseller type
                            if (licenseeWinLoss.Licensees != null && licenseeWinLoss.Licensees.Count > 0)
                            {
                                licenseeWinLoss.RevenueInfo = new LiveCasinoWinLossDetail()
                                {
                                    DistributorRevenue = distributorInfo,
                                    StarExpressRevenue = starExpressInfo,
                                    AmountDueToOneWorks = string.Format("{0}% Star Express's revenue", (currentOneWorksChargeRate * 100))
                                };

                                result.LicenseesWinLoss.Add(licenseeWinLoss);
                            }
                            #endregion

                        }

                        #region grand total
                        result.GrandTotal = new LiveCasinoWinLossDetail()
                        {
                            StarExpressRevenue = NumberFormat.DecFormat(grandTotalLupineRevenue, 2),
                            AmountDueToOneWorks = NumberFormat.DecFormat(grandTotalAmountDueTo, 2)
                        };
                        #endregion
                        AppSession.AddSession(HttpContext.Current.Session, _licenseeLiveCasinoWinLossSessionKey, result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }


        public void ExportExcelLiveCasinoWinLossReport(string normalLicensee, string operators, string fromDate, string toDate, int gameType, string reportTitle)
        {
            try
            {
                #region local varialbles
                var startCol = 0;
                var startRow = 2;
                var reportName = "Licensee-LiveCasinoWinLoss";
                var excelFile = new ExcelFile();
                var getSession = (LiveCasinoWinLoss)AppSession.GetSession(HttpContext.Current.Session, _licenseeLiveCasinoWinLossSessionKey);
                getSession = getSession ?? SelLiveCasinoWinLoss(normalLicensee, operators, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), gameType );
                var wsReport = excelFile.Worksheets.Add(string.Format("LiveCasinoWinLoss"));
                #endregion

                #region header
                ExportExcelLiveCasinoWinLossReportAssignHeader(startRow, startCol, ref wsReport, reportTitle);
                #endregion

                #region content
                if (getSession != null && getSession.LicenseesWinLoss != null && getSession.LicenseesWinLoss.Count > 0)
                {
                    int nextRow = 0;
                    var licenseesWinLoss = getSession.LicenseesWinLoss;
                    var licenseeIndex = 0;
                    var totalLicenseeGroup = licenseesWinLoss.Count;
                    foreach (LiveCasinoWinLossByGroupLicensees group in licenseesWinLoss)
                    {
                        ExportExcelLiveCasinoWinLossReportAssignRevenueInfoRow(startRow, startCol, ref nextRow, ref wsReport, group.RevenueInfo);
                        foreach (LiveCasinoWinLossByLicensee licensee in group.Licensees)
                        {
                            ExportExcelLiveCasinoWinLossReportAssignTotalRow(startRow, startCol, ref nextRow, ref wsReport, licensee.WinLossTotal);
                            foreach (LiveCasinoWinLossDetail item in licensee.LicenseeWinLossDetail)
                            {
                                ExportExcelLiveCasinoWinLossReportAssignBody(startRow, startCol, ref nextRow, ref wsReport, item);
                            }
                        }

                        if (licenseeIndex < totalLicenseeGroup - 1)
                        {
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).Merged = true;
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).Style.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
                            wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 6).Value = string.Empty;
                            nextRow++;
                        }
                        licenseeIndex++;
                    }
                    ExportExcelLiveCasinoWinLossReportAssignGrandTotalRow(startRow, startCol, ref nextRow, ref wsReport, getSession.GrandTotal);

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
        private void ExportExcelLiveCasinoWinLossReportAssignHeader(int startRow, int startCol, ref ExcelWorksheet wsReport, string title)
        {
            var bgHeaderFormat = new ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            wsReport.Rows[startRow - 1].Height = 255 * 2;
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
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Star Express's Revenue";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 25;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + (nextCol)].Value = "Amount Due To ONEWorks";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 25;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 2, startCol + nextCol).Value = title;
        }

        private void ExportExcelLiveCasinoWinLossReportAssignBody(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport,
           LiveCasinoWinLossDetail winloss)
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
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.StarExpressRevenue;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.AmountDueToOneWorks;
            nextRow++;
        }

        private void ExportExcelLiveCasinoWinLossReportAssignTotalRow(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport,
            LiveCasinoWinLossDetail winloss)
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
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.StarExpressRevenue;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.Font.Weight = ExcelFont.BoldWeight;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.AmountDueToOneWorks;
            nextRow++;
        }

        private void ExportExcelLiveCasinoWinLossReportAssignRevenueInfoRow(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport,
            LiveCasinoWinLossDetail winloss)
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
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.StarExpressRevenue;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellStyle;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.AmountDueToOneWorks;
            nextRow++;
        }

        private void ExportExcelLiveCasinoWinLossReportAssignGrandTotalRow(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport,
            LiveCasinoWinLossDetail winloss)
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
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = winloss.StarExpressRevenue;
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
