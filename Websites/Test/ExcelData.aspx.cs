using System;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper.IP;



public partial class ExcelData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
                
        try
        {
            Convert.ToInt32("fadsfasd");
        }
        catch (Exception ex)
        {
            //var helper = new Fanex.Tarus.Finance.Helper();
            LogExtensions.ExceptionHandler(ex);
        }

        
    }


}
