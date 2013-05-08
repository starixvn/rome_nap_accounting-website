using System;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.WcfServiceLibrary.Report;

public partial class Report_Deposit_ConcurrentUser : PageBase
{
    protected string ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    protected string FDay = "";
    protected string TDay = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //FDate = Request["fdate"];
        //TDate = Request["tdate"];
        ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
        FDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
        TDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);

        if (Request["export"] != null && Request["export"].Equals("eportexcel"))
        {
            var site = Request["sitelist"];
            var fDate = Request["fdate"];
            var tDate = Request["tdate"];
            var licenseeService = new LicenseeService();
            licenseeService.PeakExport2Excel(site, fDate, tDate);
        }
    }
}