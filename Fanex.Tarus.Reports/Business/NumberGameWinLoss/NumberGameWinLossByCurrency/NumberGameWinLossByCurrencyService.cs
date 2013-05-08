
namespace Fanex.Tarus.Reports.Business.NumberGame.ByCurrency
{
    using System;
    using System.Web;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using DNAide.Core;
    using DNAide.Utils;
    using Fanex.Tarus.Reports.Entities.NumberGameWinLoss;
    using Fanex.Tarus.Reports.DataAccess.NumberGameWinLoss.ByCurrency;
    using Sunplus.Accounting.Core.Extensions;
    using Fanex.Tarus.Reports.Entities.NumberGameWinLoss.NumberGameWinLossByCurrency;
    using Fanex.Tarus.Reports.Resource;
    using ExcelUtil.Spreadsheet;
    using System.Drawing;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ByCurrencyService : IByCurrencyService
    {
        IByCurrencyAccess _byCurAccess = null;
        private const string NumberGameWinLossByCur = "NumberGameWinLossByCur";
        
        public ByCurrencyService() : this(null) { }

        public ByCurrencyService(IByCurrencyAccess byCurrencyAccess)
        {
            _byCurAccess = byCurrencyAccess ?? new ByCurrencyAccess();
        }

        #region Web

        #region MainReport
        public NumberGameWinLossByCurrency_MainBO GetNumberGameMainData(NumberGameWinLossByCurrency_MainParams MainParams)
        {
            try
            {
                NumberGameWinLossByCurrency_MainBO getData = null;

                if (MainParams.FromCache)
                {
                    var getDataFromSession = SessionExtension.GetSession(HttpContext.Current.Session, NumberGameWinLossByCur);
                    if (getDataFromSession != null)
                    {
                        getData = (NumberGameWinLossByCurrency_MainBO)getDataFromSession;
                    }
                    else
                    {
                        getData = MapNumberGameMainData(MainParams);
                        SessionExtension.AddSession(HttpContext.Current.Session, NumberGameWinLossByCur, getData);
                    }
                }
                else
                {
                    getData = MapNumberGameMainData(MainParams);
                    SessionExtension.AddSession(HttpContext.Current.Session, NumberGameWinLossByCur, getData);
                }

                return getData;
            }
            catch(Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return new NumberGameWinLossByCurrency_MainBO();
        }
        public NumberGameWinLossByCurrency_MainBO MapNumberGameMainData(NumberGameWinLossByCurrency_MainParams MainParams)
        {
            try
            {
                var result = new NumberGameWinLossByCurrency_MainBO();

                result.ListMember = new List<NumberGameWinLossByCurrency_MainDTO>();

                var list = new List<NumberGameWinLossByCurrency_MainDTO>();

                var numberGameMain = _byCurAccess.GetNumberGameWinLossByCurAccess(MainParams);

                double grandNetIncome = 0;
                double grandNetTurnover = 0;
                double grandMemberTotalWL = 0;
                double grandTotalTurnOver = 0;
                if (numberGameMain.Tables.Count > 0)
                {
                    foreach (DataRow row in numberGameMain.Tables[0].Rows)
                    {
                        var item = new NumberGameWinLossByCurrency_MainDTO
                        {
                            Currency = CastAide.AsString(row["CurrencyName"]),
                            Turnover = IBCUtils.FormatHandler.FormatNumber20(row["Turnover"]),
                            MemberWinLoss = IBCUtils.FormatHandler.FormatNumber20(row["MemberWinLoss"]),
                            GamePayout = IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(row["GamePayout"]) * 100),
                            NetTurnover = IBCUtils.FormatHandler.FormatNumber20(row["NetTurnover"]),
                            PTWinLoss = IBCUtils.FormatHandler.FormatNumber20(row["PTWinLoss"]),
                            CommExpense = IBCUtils.FormatHandler.FormatNumber20(row["CommExpense"]),
                            NetIncome = IBCUtils.FormatHandler.FormatNumber20(row["NetIncome"]),
                            Margin = IBCUtils.FormatHandler.FormatNumber20(CastAide.AsDouble(row["Margin"]) * 100)
                             
                        };
                        grandNetIncome += CastAide.AsDouble(row["NetIncome"]);
                        grandNetTurnover += CastAide.AsDouble(row["NetTurnover"]);
                        grandTotalTurnOver += CastAide.AsDouble(row["Turnover"]);
                        grandMemberTotalWL += CastAide.AsDouble(row["MemberWinLoss"]);
                        list.Add(item);
                    }
                }
                result.ListMember = list;
                result.Quantity = result.ListMember.Count;

                var totalItems = (from q in list select q).ToList();
               
                double gamePayout = (grandMemberTotalWL / grandTotalTurnOver) *100;
                double grandMargin = (grandNetIncome / grandNetTurnover) * 100;

                var grandTotal = new NumberGameWinLossByCurrency_MainDTO
                {
                    Currency = "Grand Total",
                    Turnover = IBCUtils.FormatHandler.FormatNumber20(grandTotalTurnOver),
                    MemberWinLoss = IBCUtils.FormatHandler.FormatNumber20(grandMemberTotalWL),
                    GamePayout = IBCUtils.FormatHandler.FormatNumber20(gamePayout),
                    NetTurnover = IBCUtils.FormatHandler.FormatNumber20(grandNetTurnover),
                    PTWinLoss = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.PTWinLoss)).Sum()),
                    CommExpense = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.CommExpense)).Sum()),
                    NetIncome = IBCUtils.FormatHandler.FormatNumber20(grandNetIncome),
                    Margin = IBCUtils.FormatHandler.FormatNumber20(grandMargin),
                };


                result.GrandTotal = grandTotal;

                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return new NumberGameWinLossByCurrency_MainBO();
        }
        #endregion

        #region DetailReport
        public NumberGameWinLossByCurrency_DetailBO GetNumberGameDetailData(NumberGameWinLossByCurrency_DetailParams DetailParams)
        {
            try
            {
                NumberGameWinLossByCurrency_DetailBO getData = null;

                if (DetailParams.FromCache)
                {
                    var getDataFromSession = SessionExtension.GetSession(HttpContext.Current.Session, NumberGameWinLossByCur);
                    if (getDataFromSession != null)
                    {
                        getData = (NumberGameWinLossByCurrency_DetailBO)getDataFromSession;
                    }
                    else
                    {
                        getData = MapNumberGameDetailData(DetailParams);
                        SessionExtension.AddSession(HttpContext.Current.Session, NumberGameWinLossByCur, getData);
                    }
                }
                else
                {
                    getData = MapNumberGameDetailData(DetailParams);
                    SessionExtension.AddSession(HttpContext.Current.Session, NumberGameWinLossByCur, getData);
                }


                var returnData = new NumberGameWinLossByCurrency_DetailBO();
                List<NumberGameWinLossByCurrency_DetailDTO> Items = getData.ListUsername;

                var pagingItems = Items.Select(c => c).Skip(DetailParams.PageSize * (DetailParams.PageIndex - 1)).Take(DetailParams.PageSize).ToList();
                if (DetailParams.PageSize == 0)
                {
                    returnData.ListUsername = getData.ListUsername;
                }
                else
                {
                    returnData.ListUsername = pagingItems;
                }
               
                returnData.Currencies = getData.Currencies;
                returnData.Quantity = getData.Quantity;
                returnData.Total = getData.Total;
                returnData.DetailName = getData.DetailName;
                return returnData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return new NumberGameWinLossByCurrency_DetailBO();
        }
        public NumberGameWinLossByCurrency_DetailBO MapNumberGameDetailData(NumberGameWinLossByCurrency_DetailParams DetailParams)
        {
            try
            {
                var result = new NumberGameWinLossByCurrency_DetailBO();

                result.ListUsername = new List<NumberGameWinLossByCurrency_DetailDTO>();

                var list = new List<NumberGameWinLossByCurrency_DetailDTO>();

                var numberGameMain = _byCurAccess.GetNumberGameWinLossByCurDetailAccess(DetailParams);

                if (numberGameMain.Tables.Count > 0)
                {
                    foreach (DataRow row in numberGameMain.Tables[0].Rows)
                    {
                        var item = new NumberGameWinLossByCurrency_DetailDTO
                        {
                            RoleID = CastAide.AsInt32(row["RoleID"]),
                            CustID = CastAide.AsInt32(row["CustID"]),
                            Username = CastAide.AsString(row["username"]),
                            Turnover = IBCUtils.FormatHandler.FormatNumber20(row["TurnOver"]),
                            GrossComm = IBCUtils.FormatHandler.FormatNumber20(row["GrossComm"]),

                            MemberWinLoss = IBCUtils.FormatHandler.FormatNumber20(row["memberWL"]),
                            MemberComm = IBCUtils.FormatHandler.FormatNumber20(row["memberComm"]),
                            MemberTotal = IBCUtils.FormatHandler.FormatNumber20(row["memberTotal"]),

                            AgentWinLoss = IBCUtils.FormatHandler.FormatNumber20(row["agentWL"]),
                            AgentComm = IBCUtils.FormatHandler.FormatNumber20(row["agentComm"]),
                            AgentTotal = IBCUtils.FormatHandler.FormatNumber20(row["agentTotal"]),

                            MasterWinLoss = IBCUtils.FormatHandler.FormatNumber20(row["masterWL"]),
                            MasterComm = IBCUtils.FormatHandler.FormatNumber20(row["masterComm"]),  
                            MasterTotal = IBCUtils.FormatHandler.FormatNumber20(row["masterTotal"]),

                            SuperWinLoss = IBCUtils.FormatHandler.FormatNumber20(row["superWL"]),
                            SuperComm = IBCUtils.FormatHandler.FormatNumber20(row["superComm"]),
                            SuperTotal = IBCUtils.FormatHandler.FormatNumber20(row["superTotal"]),

                            Company = IBCUtils.FormatHandler.FormatNumber20(row["Company"]),
                        };
                        list.Add(item);
                    }
                }
                result.ListUsername = list;
                
                result.Quantity = result.ListUsername.Count;

                var totalItems = (from q in list select q).ToList();

                var Total = new NumberGameWinLossByCurrency_DetailDTO
                {
                    CustID = 0,
                    RoleID = 0,
                    Username = "Total",
                    Turnover = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.Turnover)).Sum()),
                    GrossComm = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.GrossComm)).Sum()),

                    MemberWinLoss = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.MemberWinLoss)).Sum()),
                    MemberComm = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.MemberComm)).Sum()),
                    MemberTotal = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.MemberTotal)).Sum()),

                    AgentWinLoss = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.AgentWinLoss)).Sum()),
                    AgentComm = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.AgentComm)).Sum()),
                    AgentTotal = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.AgentTotal)).Sum()),

                    MasterComm = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.MasterComm)).Sum()),
                    MasterWinLoss = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.MasterWinLoss)).Sum()),
                    MasterTotal = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.MasterTotal)).Sum()),

                    SuperWinLoss = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.SuperWinLoss)).Sum()),
                    SuperComm = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.SuperComm)).Sum()),
                    SuperTotal = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.SuperTotal)).Sum()),
                    Company = IBCUtils.FormatHandler.FormatNumber20((from c in totalItems select CastAide.AsDouble(c.Company)).Sum()),

                };


                result.Total = Total;
                result.Currencies = DetailParams.Currencies;
                result.DetailName = DetailParams.DetailName;
            
                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return new NumberGameWinLossByCurrency_DetailBO();
        }
        #endregion
        #endregion

        #region Export excel
        
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
       
        #region Export for Main

        public void ExportExcelNumberGameWinLossByCurMain(NumberGameWinLossByCurrency_MainParams MainParams)
        {
            NumberGameWinLossByCurrency_MainBO Report = new NumberGameWinLossByCurrency_MainBO();

            Report = GetNumberGameMainData(MainParams);

            var StarCols = 0;
            var StarRows = 3;

            var ReportName = "NumberGameWinLossByCurrencyMain";
            var ExcelFile = new ExcelFile();

            var MaxItemsInSheet = 3000;

            if (Report.Quantity > 0)
            {
                ExportNumberGameWinLossByCurMain(StarRows, StarCols, MaxItemsInSheet,ref ExcelFile, Report, MainParams);
            }
            else
            {
                var errorMsg = Report.Quantity == -1 ?
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "TimeOutError") :
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblThereisnodata");
                var wsReport = ExcelFile.Worksheets.Add(ReportName);
                wsReport.Cells[StarRows - 1, StarCols].Value = errorMsg;
            }

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", ReportName));
            ExcelFile.SaveXls(response.OutputStream);

        }
        private void ExportNumberGameWinLossByCurMain(int startRow, int startCol, int maxItemsInSheet, ref ExcelFile ExcelFile, NumberGameWinLossByCurrency_MainBO report, NumberGameWinLossByCurrency_MainParams parameters)
        {
            if (parameters.IsShowComm)
            {
                ExportNumberGameWinLossByCurMainAssignBody(startRow, startCol, maxItemsInSheet, ref ExcelFile, report, parameters);
            }
            else
            {
                ExportNumberGameWinLossByCurMainNotCommAssignBody(startRow, startCol, maxItemsInSheet, ref ExcelFile, report, parameters);
            }
        }

        #region Show Comm for Main
        private void ExportNumberGameWinLossByCurMainAssignHeader(int startRow, int startCol, ref ExcelWorksheet wsReport, string fromDate, string toDate)
        {
            var bgHeaderFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.Currency;
                

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.Turnover;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.MemberWinLoss;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.GamePayout;

            #region Company
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 5).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 5).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 5).Value = Language.Company;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.NetTurnover;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.PTWinLoss;
              
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.CommExpense;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.NetIncome;
      
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.Margin;
                
            #endregion

            #region report title
            var title = Language.NumberGameWinLossByCurrencyMain;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Value = string.Format("{0} From {1:MM/dd/yyyy} To {2:MM/dd/yyyy}", title, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate));
            #endregion
        }
        private void ExportNumberGameWinLossByCurMainAssignBody(int startRow, int startCol, int maxItemsInSheet, ref ExcelFile ExcelFile, NumberGameWinLossByCurrency_MainBO report, NumberGameWinLossByCurrency_MainParams parameters)
        {
            var totalSheet = (report.Quantity % maxItemsInSheet > 0 ? 1 : 0) + report.Quantity / maxItemsInSheet;
            var numberDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false);
            var numberIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(253, 252, 252), false, false);
            var numberFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false, true);
            var bgPercentFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, false, true);
            var bgNormalPercentFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, false, true);
            var nextRow = 0;
            var itemIndex = 0;

            for (int i = 0; i < totalSheet; i++)
            {
                var wsReport = ExcelFile.Worksheets.Add(string.Format("NumberGameWinLossByCurrencyMain_{0}", i + 1));

                ExportNumberGameWinLossByCurMainAssignHeader(startRow, startCol, ref wsReport, parameters.FromDate, parameters.ToDate);

                var totalItemsInCurrentSheet = i < totalSheet - 1 ? maxItemsInSheet : (report.Quantity - (i * maxItemsInSheet));
                for (int j = 0; j < totalItemsInCurrentSheet; j++)
                {
                    var item = report.ListMember[itemIndex];
                    var nextCol = 0;

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = item.Currency;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.Turnover);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.MemberWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgNormalPercentFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.GamePayout.Replace("%", null)) / 100;


                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.NetTurnover);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.PTWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.CommExpense);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.NetIncome);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgNormalPercentFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.Margin.Replace("%", null)) / 100;

                    nextRow++;
                    itemIndex++;
                }
                

                if (i == totalSheet - 1 && report.Quantity > 0)
                {
                    var numberTotalDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false);
                    var numberTotalFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false, true);
                    var numberTotalIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false);
                    var bgNormalGrandPercentFormat = ExcelFormat(Color.FromArgb(162, 212, 247), HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, false, true);
                    var GrandTotal = report.GrandTotal;
                    var nextColGrandTotal = 0;

                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol).Style = numberTotalDefaultFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol).Value = GrandTotal.Currency;


                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.Turnover);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.MemberWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = bgNormalGrandPercentFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.GamePayout.Replace("%", null)) / 100;
           
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.NetTurnover);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.PTWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.CommExpense);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.NetIncome);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = bgNormalGrandPercentFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.Margin.Replace("%", null)) / 100;
             
                    nextRow = 0;
                }
            
            }
        }
        #endregion

        #region Don't Show Comm for Main
        private void ExportNumberGameWinLossByCurMainNotCommAssignHeader(int startRow, int startCol, ref ExcelWorksheet wsReport, string fromDate, string toDate)
        {
            var bgHeaderFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.Currency;


            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.Turnover;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.MemberWinLoss;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.GamePayout;

            #region Company
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 4).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 4).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 4).Value = Language.Company;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.NetTurnover;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.PTWinLoss;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.NetIncome;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.Margin;

            #endregion

            #region report title
            var title = Language.NumberGameWinLossByCurrencyMain;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Value = string.Format("{0} From {1:MM/dd/yyyy} To {2:MM/dd/yyyy}", title, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate));
            #endregion
        }
        private void ExportNumberGameWinLossByCurMainNotCommAssignBody(int startRow, int startCol, int maxItemsInSheet, ref ExcelFile ExcelFile, NumberGameWinLossByCurrency_MainBO report, NumberGameWinLossByCurrency_MainParams parameters)
        {
            var totalSheet = (report.Quantity % maxItemsInSheet > 0 ? 1 : 0) + report.Quantity / maxItemsInSheet;
            var numberDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false);
            var numberIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(253, 252, 252), false, false);
            var numberFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false, true);
            var bgPercentFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, false, true);
            var bgNormalPercentFormat = ExcelFormat(Color.FromArgb(255, 255, 255), HorizontalAlignmentStyle.Right, ExcelFont.NormalWeight, false, true);
            var nextRow = 0;
            var itemIndex = 0;

            for (int i = 0; i < totalSheet; i++)
            {
                var wsReport = ExcelFile.Worksheets.Add(string.Format("NumberGameWinLossByCurrencyMain_{0}", i + 1));

                ExportNumberGameWinLossByCurMainNotCommAssignHeader(startRow, startCol, ref wsReport, parameters.FromDate, parameters.ToDate);

                var totalItemsInCurrentSheet = i < totalSheet - 1 ? maxItemsInSheet : (report.Quantity - (i * maxItemsInSheet));
                for (int j = 0; j < totalItemsInCurrentSheet; j++)
                {
                    var item = report.ListMember[itemIndex];
                    var nextCol = 0;

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = item.Currency;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.Turnover);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.MemberWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgNormalPercentFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.GamePayout.Replace("%", null)) / 100;


                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.NetTurnover);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.PTWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.NetIncome);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = bgNormalPercentFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.Margin.Replace("%", null)) / 100;

                    nextRow++;
                    itemIndex++;
                }


                if (i == totalSheet - 1 && report.Quantity > 0)
                {
                    var numberTotalDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false);
                    var numberTotalFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false, true);
                    var numberTotalIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false);
                    var bgNormalGrandPercentFormat = ExcelFormat(Color.FromArgb(162, 212, 247), HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, false, true);
                    var GrandTotal = report.GrandTotal;
                    var nextColGrandTotal = 0;

                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol).Style = numberTotalDefaultFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol).Value = GrandTotal.Currency;


                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.Turnover);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.MemberWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = bgNormalGrandPercentFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.GamePayout.Replace("%", null)) / 100;

                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.NetTurnover);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.PTWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.NetIncome);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = bgNormalGrandPercentFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(GrandTotal.Margin.Replace("%", null)) / 100;

                    nextRow = 0;
                }

            }
        }
        #endregion

        #endregion

        #region Export for Detail

        public void ExportExcelNumberGameWinLossByCurDetail(NumberGameWinLossByCurrency_DetailParams DetailParams)
        {
            NumberGameWinLossByCurrency_DetailBO Report = new NumberGameWinLossByCurrency_DetailBO();

            Report = GetNumberGameDetailData(DetailParams);

            var StarCols = 0;
            var StarRows = 3;

            var ReportName = "NumberGameWinLossByCurrencyDetail";
            var ExcelFile = new ExcelFile();

            var MaxItemsInSheet = 3000;

            if (Report.Quantity > 0)
            {
                ExportExcelNumberGameWinLossByCurDetail(StarRows, StarCols, MaxItemsInSheet, ref ExcelFile, Report, DetailParams);
            }
            else
            {
                var errorMsg = Report.Quantity == -1 ?
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "TimeOutError") :
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblThereisnodata");
                var wsReport = ExcelFile.Worksheets.Add(ReportName);
                wsReport.Cells[StarRows - 1, StarCols].Value = errorMsg;
            }

            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", ReportName));
            ExcelFile.SaveXls(response.OutputStream);
        }
        private void ExportExcelNumberGameWinLossByCurDetail(int startRow, int startCol, int maxItemsInSheet, ref ExcelFile ExcelFile, NumberGameWinLossByCurrency_DetailBO report, NumberGameWinLossByCurrency_DetailParams parameters)
        {
            if (parameters.IsShowComm)
            {
                ExportExcelNumberGameWinLossByCurDetailAssignBody(startRow, startCol, maxItemsInSheet, ref ExcelFile,report, parameters);
            }
            else
            {
                ExportExcelNumberGameWinLossByCurDetailNotCommAssignBody(startRow, startCol, maxItemsInSheet, ref ExcelFile, report, parameters);
            }
        }

        #region Show Comm for Detail
        private void ExportExcelNumberGameWinLossByCurDetailAssignHeader(int startRow, int startCol, ref ExcelWorksheet wsReport, string fromDate, string toDate)
        {
            var bgHeaderFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.Username;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.Turnover;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.GrossComm;

            #region Member
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Value = Language.Member;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.WinLoss;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.Comm;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.Total;

            #endregion

            #region Agent
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Value = Language.Agent;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.WinLoss;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.Comm;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.Total;

            #endregion

            #region Master
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Value = Language.Master;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.WinLoss;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.Comm;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.Total;

            #endregion

            #region Super
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Value = Language.Super;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.WinLoss;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.Comm;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.Total;

            #endregion

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.Company;

            #region report title
            var title = Language.NumberGameWinLossByCurrencyDetail;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Value = string.Format("{0} From {1:MM/dd/yyyy} To {2:MM/dd/yyyy}", title, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate));
            #endregion
        }
        private void ExportExcelNumberGameWinLossByCurDetailAssignBody(int startRow, int startCol, int maxItemsInSheet, ref ExcelFile ExcelFile, NumberGameWinLossByCurrency_DetailBO report, NumberGameWinLossByCurrency_DetailParams parameters)
        {
            var totalSheet = (report.Quantity % maxItemsInSheet > 0 ? 1 : 0) + report.Quantity / maxItemsInSheet;
            var numberDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false);
            var numberIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(253, 252, 252), false, false);
            var numberFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false, true);
            var nextRow = 0;
            var itemIndex = 0;

            for (int i = 0; i < totalSheet; i++)
            {
                var wsReport = ExcelFile.Worksheets.Add(string.Format("NumberGameWinLossByCurrencyDetail_{0}", i + 1));

                ExportExcelNumberGameWinLossByCurDetailAssignHeader(startRow, startCol, ref wsReport,  parameters.FromDate, parameters.ToDate);

                var totalItemsInCurrentSheet = i < totalSheet - 1 ? maxItemsInSheet : (report.Quantity - (i * maxItemsInSheet));
                for (int j = 0; j < totalItemsInCurrentSheet; j++)
                {
                    var item = report.ListUsername[itemIndex];
                    var nextCol = 0;

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = item.Username;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.Turnover);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.GrossComm);


                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.MemberWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.MemberComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.MemberTotal);

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.AgentWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.AgentComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.AgentTotal);

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.MasterWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.MasterComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.MasterTotal);

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.SuperWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.SuperComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.SuperTotal);

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.Company);


                    nextRow++;
                    itemIndex++;
                }


                if (i == totalSheet - 1 && report.Quantity > 0)
                {
                    var numberTotalDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false);
                    var numberTotalFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false, true);
                    var numberTotalIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false);
                    var bgNormalGrandPercentFormat = ExcelFormat(Color.FromArgb(162, 212, 247), HorizontalAlignmentStyle.Right, ExcelFont.BoldWeight, false, true);
                    var Total = report.Total;
                    var nextColGrandTotal = 0;

                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol).Style = numberTotalDefaultFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol).Value = Total.Username;


                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.Turnover);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.GrossComm);


                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.MemberWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.MemberComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.MemberTotal);

                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.AgentWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.AgentComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.AgentTotal);

                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.MasterWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.MasterComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.MasterTotal);

                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.SuperWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.SuperComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.SuperTotal);

                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.Company);

                    nextRow = 0;
                }

            }
        }
        #endregion

        #region Don't Show Comm for Detail
        private void ExportExcelNumberGameWinLossByCurDetailNotCommAssignHeader(int startRow, int startCol, ref ExcelWorksheet wsReport, string fromDate, string toDate)
        {
            var bgHeaderFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.Username;

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.Turnover;


            #region Member
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 1 ).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 1).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 1).Value = Language.Member;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.WinLoss;
            #endregion

            #region Agent
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 1).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 1).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 1).Value = Language.Agent;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.WinLoss;

            #endregion

            #region Master
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 1).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 1).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 1).Value = Language.Master;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.WinLoss;


            #endregion

            #region Super
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 1).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 1).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 1).Value = Language.Super;

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value = Language.WinLoss;


            #endregion

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value = Language.Company;

            #region report title
            var title = Language.NumberGameWinLossByCurrencyDetail;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Value = string.Format("{0} From {1:MM/dd/yyyy} To {2:MM/dd/yyyy}", title, CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate));
            #endregion
        }
        private void ExportExcelNumberGameWinLossByCurDetailNotCommAssignBody(int startRow, int startCol, int maxItemsInSheet, ref ExcelFile ExcelFile, NumberGameWinLossByCurrency_DetailBO report, NumberGameWinLossByCurrency_DetailParams parameters)
        {
            var totalSheet = (report.Quantity % maxItemsInSheet > 0 ? 1 : 0) + report.Quantity / maxItemsInSheet;
            var numberDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false);
            var numberIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(253, 252, 252), false, false);
            var numberFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false, true);
            var nextRow = 0;
            var itemIndex = 0;

            for (int i = 0; i < totalSheet; i++)
            {
                var wsReport = ExcelFile.Worksheets.Add(string.Format("NumberGameWinLossByCurrencyDetail_{0}", i + 1));

                ExportExcelNumberGameWinLossByCurDetailNotCommAssignHeader(startRow, startCol, ref wsReport, parameters.FromDate, parameters.ToDate);

                var totalItemsInCurrentSheet = i < totalSheet - 1 ? maxItemsInSheet : (report.Quantity - (i * maxItemsInSheet));
                for (int j = 0; j < totalItemsInCurrentSheet; j++)
                {
                    var item = report.ListUsername[itemIndex];
                    var nextCol = 0;

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = item.Username;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.Turnover);


                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.MemberWinLoss);

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.AgentWinLoss);

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.MasterWinLoss);

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.SuperWinLoss);


                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDouble(item.Company);


                    nextRow++;
                    itemIndex++;
                }


                if (i == totalSheet - 1 && report.Quantity > 0)
                {
                    var numberTotalDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false);
                    var numberTotalFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false, true);
                    var numberTotalIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false);

                    var Total = report.Total;
                    var nextColGrandTotal = 0;

                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol).Style = numberTotalDefaultFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol).Value = Total.Username;


                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.Turnover);


                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.MemberWinLoss);


                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.AgentWinLoss);


                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.MasterWinLoss);


                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.SuperWinLoss);


                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDouble(Total.Company);

                    nextRow = 0;
                }

            }
        }
        #endregion 
        #endregion
        #endregion
    }
}
