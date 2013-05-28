using System;
using Sunplus.Accounting.Business.BootStrappers;

public partial class Report_Deposit_TotalTrans : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["IsExport"] != null && Request["IsExport"] == "1")
        {
            var deposit = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();
            bool ShowCS = bool.Parse(Request["ShowVS"].ToString());
            deposit.Export2ExcelTotalTransaction(Request["reportTitle"], int.Parse(Request["resellerid"].ToString()), int.Parse(Request["licenseeid"].ToString()), Request["fdate"], bool.Parse(Request["showSB"].ToString()), bool.Parse(Request["showNG"].ToString()), bool.Parse(Request["showRB"].ToString()), bool.Parse(Request["showLC"].ToString()), bool.Parse(Request["ShowVS"].ToString()), ShowCS);
        }
    }
} 