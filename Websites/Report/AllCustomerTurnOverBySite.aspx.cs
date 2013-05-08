using System;
using System.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Text;
using IBCUtils;
using DNAide.Web;
using DNAide.Core;


/// <summary>
/// CITS
/// </summary>
public partial class Report_AllCustomerTurnOverBySite : System.Web.UI.Page
{
    protected string ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    protected string MinDay = "";
    protected string FDay = "";
    protected string TDay = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Resources.Language.AllCustomerTurnOver;
        DateTime fdate = CastAide.AsDateTime(Request["fdate"], DateTime.Now);
        DateTime tdate = CastAide.AsDateTime(Request["tdate"], DateTime.Now);
        if (!string.IsNullOrEmpty(Request["fdate"]))
        {
            if (Validator.ValidDateMMDDYYYY(Request["fdate"], out fdate).IsSuccess()) Session["fdate"] = fdate;
        }
        else
        {
            Session["fdate"] = DateTime.Now;
        }

        if (!string.IsNullOrEmpty(Request["tdate"]))
        {
            if (Validator.ValidDateMMDDYYYY(Request["tdate"], out tdate).IsSuccess()) Session["tdate"] = tdate;
        }
        else
        {
            Session["tdate"] = DateTime.Now;
        }

        if (Session["fdate"] == null && Session["fdate"] == null)
            content.Text = AlertMessage.GetMsg(IbcError.DATE_INVALID_FORMAT_MSG, AlertMessage.MsgType.Error);
        else
        {
            string tpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/AllCustomerTurnOverBySiteHeader.htm");
            ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
            MinDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today.AddMonths(-3));
            FDay = string.Format("{0:MM/dd/yyyy}", fdate);
            TDay = string.Format("{0:MM/dd/yyyy}", tdate);
            TemplateHandler.Assign(ref tpl, "{fdate}", "<input type=\"text\" id=\"fdate\" readonly=\"readonly\" class=\"txtdate\" style=\"width: 100px; background-color: white;\" />");
            TemplateHandler.Assign(ref tpl, "{tdate}", "<input type=\"text\" id=\"tdate\" readonly=\"readonly\" class=\"txtdate\" style=\"width: 100px; background-color: white;\" />");
            TemplateHandler.RemoveBlock(ref tpl, "btnSearch");
            SearchStandard.Text = tpl;
            content.Text = gencontent();
        }
    }
    string gencontent()
    {
        StringBuilder html = new StringBuilder();
        string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
        TemplateHandler.Assign(ref norow, "{tdno}", "3");
        TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");

        string title = "";
        string sitefilter = Request["site"];
        if (!string.IsNullOrEmpty(Request["site"])) { sitefilter = Request["site"]; Session["site"] = Request["site"]; }
        else if (Session["site"] == null) sitefilter = Resources.Language.All;
        else sitefilter = Session["site"].ToString();

        string fromfilter = "";
        if (Request["from"] == "Internet") { Session["betfrom"] = "Internet"; fromfilter = "i"; }
        else if (Request["from"] == "Phone") { Session["betfrom"] = "Phone"; fromfilter = "p"; }
        if (Request["from"] == "All" || Session["betfrom"] == null) { Session["betfrom"] = "Internet + Phone"; }

        if (Session["betfrom"] == null || Session["betfrom"].ToString() == "Internet + Phone") fromfilter = "i,p";
        else if (Session["betfrom"].ToString() == "Internet") fromfilter = "i";
        else if (Session["betfrom"].ToString() == "Phone") fromfilter = "p";
        title = Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("CompBalance/Customer/AllCustomerTurnOverBySiteTitle.htm", new string[] { "{site}", "{phone-internet}" }, new string[] { sitefilter, Session["betfrom"].ToString() });
        try
        {
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("CompBalance/Customer/AllCustomerTurnOverBySiteButton.htm", new string[] { "{betfrom}" }, new string[] { "" }));
            StringBuilder filterContent = new StringBuilder(SearchStandard.Text);
            filterContent.Append(html);
            SearchStandard.Text = filterContent.ToString();
            if (string.IsNullOrEmpty(Request["reload"]))
            {

                return string.Empty;
            }
            html = new StringBuilder();
            html.Append(title);
            DateTime fdate = DateTime.Today, tdate = DateTime.Today;
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("CompBalance/Customer/AllCustomerTurnOverBySiteAllHeader.htm", new string[] { "{user}", "{turnover}", "{fdate}", "{tdate}" },
                new string[] { Resources.Language.User, Resources.Language.TurnOver, Convert.ToDateTime(Session["fdate"]).ToShortDateString(), Convert.ToDateTime(Session["tdate"]).ToShortDateString() }));
            if (!(IBCUtils.Validator.ValidDateMMDDYYYYNotNull(Convert.ToDateTime(Session["fdate"]).ToShortDateString(), out fdate).IsSuccess() && IBCUtils.Validator.ValidDateMMDDYYYYNotNull(Convert.ToDateTime(Session["tdate"]).ToShortDateString(), out tdate).IsSuccess()))
            {
                TemplateHandler.Assign(ref norow, "{nodata}", (IbcError.DATE_INVALID_FORMAT_MSG));
                html.Append(norow);
                html.Append("</table>");
                return html.ToString(); ;
            }

            string rows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("CompBalance/Customer/AllCustomerTurnOverBySiteAllRow.htm");
            DataTable dt = AccountReport.Acc_Rpt_CustomerTurnoverBySite(fdate, tdate, sitefilter, fromfilter).ResultSet.Tables[0];
            if (dt.Rows.Count > 0)
            {

                string odd_even = "";
                string site = "", sitetotalrow = "";
                string rowites = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("CompBalance/Customer/AllCustomerTurnOverBySiteColspan.htm");
                double total = 0, totalover = 0;

                int index = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    index++;
                    string rowsite = rowites.Clone().ToString();
                    string row = rows.Clone().ToString();
                    if (!dr["site"].Equals(site))
                    {
                        if (site.Length != 0)
                        {
                            sitetotalrow = rows.Clone().ToString();
                            TemplateHandler.Assign(ref sitetotalrow, "{odd_even}", "footer1");
                            TemplateHandler.Assign(ref sitetotalrow, "{user}", "<b>" + site + Resources.Language.stotal + "</b>");
                            TemplateHandler.Assign(ref sitetotalrow, "{turnover}", "<b>" + FormatHandler.FormatNumber20(total) + "</b>");
                            TemplateHandler.Assign(ref sitetotalrow, "{no}", "");
                            html.Append(sitetotalrow);
                            total = 0;
                        }
                        site = Convert.ToString(dr["site"]);
                        TemplateHandler.Assign(ref rowsite, "{user}", "<b>" + Convert.ToString(dr["site"]) + "</b>");
                        html.Append(rowsite);

                    }
                    total += FormatHandler.GetDouble(dr["turnover"]);
                    totalover += FormatHandler.GetDouble(dr["turnover"]);
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    TemplateHandler.Assign(ref row, "{no}", index.ToString());
                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                    TemplateHandler.Assign(ref row, "{user}", Convert.ToString(dr["username"]));
                    TemplateHandler.Assign(ref row, "{turnover}", FormatHandler.FormatNumber20(dr["turnover"]));
                    html.Append(row);
                }

                sitetotalrow = rows.Clone().ToString();
                TemplateHandler.Assign(ref sitetotalrow, "{odd_even}", "footer1");
                TemplateHandler.Assign(ref sitetotalrow, "{user}", "<b>" + site + Resources.Language.stotal + "</b>");
                TemplateHandler.Assign(ref sitetotalrow, "{turnover}", "<b>" + FormatHandler.FormatNumber20(total) + "</b>");
                TemplateHandler.Assign(ref sitetotalrow, "{no}", "");
                html.Append(sitetotalrow);

                string grand = rows.Clone().ToString();
                TemplateHandler.Assign(ref grand, "{odd_even}", "report-footer");
                TemplateHandler.Assign(ref grand, "{user}", "<b>" + Resources.Language.GrandToTal + ":</b>");
                TemplateHandler.Assign(ref grand, "{turnover}", "<b>" + FormatHandler.FormatNumber20(totalover) + "</b>");
                TemplateHandler.Assign(ref grand, "{no}", "");
                html.Append(grand);
            }
            else
            {
                TemplateHandler.Assign(ref norow, "{nodata}", (Resources.Language.thereisnodata));
                html.Append(norow);
                html.Append("</table>");
            }
            html.Append("</table>");
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return html.ToString();
    }
}
