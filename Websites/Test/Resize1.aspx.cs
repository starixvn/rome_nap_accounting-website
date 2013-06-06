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
using Fanex.Tarus.Infrastructure.Extensions;
using System.Messaging;

public partial class ExcelData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            MessageQueue queue = new MessageQueue(@"FormatName:DIRECT=TCP:10.40.40.127\private$\entlib");

            queue.Send("First Message ", " Label ");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    
}
