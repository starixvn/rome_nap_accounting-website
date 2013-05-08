// -----------------------------------------------------------------------
// <copyright file="BingoMonthlyWinlossService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.Gaming
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Reports.Entities.Gaming;
    using Fanex.Tarus.Reports.DataAccess.Gaming;
    using System.Data;
    using DNAide.Core;
    using DNAide.Utils;
    using ExcelUtil.Spreadsheet;
    using Sunplus.Accounting.Core.Extensions;
    using System.Web;
    using System.Drawing;
    using Sunplus.Accounting.Core.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BingoMonthlyWinlossService: IBingoMonthlyWinlossService
    {
        public readonly IBingoMonthlyWLAccess _bingoWLByMonth;
        public readonly string _keySessionBingoMonthly = "keySessionBingoMonthly";

        public BingoMonthlyWinlossService()
            : this(null) 
        {
 
        }

        public BingoMonthlyWinlossService(IBingoMonthlyWLAccess bingoWLByMonth)
        {
            this._bingoWLByMonth = bingoWLByMonth ?? new BingoMonthlyWLAccess();
        }

        public BingoMonthlyWL GetDataBingoMonthlyWL(string month, string year)
        {
            var result = new BingoMonthlyWL();
            try
            {
                var datatable = _bingoWLByMonth.GetBingoReportWLByMonth(month, year);
                if (datatable != null && datatable.Rows.Count > 0)
                {
                    foreach (DataRow row in datatable.Rows)
                    {
                        result.Winlost = NumberFormat.DecFormat(row["winlost"], 2);
                        result.Position = NumberFormat.DecFormat(row["position"], 2);
                        result.Commission = NumberFormat.DecFormat(row["commission"], 2);
                        result.LineTotal = NumberFormat.DecFormat(row["linetotal"], 2);
                        result.Turnover = NumberFormat.DecFormat(row["turnover"], 2);
                        result.Margin = NumberFormat.DecFormat(row["margin"], 2);
                    }
                }
                AppSession.AddSession(System.Web.HttpContext.Current.Session, _keySessionBingoMonthly, result);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            
            return result;
        }


        public ExcelFile ExportExcelBingoMonthly(string month, string year)
        {
            var excelFile = new ExcelFile();
            var reportName = "BingoMonthlyWinLoss_" + month + "-" + year;
            try
            {
                var objMonthlyWinloss = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keySessionBingoMonthly);
                var monthlyWinloss = objMonthlyWinloss != null
                                         ? (BingoMonthlyWL)objMonthlyWinloss
                                         : GetDataBingoMonthlyWL(month, year);

                ExcelWorksheet wsBingoReport = excelFile.Worksheets.Add("BingoMonthly");
                var bgHeader = ExcelFormat(Color.FromArgb(159, 183, 213), HorizontalAlignmentStyle.Center,
                                           ExcelFont.BoldWeight, false, false);

                var numberFormat = GetStyleIntNumber(".00", Color.FromArgb(255, 255, 255), false, true);

                #region assign Header
                wsBingoReport.Columns[0].Width = 15 * 256;
                wsBingoReport.Columns[1].Width = 15 * 256;
                wsBingoReport.Columns[2].Width = 15 * 256;
                wsBingoReport.Columns[3].Width = 15 * 256;
                wsBingoReport.Columns[4].Width = 15 * 256;
                wsBingoReport.Columns[5].Width = 15 * 256;
                wsBingoReport.Cells.GetSubrangeAbsolute(0, 0, 0, 5).Merged = true;
                wsBingoReport.Cells.GetSubrangeAbsolute(0, 0, 0, 5).Style = bgHeader;
                wsBingoReport.Cells.GetSubrangeAbsolute(0, 0, 0, 5).Value =
                    string.Format("Bingo Monthly Win Loss {0}/{1}", month, year);

                var indexHeader = 0;
                wsBingoReport.Cells[1, indexHeader].Style = bgHeader;
                wsBingoReport.Cells[1, indexHeader++].Value = "Winlost";
                wsBingoReport.Cells[1, indexHeader].Style = bgHeader;

                wsBingoReport.Cells[1, indexHeader++].Value = "Position";
                wsBingoReport.Cells[1, indexHeader].Style = bgHeader;
                wsBingoReport.Columns[indexHeader].Width = 120 * 30;
                wsBingoReport.Cells[1, indexHeader++].Value = "Commission";
                wsBingoReport.Cells[1, indexHeader].Style = bgHeader;
                wsBingoReport.Cells[1, indexHeader++].Value = "Line Total";
                wsBingoReport.Cells[1, indexHeader].Style = bgHeader;
                wsBingoReport.Cells[1, indexHeader++].Value = "Turnover";
                wsBingoReport.Cells[1, indexHeader].Style = bgHeader;
                wsBingoReport.Cells[1, indexHeader++].Value = "Margin";

                #endregion

                var indexBody = 0;

                wsBingoReport.Cells[2, indexBody].Style = numberFormat;
                wsBingoReport.Cells[2, indexBody].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                wsBingoReport.Cells[2, indexBody++].Value = CastAide.AsDouble(monthlyWinloss.Winlost);

                wsBingoReport.Cells[2, indexBody].Style = numberFormat;
                wsBingoReport.Cells[2, indexBody].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                wsBingoReport.Cells[2, indexBody++].Value = CastAide.AsDouble(monthlyWinloss.Position);

                wsBingoReport.Cells[2, indexBody].Style = numberFormat;
                wsBingoReport.Cells[2, indexBody].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                wsBingoReport.Cells[2, indexBody++].Value = CastAide.AsDouble(monthlyWinloss.Commission);

                wsBingoReport.Cells[2, indexBody].Style = numberFormat;
                wsBingoReport.Cells[2, indexBody].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                wsBingoReport.Cells[2, indexBody++].Value = CastAide.AsDouble(monthlyWinloss.LineTotal);

                wsBingoReport.Cells[2, indexBody].Style = numberFormat;
                wsBingoReport.Cells[2, indexBody].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                wsBingoReport.Cells[2, indexBody++].Value = CastAide.AsDouble(monthlyWinloss.Turnover);

                wsBingoReport.Cells[2, indexBody].Style = numberFormat;
                wsBingoReport.Cells[2, indexBody].Style.Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin);
                wsBingoReport.Cells[2, indexBody++].Value = CastAide.AsDouble(monthlyWinloss.Margin);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            finally
            {
                #region response excel file
                //excelFile.Worksheets.Add(reportName);
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "application/vnd.ms-excel";
                response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
                excelFile.SaveXls(response.OutputStream);
                #endregion
            }
            return excelFile;
        }

        private static CellStyle ExcelFormat(Color backgroundColor, HorizontalAlignmentStyle alignmentStyle, int font, bool isDecFormat, bool isPercent)
        {
            var excelFormat = new CellStyle();

            excelFormat.FillPattern.SetSolid(backgroundColor);
            excelFormat.Borders.SetBorders(MultipleBorders.Outside, Color.FromArgb(65, 116, 177), LineStyle.Thin);
            excelFormat.Font.Weight = font;
            excelFormat.HorizontalAlignment = alignmentStyle;
            excelFormat.WrapText = true;
            excelFormat.Font.Name = "Tahoma";
            excelFormat.Font.Size = 160;

            if (isDecFormat)
                excelFormat.NumberFormat = "#,##0.00_);[RED]-#,##0.00";
            if (isPercent)
                excelFormat.NumberFormat = "#,##0.00%_);[RED]-#,##0.00%";
            return excelFormat;
        }

        public CellStyle GetStyleIntNumber(string symbol, Color bgColor, bool isBold, bool isPossitiveColor)
        {
            var style = new CellStyle();
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            style.NumberFormat = "#,##0.00_);[RED]-#,##0.00";
            style.FillPattern.SetSolid(bgColor);
            style.Font.Name = "Tahoma";
            style.Font.Size = 160;
            if (isBold)
                style.Font.Weight = ExcelFont.BoldWeight;

            return style;
        }
    }
}
