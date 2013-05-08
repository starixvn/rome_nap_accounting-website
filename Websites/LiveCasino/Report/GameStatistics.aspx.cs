using System;
using Sunplus.Accounting.DataAccess.Report;


public partial class LiveCasino_Report_GameStatistics : PageBase
{
    public string MoveBetDay = "";
    public string BalanceUpDay = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        MoveBetDay = ReportUtils.BalanceUp.MoveBetsDay.ToShortDateString();
        BalanceUpDay = ReportUtils.BalanceUp.BalanceUpDay.ToShortDateString();
        
    }
}