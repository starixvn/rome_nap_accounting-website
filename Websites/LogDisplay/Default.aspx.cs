using System;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;

public partial class LogDisplay_Default : PageBase
{

    protected override bool HasPermission()
    {
        return Functions.Customer.AllowViewLog && base.HasPermission();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }


}
