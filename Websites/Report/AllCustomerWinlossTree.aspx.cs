using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Business.Report.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

/// <summary>
/// CITS
/// </summary>
public partial class Report_AllCustomerWinlossTree : PageBase
{
    private RMReportUtils utilsBARM = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        string path = "";
        try
        {
            utilsBARM = new RMReportUtils(RMReportUtils.ReportName.ReportTreeWL, RMReportUtils.RMType.BARM);

            int roleid, custid;
            string date;
            if (string.IsNullOrEmpty(Request["fdate"])) date = DateTime.Today.ToShortDateString();
            else date = Request["fdate"];
            if (string.IsNullOrEmpty(Request["roleid"])) roleid = 4;
            else roleid = Convert.ToInt32(Request["roleid"]);
            if (string.IsNullOrEmpty(Request["custid"])) custid = 0;
            else custid = Convert.ToInt32(Request["custid"]);

            date = "5/23/2012";

            string username = Request["username"];
            string url = "";
            if (roleid == 4) header.Text = Title = "Super Win Loss";
            if (roleid == 3)
            {
                Session["custMaster"] = custid + "|" + username;

                url = "<a href=\"AllCustomerWinlossTree.aspx?custid=0" + "&roleid=" + (roleid + 1) +
                       "&username=" + username +
                     "&fdate=" + date + "\" target=\"centerFr\">Super</a>";
                Session["urlMaster"] = url;
                header.Text = Title = url + ": " + username + " > Master Win Loss";
            }
            if (roleid == 2)
            {
                Session["custAgent"] = custid + "|" + username;

                if ((Session["urlMaster"]) != null)
                {
                    path = Session["urlMaster"].ToString() + ": " + Session["custMaster"].ToString().Split('|')[1];
                }
                url = "<a href=\"AllCustomerWinlossTree.aspx?custid=" + Session["custMaster"].ToString().Split('|')[0] + "&roleid=" + (roleid + 1) +
                       "&username=" + Session["custMaster"].ToString().Split('|')[1] +
                     "&fdate=" + date + "\" target=\"centerFr\"> Master</a>";

                Session["urlAgent"] = url;
                header.Text = Title = path + " > " + url + ": " + username + " > Agent Win Loss";
            }
            if (roleid == 1)
            {
                if ((Session["urlMaster"]) != null)
                {
                    path = Session["urlMaster"].ToString() + ": " + Session["custMaster"].ToString().Split('|')[1];
                }
                if ((Session["urlAgent"]) != null)
                {
                    path += " > " + Session["urlAgent"].ToString() + ": " + Session["custAgent"].ToString().Split('|')[1];
                }
                url = "<a href=\"AllCustomerWinlossTree.aspx?custid=" + Session["custAgent"].ToString().Split('|')[0] + "&roleid=" + (roleid + 1) +
                       "&username=" + Session["custAgent"].ToString().Split('|')[1] +
                     "&fdate=" + date + "\" target=\"centerFr\"> Agent</a>";

                header.Text = Title = path + " > " + url + ": " + username + " > Member Win Loss";
            }
            content.Text = gencontent(roleid, custid, date);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            Response.Write(ex.Message);
        }
    }

    private string gencontent(int roleid, int custid, string date)
    {
        string tplTotal = "";
        DbResult rs = null;
        StringBuilder html = new StringBuilder();
        string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
        TemplateHandler.Assign(ref norow, "{tdno}", "9");
        TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
        DateTime fdate;
        html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("WinLost/AllCustomerWinlossTreeAllHeader.htm", new string[] { "{toverpt}", "{toverrm}", "{username}", "{comm}", "{winlost}", "{total}", "{totalrm}", "{virtaltotalrm}", "{variance}" },
            new string[] { Resources.Language.TOverPT, Resources.Language.TOverRM, Resources.Language.UserName, Resources.Language.Comm, Resources.Language.WinLost, Resources.Language.Total, Resources.Language.TotalRM, Resources.Language.lblVirtualTotalRM, Resources.Language.lblVariance }));
        try
        {
            if (!Validator.ValidDateMMDDYYYY(date, out fdate).IsSuccess())
            {
                TemplateHandler.Assign(ref norow, "{nodata}", AlertMessage.GetMsg(IbcError.DATE_INVALID_FORMAT_MSG, AlertMessage.MsgType.Error));
                html.Append(norow);
                html.Append("<table>");
                return html.ToString();
            }

            string rows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/AllCustomerWinlossTreeAllRow.htm");
            tplTotal = TemplateHandler.CopyBlock(rows, "total");
            TemplateHandler.RemoveBlock(ref rows, "total");

            DataTable dt = new DataTable();
            rs = AccountReport.Acc_Rpt_TreeWinlost(fdate, roleid, custid);
            dt = rs.ResultSet.Tables[0];
            string odd_even = "";
            roleid = roleid - 1;
            if (dt.Rows.Count > 0)
            {
                decimal grandTotalTOverRM = 0, grandTotalRM = 0, grandVirtualTotalRM = 0, grandVariance = 0;
                DataRow[] dataRow = dt.Select();
                Dictionary<int, string> exchange = CacheUtils.Exchange;
                foreach (DataRow dr in dt.Rows)
                {
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    string row = rows.Clone().ToString();
                    string url = "";
                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);

                    //TemplateHandler.Assign(ref row, "{toverpt}",  "<font color=#cdcdcd>" + Convert.ToString(dr["currency"] + "</font>" + FormatHandler.FormatNumber20(dr["PositionTurnOverRM"])));
                    TemplateHandler.Assign(ref row, "{toverpt}", "<font color=#cdcdcd>" + CacheUtils.GetExchangeName(exchange, CastAide.AsInt32(dr["currency"])) + "</font> " + FormatHandler.FormatNumber20(dr["PositionTurnOverRM"]));

                    double toverrm = FormatHandler.GetDouble(dr["TurnOverRM"]);
                    TemplateHandler.Assign(ref row, "{toverrm}", "<b>" + FormatHandler.FormatNumber20(toverrm) + "</b>");

                    if (roleid == 0) url = Convert.ToString(dr["username"]);
                    else url = "<a href=\"AllCustomerWinlossTree.aspx?custid=" + Convert.ToString(dr["custid"]) + "&roleid=" + roleid +
                        "&username=" + Convert.ToString(dr["username"]) +
                      "&fdate=" + fdate.ToShortDateString() + "\" target=\"centerFr\">" + Convert.ToString(dr["username"]) + "</a>";
                    TemplateHandler.Assign(ref row, "{username}", url);

                    double comm = FormatHandler.GetDouble(dr["commission"]);
                    TemplateHandler.Assign(ref row, "{comm}", colorvalue(comm));

                    double winlost = FormatHandler.GetDouble(dr["winloss"]);
                    TemplateHandler.Assign(ref row, "{winlost}", colorvalue(winlost));

                    double total = FormatHandler.GetDouble(dr["total"]);
                    TemplateHandler.Assign(ref row, "{total}", colorvalue(total));

                    double totalrm = CastAide.AsDouble(dr["totalrm"]);
                    TemplateHandler.Assign(ref row, "{totalrm}", colorvalue(totalrm));

                    double VirtualTotalRM = CastAide.AsDouble(dr["VirtualTotalRM"]);
                    TemplateHandler.Assign(ref row, "{virtaltotalrm}", colorvalue(VirtualTotalRM));

                    double Variance = totalrm - VirtualTotalRM;
                    TemplateHandler.Assign(ref row, "{variance}", colorvalue(Variance));

                    html.Append(row);

                    grandTotalTOverRM += CastAide.AsDecimal(dr["TurnOverRM"]);
                    grandTotalRM += CastAide.AsDecimal(dr["TotalRM"]);
                    grandVirtualTotalRM += CastAide.AsDecimal(dr["VirtualTotalRM"]);
                    grandVariance += CastAide.AsDecimal(Variance);
                }
                string rowtotal = Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("WinLost/AllCustomerWinlossTreeTotal.htm", new string[] { "{toverrm}", "{totarml}", "{virtaltotalrm}", "{variance}" },
                    new string[] { colorvalue(grandTotalTOverRM), colorvalue(grandTotalRM), colorvalue(grandVirtualTotalRM), colorvalue(grandVariance) });
                html.Append(rowtotal);

                if (roleid == 3)
                {
                    string tpl = tplTotal;
                    decimal winlost_8rm = -CastAide.AsDecimal(rs.Outputs["winlost_8rm"]);
                    decimal virtualwinlost_8rm = -CastAide.AsDecimal(rs.Outputs["virtualwinlost_8rm"]);
                    decimal winlost_barm = -CastAide.AsDecimal(rs.Outputs["winlost_barm"]);
                    decimal virtualwinlost_barm = -CastAide.AsDecimal(rs.Outputs["virtualwinlost_barm"]);

                    //testing

                    grandTotalRM += winlost_8rm + (utilsBARM.IsActive ? winlost_barm : 0);
                    grandVirtualTotalRM += virtualwinlost_8rm + (utilsBARM.IsActive ? virtualwinlost_barm : 0);

                    decimal variance_8rm = winlost_8rm - virtualwinlost_8rm;
                    decimal variance_barm = winlost_barm - virtualwinlost_barm;

                    decimal variance_total = grandTotalRM - grandVirtualTotalRM;

                    string tplRM = TemplateHandler.CopyBlock(tplTotal, "rm");
                    string tplGrandTotal = TemplateHandler.CopyBlock(tplTotal, "grandtotal");

                    //8RM
                    tpl = tplRM;

                    TemplateHandler.Assign(ref tpl, "{RM}", "8RM");
                    TemplateHandler.Assign(ref tpl, "{8rm}", colorvalue(winlost_8rm));
                    TemplateHandler.Assign(ref tpl, "{virtual8rm}", colorvalue(virtualwinlost_8rm));
                    TemplateHandler.Assign(ref tpl, "{variance_8rm}", colorvalue(variance_8rm));
                    html.Append(tpl);

                    //BARM
                    if (utilsBARM.IsActive)
                    {
                        tpl = tplRM;
                        TemplateHandler.Assign(ref tpl, "{RM}", utilsBARM.DisplayName);
                        TemplateHandler.Assign(ref tpl, "{8rm}", colorvalue(winlost_barm));
                        TemplateHandler.Assign(ref tpl, "{virtual8rm}", colorvalue(virtualwinlost_barm));
                        TemplateHandler.Assign(ref tpl, "{variance_8rm}", colorvalue(variance_barm));
                        html.Append(tpl);
                    }

                    tpl = tplGrandTotal;
                    TemplateHandler.Assign(ref tpl, "{total}", colorvalue(grandTotalRM));
                    TemplateHandler.Assign(ref tpl, "{virtualtotal}", colorvalue(grandVirtualTotalRM));
                    TemplateHandler.Assign(ref tpl, "{variance_total}", colorvalue(variance_total));
                    html.Append(tpl);

                    //----------- 12bet---------

                }
            }
            else
            {
                TemplateHandler.Assign(ref norow, "{nodata}", (Resources.Language.thereisnodata));
                html.Append(norow);
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Business.Logs.Write(ex.Message);
            TemplateHandler.Assign(ref norow, "{nodata}", AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error));
            html.Append(norow);
        }

        html.Append("</table>");

        return html.ToString();
    }

    private string colorvalue(object value)
    {
        string str = "";
        if (CastAide.AsDecimal(value) >= 0) str = "<b>" + FormatHandler.FormatNumber20(value) + "</b>";
        else str = "<b><font color='#aa0000'>" + FormatHandler.FormatNumber20(value) + "</b></font>";
        return str;
    }
}