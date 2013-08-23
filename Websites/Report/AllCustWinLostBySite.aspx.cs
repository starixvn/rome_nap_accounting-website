using System;
using System.Data;
using System.Text;
using DNAide.Data;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

public partial class Report_AllCustWinLostBySite : System.Web.UI.Page
{
    private DateTime fDate;
    private DateTime tDate;
    private string siteStr, betFromStr;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["fdate"] != null)
        {
            fDate = Convert.ToDateTime(Request["fdate"]);
            tDate = Convert.ToDateTime(Request["tdate"]);
            siteStr = Request["rdoSite"];
            betFromStr = Request["rdoBetFrom"];
            if (siteStr == null)
            {
                siteStr = "all";
            }
            if (betFromStr == null)
            {
                betFromStr = "a";
            }
            if (!IsPostBack)
            {
                MenuSite.Text = GenMenuSite();
            }
            string betViaStr = "";
            switch (betFromStr)
            {
                case "i":
                    betViaStr = "Internet";
                    break;

                case "p":
                    betViaStr = "Phone";
                    break;

                default:
                    betViaStr = "Phone+Internet";
                    break;
            }
            string titleStr = Resources.Language.CurrentSelectSite + " <b>" + siteStr + "</b> " + Resources.Language.whichbetvia + " <b>" + betViaStr + "</b>";
            titleStr += "<br/><b>" + Resources.Language.CustomerWinLost + ": " + Resources.Language.From + " " + fDate + " " + Resources.Language.To + " " + tDate + "</b>";
            CustWinLostBySiteHeaderStr.Text = titleStr;
            CustWinLostBySiteContentHtml.Text = GenAllCustWinlostContent();
        }
    }

    private string GenMenuSite()
    {
        StringBuilder menuHtml = new StringBuilder();
        try
        {
            string content = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("CustomerWinLost/SelectWinLostBySite.htm");
            string vIbc = "BEST-ODDS", vBEST = "BEST-ODDS3", v961i = "961i.com", vK998 = "k998", vTrojan = "trojan", vIBSBET = "IBSBET", vKING119 = "KING119";
            string vAll = "all", vBetI = "i", vBetP = "p", vBetAll = "a";
            switch (siteStr)
            {
                case "BEST-ODDS":
                    vIbc = "BEST-ODDS\" checked=\"true";
                    break;

                case "BEST-ODDS3":
                    vBEST = "BEST-ODDS3\" checked=\"true";
                    break;

                case "961i.com":
                    v961i = "961i.com\" checked=\"true";
                    break;

                case "k998":
                    vK998 = "k998\" checked=\"true";
                    break;

                case "trojan":
                    vTrojan = "trojan\" checked=\"true";
                    break;

                case "IBSBET":
                    vIBSBET = "IBSBET\" checked=\"true";
                    break;

                case "KING119":
                    vKING119 = "KING119\" checked=\"true";
                    break;

                default:
                    vAll = "all\" checked=\"true";
                    break;
            }
            switch (betFromStr)
            {
                case "i":
                    vBetI = "i\" checked=\"true";
                    break;

                case "p":
                    vBetP = "p\" checked=\"true";
                    break;

                default:
                    vBetAll = "a\" checked=\"true";
                    break;
            }
            TemplateHandler.Assign(ref content, "{ibc}", vIbc);
            TemplateHandler.Assign(ref content, "{ibcname}", "IBC");
            TemplateHandler.Assign(ref content, "{BEST-ODDS3}", vBEST);
            TemplateHandler.Assign(ref content, "{BEST-ODDS3name}", "BEST-ODDS3");
            TemplateHandler.Assign(ref content, "{961i}", v961i);
            TemplateHandler.Assign(ref content, "{961iname}", "961i.com");
            TemplateHandler.Assign(ref content, "{k998}", vK998);
            TemplateHandler.Assign(ref content, "{k998name}", "k998");
            TemplateHandler.Assign(ref content, "{trojan}", vTrojan);
            TemplateHandler.Assign(ref content, "{trojanname}", "TROJAN");
            TemplateHandler.Assign(ref content, "{ibsbet}", vIBSBET);
            TemplateHandler.Assign(ref content, "{ibsbetname}", "IBSBET");
            TemplateHandler.Assign(ref content, "{king119}", vKING119);
            TemplateHandler.Assign(ref content, "{king119name}", "KING119");
            TemplateHandler.Assign(ref content, "{all}", vAll);
            TemplateHandler.Assign(ref content, "{allname}", "ALL");
            TemplateHandler.Assign(ref content, "{internet}", vBetI);
            TemplateHandler.Assign(ref content, "{phone}", vBetP);
            TemplateHandler.Assign(ref content, "{intphone}", vBetAll);
            TemplateHandler.Assign(ref content, "{internetname}", "Internet");
            TemplateHandler.Assign(ref content, "{phonename}", "Phone");
            TemplateHandler.Assign(ref content, "{intphonename}", "Internet+Phone");
            menuHtml.Append(content);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return menuHtml.ToString();
    }

    private string GenAllCustWinlostContent()
    {
        StringBuilder html = new StringBuilder();
        html.Append("<form method=\"post\" name=\"fGetData\">");
        try
        {
            string rowEmpty = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("CustomerWinLost/AllCustWinLostBySiteHeader.htm", new string[] { "{username}", "{currency}", "{+}", "{-}", "{base}" },
                new string[] { Resources.Language.UserName, Resources.Language.Currency, "+", "-", Resources.Language.Base }));
            DbResult ds = MatchWinLostReport.RptAllCustomerWinLossBySiteFrom(fDate, tDate, siteStr, betFromStr);

            if (ds != null)
            {
                DataTable tbl = ds.ResultSet.Tables[0];
                if (tbl.Rows.Count > 0)
                {
                    string content = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("CustomerWinLost/AllCustWinLostBySiteRow.htm");
                    string row = "";
                    string curSite = "";
                    double curSiteTotal = 0;
                    double allSiteTotal = 0;
                    string odd_even = "";
                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {
                        odd_even = (odd_even == "") ? "evenrow" : "";
                        tbl.Rows[i]["site"] = tbl.Rows[i]["site"] as string ?? string.Empty;
                        if (string.Compare(tbl.Rows[i]["site"].ToString(), curSite, true) != 0)
                        {
                            if (curSite != "")
                            {
                                row = content;

                                TemplateHandler.Assign(ref row, "{username}", "");
                                TemplateHandler.Assign(ref row, "{currency}", "");
                                TemplateHandler.Assign(ref row, "{+}", "");
                                TemplateHandler.Assign(ref row, "{-}", curSite + "'s " + Resources.Language.Total);
                                TemplateHandler.Assign(ref row, "{base}", FormatHandler.FormatNumber20(curSiteTotal));
                                TemplateHandler.Assign(ref row, "{h5bold}", "h5bold");
                                html.Append(row);
                            }
                            row = rowEmpty;
                            curSite = tbl.Rows[i]["site"].ToString();
                            curSiteTotal = 0;
                            TemplateHandler.Assign(ref row, "{tdno}", "5");
                            TemplateHandler.Assign(ref row, "{nodata}", curSite);
                            TemplateHandler.Assign(ref row, "{bgcolor-nodata}", "bgcolor-nodata");
                            html.Append(row);
                        }
                        row = content;
                        TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                        string strUsername = tbl.Rows[i]["username"] as string ?? string.Empty;
                        string strCurrency = tbl.Rows[i]["currency"] as string ?? string.Empty;
                        TemplateHandler.Assign(ref row, "{username}", strUsername);
                        TemplateHandler.Assign(ref row, "{currency}", strCurrency);
                        double matchWinLost = tbl.Rows[i].IsNull("matchwinlost") ? 0 : FormatHandler.GetDouble(tbl.Rows[i]["matchwinlost"]);
                        if (matchWinLost >= 0)
                        {
                            TemplateHandler.Assign(ref row, "{+}", FormatHandler.FormatNumber20(matchWinLost));
                            TemplateHandler.Assign(ref row, "{-}", "");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref row, "{+}", "");
                            TemplateHandler.Assign(ref row, "{-}", FormatHandler.FormatNumber20(matchWinLost));
                        }
                        TemplateHandler.Assign(ref row, "{base}", FormatHandler.FormatNumber20(tbl.Rows[i]["curwinlost"]));
                        curSiteTotal += tbl.Rows[i].IsNull("curwinlost") ? 0 : FormatHandler.GetDouble(tbl.Rows[i]["curwinlost"]);
                        allSiteTotal += tbl.Rows[i].IsNull("curwinlost") ? 0 : FormatHandler.GetDouble(tbl.Rows[i]["curwinlost"]);
                        TemplateHandler.Assign(ref row, "{h5bold}", "");
                        html.Append(row);
                    }
                    row = content;
                    TemplateHandler.Assign(ref row, "{username}", "");
                    TemplateHandler.Assign(ref row, "{currency}", "");
                    TemplateHandler.Assign(ref row, "{+}", "");
                    TemplateHandler.Assign(ref row, "{-}", curSite + "'s " + Resources.Language.Total);
                    TemplateHandler.Assign(ref row, "{base}", FormatHandler.FormatNumber20(curSiteTotal));
                    TemplateHandler.Assign(ref row, "{h5bold}", "h5bold");
                    html.Append(row);
                    html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("CustomerWinLost/AllCustWinLostBySiteFooter.htm", new string[] { "{total}", "{totalvalue}" },
                        new string[] { Resources.Language.Total, FormatHandler.FormatNumber20(allSiteTotal) }));
                }
                else
                {
                    TemplateHandler.Assign(ref rowEmpty, "{tdno}", "5");
                    TemplateHandler.Assign(ref rowEmpty, "{nodata}", Resources.Language.thereisnodata);
                    TemplateHandler.Assign(ref rowEmpty, "{bgcolor-nodata}", "");
                    html.Append(rowEmpty);
                }
            }
        }
        catch (Exception ex)
        {
            Logs.Write(ex.Message);
        }
        html.Append("</table>");
        html.Append("</form>");
        return html.ToString();
    }
}