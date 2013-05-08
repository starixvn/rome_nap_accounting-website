using System;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Core.Web.Controls;

public partial class Report_NumberGame_DateWinLossNew : PageBase
{
    protected string ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    protected string FDay = "";
    protected string TDay = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
        FDay = string.Format("{0:MM/dd/yyyy}", DateTime.Now.AddDays(-1));
        TDay = string.Format("{0:MM/dd/yyyy}", DateTime.Now.AddDays(-1));      
        if (Request["type"] != null)
        {
            string fdate = Request["fdate"].ToString();
            string tdate = Request["tdate"].ToString();
            string type = Request["type"].ToString();
            if (type != null && type != "")
            {
                var numberGameReport = CommonBootStrapper.Locator.GetInstance<INumberGameReport>();
                numberGameReport.ExportToExcel_WinLoss_ByDate_NumberGame(fdate, tdate);
            }
        }
    }
}