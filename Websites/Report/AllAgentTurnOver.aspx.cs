using System;
using System.Data;
using System.Text;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils;
using DNAide.Data;



public partial class AllAgentTurnOver : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        agentTurnOverHeaderStr.Text = Resources.Language.AllAgentTurnOver;
        agentTurnOverContentHtml.Text = GenAgentTurnOverHtml();
    }
    private string GenAgentTurnOverHtml()
    {
        StringBuilder html = new StringBuilder();        
       
        try
        {
            DbResult ds = null;
            if (Request["fdate"] == null || Request["tdate"] == null) return "";
            DateTime fDate = Convert.ToDateTime(Request["fdate"]);
            DateTime tDate = Convert.ToDateTime(Request["tdate"]);

            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("TurnOver/AllAgentTurnOverHeader.htm", new string[] { "{tdno}", "{user}", "{turnover}", "{fdate}", "{tdate}" }, new string[] { Resources.Language.No, Resources.Language.User, Resources.Language.TurnOver, fDate.ToShortDateString(), tDate.ToShortDateString() }));
            string content = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("TurnOver/AllAgentTurnOverRow.htm");

            ds = AgentTurnOver.RptAgentTurnOver(fDate, tDate);

            string row = "";
            if (ds != null)
            {
                DataTable tbl = ds.ResultSet.Tables[0];
                int num = 0;
                string odd_even = "";
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    num = i + 1;
                    row = content;
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    string strUsername = tbl.Rows[i]["username"] as string ?? string.Empty;
                    TemplateHandler.Assign(ref row, "{odd_even}", odd_even);
                    TemplateHandler.Assign(ref row, "{tdno}", num.ToString());
                    TemplateHandler.Assign(ref row, "{user}", strUsername);
                    double turnover = FormatHandler.GetDouble(tbl.Rows[i]["home_odds"]) + FormatHandler.GetDouble(tbl.Rows[i]["away_odds"]);
                    TemplateHandler.Assign(ref row, "{turnover}", IBCUtils.FormatHandler.FormatNumber20(turnover));
                    html.Append(row);
                }
            }
            else
            {
                html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("NoInformation.htm", new string[] { "{tdno}", "{nodata}" },
                                new string[] { "3", Resources.Language.thereisnodata}));
            }
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("NoInformation.htm", new string[] { "{tdno}", "{nodata}" },
                                new string[] { "3", (ex.Message) }));
        }
        html.Append("</table>");
        return html.ToString();
    }
}
