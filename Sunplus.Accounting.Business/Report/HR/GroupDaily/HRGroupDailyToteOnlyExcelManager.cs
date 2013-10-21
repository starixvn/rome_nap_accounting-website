using System;
using System.Collections;
using System.Data;
using DNAide.Core;
using DNAide.Data;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess;

/// <summary>
/// Summary description for HRGroupDailyReport
/// </summary>
namespace BLL.Report.HR.GroupDaily
{
    public class HRGroupDailyToteOnlyExcelManager : ReportBase
    {
        #region ExcelAssignTotal_HRGroupDaily_ToteOnly

        private void ExcelAssignTotal_HRGroupDaily_ToteOnly(ref ExcelWorksheet ws, DataTable dtGroup, string condition, string title, int red, int green, int blue, ref int indexRow, ExcelGemboxUtils gembox)
        {
            string[] arr = {"turnoverAgents", "turnoverIBC", "turnover3rd", "totalTurnover",
                            "incomeIBCPT", "incomeIBCNetComm", "totalIBCIncome",
                            "income12BETPT", "income12BETComm", "total12BETIncome",
                            "groupTotalIncome",
                            };

            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 1).Value = title + " Total";
            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 1).Style = gembox.GetStyleTextTotal(red, green, blue);
            ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow, 1).Merged = true;

            for (int i = 0; i < arr.Length; i++)
            {
                ws.Cells[indexRow, i + 2].Value = CastAide.AsDecimal(dtGroup.Compute("sum(" + arr[i] + ")", condition));
                ws.Cells[indexRow, i + 2].Style = gembox.GetStyleNumberTotal(0, "", red, green, blue);
            }

            indexRow++;
        }

        #endregion ExcelAssignTotal_HRGroupDaily_ToteOnly

        #region ExcelAssignRow_HRGroupDaily_ToteOnly

        private void ExcelAssignRow_HRGroupDaily_ToteOnly(ref ExcelWorksheet ws, DataRow dr, int red, int green, int blue, ref int indexRow, ExcelGemboxUtils gembox)
        {
            string[] arr = { "HRType",
                            "turnoverAgents", "turnoverIBC", "turnover3rd", "totalTurnover",
                            "incomeIBCPT", "incomeIBCNetComm", "totalIBCIncome",
                            "income12BETPT", "income12BETComm", "total12BETIncome",
                            "groupTotalIncome"};

            for (int i = 0; i < arr.Length; i++)
            {
                if (i == 0)
                {
                    ws.Cells[indexRow, i + 1].Value = ((CastAide.AsInt32(dr[arr[i]]) == 1) ? "Tote" : "Unknow");
                    ws.Cells[indexRow, i + 1].Style = gembox.GetStyleTextCenter(red, green, blue);
                }
                else
                {
                    ws.Cells[indexRow, i + 1].Value = dr[arr[i]];
                    ws.Cells[indexRow, i + 1].Style = gembox.GetStyleNumber(0, "", red, green, blue);
                }
            }
            indexRow++;
        }

        #endregion ExcelAssignRow_HRGroupDaily_ToteOnly

        #region ExportExcelHRGroupDaily_ToteOnly

        public ExcelFile ExportExcelHRGroupDaily_ToteOnly(string fromdate, string todate)
        {
            ExcelFile excelFile = new ExcelFile();
            try
            {
                DataTable dtGroup = new DataTable();
                dtGroup = (DataTable)GetCachedObjData(HRGroupDailyCommon.keyCacheHRGroupDaily_ToteOnly);
                if (dtGroup == null)
                {
                    #region getdate

                    DateTime fdate = CastAide.AsDateTime(fromdate);
                    DateTime tdate = CastAide.AsDateTime(todate);

                    DbResult rsGroup = null, rsGroupDaily = null;

                    Hashtable param = new Hashtable();
                    if (fdate == tdate && tdate == DateTime.Today)
                    {
                        param = new Hashtable();
                        param.Add("winlostdate", tdate);
                        dtGroup = IBCProvider.ReportProvider.HRGroupDaily().WinlostHR_Group_Daily(param).ResultSet.Tables[0];
                    }
                    else
                    {
                        param = new Hashtable();
                        param.Add("fromdate", fdate);
                        param.Add("todate", tdate);
                        rsGroup = IBCProvider.ReportProvider.HRGroupDaily().WinlostHR_Group(param);
                        if (tdate >= DateTime.Today)
                        {
                            param = new Hashtable();
                            param.Add("winlostdate", tdate);
                            rsGroupDaily = IBCProvider.ReportProvider.HRGroupDaily().WinlostHR_Group_Daily(param);
                        }

                        #region Combine group daily into group

                        dtGroup = rsGroup.ResultSet.Tables[0];
                        if (rsGroupDaily != null)
                        {
                            DataTable dtGroupDaily = rsGroupDaily.ResultSet.Tables[0];
                            foreach (DataRow drGroupDaily in dtGroupDaily.Rows)
                            {
                                DataRow drGroup = dtGroup.NewRow();
                                for (int i = 0; i < drGroupDaily.ItemArray.Length; i++)
                                {
                                    drGroup[i] = drGroupDaily[i];
                                }
                                dtGroup.Rows.Add(drGroup);
                            }
                        }

                        #endregion Combine group daily into group
                    }

                    #endregion getdate
                }

                #region init excel

                ExcelGemboxUtils gembox = new ExcelGemboxUtils();

                ExcelWorksheetCollection worksheets = excelFile.Worksheets;
                string filename = "WinlostHR_OpenTote-" + DateTime.Today.ToShortDateString().Replace("/", "-");
                ExcelWorksheet ws = null;
                string tag = "Horse Racing Group Daily";
                ws = worksheets.Add(tag);

                #endregion init excel

                #region Header

                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 12).Style = gembox.GetStyleTitle(255, 171, 171);
                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 12).Value = "Horse Racing Group Daily from " + fromdate + " to " + todate;
                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 12).Merged = true;

                ws.Cells.GetSubrangeAbsolute(1, 0, 2, 0).Style = gembox.GetStyleTitle(216, 221, 226);
                ws.Cells.GetSubrangeAbsolute(1, 0, 2, 0).Value = "Date";
                ws.Cells.GetSubrangeAbsolute(1, 0, 2, 0).Merged = true;

                ws.Cells.GetSubrangeAbsolute(1, 1, 2, 1).Style = gembox.GetStyleTitle(216, 221, 226);
                ws.Cells.GetSubrangeAbsolute(1, 1, 2, 1).Value = "Partner Type";
                ws.Cells.GetSubrangeAbsolute(1, 1, 2, 1).Merged = true;

                ws.Cells.GetSubrangeAbsolute(1, 2, 1, 5).Style = gembox.GetStyleTitle(208, 232, 251);
                ws.Cells.GetSubrangeAbsolute(1, 2, 1, 5).Value = "Turnover (RM)";
                ws.Cells.GetSubrangeAbsolute(1, 2, 1, 5).Merged = true;

                ws.Cells[2, 2].Value = "Agency";
                ws.Cells[2, 2].Style = gembox.GetStyleTitle(208, 232, 251);

                ws.Cells[2, 3].Value = "IBC";
                ws.Cells[2, 3].Style = gembox.GetStyleTitle(208, 232, 251);

                ws.Cells[2, 4].Value = "Partner";
                ws.Cells[2, 4].Style = gembox.GetStyleTitle(208, 232, 251);

                ws.Cells[2, 5].Value = "Total TO";
                ws.Cells[2, 5].Style = gembox.GetStyleTitle(208, 232, 251);

                ws.Cells.GetSubrangeAbsolute(1, 6, 1, 8).Style = gembox.GetStyleTitle(216, 221, 226);
                ws.Cells.GetSubrangeAbsolute(1, 6, 1, 8).Value = "IBC (RM)";
                ws.Cells.GetSubrangeAbsolute(1, 6, 1, 8).Merged = true;

                ws.Cells[2, 6].Value = "PT";
                ws.Cells[2, 6].Style = gembox.GetStyleTitle(216, 221, 226);

                ws.Cells[2, 7].Value = "Net Comm";
                ws.Cells[2, 7].Style = gembox.GetStyleTitle(216, 221, 226);

                ws.Cells[2, 8].Value = "Total Income";
                ws.Cells[2, 8].Style = gembox.GetStyleTitle(216, 221, 226);

                ws.Cells.GetSubrangeAbsolute(1, 9, 1, 11).Style = gembox.GetStyleTitle(208, 232, 251);
                ws.Cells.GetSubrangeAbsolute(1, 9, 1, 11).Value = "12BET (RM)";
                ws.Cells.GetSubrangeAbsolute(1, 9, 1, 11).Merged = true;

                ws.Cells[2, 9].Value = "PT";
                ws.Cells[2, 9].Style = gembox.GetStyleTitle(208, 232, 251);

                ws.Cells[2, 10].Value = "Comm";
                ws.Cells[2, 10].Style = gembox.GetStyleTitle(208, 232, 251);

                ws.Cells[2, 11].Value = "Total Income";
                ws.Cells[2, 11].Style = gembox.GetStyleTitle(208, 232, 251);

                ws.Cells.GetSubrangeAbsolute(1, 12, 2, 12).Style = gembox.GetStyleTitle(216, 221, 226);
                ws.Cells.GetSubrangeAbsolute(1, 12, 2, 12).Value = "Group Total Income";
                ws.Cells.GetSubrangeAbsolute(1, 12, 2, 12).Merged = true;

                #endregion Header

                #region Content

                string bettype = "", date_p = "", date_c = "";
                string condition = "", style = "";
                int countdate = 0;
                int countFixed = 0, rowindexDate = 0;

                int indexrow = 3;
                int red = 247, green = 240, blue = 247;

                Hashtable hashPartner = new Hashtable();
                foreach (DataRow dr in dtGroup.Rows)
                {
                    if (CastAide.AsInt32(dr["HRType"]) > 1) continue;

                    date_c = CastAide.AsDateTime(dr["winlostdate"]).ToShortDateString();

                    if (date_p != date_c)
                    {
                        rowindexDate++;

                        condition = "winlostdate='" + date_p + "' and HRType=1";

                        if (date_p.Length > 0)
                        {
                            countdate = CastAide.AsInt32(dtGroup.Compute("count(winlostdate)", condition));

                            ws.Cells.GetSubrangeAbsolute(indexrow - countdate, 0, indexrow - 1, 0).Merged = true;
                            ws.Cells.GetSubrangeAbsolute(indexrow - countdate, 0, indexrow - 1, 0).Value = date_p;
                            ws.Cells.GetSubrangeAbsolute(indexrow - countdate, 0, indexrow - 1, 0).Style = gembox.GetStyleTextCenter(red, green, blue);

                            if (rowindexDate % 2 != 0)
                            {
                                red = 247; green = 240; blue = 247;
                            }
                            else
                            {
                                red = 250; green = 250; blue = 243;
                            }
                        }

                        date_p = date_c;
                        countdate++;
                    }

                    ExcelAssignRow_HRGroupDaily_ToteOnly(ref ws, dr, red, green, blue, ref indexrow, gembox);
                }

                #endregion Content

                #region lastest total by date

                condition = "winlostdate='" + date_p + "' and HRType=1";
                countdate = CastAide.AsInt32(dtGroup.Compute("count(winlostdate)", condition));

                ws.Cells.GetSubrangeAbsolute(indexrow - countdate, 0, indexrow - 1, 0).Merged = true;
                ws.Cells.GetSubrangeAbsolute(indexrow - countdate, 0, indexrow - 1, 0).Value = date_p;
                ws.Cells.GetSubrangeAbsolute(indexrow - countdate, 0, indexrow - 1, 0).Style = gembox.GetStyleTextCenter(red, green, blue);

                #endregion lastest total by date

                #region colspan before cal grand total

                ws.Cells.GetSubrangeAbsolute(indexrow, 0, indexrow, 12).Merged = true;
                ws.Cells.GetSubrangeAbsolute(indexrow, 0, indexrow, 12).Style = gembox.GetStyleText(true);
                indexrow++;

                #endregion colspan before cal grand total

                #region grand total

                //grand total
                condition = "HRType=1";
                ExcelAssignTotal_HRGroupDaily_ToteOnly(ref ws, dtGroup, condition, "Grand", 196, 208, 215, ref indexrow, gembox);

                #endregion grand total
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            return excelFile;
        }

        #endregion ExportExcelHRGroupDaily_ToteOnly

        public override string RenderHTML(Hashtable input)
        {
            throw new NotImplementedException();
        }
    }
}