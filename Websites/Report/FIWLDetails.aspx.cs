using System;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.Report;


public partial class Report_FIWLDetails : PageBase
{

    public string param = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Sunplus.Accounting.Business.Report.Reports.Title;

        string fdate = Request["fdate"];
        string tdate = Request["tdate"];

        fdate = ReportHTML.ValidDate(fdate);
        tdate = ReportHTML.ValidDate(tdate);

        param = "fdate=" + fdate + "&tdate=" + tdate + "&bettype=Finance";
        
        #region Header
        int displaydays = 0;
        TimeSpan sp = DateTime.Today - ReportUtils.BalanceUp.BalanceUpDay;
        displaydays = sp.Days;
        string strDateTimeSingle = "";
        strDateTimeSingle = Sunplus.Accounting.Business.Report.Reports.GetHeader1(fdate, tdate, "", displaydays, "");
        headerDatetime.Text = strDateTimeSingle;

        #endregion
    }
}
