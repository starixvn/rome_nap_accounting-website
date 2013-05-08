using System;
using DNAide.Web;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Core.LoggingLib;

using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Common;

public partial class Customer_TransferAllMember : PageBase
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {                        
            sTitle.Text = "TRANSFER ALL";
            sContent.Text = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Customer/TransferAllMember.htm");
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
    }


}
