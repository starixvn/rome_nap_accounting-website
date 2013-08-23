using System;
using System.Data;
using System.Text;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

public partial class Report_MarginReport : PageBase
{
    public double actual_stake = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Resources.Language.MarginReport;

        if (!string.IsNullOrEmpty(Request["txtorder"]))
        {
            if (isNumber(Request["txtorder"])) { actual_stake = FormatHandler.GetDouble(Request["txtorder"]); content.Text = getcontent(actual_stake); }
            else content.Text = "<b><font color='red'>" + Resources.Language.InvalidFormatNumber + "</b></font><br>" + getcontent(0);
        }
        else content.Text = getcontent(actual_stake);
    }

    private string getcontent(double actual_stake)
    {
        StringBuilder html = new StringBuilder();
        string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
        TemplateHandler.Assign(ref norow, "{tdno}", "7");
        TemplateHandler.Assign(ref norow, "{bgcolor-nodata}", "bgcolor-nodata");
        TemplateHandler.Assign(ref norow, "{nodata}", Resources.Language.thereisnodata);
        try
        {
            html.Append("<form action='MarginReport.aspx' method='post'>");
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("WinLost/MarginReportForm.htm", new string[] { "{order}" }, new string[] { FormatHandler.FormatNumber20(actual_stake) }));
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("WinLost/MarginReportAllHeader.htm", new string[] { "{username}", "{type}", "{winning}", "{bet}", "{ratio}", "{total}" },
                new string[] { Resources.Language.UserName, Resources.Language.Type, Resources.Language.Wining, Resources.Language.Bet, Resources.Language.Ratio, Resources.Language.Total }));

            DataTable dt = AccountReport.Acc_Rpt_BetRatioByUser(actual_stake).ResultSet.Tables[0];
            string rows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("WinLost/MarginReportAllRow.htm");
            if (dt.Rows.Count != 0)
            {
                string odd_even = "";
                int index = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    index++;
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    string row = rows.Clone().ToString();
                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                    TemplateHandler.Assign(ref row, "{no}", index.ToString());
                    TemplateHandler.Assign(ref row, "{username}", Convert.ToString(dr["username"]));
                    string type = (Convert.ToString(dr["bettype"]) == "1") ? Resources.Language.HDP : Resources.Language.OU;
                    TemplateHandler.Assign(ref row, "{type}", type.ToUpper());
                    TemplateHandler.Assign(ref row, "{winning}", FormatHandler.FormatNumber20(dr["totalwon"]));
                    TemplateHandler.Assign(ref row, "{bet}", FormatHandler.FormatNumber20(dr["turnover"]));
                    TemplateHandler.Assign(ref row, "{ratio}", FormatHandler.FormatNumber3(dr["ratio"]));
                    TemplateHandler.Assign(ref row, "{total}", FormatHandler.FormatNumber20(dr["n"]));
                    html.Append(row);
                }
            }
            else
            {
                html.Append(norow);
                html.Append("</table>");
            }
            html.Append("</form>");
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            html.Append(norow);
            html.Append("</table>");
        }
        html.Append("</table>");
        return html.ToString();
    }

    private bool isNumber(string input)
    {
        string str = "0123456789";
        int count = 0;
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < str.Length; j++)
            {
                if (input[i] == str[j])
                {
                    count++;
                    break;
                }
            }
        }
        if (count != input.Length) return false;
        else return true;
    }
}