using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using IBCUtils.Web;
using IBCUtils.DAL;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using IBCUtils;
using DNAide.Core;
using System.Text;
using DNAide.Data;
using DNAide.Web;
public partial class Customer_SuspendCustList : PageBase
{

    public string strsearchbox = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["AccId"] != null)
        {
            SuspendListContentHeaderStr.Text = "Auto Suspend";
            strsearchbox = GenSearchBox();
        }
    }

    public string GenSearchBox()
    {
        string strHtml = "";

        strHtml = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/Search.htm");
        TemplateHandler.RemoveBlock(ref strHtml, "tblFilter");
        TemplateHandler.Assign(ref strHtml, "{rowspan}", "2");
        TemplateHandler.Assign(ref strHtml, "{functionName}", "GetSuspendList");
        TemplateHandler.Assign(ref strHtml, "{Keyword}", "Keyword");

        TemplateHandler.Assign(ref strHtml, "{search-Val}", "All");
        TemplateHandler.Assign(ref strHtml, "{listAlphaValue}", AccCommon.GetListAlPhabet(""));
        return strHtml;
    }
}
