using System;
using DNAide.Core;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.DataAccess.Report;

public partial class Report_ResetWinning_ResetWinning : System.Web.UI.Page
{
    public string strDate = "";
    public string fdate = "";
    public string serverTime = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        serverTime = DateTime.Now.ToString();
        DateTime MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay.AddDays(1);
        int displaydays = 0;
        TimeSpan sp = DateTime.Today - MoveBetsDay;
        displaydays = sp.Days;
        strDate = Calendar.GenCalendarControl("date", "../", "fdateTrigger", DateTime.Now.ToShortDateString(), false, false, 0, displaydays, "", "reportlist");
    }
}