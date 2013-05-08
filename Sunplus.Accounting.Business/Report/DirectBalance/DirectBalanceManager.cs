using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using DNAide.Core;
using DNAide.Utils;
using DNAide.Web;

using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Core.LoggingLib;
//using Sunplus.Core.LoggingLib;


namespace BLL.Report.DirectBalance
{
    public class DirectBalanceManager : ReportBase
    {
        private string msg = "<span class='error-message'>{content}</span>";

        public string LoadHeader()
        {
            string header = WebUtils.LoadTemplate("Report/NonePT/NonePTHeader.htm");
            return header;
        }

        public string GetDirectBalance(string displayType)
        {
            string tpl = WebUtils.LoadTemplate("DirectBalance/DirectBalance.htm");
            //hard code db = rep (will get data from DBType.REPORT)
            string db = "rep";
            try
            {
                //Load Header
                TemplateHandler.Assign(ref tpl, "{No}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "No"));
                TemplateHandler.Assign(ref tpl, "{Curr}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Curr"));
                TemplateHandler.Assign(ref tpl, "{Balance}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Balance"));
                TemplateHandler.Assign(ref tpl, "{Base}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Base"));


                //Get data
                DataTable dt = null;
                if (displayType == "member")
                {
                    TemplateHandler.Assign(ref tpl, "{Username}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Member"));
                    TemplateHandler.Assign(ref tpl, "{DirectBalanceHeader}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "DirectMemberBalance"));
                    dt = IBCProvider.ReportProvider.DirectBalance().Balance_Direct_Member(db).ResultSet.Tables[0];
                }
                else if (displayType == "agent")
                {
                    TemplateHandler.Assign(ref tpl, "{Username}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent"));
                    TemplateHandler.Assign(ref tpl, "{DirectBalanceHeader}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "DirectAgentBalance"));
                    dt = IBCProvider.ReportProvider.DirectBalance().Balance_Direct_Agent(db).ResultSet.Tables[0];
                }
                else
                {
                    TemplateHandler.Assign(ref tpl, "{Username}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Master"));
                    TemplateHandler.Assign(ref tpl, "{DirectBalanceHeader}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "DirectMasterBalance"));
                    dt = IBCProvider.ReportProvider.DirectBalance().Balance_Direct_Master(db).ResultSet.Tables[0];
                }


                //Load Rows
                if (dt != null && dt.Rows.Count > 0)
                {
                    var html = new StringBuilder();
                    string tpl_row = TemplateHandler.CopyBlock(tpl, "Rows");
                    string odd_even = "";
                    int index = 1;
                    decimal dec_Total = 0;
                    Dictionary<int, string> exchange = CacheUtils.Exchange;
                    DataRow[] rows = dt.Select();
                    foreach (DataRow dr in rows)
                    {
                        dec_Total += CastAide.AsDecimal(dr["basebalance"]);



                        odd_even = index % 2 > 0 ? "" : "evenrow";

                        string row = tpl_row.Clone().ToString();
                        TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                        TemplateHandler.Assign(ref row, "{No_vl}", index.ToString());

                        //display link to view downline in username
                        if (displayType == "master")
                            TemplateHandler.Assign(ref row, "{Username_vl}",
                                                   "<span class='lnk' onclick='View_DL(3,\"" +
                                                   Convert.ToString(dr["username"]) + "\"," +
                                                   Convert.ToString(dr["custid"]) + ", \"\",0)'>" +
                                                   Convert.ToString(dr["username"]) + "</span>");
                        else if (displayType == "agent")
                            TemplateHandler.Assign(ref row, "{Username_vl}",
                                                   "<span class='lnk' onclick='View_DL(2,\"" +
                                                   Convert.ToString(dr["username"]) + "\"," +
                                                   Convert.ToString(dr["custid"]) + ", \"\",0)'>" +
                                                   Convert.ToString(dr["username"]) + "</span>");
                        else
                            TemplateHandler.Assign(ref row, "{Username_vl}", Convert.ToString(dr["username"]));

                        //TemplateHandler.Assign(ref row, "{Curr_vl}", Convert.ToString(dr["currencyName"]));
                        TemplateHandler.Assign(ref row, "{Curr_vl}",
                                               CacheUtils.GetExchangeName(exchange, CastAide.AsInt32(dr["currency"])));
                        TemplateHandler.Assign(ref row, "{Balance_vl}", NumberFormat.DecFormat(dr["balance"], 2));
                        TemplateHandler.Assign(ref row, "{Base_vl}", NumberFormat.DecFormat(dr["basebalance"], 2));
                        html.Append(row);
                        index++;
                        
                    }
                    TemplateHandler.ReplaceBlock(ref tpl, "Rows", html.ToString());
                    TemplateHandler.Assign(ref tpl, "{total}", NumberFormat.DecFormat(dec_Total, 2));
                }
                else
                {
                    string norow = WebUtils.LoadTemplate("NoInformation.htm");
                    TemplateHandler.Assign(ref norow, "{tdno}", "5");
                    TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
                    TemplateHandler.Assign(ref norow, "{nodata}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"));
                    TemplateHandler.ReplaceBlock(ref tpl, "Rows", norow);
                    TemplateHandler.RemoveBlock(ref tpl, "Total");
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            ReportCache.REPORT_DIRECT_BAL = tpl;
            return tpl;
        }

        public string GetDirectBalance_Downline(int custid, int pcustid, int roleid, string user)
        {
            //hard code db = rep (will get data from DBType.REPORT)
            string db = "rep";

            string tpl = WebUtils.LoadTemplate("DirectBalance/DirectBalance_Downline.htm");
            tpl = TemplateHandler.CopyBlock(tpl, "Content");


            //Load Header
            TemplateHandler.Assign(ref tpl, "{No}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "No"));
            TemplateHandler.Assign(ref tpl, "{Curr}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Curr"));
            TemplateHandler.Assign(ref tpl, "{Balance}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Balance"));
            TemplateHandler.Assign(ref tpl, "{Base}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Base"));
            try
            {
                //Get data
                DataTable dt = null;
                if (custid > 0)
                {
                    if (roleid == 3 && pcustid == 0)
                    {
                        TemplateHandler.Assign(ref tpl, "{Username}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Agent"));
                        TemplateHandler.Assign(ref tpl, "{BalanceHeader}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "AgentBalance"));
                        dt =
                            IBCProvider.ReportProvider.DirectBalance().Balance_Direct_MasterDownline(db, custid).
                                ResultSet.Tables[0];
                    }


                    else if (roleid == 2 || (roleid == 3 && pcustid != 0))
                    {
                        TemplateHandler.Assign(ref tpl, "{Username}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Member"));
                        dt =
                            IBCProvider.ReportProvider.DirectBalance().Balance_Direct_AgentDownline(db, custid).
                                ResultSet.Tables[0];
                        TemplateHandler.Assign(ref tpl, "{BalanceHeader}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MemberBalance"));
                    }
                }
                else
                {
                    TemplateHandler.Assign(ref tpl, "{Username}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Member"));
                    TemplateHandler.Assign(ref tpl, "{BalanceHeader}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "MemberBalance"));
                }

                //Load Rows

                if (dt != null && dt.Rows.Count != 0)
                {
                    var html = new StringBuilder();
                    string tpl_row = TemplateHandler.CopyBlock(tpl, "Rows");
                    string odd_even = "";
                    decimal dec_Total = 0;
                    bool isBalExclude = false;
                    int index = 0;
                    DataRow[] rows = dt.Select();
                    Dictionary<int, string> exchange = CacheUtils.Exchange;
                    string currencyName = CacheUtils.GetExchangeName(exchange, CastAide.AsInt32(rows[0]["currency"]));
                    foreach (DataRow dr in rows)
                    {
                        isBalExclude = false;
                        string row = tpl_row.Clone().ToString();
                        //display link to view downline in username
                        if (roleid == 3 && pcustid == 0)
                        {
                            if (CastAide.AsDecimal(NumberFormat.DecFormat(dr["bal"], 2)) == 0 &&
                                CastAide.AsDecimal(NumberFormat.DecFormat(dr["basebalance"], 2)) == 0)
                                continue;
                            //roleid, user, custid, puser, pcustid
                            TemplateHandler.Assign(ref row, "{Username_vl}",
                                                   "<span class='lnk' onclick='View_DL(3,\"" +
                                                   Convert.ToString(dr["username"]) + "\",\"" +
                                                   Convert.ToString(dr["custid"]) + "\",\"" + user + "\"," + custid +
                                                   ")'>" + Convert.ToString(dr["username"]) + "</span>");
                            TemplateHandler.Assign(ref row, "{Balance_vl}", NumberFormat.DecFormat(dr["bal"], 2));

                            //if (CastAide.AsDecimal(dr["bal"]) == 0 && CastAide.AsDecimal(dr["basebalance"]) == 0)
                            if (CastAide.AsDecimal(NumberFormat.DecFormat(dr["bal"], 2)) == 0 &&
                                CastAide.AsDecimal(NumberFormat.DecFormat(dr["basebalance"], 2)) == 0)
                                isBalExclude = true;
                        }
                        else if (roleid == 2 || (roleid == 3 && pcustid != 0))
                        {
                            if (CastAide.AsDecimal(NumberFormat.DecFormat(dr["balance"], 2)) == 0 &&
                                CastAide.AsDecimal(NumberFormat.DecFormat(dr["basebalance"], 2)) == 0)
                                continue;
                            TemplateHandler.Assign(ref row, "{Username_vl}", Convert.ToString(dr["username"]));
                            TemplateHandler.Assign(ref row, "{Balance_vl}", NumberFormat.DecFormat(dr["balance"], 2));

                            if (CastAide.AsDecimal(NumberFormat.DecFormat(dr["balance"], 2)) == 0 &&
                                CastAide.AsDecimal(NumberFormat.DecFormat(dr["basebalance"], 2)) == 0)
                                isBalExclude = true;
                        }

                        //TemplateHandler.Assign(ref row, "{Curr_vl}", Convert.ToString(dr["currency"]));
                        TemplateHandler.Assign(ref row, "{Curr_vl}", currencyName);

                        TemplateHandler.Assign(ref row, "{Base_vl}", NumberFormat.DecFormat(dr["basebalance"], 2));
                        dec_Total += CastAide.AsDecimal(dr["basebalance"]);

                        //remove column which has balance = 0
                        if (isBalExclude)
                            continue;
                        else
                        {
                            index++;
                            odd_even = (odd_even == "") ? "evenrow" : "";
                            TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                            TemplateHandler.Assign(ref row, "{No_vl}", index.ToString());
                            html.Append(row);
                        }
                    }

                    if (index == 0)
                    {
                        string norow = WebUtils.LoadTemplate("NoInformation.htm");
                        TemplateHandler.Assign(ref norow, "{tdno}", "5");
                        TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
                        TemplateHandler.Assign(ref norow, "{nodata}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"));
                        TemplateHandler.ReplaceBlock(ref tpl, "Rows", norow);
                        TemplateHandler.RemoveBlock(ref tpl, "Total");
                    }
                    else
                    {
                        TemplateHandler.ReplaceBlock(ref tpl, "Rows", html.ToString());
                        TemplateHandler.Assign(ref tpl, "{total}", NumberFormat.DecFormat(dec_Total, 2));
                    }
                }
                else
                {
                    string norow = WebUtils.LoadTemplate("NoInformation.htm");
                    TemplateHandler.Assign(ref norow, "{tdno}", "5");
                    TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
                    TemplateHandler.Assign(ref norow, "{nodata}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata"));
                    TemplateHandler.ReplaceBlock(ref tpl, "Rows", norow);
                    TemplateHandler.RemoveBlock(ref tpl, "Total");
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }


            ReportCache.REPORT_DIRECT_BAL = tpl;
            return tpl;
        }

        public string getHeaderDownLine(int roleid, string user, string custid, string puser, int pcustid)
        {
            string strHeader = string.Empty;
            string tpl = WebUtils.LoadTemplate("DirectBalance/DirectBalance_Downline.htm");
            tpl = TemplateHandler.CopyBlock(tpl, "headerLink");

            if (roleid == 3)
            {
                strHeader = "<a href='DirectBalance.aspx?display=master'>" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "DirectMaster") + "</a>"
                            //View_DL(roleid, user, custid, puser, pcustid)
                            +
                            (!string.IsNullOrEmpty(puser)
                                 ? (" > <a href='#' onclick=\"View_DL(3, '" + puser + "'," + pcustid + ",'" + "" + "','" +
                                    0 + "');\">" + puser + "</a>")
                                 : "");
                strHeader += (!string.IsNullOrEmpty(user)) ? " > " + user : "";
            }
            else if (roleid == 2)
            {
                strHeader = "<a href='DirectBalance.aspx?display=agent'>" + ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "DirectAgent") + "</a>" +
                            ((!string.IsNullOrEmpty(user) ? " > " + Convert.ToString(user) : ""));
            }

            //append export to excel
            strHeader +=
                "<span style=\"padding-left:10px;text-transform:capitalize\"><input class=\"btnExcel\" type =\"button\" id =\"btnExport\" name=\"btnExport\" value=\"   \"  onclick=\"ExportToExcel('" +
                roleid + "','" + user + "','" + puser + "','" + custid + "','" + pcustid +
                "');\" title='Export to Excel' /></span>";

            TemplateHandler.Assign(ref tpl, "{headerContent}", strHeader);

            tpl = tpl + "";

            return tpl;
        }

        public override string RenderHTML(Hashtable input)
        {
            return string.Empty;
        }
    }
}