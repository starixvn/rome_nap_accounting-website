using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Web;

using DNAide.Core;
using DNAide.Data;
using Microsoft.Office.Interop.Excel;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess;
using Sunplus.Core.LoggingLib;
using DataTable = System.Data.DataTable;

//using Font = Microsoft.Office.Interop.Excel.Font;

/// <summary>
/// Summary description for GenerateChartAgencyAnalysis
/// </summary>

namespace BLL.Report.Punter
{
    public class GenerateChartAgencyAnalysis : ReportBase
    {
        #region GetMaxDayByMonth

        private int GetMaxDayByMonth(string month)
        {
            int day = 0;
            switch (CastAide.AsInt32(month))
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    day = 31;
                    break;
                case 2:
                    day = 28;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    day = 30;
                    break;
            }
            return day;
        }

        #endregion

        #region GenerateChartByCustId

        private void CleanExcelProcess()
        {
            try
            {
                Process[] processesByName = Process.GetProcessesByName("EXCEL");
                for (int i = 0; i < processesByName.Length; i++)
                {
                    Process.GetProcessById(processesByName[i].Id).Kill();
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public string GenerateChartByCustId(string custId, string username, string fMonth, string fYear, string tMonth,
                                            string tYear)
        {
            string fromdate = fMonth + "/01/" + fYear;
            string todate = tMonth + "/" + GetMaxDayByMonth(tMonth) + "/" + tYear;
            string msg = "", fullpath = "";
            bool hashError = false;
            try
            {
                var hash = new Hashtable();
                hash.Add("fromdate", fromdate);
                hash.Add("todate", todate);

                if (custId.Length > 0)
                    hash.Add("custid", custId);
                else
                    hash.Add("total", 1);

                DbResult rs;
                var dataTable = new DataTable();
                var dtTotal = new DataTable();
                DataRow[] dataRows = {};
                int totalColumn = 0;

                try
                {
                    rs = IBCProvider.ReportProvider.PunterAnalysis().DirectCustomerReport(hash);
                    dataTable = rs.ResultSet.Tables[0];
                    totalColumn = dataTable.Columns.Count;
                    dataRows = dataTable.Select();
                }
                catch (Exception ex)
                {
                    hashError = true;
                    msg = ex.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }

                if (!hashError)
                {
                    #region export

                    var xls = new ExcelAutomationUtils();
                    string path = HttpContext.Current.Server.MapPath("TemplateAgencyAnalysisChart.xls");

                    Worksheet ws = null;
                    Workbook excelWorkbook = xls.OpenWorksheetFromTemplate(path, ref ws, "Sheet1");

                    #region Title

                    string value = "Analysis User " + username + " from " + fromdate + " to " + todate;
                    xls.SetValueRangeStyle(ws, 1, 1, 1, 11, value, true,
                                           ExcelAutomationUtils.ColorCode.White,
                                           ExcelAutomationUtils.ColorCode.Grey, XlHAlign.xlHAlignCenter,
                                           XlVAlign.xlVAlignCenter, true);

                    #endregion

                    #region Get Data to mapping chart

                    var dtData = new DataTable();
                    bool IsBeginColumn = false;
                    bool IsTotal = custId.Length == 0;
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        if (!IsBeginColumn)
                        {
                            if (dataTable.Columns[i].ColumnName.ToLower() == "name")
                            {
                                IsBeginColumn = true;
                            }
                        }

                        if (IsBeginColumn)
                        {
                            dtData.Columns.Add(dataTable.Columns[i].ColumnName, dataTable.Columns[i].DataType);
                        }
                    }


                    foreach (DataRow dr in dataRows)
                    {
                        DataRow drNewRow = dtData.NewRow();

                        if (IsTotal)
                        {
                            for (int i = 0; i < dr.ItemArray.Length; i++)
                            {
                                drNewRow[i] = dr[i];
                            }
                        }
                        else
                        {
                            for (int i = 4; i < dr.ItemArray.Length; i++)
                            {
                                drNewRow[i - 4] = dr[i];
                            }
                        }
                        dtData.Rows.Add(drNewRow);
                    }

                    #endregion

                    int rowindex = 53;

                    #region Header Data

                    var hashYear = new Hashtable();

                    //month
                    for (int i = 1; i < dtData.Columns.Count; i++)
                    {
                        string[] arr = dtData.Columns[i].ColumnName.Split('-');
                        {
                            xls.SetValueAtCellStyle(ws, rowindex - 1, i + 1, arr[1], true,
                                                    ExcelAutomationUtils.ColorCode.White,
                                                    ExcelAutomationUtils.ColorCode.Grey, XlHAlign.xlHAlignCenter,
                                                    XlVAlign.xlVAlignCenter);

                            if (hashYear.ContainsKey(arr[0]))
                                hashYear[arr[0]] = CastAide.AsInt32(hashYear[arr[0]]) + 1;
                            else
                                hashYear.Add(arr[0], 1);
                        }
                    }

                    //style for Name label
                    xls.SetValueRangeStyle(ws, 51, 1, 52, 1, "Values", true, ExcelAutomationUtils.ColorCode.White,
                                           ExcelAutomationUtils.ColorCode.Grey, XlHAlign.xlHAlignCenter,
                                           XlVAlign.xlVAlignCenter, true);

                    //year
                    xls.SetValueRangeStyle(ws, rowindex - 2, 2, rowindex - 2, CastAide.AsInt32(hashYear[fYear]) + 1,
                                           fYear, true, ExcelAutomationUtils.ColorCode.White,
                                           ExcelAutomationUtils.ColorCode.Grey, XlHAlign.xlHAlignCenter,
                                           XlVAlign.xlVAlignCenter, true);


                    if (fYear != tYear)
                    {
                        xls.SetValueRangeStyle(ws, rowindex - 2, CastAide.AsInt32(hashYear[fYear]) + 2, rowindex - 2,
                                               dtData.Columns.Count, tYear, true, ExcelAutomationUtils.ColorCode.White,
                                               ExcelAutomationUtils.ColorCode.Grey, XlHAlign.xlHAlignCenter,
                                               XlVAlign.xlVAlignCenter, true);
                    }

                    #endregion

                    #region Content Data

                    string symbol = "";
                    int index = 0;
                    foreach (DataRow dr in dtData.Select())
                    {
                        index++;
                        if (index == 3)
                            symbol = "%";
                        else
                            symbol = "";

                        for (int columnIndex = 0; columnIndex < dr.ItemArray.Length; columnIndex++)
                        {
                            if (CastAide.AsDecimal(dr[columnIndex]) < 0)
                                xls.SetValueAtCellStyleNumber(ws, rowindex, columnIndex + 1, dr[columnIndex], false,
                                                              ExcelAutomationUtils.ColorCode.Red,
                                                              ExcelAutomationUtils.ColorCode.White,
                                                              XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, symbol);
                            else
                                xls.SetValueAtCellStyleNumber(ws, rowindex, columnIndex + 1, dr[columnIndex], false,
                                                              ExcelAutomationUtils.ColorCode.Black,
                                                              ExcelAutomationUtils.ColorCode.White,
                                                              XlHAlign.xlHAlignRight, XlVAlign.xlVAlignCenter, symbol);
                        }
                        rowindex++;
                    }

                    #endregion

                    #region Saves the Excel file.

                    string user = "[" + AppSession.Username + "]";
                    string filename = "Analysis-Chart-User-" + username + "-" +
                                      DateTime.Today.ToShortDateString().Replace("/", "-");
                    //ReportingServices rpt = new ReportingServices();
                    //fullpath = rpt.GetPathExportStatement + "\\" + filename + user + ".xls";

                    //excelWorkbook.SaveCopyAs(fullpath);

                    base.workBook = excelWorkbook;
                    ExportExcelFile(filename);

                    CleanExcelProcess();

                    #endregion

                    #endregion
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                msg += " " + ex.Message;
            }

            var h = new Hashtable();
            h.Add("fullpath", fullpath);
            h.Add("errmsg", msg);
            return JsonUtil.ToJson(h);
        }

        /*
        public string GenerateChartByCustId_bk(string custId, string username, string fMonth, string fYear, string tMonth, string tYear)
        {

            string fromdate = fMonth + "/01/" + fYear;
            string todate = tMonth + "/" + GetMaxDayByMonth(tMonth).ToString() + "/" + tYear;
            string msg = "", fullpath = "";
            bool hashError = false;
            try
            {
                Hashtable hash = new Hashtable();
                hash.Add("fromdate", fromdate);
                hash.Add("todate", todate);

                if (custId.Length > 0)
                    hash.Add("custid", custId);
                else
                    hash.Add("total", 1);

                DbResult rs;
                DataTable dataTable = new DataTable();
                DataTable dtTotal = new DataTable();
                DataRow[] dataRows = { };
                int totalColumn = 0;

                try
                {
                    rs = IBCProvider.ReportProvider.PunterAnalysis().DirectCustomerReport(hash);
                    dataTable = rs.ResultSet.Tables[0];
                    totalColumn = dataTable.Columns.Count;
                    dataRows = dataTable.Select();
                }
                catch (Exception ex)
                {
                    hashError = true;
                    msg = ex.Message;
                }

                if (!hashError)
                {
                    #region export
                    string path = HttpContext.Current.Server.MapPath("TemplateAgencyAnalysisChart.xlsx");

                    ExcelFile ef = new ExcelFile();
                    // Loads template Excel file that contains chart on the empty dataset.
                    ef.LoadXlsx(path, XlsxOptions.PreserveKeepOpen);
                    // Selects the first worksheet.
                    ExcelWorksheet ws = ef.Worksheets[0];

                    ws.Cells.GetSubrangeAbsolute(0, 0, 0, 10).Style = GetStyleTitle();
                    ws.Cells.GetSubrangeAbsolute(0, 0, 0, 10).Merged = true;
                    ws.Cells.GetSubrangeAbsolute(0, 0, 0, 10).Value = "Analysis User " + username + " from " + fromdate + " to " + todate;

                    // Fills the data used to create chart.

                    int indexrow = 0;
                    int i = 52;//excel index row
                    string spec = "";

                    string year = "";
                    string strMonth = "";

                    ws.Cells.GetSubrangeAbsolute(i - 2, 0, i - 1, 0).Value = "Value";
                    ws.Cells.GetSubrangeAbsolute(i - 2, 0, i - 1, 0).Style = GetStyleTitle();
                    ws.Cells.GetSubrangeAbsolute(i - 2, 0, i - 1, 0).Merged = true;

                    Hashtable hashYear = new Hashtable();
                    if (custId.Length > 0) // by custid
                    {
                        ws.Cells.GetSubrangeAbsolute(i - 1, 0, i - 1, totalColumn - 5).Style = GetStyleTitle();

                        for (int j = 5; j < totalColumn; j++)
                        {
                            string[] arr = dataTable.Columns[j].ColumnName.Split('-');
                            strMonth = arr[1];
                            year = (arr[0]);

                            if (hashYear.ContainsKey(year))
                                hashYear[year] = CastAide.AsInt32(hashYear[year]) + 1;
                            else
                                hashYear.Add(year, 1);

                            ws.Cells[i - 1, j - 4].Value = strMonth;
                        }
                    }
                    else // TOTAL
                    {
                        ws.Cells.GetSubrangeAbsolute(i - 1, 0, i - 1, totalColumn - 1).Style = GetStyleTitle();
                        for (int j = 1; j < totalColumn; j++)
                        {
                            string[] arr = dataTable.Columns[j].ColumnName.Split('-');
                            strMonth = arr[1];
                            year = (arr[0]);

                            if (hashYear.ContainsKey(year))
                                hashYear[year] = CastAide.AsInt32(hashYear[year]) + 1;
                            else
                                hashYear.Add(year, 1);

                            ws.Cells[i - 1, j].Value = strMonth;
                        }
                    }

                    ws.Cells.GetSubrangeAbsolute(i - 2, 1, i - 2, CastAide.AsInt32(hashYear[fYear])).Value = fYear;
                    ws.Cells.GetSubrangeAbsolute(i - 2, 1, i - 2, CastAide.AsInt32(hashYear[fYear])).Style = GetStyleTitle();
                    ws.Cells.GetSubrangeAbsolute(i - 2, 1, i - 2, CastAide.AsInt32(hashYear[fYear])).Merged = true;

                    if (tYear != fYear)
                    {
                        ws.Cells.GetSubrangeAbsolute(i - 2, 1 + CastAide.AsInt32(hashYear[fYear]), i - 2, CastAide.AsInt32(hashYear[fYear]) + CastAide.AsInt32(hashYear[tYear])).Value = tYear;
                        ws.Cells.GetSubrangeAbsolute(i - 2, 1 + CastAide.AsInt32(hashYear[fYear]), i - 2, CastAide.AsInt32(hashYear[fYear]) + CastAide.AsInt32(hashYear[tYear])).Style = GetStyleTitle();
                        ws.Cells.GetSubrangeAbsolute(i - 2, 1 + CastAide.AsInt32(hashYear[fYear]), i - 2, CastAide.AsInt32(hashYear[fYear]) + CastAide.AsInt32(hashYear[tYear])).Merged = true;
                    }

                    if (custId.Length > 0) // by custid
                    {
                        foreach (DataRow dr in dataRows)
                        {
                            if (indexrow == 2)
                                spec = "%";
                            else
                                spec = "";

                            for (int j = 4; j < totalColumn; j++)
                            {
                                if (dr[j].ToString().Length > 0)
                                    ws.Cells[i, j - 4].Value = dr[j];
                                if (j == 4)
                                    ws.Cells[i, j - 4].Style = GetStyleText();
                                else
                                    ws.Cells[i, j - 4].Style = GetStyleNumber(CastAide.AsDecimal(dr[j]), spec);

                            }
                            i++;
                            indexrow++;
                        }
                    }
                    else //TOTAL
                    {
                        foreach (DataRow dr in dataRows)
                        {
                            if (indexrow == 2)
                                spec = "%";
                            else
                                spec = "";

                            for (int j = 0; j < totalColumn; j++)
                            {
                                ws.Cells[i, j].Value = dr[j];
                                if (j == 0)
                                    ws.Cells[i, j].Style = GetStyleText();
                                else
                                    ws.Cells[i, j].Style = GetStyleNumber(CastAide.AsDecimal(dr[j]), spec);

                            }
                            i++;
                            indexrow++;
                        }
                    }

                    // Saves the Excel file.
                    string user = "[" + AppSession.Username + "]";
                    string filename = "Analysis-Chart-User-" + username + "-" + DateTime.Today.ToShortDateString().Replace("/", "-");
                    ReportingServices rpt = new ReportingServices();
                    fullpath = rpt.GetPathExportStatement + "\\" + filename + user + ".xls";

                    ef.SaveXlsx(fullpath);
                    ef.ClosePreservedXlsx();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                msg += " " + ex.Message;
            }

            Hashtable h = new Hashtable();
            h.Add("fullpath", fullpath);
            h.Add("errmsg", msg);
            return JsonUtil.ToJson(h);
        }
         * */

        #endregion

        public override string RenderHTML(Hashtable input)
        {
            throw new NotImplementedException();
        }
    }
}