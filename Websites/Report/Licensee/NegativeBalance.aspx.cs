using System;
using DNAide.Core;
using Sunplus.Accounting.Business.BootStrappers;

public partial class Report_Deposit_NagativeBalance : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["IsExport"] != null && Request["IsExport"] == "1")
        {
            var deposit = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            deposit.Export2ExcelNegativeBalanceReport(Request["reportTitle"], Request["site"]);
        }
    }
}