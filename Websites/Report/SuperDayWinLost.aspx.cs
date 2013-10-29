using System;
using System.Data;
using System.Text;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

/// <summary>
/// CITS
/// </summary>
public partial class SuperDayWinLost : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int roleid, custid;
        if (string.IsNullOrEmpty(Request["roleid"])) roleid = -1;
        else roleid = Convert.ToInt32(Request["roleid"]);
        if (string.IsNullOrEmpty(Request["custid"])) custid = 0;
        else custid = Convert.ToInt32(Request["custid"]);
        if (roleid == -1) header.Text = Title = Resources.Language.SuperDay;
        if (roleid == 3) header.Text = Title = Resources.Language.MasterDay;
        if (roleid == 2) header.Text = Title = Resources.Language.AgentDay;
        if (roleid == 1) header.Text = Title = Resources.Language.MemberDay;
        DateTime matchday;
        if (string.IsNullOrEmpty(Request["matchday"])) matchday = DateTime.Today;
        else matchday = Convert.ToDateTime(Request["matchday"]);

        content.Text = gencontent(roleid, custid, matchday);
    }

    private string gencontent(int roleid, int custid, DateTime matchdate)
    {
        StringBuilder html = new StringBuilder();
        try
        {
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("Balance/SuperBalanceAllHeader.htm", new string[] { "{username}", "{currentcy}", "{balance}", "{base}" },
            new string[] { Resources.Language.UserName, Resources.Language.Currency, Resources.Language.Balance, Resources.Language.Base }));
            string rows = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Balance/SuperBalanceAllRow.htm");
            DataTable dt = new DataTable();
            dt = AccountReport.RptDayWinLost(roleid, custid, matchdate).ResultSet.Tables[0];
            if (roleid == -1) roleid = 3;
            else roleid = roleid - 1;
            string odd_even = "";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    string row = rows.Clone().ToString();
                    string url = "";
                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                    if (roleid == 0) url = Convert.ToString(dr["username"]);
                    else url = "<a href=\"SuperDayWinLost.aspx?custid=" + Convert.ToString(dr["custid"]) + "&roleid=" + roleid +
                        "\" target=\"centerFr\">" + Convert.ToString(dr["username"]) + "</a>";
                    TemplateHandler.Assign(ref row, "{username}", url);
                    TemplateHandler.Assign(ref row, "{currentcy}", Convert.ToString(dr["currency"]));
                    TemplateHandler.Assign(ref row, "{balance}", FormatHandler.FormatNumber20(dr["actualrate"]));
                    TemplateHandler.Assign(ref row, "{base}", FormatHandler.FormatNumber20(dr["bal"]));
                    html.Append(row);
                }
            }
            else
            {
                string norow = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("NoInformation.htm");
                TemplateHandler.Assign(ref norow, "{tdno}", "4");
                TemplateHandler.Assign(ref norow, "{nodata}", Resources.Language.thereisnodata);
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
}