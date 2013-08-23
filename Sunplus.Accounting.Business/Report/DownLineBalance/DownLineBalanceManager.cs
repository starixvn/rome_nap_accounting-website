using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

using DNAide.Core;
using DNAide.Utils;
using DNAide.Web;

using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.Report;

//using Sunplus.Core.LoggingLib;

namespace BLL.Report.DownLineBalance
{
    public class DownLineBalanceManager : ReportBase
    {
        private string msg = "<span class='error-message'>{content}</span>";

        public string LoadHeader()
        {
            string header = WebUtils.LoadTemplate("Report/NonePT/NonePTHeader.htm");
            return header;
        }

        public string GetCompanyBalanceHistory()
        {
            string tpl = WebUtils.LoadTemplate("Balance/CompBalanceDiff.htm");
            try
            {
                //applied language
                TemplateHandler.Assign(ref tpl, "{No}", "No");
                TemplateHandler.Assign(ref tpl, "{BalanceUpDay}", "Balance-Up Day");
                TemplateHandler.Assign(ref tpl, "{CompBeforeBU}", "Company Balance Before BU");
                TemplateHandler.Assign(ref tpl, "{CompAfterBU}", "Company Balance After BU");
                TemplateHandler.Assign(ref tpl, "{Difference}", "Difference");
                TemplateHandler.Assign(ref tpl, "{date}", ReportUtils.BalanceUp.BalanceUpDay.ToString("MMMM dd"));
                //Get data
                DataTable dt = null;
                dt = IBCProvider.ReportProvider.DownLineBalance().BalanceCompHistory().ResultSet.Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    string tplRow = TemplateHandler.CopyBlock(tpl, "Rows");
                    string odd_even = "";
                    DataRow[] rows = dt.Select();
                    bool flag = false;
                    decimal decDiff = 0;
                    var stringBuilder = new StringBuilder();
                    int i = 0;
                    foreach (DataRow dr in rows)
                    {
                        i++;
                        string row = tplRow;
                        odd_even = (odd_even == "") ? "evenrow" : "";
                        if (flag == false)
                        {
                            if (ReportUtils.BalanceUp.BalanceUpDay.CompareTo(CastAide.AsDateTime(dr["balanceupday"])) == 0)
                            {
                                flag = true;
                                decDiff = CastAide.AsDecimal(dr["difference"]);
                            }
                        }
                        TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                        TemplateHandler.Assign(ref row, "{No_vl}", i.ToString());
                        TemplateHandler.Assign(ref row, "{BU_vl}",
                                               CastAide.AsDateTime(dr["BalanceUpDay"]).ToString("MM/dd/yyyy"));
                        TemplateHandler.Assign(ref row, "{ComBefore_vl}", NumberFormat.DecFormat(dr["CompBalBefore"], 2));
                        TemplateHandler.Assign(ref row, "{ComAfter_vl}", NumberFormat.DecFormat(dr["CompBalAfter"], 2));
                        TemplateHandler.Assign(ref row, "{Diff_vl}", NumberFormat.DecFormat(dr["Difference"], 2));
                        stringBuilder.Append(row);
                    }
                    //if got difference for this week then display the notice
                    if (flag)
                    {
                        TemplateHandler.Assign(ref tpl, "{diff}", NumberFormat.DecFormat(decDiff, 2));
                    }
                    else
                    {
                        TemplateHandler.RemoveBlock(ref tpl, "DiffMessage");
                    }

                    //replace rows
                    TemplateHandler.ReplaceBlock(ref tpl, "Rows", stringBuilder.ToString());
                }
                else
                {
                    string norow = WebUtils.LoadTemplate("NoInformation.htm");
                    TemplateHandler.Assign(ref norow, "{tdno}", "5");
                    TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
                    TemplateHandler.Assign(ref norow, "{nodata}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"));
                    TemplateHandler.ReplaceBlock(ref tpl, "Rows", norow);
                }
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return ex.Message;
            }
            return tpl;
        }

        public override string RenderHTML(Hashtable input)
        {
            return string.Empty;
        }

        #region MasterBalnce

        /// <summary>
        /// ...
        /// </summary>
        /// <param name="db"></param>
        /// <param name="roleid"></param>
        /// <param name="custid"></param>
        /// <param name="isYesterday"></param>
        /// <returns></returns>
        public string MasterBalance(string db, int roleid, int custid, bool isYesterday, string usernameSearch,
                                    string firstletter, string currency)
        {
            if (string.IsNullOrEmpty(db)) db = "rep";
            var html = new StringBuilder();
            string norow = WebUtils.LoadTemplate("NoInformation.htm");
            try
            {
                TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
                TemplateHandler.Assign(ref norow, "{tdno}", "5");
                TemplateHandler.Assign(ref norow, "{nodata}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"));
                string username = "Master", title = "";
                if (roleid == 3)
                {
                    username = "Master";
                    title = WebUtils.LoadHtml("Balance/MasterBalanceTitle.htm", new[] { "{super_master}" },
                                              new[] { username });
                }
                else if (roleid == 2) username = "AGENT";
                else if (roleid == 1) username = "Member";

                html.Append(WebUtils.LoadHtml("Balance/MasterBalanceAllHeader.htm",
                                              new[] { "{user}", "{currentcy}", "{balance}", "{base}", "{title}" },
                                              new[] { username, ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Currency"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Balance"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Base"), title }));
                string rows = WebUtils.LoadTemplate("Balance/MasterBalanceAllRow.htm");
                DataTable dt =
                    IBCProvider.ReportProvider.DownLineBalance().RptMABalance(db, roleid, custid, isYesterday,
                                                                              usernameSearch, firstletter, currency).
                        ResultSet.Tables[0];
                string odd_even = "";
                int index = 0;
                double total = 0, totalbal = 0;
                string _options = "&isYesterday=" + ((isYesterday) ? "1" : "0");
                string IsRep = HttpContext.Current.Request["IsRep"];
                bool isBalExclude = false;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] dataRow = dt.Select();
                    Dictionary<int, string> exchange = CacheUtils.Exchange;
                    foreach (DataRow dr in dataRow)
                    {
                        isBalExclude = false;

                        string row = rows.Clone().ToString();

                        int role = roleid - 1;
                        string page = "&page=super";

                        if (string.IsNullOrEmpty(HttpContext.Current.Request["page"])) page = "";
                        string url = "<a href=\"MasterBalance.aspx?custid=" + Convert.ToString(dr["custid"]) +
                                     "&roleid=" + role + page + _options + "&IsRep=" + IsRep +
                                     "\" target=\"centerFr\">" + Convert.ToString(dr["username"]) + "</a>";
                        if (role == 0) url = Convert.ToString(dr["username"]);
                        TemplateHandler.Assign(ref row, "{master}", url);
                        TemplateHandler.Assign(ref row, "{currentcy}",
                                               CacheUtils.GetExchangeName(exchange, CastAide.AsInt32(dr["currency"])));

                        double baseRM = 0;
                        if (roleid == 3 || roleid == 1)
                        {
                            if (CastAide.AsDecimal(NumberFormat.DecFormat(dr["balance"], 2)) == 0 &&
                                CastAide.AsDecimal(NumberFormat.DecFormat(dr["basebalance"], 2)) == 0)
                                isBalExclude = true;
                            else
                            {
                                //baseRM = FormatHandler.GetDouble(dr["balance"]) * FormatHandler.GetDouble(dr["ACTUALRATE"]);
                                baseRM = CastAide.AsDouble(dr["baseBalance"]);
                                TemplateHandler.Assign(ref row, "{balance}", NumberFormat.DecFormat(dr["balance"], 2));
                                total += baseRM;
                                totalbal += CastAide.AsDouble(dr["balance"]);
                            }
                        }
                        else if (roleid == 2)
                        {
                            if (CastAide.AsDecimal(NumberFormat.DecFormat(dr["bal"], 2)) == 0 &&
                                CastAide.AsDecimal(NumberFormat.DecFormat(dr["basebalance"], 2)) == 0)
                                isBalExclude = true;
                            else
                            {
                                baseRM = CastAide.AsDouble(dr["baseBalance"]);
                                TemplateHandler.Assign(ref row, "{balance}", NumberFormat.DecFormat(dr["bal"], 2));
                                total += baseRM;
                                totalbal += CastAide.AsDouble(dr["bal"]);
                            }
                        }
                        TemplateHandler.Assign(ref row, "{base}", NumberFormat.DecFormat(baseRM, 2));
                        if (isBalExclude)
                            continue;
                        else
                        {
                            index++;
                            odd_even = (odd_even == "") ? "evenrow" : "";
                            TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                            TemplateHandler.Assign(ref row, "{no}", index.ToString());
                            html.Append(row);
                        }
                    }
                    string bal = "";
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request["page"]) && roleid == 3)
                        bal = NumberFormat.DecFormat(totalbal, 2);
                    html.Append(WebUtils.LoadHtml("Balance/MasterBalanceTotalRow.htm", new[] { "{total}", "{totalbal}" },
                                                  new[] { NumberFormat.DecFormat(total, 2), bal }));
                }
                else html.Append(norow);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                string s = ex.Message;
                html.Append(norow);
                html.Append("</table>");
            }
            html.Append("</table>");
            return html.ToString();
        }

        #endregion MasterBalnce

        #region SuperBalance

        /// <summary>
        /// ....
        /// </summary>
        /// <param name="db"></param>
        /// <param name="isYesterday"></param>
        /// <param name="usernameSearch"></param>
        /// <param name="firstletter"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public string SuperBalance(string db, bool isYesterday, string usernameSearch, string firstletter,
                                   string currency)
        {
            var html = new StringBuilder();
            try
            {
                string title = WebUtils.LoadHtml("Balance/MasterBalanceTitle.htm", new[] { "{super_master}" },
                                                 new[] { "Super" });
                html.Append(WebUtils.LoadHtml("Balance/SuperBalanceAllHeader.htm",
                                              new[] { "{username}", "{currentcy}", "{balance}", "{base}", "{title}" },
                                              new[] { "Super", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Currency"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Balance"), ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Base"), title }));
                string rows = WebUtils.LoadTemplate("Balance/SuperBalanceAllRow.htm");
                var dt = new DataTable();

                var hash = new Hashtable();
                if (!string.IsNullOrEmpty(usernameSearch) && !usernameSearch.Equals("All"))
                    hash.Add("keyword", usernameSearch);
                if (!string.IsNullOrEmpty(firstletter) && !firstletter.Equals("All"))
                    hash.Add("firstchar", firstletter);
                if (!string.IsNullOrEmpty(currency))
                    hash.Add("currency", currency);

                dt =
                    IBCProvider.ReportProvider.DownLineBalance().RptSABalance(db, isYesterday, hash).ResultSet.Tables[0];
                //super

                string odd_even = "";
                int index = 0;
                double total = 0;
                string _options = "&isYesterday=" + ((isYesterday) ? "1" : "0");
                _options += "&IsRep=" + ((db.Equals("rep")) ? "1" : "0");
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow[] dataRow = dt.Select();
                    Dictionary<int, string> exchange = CacheUtils.Exchange;
                    foreach (DataRow dr in dataRow)
                    {
                        if (CastAide.AsDecimal(NumberFormat.DecFormat(dr["balance"], 2)) == 0 &&
                            CastAide.AsDecimal(NumberFormat.DecFormat(dr["basebalance"], 2)) == 0)
                            continue;
                        else
                        {
                            index++;
                            odd_even = (odd_even == "") ? "evenrow" : "";
                            string row = rows.Clone().ToString();
                            string url = "";
                            TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                            url = "<a href=\"MasterBalance.aspx?custid=" + Convert.ToString(dr["custid"]) + "&roleid=3" +
                                  "&page=super" + _options +
                                  "\" target=\"centerFr\">" + Convert.ToString(dr["username"]) + "</a>";
                            TemplateHandler.Assign(ref row, "{no}", index.ToString());
                            TemplateHandler.Assign(ref row, "{username}", url);
                            //TemplateHandler.Assign(ref row, "{currentcy}", Convert.ToString(dr["currency"]));
                            TemplateHandler.Assign(ref row, "{currentcy}",
                                                   CacheUtils.GetExchangeName(exchange, CastAide.AsInt32(dr["currency"])));
                            TemplateHandler.Assign(ref row, "{balance}", NumberFormat.DecFormat(dr["balance"], 2));
                            TemplateHandler.Assign(ref row, "{base}", NumberFormat.DecFormat(dr["basebalance"], 2));
                            html.Append(row);
                            total += CastAide.AsDouble(dr["basebalance"]);
                        }
                    }

                    html.Append(WebUtils.LoadHtml("Balance/SuperBalanceTotalRow.htm", new[] { "{total}" },
                                                  new[] { NumberFormat.DecFormat(total, 2) }));
                }
                else
                {
                    string norow = WebUtils.LoadTemplate("NoInformation.htm");
                    TemplateHandler.Assign(ref norow, "{tdno}", "5");
                    TemplateHandler.Assign(ref norow, "{nodata}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"));
                    html.Append(norow);
                }
                html.Append("</table>");
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return html.ToString();
        }

        #endregion SuperBalance
    }
}