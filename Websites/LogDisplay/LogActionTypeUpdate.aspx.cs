using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;

public partial class LogDisplay_LogActionTypeUpdate : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string actionTypeID = CastAide.AsString(Request["actiontypeid"]);
        this.lContent.Text = Sunplus.Accounting.Business.LogDisplay.LogAction.GetActionTypeEditForm(actionTypeID);
    }
}
