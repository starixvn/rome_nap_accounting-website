using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using DNAide.Utils;
using System.Text;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using IBCUtils.DAL;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.DataAccess.Report;

public partial class Report_HRWLDetails : PageBase
{

    public StringBuilder html = new StringBuilder();    
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Sunplus.Accounting.Business.Report.Reports.Title;        
        string fdate = Request["fdate"];

        fdate = ReportHTML.ValidDate(fdate);

        #region Header
        int displaydays = 0;
        TimeSpan sp = DateTime.Today - ReportUtils.BalanceUp.BalanceUpDay;
        displaydays = sp.Days;
        string evt = "SetEvtGetdate()";
        string strDateTimeSingle = Sunplus.Accounting.Business.Report.Reports.GetDateTimeSingle(fdate, "Date", string.Empty, displaydays, "../", string.Empty, evt);

        TemplateHandler.Assign(ref strDateTimeSingle, "strDispday", "");

        string db = Request["db"];
        if (db == null) db = "Rep";
        if (db == "Rep")
        {
            TemplateHandler.Assign(ref strDateTimeSingle, "status-Rep", "checked=checked");
            TemplateHandler.Assign(ref strDateTimeSingle, "status-Main", "");
        }
        else
        {
            TemplateHandler.Assign(ref strDateTimeSingle, "status-Rep", "");
            TemplateHandler.Assign(ref strDateTimeSingle, "status-Main", "checked=checked");
        }
        TemplateHandler.Assign(ref strDateTimeSingle, "db", db);
        TemplateHandler.RemoveBlock(ref strDateTimeSingle, "path");

        headerDatetime.Text = strDateTimeSingle;

        #endregion
         
        
        string strErrMsg = "";
        if (Request["IsExport"] == "1") Sunplus.Accounting.Business.Report.Reports.ExportToExcel(ReportHTML.TOddsHR12BETWLReport(HttpContext.Current, ref strErrMsg), "Horse-Racing-Daily-Report-(12BET)-" + Request["wldate"]);
    }
  
}

