using System;
using System.Data;
using System.Text;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using IBCUtils;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using DNAide.Data;


public partial class Statement_StatementByDay : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try {
            string transType = Request["transtype"];
            StringBuilder ReportHtml = new StringBuilder(Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Statement\\StatementByDayHeader.htm"));
            DateTime fdt;

            if (Validator.ValidDateMMDDYYYYNotNull(Request["fdate"], out fdt).IsSuccess())
            {
                StDay.Text = FormatHandler.FormatDateEn(fdt);
                DbResult ds = AccountStatement.Acc_AllStatementCompanyDetail(fdt, transType);
                if (ds != null)
                {
                    GenReportRows(ReportHtml, ds.ResultSet.Tables[0].Rows);
                }
                else
                {
                    ReportHtml.Append("</table>" + AlertMessage.GetMsg(IbcError.INTERNAL_ERROR_MSG, AlertMessage.MsgType.Error));
                }
            }
            else
            {
                ReportHtml.Append("</table>" + AlertMessage.GetMsg(IbcError.DATE_INVALID_FORMAT_MSG, AlertMessage.MsgType.Error));
            }
            ReportLiteral.Text = ReportHtml.ToString();
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }

      
    }
    void GenReportRows(StringBuilder reportHtml, DataRowCollection rows)
    {
        try {
            string rowTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Statement\\StatementByDayRow.htm");
            decimal lineTotal = 0;
            bool hasData = false;
            foreach (DataRow row in rows)
            {
                hasData = true;
                string rowHtml = (string)rowTpl.Clone();
                decimal total = FormatHandler.GetDecimal(row["totalamt"]);
                lineTotal += total;
                TemplateHandler.Assign(ref rowHtml, "{User}", row["username"] + "");
                TemplateHandler.Assign(ref rowHtml, "{Total}", FormatHandler.FormatNumber20(total));
                TemplateHandler.Assign(ref rowHtml, "{LineTotal}", FormatHandler.FormatNumber20(lineTotal));
                reportHtml.Append(rowHtml);
            }
            if (!hasData)
            {
                reportHtml.Append(AccCommon.GenNoInformation("3"));
            }
            reportHtml.Append("</table>");
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        
    }
}
