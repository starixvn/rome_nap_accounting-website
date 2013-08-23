using System;
using System.Data;
using System.Text;
using DNAide.Data;
using DNAide.Web;
using IBCUtils;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

public partial class Report_CompBalance : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            bool hasData = false;
            //DataResult ds = Balance.CompanyBalance();
            DbResult ds = Balance.CompanyBalance();
            StringBuilder html = new StringBuilder();
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("CompBalance\\CompBalanceHeader.htm"));
            double totalRmAmount = 0;
            if (ds != null)
            {
                string rowTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("CompBalance\\CompBalanceRow.htm");
                string odd_even = "";
                //foreach (DataRow row in ds.ResultsSet.Tables[0].Rows)

                foreach (DataRow row in ds.ResultSet.Tables[0].Rows)
                {
                    hasData = true;
                    string rowHtml = (string)rowTpl.Clone();

                    string currency = Convert.IsDBNull(row["currency"]) ? "ERROR" : row["currency"].ToString();
                    string localAmount = Convert.IsDBNull(row["totalamtlocal"]) ? "0" : FormatHandler.FormatNumber20(row["totalamtlocal"]);
                    double rmAmount = Convert.IsDBNull(row["totalamt"]) ? 0 : Convert.ToDouble(row["totalamt"], FormatHandler.EngCult);
                    totalRmAmount += rmAmount;
                    TemplateHandler.Assign(ref rowHtml, "{Currency}", currency);
                    TemplateHandler.Assign(ref rowHtml, "{LocalAmt}", localAmount);
                    TemplateHandler.Assign(ref rowHtml, "{RmAmount}", FormatHandler.FormatNumber20(rmAmount));
                    odd_even = (odd_even == "") ? "evenrow" : "";
                    TemplateHandler.Assign(ref rowHtml, "{odd_even}", odd_even);
                    html.Append(rowHtml + "\n");
                }
            }
            if (hasData)
            {
                html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("CompBalance\\CompBalanceFooter.htm", new string[] { "{Total}" }, new string[] { FormatHandler.FormatNumber20(totalRmAmount) }));
            }
            else
            {
                html.Append(AccCommon.GenNoInformation("3"));
                html.Append("</table>");
            }
            ReportHtmlContent.Text = html.ToString();
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }
}