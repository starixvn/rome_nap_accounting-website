using System;
using System.Data;
using System.Text;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils;
using DNAide.Data;


public partial class Report_AllCustWinLost : System.Web.UI.Page
{
    private DateTime fDate;
    private DateTime tDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["fdate"] != null && Request["tdate"] != null)
        {
            fDate = Convert.ToDateTime(Request["fdate"]);
            tDate = Convert.ToDateTime(Request["tdate"]);
            CustWinLostHeaderStr.Text = Resources.Language.CustomerWinLost + ": " + Resources.Language.From + " " + fDate.ToShortDateString() + " " + Resources.Language.To + " " + tDate.ToShortDateString();
            CustWinLostContentHtml.Text = GenCustWinLostContent();
        }

    }
    private string GenCustWinLostContent()
    {
        StringBuilder html = new StringBuilder();
        try
        {
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("CustomerWinLost/AllCustWinLostHeader.htm",
                new string[] { "{username}", "{currency}", "{+}", "{-}", "{base}" },
                new string[] { Resources.Language.UserName, Resources.Language.Currency, "+", "-", Resources.Language.Base }));
            string content = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("CustomerWinLost/AllCustWinLostRow.htm");
            DbResult ds = AccountReport.RptCustomerWinloss(fDate,tDate);
            if (ds != null)
            {
                DataTable tbl= ds.ResultSet.Tables[0];
                if (tbl.Rows.Count > 0)
                {
                    string row = "";
                    string odd_even = "";
                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {
                        odd_even = (odd_even == "") ? "evenrow" : "";
                        row = content;
                        string strUsername=tbl.Rows[i]["username"] as string ?? string.Empty;
                        string strCurrency=tbl.Rows[i]["currency"] as string ?? string.Empty;                        
                        TemplateHandler.Assign(ref row, "{username}",strUsername);
                        TemplateHandler.Assign(ref row, "{currency}", strCurrency);
                        TemplateHandler.Assign(ref row, "{+}",FormatHandler.FormatNumber20(tbl.Rows[i]["PositiveWinLost"]));
                        TemplateHandler.Assign(ref row, "{-}", FormatHandler.FormatNumber20(tbl.Rows[i]["NegativeWinLost"]));
                        TemplateHandler.Assign(ref row, "{base}", FormatHandler.FormatNumber20(tbl.Rows[i]["BaseWinLost"]));
                        TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                        html.Append(row);
                    }
                    html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("CustomerWinLost/AllCustWinLostFooter.htm", new string[] { "{total}", "{totalvalue}" },
                        new string[] { Resources.Language.Total, FormatHandler.FormatNumber20(ds.Outputs["totalbasewinlost"]) }));
                }
                else
                {
                    html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("NoInformation.htm", new string[] { "{tdno}", "{nodata}" },
                        new string[] { "5", Resources.Language.thereisnodata }));
                }
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        html.Append("</table>");
        return html.ToString();
    }
}
