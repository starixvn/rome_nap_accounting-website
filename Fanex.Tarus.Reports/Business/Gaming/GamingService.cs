// -----------------------------------------------------------------------
// <copyright file="GamingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.Gaming
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DNAide.Core;
    using DNAide.Utils;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Infrastructure.Caching;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Fanex.Tarus.Infrastructure.Helper;
    using Fanex.Tarus.Reports.DataAccess.Gaming;
    using Fanex.Tarus.Reports.Entities.Gaming;
    using Fanex.Tarus.Web.Controllers.Filters;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GamingService : IGamingService
    {
        IGamingDataAccess _igamingDataAccess;
        string _keyNumberGameWL = "NumberGameWL." + AppSession.AccUserName;
        public GamingService() : this(null) { }
        public GamingService(IGamingDataAccess igamingDataAccess)
        {
            _igamingDataAccess = igamingDataAccess ?? new GamingDataAccess();
        }

        [AuthenticateRead]
        public NumberGameWLModel SelNumberGameWinlossByDate(string fromdate, string todate)
        {
            NumberGameWLModel game = new NumberGameWLModel();
            game.LiveIndicators = new List<NumberGameWL>();
            game.Total = new NumberGameWL();
            try
            {
                DataTable dt = _igamingDataAccess.SelNumberGameWinlossByDate(fromdate, todate);
                int[] LiveIndicators = { 1, 0 };

                foreach (int liveindicator in LiveIndicators)
                {
                    var data = dt.AsEnumerable().Where(s => CastAide.AsInt32(s["islive"]) == liveindicator).Select(s => s).ToList();
                    NumberGameWL wlByLiveindicator = new NumberGameWL();
                    decimal lineTotal = data.Sum(s => CastAide.AsDecimal(s["LineTotal"]));
                    decimal turnOver = data.Sum(s => CastAide.AsDecimal(s["TurnOver"]));

                    wlByLiveindicator.Name = liveindicator == 1 ? "Live" : "None Live";
                    wlByLiveindicator.WinLoss = NumberFormat.DecFormat(data.Sum(s => CastAide.AsDecimal(s["WinLost"])), 2);
                    wlByLiveindicator.Position = NumberFormat.DecFormat(data.Sum(s => CastAide.AsDecimal(s["Position"])), 2);
                    wlByLiveindicator.Comm = NumberFormat.DecFormat(data.Sum(s => CastAide.AsDecimal(s["Commission"])), 2);
                    wlByLiveindicator.LineTotal = NumberFormat.DecFormat(lineTotal, 2);
                    wlByLiveindicator.Turnover = NumberFormat.DecFormat(turnOver, 2);
                    wlByLiveindicator.Margin = turnOver == 0 ? "N/A" : (NumberFormat.DecFormat(lineTotal / turnOver * 100, 2) + "%");
                    wlByLiveindicator.Childs = new List<NumberGameWL>();

                    foreach (DataRow row in data)
                    {
                        NumberGameWL wlbyBettype = new NumberGameWL();

                        decimal linetotal = CastAide.AsDecimal(row["LineTotal"]);
                        decimal turnover = CastAide.AsDecimal(row["TurnOver"]);

                        wlbyBettype.Name = CacheRef.GetBettype(row["bettype"]);
                        wlbyBettype.WinLoss = NumberFormat.DecFormat(row["WinLost"], 2);
                        wlbyBettype.Position = NumberFormat.DecFormat(row["Position"], 2);
                        wlbyBettype.Comm = NumberFormat.DecFormat(row["Commission"], 2);
                        wlbyBettype.LineTotal = NumberFormat.DecFormat(linetotal, 2);
                        wlbyBettype.Turnover = NumberFormat.DecFormat(turnover, 2);
                        wlbyBettype.Margin = turnover == 0 ? "N/A" : (NumberFormat.DecFormat(linetotal / turnover * 100, 2) + "%");

                        wlByLiveindicator.Childs.Add(wlbyBettype);
                    }

                    game.LiveIndicators.Add(wlByLiveindicator);

                }

                // Total
                decimal _lineTotal = dt.AsEnumerable().Sum(s => CastAide.AsDecimal(s["LineTotal"]));
                decimal _turnOver = dt.AsEnumerable().Sum(s => CastAide.AsDecimal(s["TurnOver"]));
                game.Total.Name = "Total";
                game.Total.WinLoss = NumberFormat.DecFormat(dt.AsEnumerable().Sum(s => CastAide.AsDecimal(s["WinLost"])), 2);
                game.Total.Position = NumberFormat.DecFormat(dt.AsEnumerable().Sum(s => CastAide.AsDecimal(s["Position"])), 2);
                game.Total.Comm = NumberFormat.DecFormat(dt.AsEnumerable().Sum(s => CastAide.AsDecimal(s["Commission"])), 2);
                game.Total.LineTotal = NumberFormat.DecFormat(_lineTotal, 2);
                game.Total.Turnover = NumberFormat.DecFormat(_turnOver, 2);
                game.Total.Margin = _turnOver == 0 ? "N/A" : (NumberFormat.DecFormat(_lineTotal / _turnOver * 100, 2) + "%");
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            //Store in Session
            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyNumberGameWL, game);
            return game;
        }


        public void ExportExcelNumberGameWinlossByDate(string fromdate, string todate)
        {
            try
            {
                ExcelFile excel = new ExcelFile();
                ExcelGemboxUtils xlsUtils = new ExcelGemboxUtils(false);
                var game = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyNumberGameWL) as NumberGameWLModel ?? SelNumberGameWinlossByDate(fromdate, todate);

                string reportName = "NumberGameFinancialReport";
                ExcelWorksheet wsNG = excel.Worksheets.Add("Number Game Financial Report");
                Fanex.Tarus.Infrastructure.Helper.ExcelGemboxUtils.ColorNumber colorNumber = ExcelGemboxUtils.ColorNumber.RedColorOnGreaterThanZeroNumber;
                decimal margin = 0;
                string symbol = "";


                #region assign Header
                wsNG.Cells.GetSubrangeAbsolute(0, 0, 0, 6).Style = xlsUtils.GetStyleText(205, 222, 243, true);
                wsNG.Cells.GetSubrangeAbsolute(0, 0, 0, 6).Value = "Number Game Financial Report";
                wsNG.Cells.GetSubrangeAbsolute(0, 0, 0, 6).Merged = true;

                wsNG.Cells.GetSubrangeAbsolute(1, 0, 1, 6).Style = xlsUtils.GetStyleText(205, 222, 243, false);
                wsNG.Cells.GetSubrangeAbsolute(1, 0, 1, 6).Value = "From: " + fromdate + " To: " + todate;
                wsNG.Cells.GetSubrangeAbsolute(1, 0, 1, 6).Merged = true;

                wsNG.Cells[2, 0].Value = "Bettype";
                wsNG.Cells[2, 0].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                wsNG.Cells[2, 1].Value = "Winlost";
                wsNG.Cells[2, 1].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                wsNG.Cells[2, 2].Value = "Position";
                wsNG.Cells[2, 2].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                wsNG.Cells[2, 3].Value = "Commission";
                wsNG.Cells[2, 3].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                wsNG.Cells[2, 4].Value = "Line Total";
                wsNG.Cells[2, 4].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                wsNG.Cells[2, 5].Value = "Turnover";
                wsNG.Cells[2, 5].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                wsNG.Cells[2, 6].Value = "Margin";
                wsNG.Cells[2, 6].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                for (int i = 0; i <= 6; i++)
                {
                    wsNG.Columns[i].Width = 15 * 256;
                }
                #endregion

                #region Content
                int index = 2;
                foreach (NumberGameWL parent in game.LiveIndicators)
                {
                    index++;

                    wsNG.Cells[index, 0].Style = xlsUtils.GetStyleText(255, 255, 255, HorizontalAlignmentStyle.Left, true);
                    wsNG.Cells[index, 0].Value = parent.Name;

                    wsNG.Cells[index, 1].Style = xlsUtils.GetStyleNumber(CastAide.AsDecimal(parent.WinLoss), "", 255, 255, 255, true, colorNumber);
                    wsNG.Cells[index, 1].Value = CastAide.AsDecimal(parent.WinLoss);

                    wsNG.Cells[index, 2].Style = xlsUtils.GetStyleNumber(0, "", 255, 255, 255, true);
                    wsNG.Cells[index, 2].Value = CastAide.AsDecimal(parent.Position);

                    wsNG.Cells[index, 3].Style = xlsUtils.GetStyleNumber(0, "", 255, 255, 255, true);
                    wsNG.Cells[index, 3].Value = CastAide.AsDecimal(parent.Comm);

                    wsNG.Cells[index, 4].Style = xlsUtils.GetStyleNumber(CastAide.AsDecimal(parent.LineTotal), "", 255, 255, 255, true, colorNumber);
                    wsNG.Cells[index, 4].Value = CastAide.AsDecimal(parent.LineTotal);

                    wsNG.Cells[index, 5].Style = xlsUtils.GetStyleNumber(0, "", 255, 255, 255, true);
                    wsNG.Cells[index, 5].Value = CastAide.AsDecimal(parent.Turnover);

                    margin = CastAide.AsDecimal(parent.Margin.Replace("%", ""));
                    symbol = parent.Margin == "N/A" ? "" : "%";

                    wsNG.Cells[index, 6].Style = xlsUtils.GetStyleNumber(margin, symbol, 255, 255, 255, true, colorNumber);
                    wsNG.Cells[index, 6].Value = CastAide.AsDecimal(margin / 100);

                    foreach (NumberGameWL child in parent.Childs)
                    {
                        index++;

                        wsNG.Cells[index, 0].Style = xlsUtils.GetStyleText(255, 255, 255, HorizontalAlignmentStyle.Right, false);
                        wsNG.Cells[index, 0].Value = child.Name;

                        wsNG.Cells[index, 1].Style = xlsUtils.GetStyleNumber(CastAide.AsDecimal(child.WinLoss), "", 255, 255, 255, colorNumber);
                        wsNG.Cells[index, 1].Value = CastAide.AsDecimal(child.WinLoss);

                        wsNG.Cells[index, 2].Style = xlsUtils.GetStyleNumber(0, "", 255, 255, 255);
                        wsNG.Cells[index, 2].Value = CastAide.AsDecimal(child.Position);

                        wsNG.Cells[index, 3].Style = xlsUtils.GetStyleNumber(0, "", 255, 255, 255);
                        wsNG.Cells[index, 3].Value = CastAide.AsDecimal(child.Comm);

                        wsNG.Cells[index, 4].Style = xlsUtils.GetStyleNumber(CastAide.AsDecimal(child.LineTotal), "", 255, 255, 255, colorNumber);
                        wsNG.Cells[index, 4].Value = CastAide.AsDecimal(child.LineTotal);

                        wsNG.Cells[index, 5].Style = xlsUtils.GetStyleNumber(0, "", 255, 255, 255);
                        wsNG.Cells[index, 5].Value = CastAide.AsDecimal(child.Turnover);

                        margin = CastAide.AsDecimal(child.Margin.Replace("%", ""));
                        symbol = child.Margin == "N/A" ? "" : "%";

                        wsNG.Cells[index, 6].Style = xlsUtils.GetStyleNumber(margin, symbol, 255, 255, 255, colorNumber);
                        wsNG.Cells[index, 6].Value = CastAide.AsDecimal(margin / 100);
                    }

                }
                #endregion

                #region Footer
                index++;

                wsNG.Cells[index, 0].Style = xlsUtils.GetStyleText(255, 255, 255, HorizontalAlignmentStyle.Center, true);
                wsNG.Cells[index, 0].Value = game.Total.Name;

                wsNG.Cells[index, 1].Style = xlsUtils.GetStyleNumber(CastAide.AsDecimal(game.Total.WinLoss), "", 255, 255, 255, true, colorNumber);
                wsNG.Cells[index, 1].Value = CastAide.AsDecimal(game.Total.WinLoss);

                wsNG.Cells[index, 2].Style = xlsUtils.GetStyleNumber(0, "", 255, 255, 255, true);
                wsNG.Cells[index, 2].Value = CastAide.AsDecimal(game.Total.Position);

                wsNG.Cells[index, 3].Style = xlsUtils.GetStyleNumber(0, "", 255, 255, 255, true);
                wsNG.Cells[index, 3].Value = CastAide.AsDecimal(game.Total.Comm);

                wsNG.Cells[index, 4].Style = xlsUtils.GetStyleNumber(CastAide.AsDecimal(game.Total.LineTotal), "", 255, 255, 255, true, colorNumber);
                wsNG.Cells[index, 4].Value = CastAide.AsDecimal(game.Total.LineTotal);

                wsNG.Cells[index, 5].Style = xlsUtils.GetStyleNumber(0, "", 255, 255, 255, true);
                wsNG.Cells[index, 5].Value = CastAide.AsDecimal(game.Total.Turnover);

                margin = CastAide.AsDecimal(game.Total.Margin.Replace("%", ""));
                symbol = game.Total.Margin == "N/A" ? "" : "%";

                wsNG.Cells[index, 6].Style = xlsUtils.GetStyleNumber(margin, symbol, 255, 255, 255, true, colorNumber);
                wsNG.Cells[index, 6].Value = CastAide.AsDecimal(margin / 100);
                #endregion

                new ReportHelpers().ReponseExcelFile(excel, reportName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }
    }
}
