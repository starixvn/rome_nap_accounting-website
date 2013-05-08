using System;
using System.Data;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using IBCUtils;
using System.Text;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using DNAide.Data;


public partial class Report_StatementAll : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RepHeaderStr.Text = Resources.Language.Statement;
        StringBuilder html = new StringBuilder();
        //generate report column headers
        html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("Statement\\StatementAllHeader.htm", new string[] { "{date}", "{type}", "{total}", "{accum_total}" },
            new string[] { Resources.Language.Date, Resources.Language.Type, Resources.Language.Total, Resources.Language.AccumTotal }));
        //generate report content
        GenReportContent(ref html);
        RepContentHtml.Text = html.ToString();
    }
    void GenReportContent(ref StringBuilder reportStr)
    {
        string fdateStr = Request["fdate"];
        string tdateStr = Request["tdate"];
        DateTime fdate, tdate;
        if (!(IBCUtils.Validator.ValidDateMMDDYYYYNotNull(fdateStr, out fdate).IsSuccess() && IBCUtils.Validator.ValidDateMMDDYYYYNotNull(tdateStr, out tdate).IsSuccess()))
        {
            reportStr.Append("</table>");
            return;
        }
        try
        {
            DbResult ds = AccountReport.RptAllStatementCompany(fdate, tdate);
            if (ds != null)
            {
                float accTotal = 0;
                string even_odd_row = "even";
                string rowTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Statement\\StatementAllRow.htm");
                foreach (DataRow row in ds.ResultSet.Tables[0].Rows)
                {
                    string rowHtml = (string)rowTpl.Clone();
                    even_odd_row = (even_odd_row == "even") ? "odd" : "even";
                    string date = Convert.ToDateTime(row["accdate"], FormatHandler.EngCult).ToShortDateString();
                    string transType = row["transtype"].ToString();
                    float total = Convert.IsDBNull(row["totalamt"]) ? 0 : Convert.ToSingle(row["totalamt"], FormatHandler.EngCult);
                    accTotal += total;
                    TemplateHandler.Assign(ref rowHtml, "{odd_even}", even_odd_row);
                    TemplateHandler.Assign(ref rowHtml, "{date}", date);
                    TemplateHandler.Assign(ref rowHtml, "{type}", transType);
                    TemplateHandler.Assign(ref rowHtml, "{total}", string.Format("{0:n}", total));
                    TemplateHandler.Assign(ref rowHtml, "{accum_total}", string.Format("{0:n}", accTotal));
                    
                    reportStr.Append(rowHtml);
                    //reportStr.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("StatementAllRow.tpl", new string[] { "{odd_even}", "{date}", "{type}", "{total}", "{accum_total}" },
                    //    new string[] { even_odd_row, date, transType, string.Format("{0:n}", total), string.Format("{0:n}", accTotal) }));
                }
            }

            reportStr.Append("</table>");
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            reportStr.Append("<tr><td colspan='4'>ERROR WHILE GETTING DATA</td></tr></table>");
           
        }
    }
}

