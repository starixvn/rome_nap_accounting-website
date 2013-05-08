using System;
using System.Web.UI;
using DNAide.Core;

public partial class BA_ViewLogComm : PageBase
{
    public int licenseeId;
    protected void Page_Load(object sender, EventArgs e)
    {
        licenseeId = (string.IsNullOrEmpty(Request["licenseeId"])) ? 0 : CastAide.AsInt32(Request["licenseeId"], 0);
    }

}