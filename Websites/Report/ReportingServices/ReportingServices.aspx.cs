using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL.Report;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;

public partial class Report_ReportingServices : PageBase
{

    public string html = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ReportingServices report = new ReportingServices();
        html = report.GetFilesDownLoad(report.GetPath, false);
    }
}
