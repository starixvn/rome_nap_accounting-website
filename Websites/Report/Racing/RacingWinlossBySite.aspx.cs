using System;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Core.Configurations;

public partial class Report_Racing_RacingWinlossBySite : PageBase
{
    protected string ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    protected string MinDay = "";
    protected string FDay = "";
    protected string MaxDay = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime BLDate = ReportUtils.BalanceUp.BalanceUpDay;
        int displaydays = 0;
        TimeSpan sp = DateTime.Today - BLDate;
        displaydays = sp.Days;
        ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
        MinDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today.AddDays(-displaydays));
        FDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
        MaxDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today.AddDays(7));
    }
}