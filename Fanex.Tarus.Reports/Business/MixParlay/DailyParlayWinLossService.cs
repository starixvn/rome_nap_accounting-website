// -----------------------------------------------------------------------
// <copyright file="ParlayCombinationService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Business.MixParlay
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using DNAide.Core;
    using DNAide.Utils;
    using ExcelUtil.Spreadsheet;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Infrastructure.Extensions;
    using Fanex.Tarus.Infrastructure.Helper;
    using Fanex.Tarus.Reports.DataAccess.MixParlay;
    using Fanex.Tarus.Reports.Entities.MixParlay;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DailyParlayWinLossService
    {
        private readonly IMixParlayReportAccess _mixParlayReportAccess;
        private string _keyDailyParlayWinLoss = "DailyParlayWinLoss." + AppSession.Username;
        private string _keyAdvanceParalyWinLoss = "AdvanceParalyWinLoss." + AppSession.Username;
        public DailyParlayWinLossService()
        {
            _mixParlayReportAccess = new MixParlayReportAccess();
        }

        private DailyParlayWinLoss AssignDataDailyParlayWinLoss(DataRow row)
        {
            DailyParlayWinLoss parlay = new DailyParlayWinLoss();
            try
            {
                if (row == null)
                    return parlay;

                decimal lineTotal = CastAide.AsDecimal(row["lineTotal"]);
                decimal turnover = CastAide.AsDecimal(row["turnover"]);

                parlay.WinLoss = NumberFormat.DecFormat(row["WinLost"], 2);
                parlay.BetCount = NumberFormat.DecFormat(row["BetCount"], 0);
                parlay.MemberTurnover = NumberFormat.DecFormat(row["turnoverp"], 2);
                parlay.Position = NumberFormat.DecFormat(row["Position"], 2);
                parlay.Comm = NumberFormat.DecFormat(row["Commission"], 2);
                parlay.LineTotal = NumberFormat.DecFormat(lineTotal, 2);
                parlay.Turnover = NumberFormat.DecFormat(turnover, 2);
                parlay.Margin = turnover == 0 ? "N/A" : NumberFormat.DecFormat((lineTotal / turnover * 100), 2) + "%";
                parlay.Rows = new List<DailyParlayWinLoss>();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return parlay;
        }

        private DailyParlayWinLoss AssignDataDailyParlayWinLoss(List<DataRow> total)
        {
            DailyParlayWinLoss parlay = new DailyParlayWinLoss();
            try
            {
                if (total == null)
                    return parlay;

                decimal lineTotal = CastAide.AsDecimal(total.Sum(s => CastAide.AsDecimal(s["lineTotal"])));
                decimal turnover = CastAide.AsDecimal(total.Sum(s => CastAide.AsDecimal(s["turnover"])));

                parlay.WinLoss = NumberFormat.DecFormat(total.Sum(s => CastAide.AsDecimal(s["WinLost"])), 2);
                parlay.BetCount = NumberFormat.DecFormat(total.Sum(s => CastAide.AsDecimal(s["BetCount"])), 0);
                parlay.MemberTurnover = NumberFormat.DecFormat(total.Sum(s => CastAide.AsDecimal(s["turnoverp"])), 2);
                parlay.Position = NumberFormat.DecFormat(total.Sum(s => CastAide.AsDecimal(s["Position"])), 2);
                parlay.Comm = NumberFormat.DecFormat(total.Sum(s => CastAide.AsDecimal(s["Commission"])), 2);
                parlay.LineTotal = NumberFormat.DecFormat(lineTotal, 2);
                parlay.Turnover = NumberFormat.DecFormat(turnover, 2);
                parlay.Margin = turnover == 0 ? "N/A" : NumberFormat.DecFormat((lineTotal / turnover * 100), 2) + "%";
                parlay.Rows = new List<DailyParlayWinLoss>();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return parlay;
        }

        private string ConvertCombinationType(string combination)
        {
            string result = string.Empty;
            switch (combination)
            {
                case "m2":
                    result = "2XCombinations";
                    break;
                case "m3":
                    result = "3XCombinations";
                    break;
                case "m4":
                    result = "4XCombinations";
                    break;
                case "m5":
                    result = "5XCombinations";
                    break;
                case "m6":
                    result = "6XCombinations & up ";
                    break;
                default:
                    result = "Unknow";
                    break;

            }
            return result;

        }

        private string ConvertLiveType(int liveType)
        {
            string result = string.Empty;
            switch (liveType)
            {
                case 0:
                    result = "Non-Live";
                    break;
                case 1:
                    result = "Live";
                    break;
                case 2:
                    result = "Mix Live and Non-Live";
                    break;
                default:
                    result = "Unknow";
                    break;
            }
            return result;
        }

        public DailyParlayWinLoss SelDailyParlayWinLoss(DateTime fromDate, DateTime toDate, bool isViewDW)
        {
            DailyParlayWinLoss parlay = new DailyParlayWinLoss();
            parlay.Rows = new List<DailyParlayWinLoss>();

            try
            {
                DataTable dt = _mixParlayReportAccess.SelDailyParlayCombinationWinLoss(fromDate, toDate, isViewDW);
                if (dt == null)
                    return parlay;

                //Grand Total
                List<DataRow> grandTotal = dt.AsEnumerable().Select(s => s).ToList();
                parlay = AssignDataDailyParlayWinLoss(grandTotal);
                parlay.Name = "Grand Total";

                //Group Combinations
                List<string> combinations = dt.AsEnumerable().Select(s => CastAide.AsString(s["Combinations"]).ToLower()).Distinct().ToList();

                //each combination
                foreach (string combination in combinations)
                {
                    DailyParlayWinLoss parlayCombination = new DailyParlayWinLoss();
                    List<DataRow> dataCombination = dt.AsEnumerable().Where(s => CastAide.AsString(s["Combinations"]) == combination).ToList();

                    //Total Combination
                    parlayCombination = AssignDataDailyParlayWinLoss(dataCombination);
                    parlayCombination.Name = "Total";
                    parlayCombination.ParentName = ConvertCombinationType(combination);
                    parlayCombination.rowspan = dataCombination.Count() + dataCombination.Select(s => CastAide.AsInt32(s["LiveType"])).Distinct().Count();

                    //Group live types of each combination
                    List<int> livetypes = dataCombination.Select(s => CastAide.AsInt32(s["LiveType"])).Distinct().ToList();

                    //each Type
                    foreach (int livetype in livetypes)
                    {
                        DailyParlayWinLoss parlayLiveType = new DailyParlayWinLoss();
                        List<DataRow> dataLiveType = dataCombination.AsEnumerable().Where(s => CastAide.AsInt32(s["LiveType"]) == livetype).ToList();

                        //Total LiveType
                        parlayLiveType = AssignDataDailyParlayWinLoss(dataLiveType);
                        parlayLiveType.Name = "Sub Total";
                        parlayLiveType.ParentName = ConvertLiveType(livetype);
                        parlayLiveType.rowspan = dataLiveType.Count;

                        //each bettype
                        foreach (DataRow bettype in dataLiveType)
                        {
                            DailyParlayWinLoss parlayEachBettype = AssignDataDailyParlayWinLoss(bettype);
                            parlayEachBettype.Name = "Mix Parlay (" + CastAide.AsString(bettype["status"]) + ")";
                            parlayLiveType.Rows.Add(parlayEachBettype);
                        }

                        //Add list LiveType into Combination
                        parlayCombination.Rows.Add(parlayLiveType);
                    }

                    //Add list Combination into ParlayWinLossByCombination
                    parlay.Rows.Add(parlayCombination);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyDailyParlayWinLoss, parlay);
            return parlay;

        }

        private string getSportName(int sportId)
        {
            string name = "";
            switch (sportId)
            {
                case 1:
                    name = "Soccer Sport";
                    break;
                case 2:
                    name = "Other Sports";
                    break;
                case 3:
                    name = "Multi Sports";
                    break;
            }
            return name;
        }

        private string getLiveTypeName(int liveType)
        {
            string name = "";
            switch (liveType)
            {
                case 0:
                    name = "Non Live";
                    break;
                case 1:
                    name = "Live";
                    break;
                case 2:
                    name = "Mix Live and Non Live";
                    break;
            }
            return name;
        }

        private DailyParlayWinLoss AssignDataAdvanceParlayWinLoss(List<DataRow> total)
        {
            DailyParlayWinLoss parlay = new DailyParlayWinLoss();
            try
            {
                if (total == null)
                    return parlay;

                decimal lineTotal = CastAide.AsDecimal(total.Sum(s => CastAide.AsDecimal(s["lineTotal"])));
                decimal turnover = CastAide.AsDecimal(total.Sum(s => CastAide.AsDecimal(s["turnover"])));

                parlay.WinLoss = NumberFormat.DecFormat(total.Sum(s => CastAide.AsDecimal(s["WinLost"])), 2);
                parlay.Position = NumberFormat.DecFormat(total.Sum(s => CastAide.AsDecimal(s["Position"])), 2);
                parlay.Comm = NumberFormat.DecFormat(total.Sum(s => CastAide.AsDecimal(s["Commission"])), 2);
                parlay.LineTotal = NumberFormat.DecFormat(lineTotal, 2);
                parlay.Turnover = NumberFormat.DecFormat(turnover, 2);
                parlay.Margin = turnover == 0 ? "N/A" : NumberFormat.DecFormat((lineTotal / turnover * 100), 2) + "%";
                parlay.Rows = new List<DailyParlayWinLoss>();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return parlay;
        }

        private DailyParlayWinLoss AssignDataAdvanceParlayWinLoss(DataRow row)
        {
            DailyParlayWinLoss parlay = new DailyParlayWinLoss();
            try
            {
                if (row == null)
                    return parlay;

                decimal lineTotal = CastAide.AsDecimal(row["lineTotal"]);
                decimal turnover = CastAide.AsDecimal(row["turnover"]);

                parlay.WinLoss = NumberFormat.DecFormat(row["WinLost"], 2);
                parlay.Position = NumberFormat.DecFormat(row["Position"], 2);
                parlay.Comm = NumberFormat.DecFormat(row["Commission"], 2);
                parlay.LineTotal = NumberFormat.DecFormat(lineTotal, 2);
                parlay.Turnover = NumberFormat.DecFormat(turnover, 2);
                parlay.Margin = turnover == 0 ? "N/A" : NumberFormat.DecFormat((lineTotal / turnover * 100), 2) + "%";
                parlay.Rows = new List<DailyParlayWinLoss>();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return parlay;
        }

        public DailyParlayWinLoss SelAdvanceParalyWinLoss(DateTime fromDate, DateTime toDate, bool isViewDW)
        {
            DailyParlayWinLoss parlay = new DailyParlayWinLoss();
            parlay.Rows = new List<DailyParlayWinLoss>();

            try
            {
                DataTable dt = _mixParlayReportAccess.SelAdvanceMixParlayReport(fromDate, toDate, isViewDW, 9);
                if (dt == null)
                    return parlay;

                //Grand Total
                List<DataRow> grandTotal = dt.AsEnumerable().Select(s => s).ToList();
                parlay = AssignDataAdvanceParlayWinLoss(grandTotal);
                parlay.Name = "Grand Total";

                //Group Live Type
                List<int> liveTypes = dt.AsEnumerable().Select(s => CastAide.AsInt32(s["LiveType"])).Distinct().ToList();

                //each Live Type
                foreach (int livetype in liveTypes)
                {
                    DailyParlayWinLoss parlayLiveType = new DailyParlayWinLoss();
                    List<DataRow> dataLiveType = dt.AsEnumerable().Where(s => CastAide.AsInt32(s["LiveType"]) == livetype).ToList();

                    //Total Live Type
                    parlayLiveType = AssignDataAdvanceParlayWinLoss(dataLiveType);
                    parlayLiveType.Name = "Total";
                    parlayLiveType.ParentName = getLiveTypeName(livetype);
                    parlayLiveType.rowspan = dataLiveType.Count() + dataLiveType.Select(s => CastAide.AsInt32(s["SportType"])).Distinct().Count();

                    //Group sport type of live type
                    List<int> sports = dataLiveType.Select(s => CastAide.AsInt32(s["sporttype"])).Distinct().ToList();

                    //each sport
                    foreach (int sport in sports)
                    {
                        DailyParlayWinLoss parlaysport = new DailyParlayWinLoss();
                        List<DataRow> dataSport = dataLiveType.AsEnumerable().Where(s => CastAide.AsInt32(s["sporttype"]) == sport).ToList();

                        //Total Sport
                        parlaysport = AssignDataAdvanceParlayWinLoss(dataSport);
                        parlaysport.Name = "Sub Total";
                        parlaysport.ParentName = getSportName(sport);
                        parlaysport.rowspan = dataSport.Count;

                        //each bettype
                        foreach (DataRow bettype in dataSport)
                        {
                            DailyParlayWinLoss parlayEachBettype = AssignDataAdvanceParlayWinLoss(bettype);
                            parlayEachBettype.Name = "Mix Parlay (" + CastAide.AsString(bettype["status"]) + ")";
                            parlaysport.Rows.Add(parlayEachBettype);
                        }

                        //Add list LiveType into Combination
                        parlayLiveType.Rows.Add(parlaysport);
                    }

                    //Add list Live Type into ParlayWinLossByCombination
                    parlay.Rows.Add(parlayLiveType);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyAdvanceParalyWinLoss, parlay);
            return parlay;
        }


        private void AssignDataExcelDailyParlayWinLoss(DailyParlayWinLoss row, int index, ExcelWorksheet wsParlay, int red, int green, int blue, bool isbold, HorizontalAlignmentStyle nameAlign, Color fontColor)
        {
            Fanex.Tarus.Infrastructure.Helper.ExcelGemboxUtils.ColorNumber colorNumber = ExcelGemboxUtils.ColorNumber.RedColorOnGreaterThanZeroNumber;
            ExcelGemboxUtils xlsUtils = new ExcelGemboxUtils(false);

            decimal linetotal = 0, turnover = 0, margin = 0;
            linetotal = CastAide.AsDecimal(row.LineTotal);
            turnover = CastAide.AsDecimal(row.Turnover);
            var currentCol = 2;
            var currentCell = wsParlay.Cells[index, currentCol];
            currentCell.Value = row.Name;
            currentCell.Style = xlsUtils.GetStyleText(red, green, blue, nameAlign, isbold, fontColor);
            currentCol++;

            currentCell = wsParlay.Cells[index, currentCol];
            currentCell.Value = CastAide.AsDecimal(row.WinLoss);
            currentCell.Style = xlsUtils.GetStyleNumber(CastAide.AsDecimal(row.WinLoss), "", red, green, blue, isbold, colorNumber, fontColor);
            currentCol++;

            currentCell = wsParlay.Cells[index, currentCol];
            currentCell.Value = CastAide.AsDecimal(row.MemberTurnover);
            currentCell.Style = xlsUtils.GetStyleNumber(0, "", red, green, blue, isbold, fontColor);
            currentCol++;

            currentCell = wsParlay.Cells[index, currentCol];
            currentCell.Value = CastAide.AsDecimal(row.BetCount);
            currentCell.Style = xlsUtils.GetStyleIntNumber(0, "", red, green, blue, isbold, colorNumber, fontColor);
            currentCol++;

            currentCell = wsParlay.Cells[index, currentCol];
            currentCell.Value = CastAide.AsDecimal(row.Position);
            currentCell.Style = xlsUtils.GetStyleNumber(0, "", red, green, blue, isbold, fontColor);
            currentCol++;

            currentCell = wsParlay.Cells[index, currentCol];
            currentCell.Value = CastAide.AsDecimal(row.Comm);
            currentCell.Style = xlsUtils.GetStyleNumber(0, "", red, green, blue, isbold, fontColor);
            currentCol++;

            currentCell = wsParlay.Cells[index, currentCol];
            currentCell.Value = linetotal;
            currentCell.Style = xlsUtils.GetStyleNumber(CastAide.AsDecimal(row.LineTotal), "", red, green, blue, isbold, colorNumber, fontColor);
            currentCol++;

            currentCell = wsParlay.Cells[index, currentCol];
            currentCell.Value = turnover;
            currentCell.Style = xlsUtils.GetStyleNumber(0, "", red, green, blue, isbold, fontColor);
            currentCol++;

            currentCell = wsParlay.Cells[index, currentCol];
            if (turnover == 0)
            {
                currentCell.Value = "N/A";
                currentCell.Style = xlsUtils.GetStyleText(red, green, blue, HorizontalAlignmentStyle.Right, isbold, fontColor);
            }
            else
            {
                margin = linetotal / turnover;
                currentCell.Value = row.Margin;
                currentCell.Style = GetStyleNumber(margin, "%", red, green, blue, isbold, HorizontalAlignmentStyle.Right, colorNumber, fontColor);
            }
        }

        private void ExcelConvertCombinationType(string combination, ref int red, ref int green, ref int blue)
        {
            if (combination.IndexOf("2X") > -1)
            {
                red = 221;
                green = 217;
                blue = 196;
            }
            else if (combination.IndexOf("3X") > -1)
            {
                red = 202;
                green = 240;
                blue = 145;
            }
            else if (combination.IndexOf("4X") > -1)
            {
                red = 183;
                green = 222;
                blue = 232;
            }
            else if (combination.IndexOf("5X") > -1)
            {
                red = 253;
                green = 233;
                blue = 217;
            }
            else if (combination.IndexOf("6X") > -1)
            {
                red = 242;
                green = 220;
                blue = 219;
            }
        }
        public void ExportExcelDailyParlayWinLoss(DateTime fromDate, DateTime toDate, bool isViewDW)
        {
            try
            {
                ExcelFile excel = new ExcelFile();
                ExcelGemboxUtils xlsUtils = new ExcelGemboxUtils(false);
                var parlay = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyDailyParlayWinLoss) as DailyParlayWinLoss ?? SelDailyParlayWinLoss(fromDate, toDate, isViewDW);

                string reportName = "DailyParlayWinLoss";
                ExcelWorksheet wsParlay = excel.Worksheets.Add("DailyParlayWinLoss");
                Fanex.Tarus.Infrastructure.Helper.ExcelGemboxUtils.ColorNumber colorNumber = ExcelGemboxUtils.ColorNumber.RedColorOnGreaterThanZeroNumber;

                #region assign Header
                wsParlay.Cells.GetSubrangeAbsolute(0, 0, 1, 10).Style = xlsUtils.GetStyleText(188, 210, 239, true);
                wsParlay.Cells.GetSubrangeAbsolute(0, 0, 1, 10).Value = "Parlay Combination Win Loss - From " + fromDate.ToShortDateString() + " To " + toDate.ToShortDateString();
                wsParlay.Cells.GetSubrangeAbsolute(0, 0, 1, 10).Merged = true;

                // First Row
                var currentRow = 2;
                var currentCol = 0;
                var currentCellRange = wsParlay.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 2);
                currentCellRange.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCellRange.Merged = true;
                currentCol += 3;

                currentCellRange = wsParlay.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 2);
                currentCellRange.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCellRange.Merged = true;
                currentCellRange.Value = "Member";
                currentCol += 3;

                currentCellRange = wsParlay.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 1);
                currentCellRange.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCellRange.Merged = true;
                currentCellRange.Value = "Agency";
                currentCol += 2;

                currentCellRange = wsParlay.Cells.GetSubrangeAbsolute(currentRow, currentCol, currentRow, currentCol + 2);
                currentCellRange.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCellRange.Merged = true;
                currentCellRange.Value = "Company";
                currentCol += 3;
                currentRow++;

                // SecondRow
                currentCol = 0;
                var currentCell = wsParlay.Cells[currentRow, currentCol];
                currentCell.Value = "Combination";
                currentCell.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCol++;

                currentCell = wsParlay.Cells[currentRow, currentCol];
                currentCell.Value = "Type";
                currentCell.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCol++;

                currentCell = wsParlay.Cells[currentRow, currentCol];
                currentCell.Value = "Bettype";
                currentCell.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCol++;

                currentCell = wsParlay.Cells[currentRow, currentCol];
                currentCell.Value = "WinLoss";
                currentCell.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCol++;

                currentCell = wsParlay.Cells[currentRow, currentCol];
                currentCell.Value = "Turnover";
                currentCell.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCol++;

                currentCell = wsParlay.Cells[currentRow, currentCol];
                currentCell.Value = "Bet Count";
                currentCell.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCol++;

                currentCell = wsParlay.Cells[currentRow, currentCol];
                currentCell.Value = "Position";
                currentCell.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCol++;

                currentCell = wsParlay.Cells[currentRow, currentCol];
                currentCell.Value = "Comm";
                currentCell.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCol++;

                currentCell = wsParlay.Cells[currentRow, currentCol];
                currentCell.Value = "Line Total";
                currentCell.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCol++;

                currentCell = wsParlay.Cells[currentRow, currentCol];
                currentCell.Value = "Turnover";
                currentCell.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCol++;

                currentCell = wsParlay.Cells[currentRow, currentCol];
                currentCell.Value = "Margin";
                currentCell.Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);
                currentCol++;

                for (int i = 0; i <= 10; i++)
                {
                    if (i <= 1)
                    {
                        wsParlay.Columns[i].Width = 18 * 256;
                    }
                    else
                    {
                        wsParlay.Columns[i].Width = 15 * 256;
                    }
                }

                #endregion

                #region Content
                int index = 3;
                decimal linetotal = 0, turnover = 0, margin = 0;
                int r = 255, g = 255, b = 255;
                foreach (DailyParlayWinLoss combination in parlay.Rows)
                {
                    ExcelConvertCombinationType(combination.ParentName, ref r, ref g, ref b);
                    wsParlay.Cells.GetSubrangeAbsolute(index + 1, 0, index + 1 + combination.rowspan, 0).Value = combination.ParentName;
                    wsParlay.Cells.GetSubrangeAbsolute(index + 1, 0, index + 1 + combination.rowspan, 0).Style = xlsUtils.GetStyleText(r, g, b, HorizontalAlignmentStyle.Center, true);
                    wsParlay.Cells.GetSubrangeAbsolute(index + 1, 0, index + 1 + combination.rowspan, 0).Merged = true;

                    foreach (DailyParlayWinLoss livetype in combination.Rows)
                    {
                        wsParlay.Cells.GetSubrangeAbsolute(index + 1, 1, index + 1 + livetype.rowspan, 1).Value = livetype.ParentName;
                        wsParlay.Cells.GetSubrangeAbsolute(index + 1, 1, index + 1 + livetype.rowspan, 1).Style = xlsUtils.GetStyleText(255, 255, 255, HorizontalAlignmentStyle.Center, true);
                        wsParlay.Cells.GetSubrangeAbsolute(index + 1, 1, index + 1 + livetype.rowspan, 1).Merged = true;

                        foreach (DailyParlayWinLoss bettype in livetype.Rows)
                        {
                            index++;
                            AssignDataExcelDailyParlayWinLoss(bettype, index, wsParlay, 255, 255, 255, false, HorizontalAlignmentStyle.Left, Color.Black);
                        }
                        //Total each Live Type
                        index++;
                        AssignDataExcelDailyParlayWinLoss(livetype, index, wsParlay, 222, 210, 190, true, HorizontalAlignmentStyle.Center, Color.Black);
                    }
                    //Total each combination
                    index++;
                    wsParlay.Cells[index, 1].Value = "";
                    wsParlay.Cells[index, 1].Style = xlsUtils.GetStyleText(33, 89, 103, HorizontalAlignmentStyle.Center, true, Color.White);
                    AssignDataExcelDailyParlayWinLoss(combination, index, wsParlay, 33, 89, 103, true, HorizontalAlignmentStyle.Center, Color.White);
                }
                #endregion

                #region Footer

                index++;
                AssignDataExcelDailyParlayWinLoss(parlay, index, wsParlay, 218, 241, 187, true, HorizontalAlignmentStyle.Center, Color.Black);
                wsParlay.Cells.GetSubrangeAbsolute(index, 0, index, 2).Style = xlsUtils.GetStyleText(218, 241, 187, HorizontalAlignmentStyle.Center, true);
                wsParlay.Cells.GetSubrangeAbsolute(index, 0, index, 2).Merged = true;

                wsParlay.Cells[index, 8].Style = xlsUtils.GetStyleNumber(CastAide.AsDecimal(parlay.LineTotal), "", 181, 196, 208, true, colorNumber, Color.Black);
                #endregion

                new ReportHelpers().ReponseExcelFile(excel, reportName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }


        private void AssignDataExcelAdvanceParlayWinLoss(DailyParlayWinLoss row, int index, ExcelWorksheet wsParlay, int red, int green, int blue, bool isbold, HorizontalAlignmentStyle nameAlign, Color fontColor)
        {
            Fanex.Tarus.Infrastructure.Helper.ExcelGemboxUtils.ColorNumber colorNumber = ExcelGemboxUtils.ColorNumber.RedColorOnGreaterThanZeroNumber;
            ExcelGemboxUtils xlsUtils = new ExcelGemboxUtils(false);

            decimal linetotal = 0, turnover = 0, margin = 0;
            linetotal = CastAide.AsDecimal(row.LineTotal);
            turnover = CastAide.AsDecimal(row.Turnover);

            wsParlay.Cells[index, 2].Value = row.Name;
            wsParlay.Cells[index, 2].Style = xlsUtils.GetStyleText(red, green, blue, nameAlign, isbold, fontColor);

            wsParlay.Cells[index, 3].Value = CastAide.AsDecimal(row.WinLoss);
            wsParlay.Cells[index, 3].Style = xlsUtils.GetStyleNumber(CastAide.AsDecimal(row.WinLoss), "", red, green, blue, isbold, colorNumber, fontColor);

            wsParlay.Cells[index, 4].Value = CastAide.AsDecimal(row.Position);
            wsParlay.Cells[index, 4].Style = xlsUtils.GetStyleNumber(0, "", red, green, blue, isbold, fontColor);

            wsParlay.Cells[index, 5].Value = CastAide.AsDecimal(row.Comm);
            wsParlay.Cells[index, 5].Style = xlsUtils.GetStyleNumber(0, "", red, green, blue, isbold, fontColor);

            wsParlay.Cells[index, 6].Value = linetotal;
            wsParlay.Cells[index, 6].Style = xlsUtils.GetStyleNumber(CastAide.AsDecimal(row.LineTotal), "", red, green, blue, isbold, colorNumber, fontColor);

            wsParlay.Cells[index, 7].Value = turnover;
            wsParlay.Cells[index, 7].Style = xlsUtils.GetStyleNumber(0, "", red, green, blue, isbold, fontColor);
            if (turnover == 0)
            {
                wsParlay.Cells[index, 8].Value = "N/A";
                wsParlay.Cells[index, 8].Style = xlsUtils.GetStyleText(red, green, blue, HorizontalAlignmentStyle.Right, isbold, fontColor);
            }
            else
            {
                margin = linetotal / turnover;
                wsParlay.Cells[index, 8].Value = row.Margin;

                wsParlay.Cells[index, 8].Style = GetStyleNumber(margin, "%", red, green, blue, isbold, HorizontalAlignmentStyle.Right, colorNumber, fontColor);
            }

        }
        private void ExcelConvertLiveType(string livetype, ref int red, ref int green, ref int blue)
        {
            if (livetype == "Non Live")
            {
                red = 202;
                green = 240;
                blue = 145;
            }
            else if (livetype == "Live")
            {
                red = 221;
                green = 217;
                blue = 196;
            }
            else if (livetype == "Mix Live and Non Live")
            {
                red = 197;
                green = 217;
                blue = 241;
            }
        }
        public void ExportExcelAdvanceParlayWinLoss(DateTime fromDate, DateTime toDate, bool isViewDW)
        {
            try
            {
                ExcelFile excel = new ExcelFile();
                ExcelGemboxUtils xlsUtils = new ExcelGemboxUtils(false);
                var parlay = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyAdvanceParalyWinLoss) as DailyParlayWinLoss ?? SelAdvanceParalyWinLoss(fromDate, toDate, isViewDW);

                string reportName = "AdvanceParlayWinLoss";
                ExcelWorksheet wsParlay = excel.Worksheets.Add("AdvanceParlayWinLoss");
                Fanex.Tarus.Infrastructure.Helper.ExcelGemboxUtils.ColorNumber colorNumber = ExcelGemboxUtils.ColorNumber.RedColorOnGreaterThanZeroNumber;

                #region assign Header
                wsParlay.Cells.GetSubrangeAbsolute(0, 0, 1, 8).Style = xlsUtils.GetStyleText(188, 210, 239, true);
                wsParlay.Cells.GetSubrangeAbsolute(0, 0, 1, 8).Value = "Parlay Win Loss by Product - From " + fromDate.ToShortDateString() + " To " + toDate.ToShortDateString();
                wsParlay.Cells.GetSubrangeAbsolute(0, 0, 1, 8).Merged = true;

                wsParlay.Cells[2, 0].Value = "";
                wsParlay.Cells[2, 0].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                wsParlay.Cells[2, 1].Value = "Sporttype";
                wsParlay.Cells[2, 1].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                wsParlay.Cells[2, 2].Value = "Bettype";
                wsParlay.Cells[2, 2].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                wsParlay.Cells[2, 3].Value = "WinLoss";
                wsParlay.Cells[2, 3].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                wsParlay.Cells[2, 4].Value = "Position";
                wsParlay.Cells[2, 4].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                wsParlay.Cells[2, 5].Value = "Comm";
                wsParlay.Cells[2, 5].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                wsParlay.Cells[2, 6].Value = "Line Total";
                wsParlay.Cells[2, 6].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                wsParlay.Cells[2, 7].Value = "Turnover";
                wsParlay.Cells[2, 7].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                wsParlay.Cells[2, 8].Value = "Margin";
                wsParlay.Cells[2, 8].Style = xlsUtils.GetStyleText(205, 222, 243, HorizontalAlignmentStyle.Center, true);

                for (int i = 0; i <= 8; i++)
                {
                    if (i <= 1)
                    {
                        wsParlay.Columns[i].Width = 18 * 256;
                    }
                    else
                    {
                        wsParlay.Columns[i].Width = 15 * 256;
                    }
                }
                #endregion

                #region Content
                int index = 2;
                decimal linetotal = 0, turnover = 0, margin = 0;
                int r = 255, g = 255, b = 255;
                foreach (DailyParlayWinLoss livetype in parlay.Rows)
                {
                    ExcelConvertLiveType(livetype.ParentName, ref r, ref g, ref b);
                    wsParlay.Cells.GetSubrangeAbsolute(index + 1, 0, index + 1 + livetype.rowspan, 0).Value = livetype.ParentName;
                    wsParlay.Cells.GetSubrangeAbsolute(index + 1, 0, index + 1 + livetype.rowspan, 0).Style = xlsUtils.GetStyleText(r, g, b, HorizontalAlignmentStyle.Center, true);
                    wsParlay.Cells.GetSubrangeAbsolute(index + 1, 0, index + 1 + livetype.rowspan, 0).Merged = true;

                    foreach (DailyParlayWinLoss sporttype in livetype.Rows)
                    {
                        wsParlay.Cells.GetSubrangeAbsolute(index + 1, 1, index + 1 + sporttype.rowspan, 1).Value = sporttype.ParentName;
                        wsParlay.Cells.GetSubrangeAbsolute(index + 1, 1, index + 1 + sporttype.rowspan, 1).Style = xlsUtils.GetStyleText(255, 255, 255, HorizontalAlignmentStyle.Center, true);
                        wsParlay.Cells.GetSubrangeAbsolute(index + 1, 1, index + 1 + sporttype.rowspan, 1).Merged = true;

                        foreach (DailyParlayWinLoss bettype in sporttype.Rows)
                        {
                            index++;
                            AssignDataExcelAdvanceParlayWinLoss(bettype, index, wsParlay, 255, 255, 255, false, HorizontalAlignmentStyle.Left, Color.Black);
                        }
                        //Total each Live Type
                        index++;
                        AssignDataExcelAdvanceParlayWinLoss(sporttype, index, wsParlay, 222, 210, 190, true, HorizontalAlignmentStyle.Center, Color.Black);
                    }
                    //Total each combination
                    index++;
                    wsParlay.Cells[index, 1].Value = "";
                    wsParlay.Cells[index, 1].Style = xlsUtils.GetStyleText(33, 89, 103, HorizontalAlignmentStyle.Center, true, Color.White);
                    AssignDataExcelAdvanceParlayWinLoss(livetype, index, wsParlay, 33, 89, 103, true, HorizontalAlignmentStyle.Center, Color.White);
                }
                #endregion

                #region Footer
                index++;
                AssignDataExcelAdvanceParlayWinLoss(parlay, index, wsParlay, 218, 241, 187, true, HorizontalAlignmentStyle.Center, Color.Black);
                wsParlay.Cells.GetSubrangeAbsolute(index, 0, index, 2).Style = xlsUtils.GetStyleText(218, 241, 187, HorizontalAlignmentStyle.Center, true);
                wsParlay.Cells.GetSubrangeAbsolute(index, 0, index, 2).Merged = true;

                wsParlay.Cells[index, 6].Style = xlsUtils.GetStyleNumber(CastAide.AsDecimal(parlay.LineTotal), "", 181, 196, 208, true, colorNumber, Color.Black);
                #endregion

                new ReportHelpers().ReponseExcelFile(excel, reportName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
        }

        private CellStyle GetStyleNumber(decimal num, string symbol, int red, int green, int blue, bool IsBold, HorizontalAlignmentStyle textAlign, Fanex.Tarus.Infrastructure.Helper.ExcelGemboxUtils.ColorNumber colorNumber, Color fontColor)
        {
            var style = new CellStyle();
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.HorizontalAlignment = textAlign;
            style.Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thin);
            style.NumberFormat = "#,##0.00" + symbol + "_)";
            style.FillPattern.SetSolid(Color.FromArgb(red, green, blue));
            style.Font.Name = "Tahoma";
            if (IsBold)
                style.Font.Weight = ExcelFont.BoldWeight;
            style.Font.Size = 160;

            switch (colorNumber)
            {
                case Fanex.Tarus.Infrastructure.Helper.ExcelGemboxUtils.ColorNumber.RedColorOnGreaterThanZeroNumber:
                    if (num > 0)
                        style.Font.Color = Color.Red;
                    else
                        style.Font.Color = fontColor;
                    break;
                case Fanex.Tarus.Infrastructure.Helper.ExcelGemboxUtils.ColorNumber.RedColorOnLessThanZeroNumber:
                    if (num < 0)
                        style.Font.Color = Color.Red;
                    else
                        style.Font.Color = fontColor;
                    break;
            }

            return style;
        }

    }
}