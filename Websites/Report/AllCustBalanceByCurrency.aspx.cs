using System;
using System.Data;
using System.Text;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils;
using DNAide.Data;


public partial class AllCustBalanceByCurrency : System.Web.UI.Page
{
    private DateTime fDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["fdate"] != null)
        {
            fDate = Convert.ToDateTime(Request["fdate"]);
            CustBalanceByCurrencyHeaderStr.Text = Resources.Language.AllCustBalanceByCurrency + " at: " +fDate;
            CustBalanceByCurrencyHeaderContentHtml.Text = GenCustBalanceByCurrency();
        }
        
    }
    private string GenCustBalanceByCurrency()
    {
        StringBuilder html = new StringBuilder();
        html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("Balance/AllCustBalanceByCurrencyHeader.tpl", new string[] { "{username}", "{currency}", "{+}","{-}","{base}","{oustxn}" }, 
            new string[] { Resources.Language.UserName, Resources.Language.Currency,"+","-", Resources.Language.Base ,Resources.Language.Oustxn}));
        string content = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Balance/AllCustBalanceByCurrencyRow.tpl");
        try
        {
            DbResult ds = null;            
            int currency =Convert.ToInt32(Request["currency"]);
            ds = AccountReport.RptAllCustomerBalanceByCurrency(fDate, currency);
            string row = "";
            double totalVal = 0;
            if (ds != null)
            {
                DataTable tbl = ds.ResultSet.Tables[0];
                int num = 0;
                if (tbl.Rows.Count > 0)
                {
                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {
                        num = i + 1;
                        row = content;
                        totalVal += tbl.Rows[i].IsNull("basebalance")? 0: FormatHandler.GetDouble(tbl.Rows[i]["basebalance"]);
                        string strUsername=tbl.Rows[i]["username"] as string ?? string.Empty;
                        string strCurrency =tbl.Rows[i]["currency"] as string ?? string.Empty;
                        TemplateHandler.Assign(ref row, "{username}",strUsername);
                        TemplateHandler.Assign(ref row, "{currency}",strCurrency);
                        TemplateHandler.Assign(ref row, "{+}", FormatHandler.FormatNumber20(tbl.Rows[i]["positivebalance"]));
                        TemplateHandler.Assign(ref row, "{-}", FormatHandler.FormatNumber20(tbl.Rows[i]["negativebalance"]));
                        TemplateHandler.Assign(ref row, "{base}", FormatHandler.FormatNumber20(tbl.Rows[i]["basebalance"]));
                        TemplateHandler.Assign(ref row, "{oustxn}", FormatHandler.FormatNumber20(tbl.Rows[i]["oustxn"]));
                        html.Append(row);
                    }
                    html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("Balance/AllCustBalanceByCurrencyFooter.tpl", new string[] { "{total}", "{totalvalue}", "" },
                        new string[] { Resources.Language.Total, FormatHandler.FormatNumber20(totalVal), "" }));
                }
                else
                {
                    html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("NoInformation.tpl", new string[] { "{tdno}", "{nodata}"},
                        new string[] { "6", Resources.Language.thereisnodata }));
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
