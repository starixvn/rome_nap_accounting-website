using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sunplus.Accounting.Business.Report.NonePT;

public partial class Report_NonePT_NonePTMemSummary : System.Web.UI.Page
{
    public string header = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        IServiceNonePT s = new ServiceNonePT();
        header = s.LoadHeader();
    }
}
