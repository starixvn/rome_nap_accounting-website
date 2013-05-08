using System;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;

public partial class Statement_StatementOptions : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string htmlStr = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Statement\\StatementOptions.htm");
        if (Session["CustRoleId"] == null || (int)Session["CustRoleId"] < 2)
        {
            TemplateHandler.Assign(ref htmlStr, "{IsAgent}", "hidden");
        }
        html.Text = htmlStr;
    }
}
