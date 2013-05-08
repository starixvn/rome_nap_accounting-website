using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DNAide.Data;
using DNAide.Core;
using DNAide.Web;
using DNAide.Utils;
using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;

public partial class Report_HRGroupDaily : PageBase
{


    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Sunplus.Accounting.Business.Report.Reports.Title;
        string fdate = CastAide.AsDateTime(Request["fdate"], DateTime.Today).ToShortDateString();
        string tdate = CastAide.AsDateTime(Request["tdate"], DateTime.Today).ToShortDateString();
        string strHeader = Sunplus.Accounting.Business.Report.Reports.GetHeader1(fdate, tdate, "../");

        TemplateHandler.ReplaceBlock(ref  strHeader, "export", "<input onclick='ViewReport(1);' class='btn' type='button' value='Send Email'/>");

        Header.Text = strHeader;

    }


}
