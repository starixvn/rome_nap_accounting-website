using System;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;


public partial class Customer_EditUserName : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            EditUserNameHeaderStr.Text = "Edit Username";
            string strHtml = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/EditUserName.htm");
            //TemplateHandler.Assign(ref strHtml, "{title}", "");
            TemplateHandler.RemoveBlock(ref strHtml, "PageTitle");
            TemplateHandler.Assign(ref strHtml, "{txtName1}", "txtOldUname");
            TemplateHandler.Assign(ref strHtml, "{oldUname}", "Old Username");
            TemplateHandler.Assign(ref strHtml, "{newUname}", "New Username");
            TemplateHandler.Assign(ref strHtml, "{txtName2}", "txtNewUname");
            TemplateHandler.Assign(ref strHtml, "{ddl-hidden}", "hidden");
            EditUserNameContentHtml.Text = strHtml;
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
    }
 
}
