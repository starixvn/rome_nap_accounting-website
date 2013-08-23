using System;
using System.Text;
using System.Web;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;

public partial class NewCustomer_OperatorList : PageBase
{
    private string searchText = "All", selectAlpha = "All";
    private string strQuery = "";
    private int PageIndex = 1, TotalPages = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        if (Session["AccId"] != null)
        {
            OperatorListHeaderStr.Text = "Operator List";
            OperatorListContentHtml.Text = GenOpertorListContent();
        }
    }

    private string GenOpertorListContent()
    {
        StringBuilder html = new StringBuilder();
        try
        {
            string strHtml = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/Search.htm");
            TemplateHandler.RemoveBlock(ref strHtml, "tblFilter");
            TemplateHandler.Assign(ref strHtml, "{rowspan}", "2");
            TemplateHandler.Assign(ref strHtml, "{functionName}", "GetOperatorList");
            TemplateHandler.Assign(ref strHtml, "{Keyword}", "Keyword");
            TemplateHandler.Assign(ref strHtml, "{search-Val}", searchText);
            TemplateHandler.Assign(ref strHtml, "{listAlphaValue}", AccCommon.GetListAlPhabet(selectAlpha));
            Search.Text = strHtml;
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
        return html.ToString();
    }
}