using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Sunplus.Accounting.Business.BetList;
using Sunplus.Accounting.Core.Common;

public partial class BetList_DeleteBet : System.Web.UI.Page
{
    public string userId;
    protected void Page_Load(object sender, EventArgs e)
    {
        lContent.Text = DeleteBet.LoadContent();
        userId = AppSession.AccUserName;

    }
}
