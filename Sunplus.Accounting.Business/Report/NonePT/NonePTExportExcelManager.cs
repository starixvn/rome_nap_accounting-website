using System;
using System.Collections;
using System.Data;

using DNAide.Core;
using ExcelUtil.Spreadsheet;

using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for OpenToteSettlementExportExceManager
/// </summary>

namespace BLL.Report.NonePT
{
    public class NonePTExportExcelManager : BaseManager
    {
        #region Export2Excel

        public ExcelFile ExportToExcel_NonePTMemSummary(string username, string currpage)
        {
            var excelFile = new ExcelFile();
            try
            {
                var dataTable = new DataTable();

                dataTable = (DataTable) GetCachedObjData("accrptnoneptmemsummary." + AppSession.Username);
                if (dataTable == null)
                {
                    var param = new Hashtable();
                    param.Add("username", username);
                    param.Add("pagenum", currpage);
                    param.Add("totalpages", 0);
                    param.Add("errcode", 0);
                    param.Add("errmsg", "");

                    dataTable = IBCProvider.ReportProvider.NonePT().NonePTMemSummary(param).ResultSet.Tables[0];
                }

                var gembox = new ExcelGemboxUtils();

                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                string filename = "NonePT-Members-Summary-" + DateTime.Today.ToShortDateString().Replace("/", "-");
                string strTitle = "";
                ExcelWorksheet ws = null;
                string tag = "NonePTMembers";
                ws = worksheets.Add(tag);

                #region Header

                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 8).Style = gembox.GetStyleTitle(211, 255, 200);
                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 8).Value =
                    "List of Members have Uplines without taking up Position Taking.";
                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 8).Merged = true;

                string[] arrheaders = {
                                          "#", "Currency", "Super", "Master", "Agent", "Member", "Bet Count",
                                          "Running TO Total", "TO Total"
                                      };

                for (int i = 0; i < arrheaders.Length; i++)
                {
                    ws.Cells[1, i].Value = arrheaders[i];
                    ws.Cells[1, i].Style = gembox.GetStyleTitle(181, 196, 208);
                }

                #endregion

                #region content

                int indexrow = 2;
                DataRow[] Rows = dataTable.Select("");
                string[] arrContent = {
                                          "currency", "super", "master", "agent", "member", "BetCount", "RunningTOTotal",
                                          "TOTotal"
                                      };
                int red = 0, green = 0, blue = 0;
                string supername = "", supername_p = "";
                int countsuper = 0;

                currpage = (currpage.Length == 0) ? "1" : currpage;

                int order = (CastAide.AsInt32(currpage) - 1)*1000 + 1;
                foreach (DataRow dr in Rows)
                {
                    supername_p = dr["super"].ToString();
                    if (supername != supername_p)
                    {
                        if (countsuper%2 == 0)
                        {
                            red = 255;
                            green = 255;
                            blue = 255;
                        }
                        else
                        {
                            red = 223;
                            green = 234;
                            blue = 255;
                        }
                        supername = supername_p;
                        countsuper++;
                    }

                    ws.Cells[indexrow, 0].Value = order;
                    ws.Cells[indexrow, 0].Style = gembox.GetStyleIntNumber(0, "", red, green, blue);

                    for (int j = 0; j < arrContent.Length; j++)
                    {
                        if (j == 0)
                            ws.Cells[indexrow, j + 1].Value =
                                CacheUtils.GetExchangeName(CastAide.AsInt32(dr[arrContent[j]]));
                        else
                            ws.Cells[indexrow, j + 1].Value = dr[arrContent[j]];

                        if (j > 4)
                        {
                            if (j == 5)
                                ws.Cells[indexrow, j + 1].Style = gembox.GetStyleIntNumber(0, "", red, green, blue);
                            else
                                ws.Cells[indexrow, j + 1].Style = gembox.GetStyleNumber(0, "", red, green, blue);
                        }
                        else
                            ws.Cells[indexrow, j + 1].Style = gembox.GetStyleText(red, green, blue);
                    }
                    indexrow++;
                    order++;
                }

                #endregion

                #region footer

                ws.Cells.GetSubrangeAbsolute(indexrow, 0, indexrow, 5).Value = "Total";
                ws.Cells.GetSubrangeAbsolute(indexrow, 0, indexrow, 5).Style = gembox.GetStyleTextTotal(196, 208, 215);
                ws.Cells.GetSubrangeAbsolute(indexrow, 0, indexrow, 5).Merged = true;

                ws.Cells[indexrow, 6].Value = dataTable.Compute("sum(betcount)", "");
                ws.Cells[indexrow, 6].Style = gembox.GetStyleIntNumber(0, "", 196, 208, 215);

                ws.Cells.GetSubrangeAbsolute(indexrow, 7, indexrow, 8).Value = "";
                ws.Cells.GetSubrangeAbsolute(indexrow, 7, indexrow, 8).Style = gembox.GetStyleTextTotal(196, 208, 215);
                ws.Cells.GetSubrangeAbsolute(indexrow, 7, indexrow, 8).Merged = true;

                #endregion
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return excelFile;
        }

        #endregion
    }
}