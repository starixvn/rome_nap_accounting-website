
namespace Fanex.Tarus.Reports.Business.Licensee.RNGCasinoWinLoss
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Web;
    using DNAide.Core;
    using DNAide.Utils;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Fanex.Tarus.Infrastructure.Helper;
    using Fanex.Tarus.Reports.DataAccess.Licensee.RNGCasinoWinLoss;
    using Fanex.Tarus.Reports.Entities.Licensee.RNGCasinoWinLoss;

    public class RNGCasinoWinLossService: IRNGCasinoWinLossService
    {
        private IRNGCasinoWinLossDataAccess _ingWinLossDataAccess = null;
        private readonly string _rngWinLossSessionKey = "Fanex.Tarus.Reports.Business.Licensee.RNGCasinoWinLoss";

        public RNGCasinoWinLossService(): this(null)
        {
        }

        public RNGCasinoWinLossService(IRNGCasinoWinLossDataAccess ingWinLossDataAccess) 
        {
            _ingWinLossDataAccess = ingWinLossDataAccess ?? new RNGCasinoWinLossDataAccess();
        }

        public IList<Licensee> GetResellerProductCharge()
        {
            var resellers = new List<Licensee>();
            try {
                var dtResellers = _ingWinLossDataAccess.GetResellerProductCharge();
                if (dtResellers != null && dtResellers.Rows.Count > 0)
                {
                    foreach (DataRow drReseller in dtResellers.Rows)
                    {
                        resellers.Add(new Licensee()
                        {
                            Id = string.Format("{0}", drReseller["licenseename"]),
                            Name = string.Format("{0}", drReseller["resellername"]),
                            DistributorRevenue = "0",
                            OneWorksRevenue = string.Format("{0}", drReseller["oneworks"]),
                            StarExpressRevenue = string.Format("{0}", drReseller["starexpress"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return resellers;
        }

        public IList<OptionDetail> SelTSTGameGroups()
        {
            var gameGroups = new List<OptionDetail>();
            try 
            {
                var dtGameGroups = new CasinoLibrary.SBAgent.BasicInfo().GetTSTGameGroupList();
                if(dtGameGroups != null && dtGameGroups.Rows.Count > 0)
                {
                    foreach (DataRow gameGroup in dtGameGroups.Rows)
                    {
                        gameGroups.Add(new OptionDetail() { 
                            Id = string.Format("{0}", gameGroup["gamegroupid"]),
                            Name = string.Format("{0}", gameGroup["gamegroupname"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return gameGroups;            
        }

        public IList<OptionDetail> SelTSTGameTypesByGroup(string gameGroupIds)
        {
            var gameTypes = new List<OptionDetail>();
            try
            {
                var casinoLibrary = new CasinoLibrary.SBAgent.BasicInfo();
                foreach (string groupId in gameGroupIds.Split(','))
                {
                    var dtGameGroups = casinoLibrary.GetTSTGameTypeByGameGroup(CastAide.AsInt32(groupId));
                    if (dtGameGroups != null && dtGameGroups.Rows.Count > 0)
                    {
                        foreach (DataRow gameGroup in dtGameGroups.Rows)
                        {
                            gameTypes.Add(new OptionDetail()
                            {
                                Id = string.Format("{0}", gameGroup["gametypeid"]),
                                Name = string.Format("{0}", gameGroup["gametypename"])
                            });
                        }
                    }
                }                       
                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return gameTypes;         
        }

        public RNGCasinoWinLoss SelRNGCasinoWinLossReport(List<Licensee> licensees, string fromDate, string toDate, string gameTypes)
        {
            var winlossResult = new RNGCasinoWinLoss();

            try
            {
                AppSession.AddSession(HttpContext.Current.Session, _rngWinLossSessionKey, null);
                
                winlossResult.LicenseesGroup = new List<RNGCasinoWinLossGroup>();
                #region local varialbles
                var liceneeIndex = 0;
                var casinoLibrary = new CasinoLibrary.SBAgent.Report1();

                var licenseesWinLoss = new RNGCasinoWinLossGroup();
                licenseesWinLoss.RevenueInfo = new RNGCasinoWinLossDetail();
                licenseesWinLoss.Licensees = new List<RNGCasinoWinLossDetail>();

                double grandTotalStarExpressRevenue = 0, grandTotalAmountDueToOneWorks = 0;
                double starExpressRevenue = 0, amountDueToOneWorks = 0;
                #endregion
                             
                foreach (Licensee licensee in licensees)
                {
                    var dtWinLoss = casinoLibrary.SumTSTWinLossBySite(licensee.Id, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), gameTypes);
                    
                    if (dtWinLoss != null && dtWinLoss.Rows.Count > 0)
                    {
                        #region local variances
                        var drWinLoss = dtWinLoss.Rows[0];
                        var memberWinloss = CastAide.AsDouble(drWinLoss["memberwinloss"]);
                        memberWinloss = memberWinloss >= 0 ? 0 : - memberWinloss;
                        var starExpressRevenuePercent = CastAide.AsDouble(licensee.StarExpressRevenue);
                        var oneWorksPercent = CastAide.AsDouble(licensee.OneWorksRevenue);
                        starExpressRevenue = (memberWinloss * starExpressRevenuePercent);
                        amountDueToOneWorks = (starExpressRevenue * oneWorksPercent);
                        #endregion

                        #region assign group licensee
                        if (liceneeIndex != 0 && (licensee.DistributorRevenue != licensees[liceneeIndex].DistributorRevenue
                            | licensee.StarExpressRevenue != licensees[liceneeIndex].StarExpressRevenue
                            | licensee.OneWorksRevenue != licensees[liceneeIndex].OneWorksRevenue))
                        {
                            licenseesWinLoss.RevenueInfo.DistributorRevenue = string.Empty;
                            licenseesWinLoss.RevenueInfo.StarExpressRevenue = string.Format("{0}% gross win", (starExpressRevenuePercent * 100));
                            licenseesWinLoss.RevenueInfo.AmontDueToOneWorks = string.Format("{0}% Star Express's revenue", (oneWorksPercent * 100));

                            winlossResult.LicenseesGroup.Add(licenseesWinLoss);
                            licenseesWinLoss = new RNGCasinoWinLossGroup();
                            licenseesWinLoss.RevenueInfo = new RNGCasinoWinLossDetail();
                            licenseesWinLoss.Licensees = new List<RNGCasinoWinLossDetail>();
                        }
                        #endregion

                        #region assign winloss data for each licensee
                        var licenseeWinLoss = new RNGCasinoWinLossDetail()
                        {
                            Licensee = licensee.Name,
                            Turnover = NumberFormat.DoubleFormat(drWinLoss["stake"], 2),
                            TotalTran = NumberFormat.DoubleFormat(drWinLoss["betcount"], 0),
                            GrossWin = NumberFormat.DoubleFormat(memberWinloss, 2),
                            DistributorRevenue = "-",
                            StarExpressRevenue = NumberFormat.DoubleFormat(starExpressRevenue, 2),
                            AmontDueToOneWorks = NumberFormat.DoubleFormat(amountDueToOneWorks, 2)
                        };
                        licenseesWinLoss.Licensees.Add(licenseeWinLoss);

                        #region grand total
                        grandTotalStarExpressRevenue += starExpressRevenue;
                        grandTotalAmountDueToOneWorks += amountDueToOneWorks;                        
                        #endregion
                        #endregion
                        liceneeIndex++;
                    }
                }

                #region latest group
                if (licenseesWinLoss != null && licenseesWinLoss.Licensees.Count > 0)
                {
                    licenseesWinLoss.RevenueInfo.DistributorRevenue = string.Empty;
                    licenseesWinLoss.RevenueInfo.StarExpressRevenue = string.Format("{0}% gross win", CastAide.AsDouble(licensees[liceneeIndex -1].StarExpressRevenue)*100);
                    licenseesWinLoss.RevenueInfo.AmontDueToOneWorks = string.Format("{0}% Star Express's revenue", CastAide.AsDouble(licensees[liceneeIndex -1].OneWorksRevenue)*100);
                    winlossResult.LicenseesGroup.Add(licenseesWinLoss);
                }
                #endregion

                #region grand total
                winlossResult.GrandTotal = new RNGCasinoWinLossDetail()
                {
                    Licensee = string.Empty,
                    Turnover = string.Empty,
                    TotalTran = string.Empty,
                    GrossWin = string.Empty,
                    DistributorRevenue = "Total",
                    StarExpressRevenue = NumberFormat.DoubleFormat(grandTotalStarExpressRevenue, 2),
                    AmontDueToOneWorks = NumberFormat.DoubleFormat(grandTotalAmountDueToOneWorks, 2)
                };
                #endregion

                #region add session
                if (winlossResult != null && winlossResult.LicenseesGroup != null && winlossResult.LicenseesGroup.Count > 0)
                {
                    AppSession.AddSession(HttpContext.Current.Session, _rngWinLossSessionKey, winlossResult);
                }
                #endregion

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return winlossResult;
        }

        public void ExportWinLossReportToExcelFile(List<Licensee> licensees, string fromDate, string toDate, string gameTypes, string reportTitle)
        {
            try
            {
                #region local varialbles
                var startCol = 0;
                var startRow = 2;
                var reportName = "RNGCasinoWinLossReport";
                var excelFile = new ExcelFile();
                var getSession = (RNGCasinoWinLoss)AppSession.GetSession(HttpContext.Current.Session, _rngWinLossSessionKey);
                #region get new data when session is null
                if (getSession == null)
                {
                    getSession = SelRNGCasinoWinLossReport(licensees, fromDate, toDate, gameTypes);
                }
                #endregion
                var wsReport = excelFile.Worksheets.Add(string.Format("RNGCasinoWinLossReport"));
                #endregion

                #region header
                ExportWinLossReportToExcelFileAssignHeader(startRow, startCol, ref wsReport, reportTitle);
                #endregion

                #region content
                if (getSession != null && getSession.LicenseesGroup.Count > 0)
                {
                    var nextRow = 1;
                    ExportWinLossReportToExcelFileAssignContent(startRow, startCol, ref nextRow, ref wsReport, getSession.LicenseesGroup);
                    ExportWinLossReportToExcelFileAssignFooter(startRow, startCol, ref nextRow, ref wsReport, getSession.GrandTotal);
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

        #region private functions
        private void ExportWinLossReportToExcelFileAssignHeader(int startRow, int startCol, ref ExcelWorksheet wsReport, string title)
        {
            var bgHeaderFormat = new ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            #region header
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol, startRow, startCol).Value = "Licensee";
            wsReport.Columns[startCol].Width = 255 * 20;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Value = "Turnover";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Value = "Total Transaction";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Value = "Gross Win";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 15;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Value = "Distributor's Revenue";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Value = "Star Express's Revenue";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 20;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + (nextCol += 1), startRow, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 1, startCol + nextCol, startRow, startCol + nextCol).Value = "Amount Due To ONEWoks";
            wsReport.Columns[startCol + (nextCol)].Width = 255 * 25;
            #endregion

            #region report title
            wsReport.Cells.GetSubrangeAbsolute(startRow - startRow, startCol, startRow - startRow, startCol + (nextCol)).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - startRow, startCol, startRow - startRow, startCol + (nextCol)).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - startRow, startCol, startRow - startRow, startCol + (nextCol)).Value = title;
            #endregion
        }

        private void ExportWinLossReportToExcelFileAssignContent(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport, List<RNGCasinoWinLossGroup> winloss)
        {
            var cellNumberNormalFormat = GetStyleDecimalFormat(".00", Color.FromArgb(255, 255, 255), false);
            
            var cellRevenueInfoFormat = new ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(255, 255, 255), true, false);
            cellRevenueInfoFormat.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            cellRevenueInfoFormat.Font.Italic = true;
            cellRevenueInfoFormat.Font.Color = Color.FromArgb(224, 83, 7);

            foreach (RNGCasinoWinLossGroup groupWinloss in winloss)
            {
                var nextCol = 0;
                #region revenue info
                //wsReport.Cells[startRow + nextRow, startCol].Style = cellRevenueInfoFormat;
                //wsReport.Cells[startRow + nextRow, startCol].Value = string.Empty;
                //wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellRevenueInfoFormat;
                //wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = string.Empty;
                //wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellRevenueInfoFormat;
                //wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = string.Empty;
                //wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellRevenueInfoFormat;
                //wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = string.Empty;
                //wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellRevenueInfoFormat;
                //wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = string.Empty;

                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 4).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 4).Style = cellNumberNormalFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + (nextCol += 4)).Value = string.Empty;
                
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellRevenueInfoFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = groupWinloss.RevenueInfo.StarExpressRevenue;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellRevenueInfoFormat;
                wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = groupWinloss.RevenueInfo.AmontDueToOneWorks;
                #endregion                                                                                                                 
                nextRow++;
                #region licensee wl 
                foreach (RNGCasinoWinLossDetail licensee in groupWinloss.Licensees)
                {
                    nextCol = 0;
                    wsReport.Cells[startRow + nextRow, startCol].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol].Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Cells[startRow + nextRow, startCol].Value = licensee.Licensee;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(licensee.Turnover);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(licensee.TotalTran);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(licensee.GrossWin);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(licensee.DistributorRevenue);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(licensee.StarExpressRevenue);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style.Font.Weight = ExcelFont.BoldWeight;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(licensee.AmontDueToOneWorks);

                    nextRow++;
                }
                #endregion
                
            }
        }

        private void ExportWinLossReportToExcelFileAssignFooter(int startRow, int startCol, ref int nextRow, ref ExcelWorksheet wsReport, RNGCasinoWinLossDetail grandTotal)
        {
            var cellNumberNormalFormat = GetStyleDecimalFormat(".00", Color.FromArgb(205, 222, 255), true);
            cellNumberNormalFormat.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            var nextCol = 0;

            wsReport.Cells[startRow + nextRow, startCol].Style = cellNumberNormalFormat;
            wsReport.Cells[startRow + nextRow, startCol].Value = string.Empty;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = string.Empty;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = string.Empty;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = string.Empty;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = "Total";
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(grandTotal.StarExpressRevenue);
            wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = cellNumberNormalFormat;
            wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(grandTotal.AmontDueToOneWorks);

        }

        private CellStyle GetStyleDecimalFormat(string symbol, Color bgColor, bool isBold)
        {
            var style = new CellStyle();
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            style.NumberFormat = string.Format("[>0]#,##0{0};[<0]-#,##0{0};#,##0{0}", symbol);
            style.FillPattern.SetSolid(bgColor);
            style.Font.Name = "Tahoma";
            style.Font.Size = 160;
            if (isBold)
            {
                style.Font.Weight = ExcelFont.BoldWeight;
            }

            return style;
        }
        #endregion

    }
}