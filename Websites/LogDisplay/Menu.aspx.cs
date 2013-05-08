using System;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;

public partial class Others_Menu : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string tplMenu = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("LogMenu.htm");
        this.sMenu.Text = tplMenu;        
    }
}
