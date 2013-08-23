using System;
using System.Collections;
using System.Data;
using System.Text;

using DNAide.Core;
using DNAide.Data;
using DNAide.Utils;
using DNAide.Web;

using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.Report;



/// <summary>
/// Summary description for FIWLDetails
/// </summary>

namespace BLL.Report.FI
{
    public class FIWLDetails : ReportBase
    {
        public string Key = "accreportfiwldetails." + AppSession.Username;

        public override string RenderHTML(Hashtable input)
        {
            var html = new StringBuilder();
            try
            {
                string tpl = WebUtils.LoadTemplate("WinLost/FIWLDetail.htm");
                string tplTotal = TemplateHandler.CopyBlock(tpl, "total");

                DbResult rs = null;
                DateTime fromdate = CastAide.AsDateTime(input["fromdate"]);
                DateTime todate = CastAide.AsDateTime(input["todate"]);

                TemplateHandler.Assign(ref tpl, "fdate", fromdate.ToShortDateString());
                TemplateHandler.Assign(ref tpl, "tdate", todate.ToShortDateString());

                input.Add("movebetsday", ReportUtils.BalanceUp.MoveBetsDay);

                rs = IBCProvider.FIWLDetails.FIWLDetails(input);

                DataTable dtFI = rs.ResultSet.Tables[0];
                //if (dtFI.Rows.Count == 0)
                //{
                //    string norow = Sunplus.Accounting.Core.Helper.WebUtils.LoadTemplate("NoInformation.htm");
                //    TemplateHandler.Assign(ref norow, "{tdno}", "8");
                //    TemplateHandler.Assign(ref norow, "{nodata}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"));
                //    TemplateHandler.RemoveBlock(ref tpl, "content");
                //    TemplateHandler.ReplaceBlock(ref tpl, "total", norow);
                //}
                //else
                //{

                #region Sort Data after got from DB

                dtFI.Columns.Add("IndexName", typeof(string));
                foreach (DataRow dr in dtFI.Rows)
                {
                    dr["IndexName"] = CacheRef.GetFncIndexName(dr["homeid"]);
                }

                DataView dvFI = dtFI.DefaultView;
                dvFI.Sort = "IndexName asc, homeid asc";
                DataTable dt = dvFI.ToTable();

                #endregion Sort Data after got from DB

                #region Declare Variable

                var rows = new StringBuilder();
                string tplRow = TemplateHandler.CopyBlock(tpl, "content");
                string index = "", index_p = "";
                int countIndex = 0;
                string indexName_p = "", indexName = "", strIndexTotal = "";
                ;

                #endregion Declare Variable

                #region foreach (DataRow dr in dt.Rows)

                string odd_even = "";
                foreach (DataRow dr in dt.Rows)
                {
                    countIndex++;
                    string row = tplRow;
                    index_p = CastAide.AsString(dr["homeid"], " ");
                    indexName_p = CastAide.AsString(dr["homeid"], " ");

                    if (index_p != index)
                    {
                        odd_even = (odd_even == "") ? "evenrow" : "";
                        rows.Replace("{rowspan}", countIndex.ToString());
                        TemplateHandler.Assign(ref row, "RowsSpanIndex",
                                               "<td class='c' rowspan='{rowspan}'>" +
                                               CacheRef.GetFncIndexName(indexName_p) + "</td>");
                        if (index.Length > 0)
                        {
                            strIndexTotal = tplTotal;
                            TemplateHandler.Assign(ref strIndexTotal, "{bgcolor}", "footer2");

                            TemplateHandler.Assign(ref strIndexTotal, "TotalIndex",
                                                   CacheRef.GetFncIndexName(indexName) + " Total");
                            TemplateHandler.Assign(ref strIndexTotal, "Turnover",
                                                   NumberFormat.DecFormat(
                                                       dt.Compute("sum(Turnover)", "homeid ='" + index + "'"), 2));
                            TemplateHandler.Assign(ref strIndexTotal, "WinLoss",
                                                   NumberFormat.DecFormat(
                                                       dt.Compute("sum(WinLost)", "homeid ='" + index + "'"), 2));
                            TemplateHandler.Assign(ref strIndexTotal, "Commission",
                                                   NumberFormat.DecFormat(
                                                       dt.Compute("sum(scomm)", "homeid ='" + index + "'"), 2));
                            //"IndexName=" + index
                            TemplateHandler.Assign(ref strIndexTotal, "NetWinLoss",
                                                   NumberFormat.DecFormat(
                                                       dt.Compute("sum(NetWinLost)", "homeid ='" + index + "'"), 2));
                            TemplateHandler.Assign(ref strIndexTotal, "BetsForTradersWL",
                                                   NumberFormat.DecFormat(
                                                       dt.Compute("sum(BetsForTrader)", "homeid ='" + index + "'"), 2));
                            TemplateHandler.Assign(ref strIndexTotal, "IBCWinLoss",
                                                   NumberFormat.DecFormat(
                                                       dt.Compute("sum(IBCWinLost)", "homeid ='" + index + "'"), 2));

                            rows.Append(strIndexTotal);
                        }

                        index = index_p;
                        indexName = indexName_p;
                        countIndex = 0;
                    }

                    TemplateHandler.Assign(ref row, "RowsSpanIndex", "");
                    TemplateHandler.Assign(ref row, "report-row", odd_even);
                    TemplateHandler.Assign(ref row, "Index", (dr["homeid"]));
                    TemplateHandler.Assign(ref row, "BetType", CacheRef.GetBettype(dr["bettype"]));
                    TemplateHandler.Assign(ref row, "Turnover", NumberFormat.DecFormat(dr["Turnover"], 2));
                    TemplateHandler.Assign(ref row, "WinLoss", NumberFormat.DecFormat(dr["WinLost"], 2));
                    TemplateHandler.Assign(ref row, "Commission", NumberFormat.DecFormat(dr["scomm"], 2));
                    TemplateHandler.Assign(ref row, "NetWinLoss", NumberFormat.DecFormat(dr["NetWinLost"], 2));
                    TemplateHandler.Assign(ref row, "BetsForTradersWL", NumberFormat.DecFormat(dr["BetsForTrader"], 2));
                    TemplateHandler.Assign(ref row, "IBCWinLoss", NumberFormat.DecFormat(dr["IBCWinLost"], 2));
                    rows.Append(row);
                }

                #endregion foreach (DataRow dr in dt.Rows)

                #region Totla Index

                strIndexTotal = tplTotal;
                TemplateHandler.Assign(ref strIndexTotal, "{bgcolor}", "footer1");
                TemplateHandler.Assign(ref strIndexTotal, "TotalIndex", CacheRef.GetFncIndexName(indexName_p) + " Total");
                TemplateHandler.Assign(ref strIndexTotal, "Turnover",
                                       NumberFormat.DecFormat(dt.Compute("sum(Turnover)", "homeid ='" + index + "'"), 2));
                TemplateHandler.Assign(ref strIndexTotal, "WinLoss",
                                       NumberFormat.DecFormat(dt.Compute("sum(WinLost)", "homeid ='" + index + "'"), 2));
                TemplateHandler.Assign(ref strIndexTotal, "Commission",
                                       NumberFormat.DecFormat(dt.Compute("sum(scomm)", "homeid ='" + index + "'"), 2));
                TemplateHandler.Assign(ref strIndexTotal, "NetWinLoss",
                                       NumberFormat.DecFormat(dt.Compute("sum(NetWinLost)", "homeid ='" + index + "'"),
                                                              2));
                TemplateHandler.Assign(ref strIndexTotal, "BetsForTradersWL",
                                       NumberFormat.DecFormat(
                                           dt.Compute("sum(BetsForTrader)", "homeid ='" + index + "'"), 2));
                TemplateHandler.Assign(ref strIndexTotal, "IBCWinLoss",
                                       NumberFormat.DecFormat(dt.Compute("sum(IBCWinLost)", "homeid ='" + index + "'"),
                                                              2));
                rows.Replace("{rowspan}", (countIndex + 1).ToString());
                rows.Append(strIndexTotal);

                #endregion Totla Index

                #region Grand Total

                TemplateHandler.Assign(ref tpl, "TotalIndex", "Grand Total");
                TemplateHandler.Assign(ref tpl, "Turnover", NumberFormat.DecFormat(dt.Compute("sum(Turnover)", ""), 2));
                TemplateHandler.Assign(ref tpl, "WinLoss", NumberFormat.DecFormat(dt.Compute("sum(WinLost)", ""), 2));
                TemplateHandler.Assign(ref tpl, "Commission", NumberFormat.DecFormat(dt.Compute("sum(scomm)", ""), 2));
                TemplateHandler.Assign(ref tpl, "NetWinLoss",
                                       NumberFormat.DecFormat(dt.Compute("sum(NetWinLost)", ""), 2));
                TemplateHandler.Assign(ref tpl, "BetsForTradersWL",
                                       NumberFormat.DecFormat(dt.Compute("sum(BetsForTrader)", ""), 2));
                TemplateHandler.Assign(ref tpl, "IBCWinLoss",
                                       NumberFormat.DecFormat(dt.Compute("sum(IBCWinLost)", ""), 2));

                #endregion Grand Total

                TemplateHandler.Assign(ref tpl, "{bgcolor}", "report-footer");
                TemplateHandler.ReplaceBlock(ref tpl, "content", rows.ToString());
                //}
                TemplateHandler.Assign(ref tpl, "bettype", "Finance");
                TemplateHandler.Assign(ref tpl, "wldate", fromdate.ToShortDateString());
                html.Append(tpl);

                SetCacheReport(Key, html.ToString(), DateTime.Now.AddHours(5));
            }
            catch (InvalidOperationException invalidOEx)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(invalidOEx);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                html.Append(ex.Message);
            }

            return html.ToString();
        }

        public void Export(Hashtable input)
        {
            FileName = "FIWLDetails_" + CastAide.AsDateTime(input["fromdate"]).ToShortDateString() + "_" +
                       CastAide.AsDateTime(input["todate"]).ToShortDateString();
            input.Add(FileName, FileName);
            string str = GetCachedData(Key);
            if (str == null)
                str = RenderHTML(input);
            ExcelContent = str;
            str.Replace("{border}", "1");
            Export2Excel(input);
        }
    }
}