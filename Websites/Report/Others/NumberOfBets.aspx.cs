using System;
using DNAide.Core;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Report.Other;

public partial class Report_Deposit_NumberOfBets : PageBase
{
    protected string ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    protected string FDay = "";
    protected string TDay = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        var fdate = DateTime.Today.AddDays(-1);
        var tdate = DateTime.Today.AddDays(-1);
        ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
        FDay = string.Format("{0:MM/dd/yyyy}", fdate);
        TDay = string.Format("{0:MM/dd/yyyy}", tdate);

        if (Request["export"] != null && Request["export"].Equals("1"))
        {
            var selFromDate = Request["fdate"] ?? DateTime.Now.ToShortDateString();
            var selToDate = Request["tdate"] ?? DateTime.Now.ToShortDateString();
            var register =
                CommonBootStrapper.Locator.GetInstance
                    <INumberOfBetsManager>();
            register.ExportExcelNumberOfBetsForCreditSites(CastAide.AsDateTime(selFromDate), CastAide.AsDateTime(selToDate));
        }
    }
}