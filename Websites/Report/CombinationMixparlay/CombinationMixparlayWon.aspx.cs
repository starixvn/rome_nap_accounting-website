using System;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess.Report;
using ReportService = Sunplus.Accounting.WcfServiceLibrary.Report.ReportService;

public partial class Report_CombinationMixparlayWon : PageBase
{
    protected string ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    protected string MinDay = "";
    protected string FDay = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime BalanceUpDay = ReportUtils.BalanceUp.BalanceUpDay;
        int displaydays = 0;
        TimeSpan sp = DateTime.Today - BalanceUpDay;
        displaydays = sp.Days;
        ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
        MinDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today.AddDays(-displaydays));
        FDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);

        if (Request["type"] != null && Request["type"] == "export")
        {
            var isSoccer = Request["issoccer"] != null ? bool.Parse(Request["issoccer"]) : false;
            var date = Request["fdate"];
            new ReportService().CombinationMixparlayWonExportToExcel(isSoccer, date);
        }
    }

}
