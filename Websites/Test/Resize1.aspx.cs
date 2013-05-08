using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL.Report;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;

using System.Text.RegularExpressions;

public partial class ExcelData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            Response.Write(RemotingRef.RefData.GetBetTypeName("1") + "<br>");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    
}
