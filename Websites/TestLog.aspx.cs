using Fanex.Data;
using Fanex.Logging;
using Sunplus.Accounting.Core.Extensions;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.UI;

public partial class TestLog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    
        if (this.IsPostBack)
        {
            Fanex.Logging.LogService.Error("Test log");
        }
    }
}