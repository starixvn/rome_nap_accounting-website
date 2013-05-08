using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sunplus.Accounting.Core.Common;

public partial class SpecialDelete_NumberGame_FormDeleteBet : PageBase
{
    public string Username;
    protected void Page_Load(object sender, EventArgs e)
    {
        Username = AppSession.AccUserName;
    }
}