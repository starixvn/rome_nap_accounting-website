using System;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;

public partial class LogDisplay_LogActionType : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string[] strListUser = AccCommon.FuncMgr.Split(';');
            bool isMgr = false;
            for (int i = 0; i < strListUser.Length; i++)
                if (string.Compare(Session["Username"].ToString(), strListUser[i], true) == 0)
                {
                    isMgr = true;
                    break;
                }
            if (!isMgr)
            {
                Response.Redirect("../AccessDenied.aspx");
                return;
            }


            LoadContent();
        }
    }
    private void LoadContent()
    {
        string template = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("LogDisplay/LogActionTypeFilter.htm");
        this.lContent.Text = template;
    }
}
