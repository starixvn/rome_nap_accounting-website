using System;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Core.Configurations;
using System.Linq;
using Sunplus.Accounting.Business.Security;

public partial class Report_Deposit_Deposit : System.Web.UI.Page
{
    public string strfdate = string.Empty, strtdate = string.Empty, strSite = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["IsExport"] == "1")
        {
            ExcelFile excelFile = new ExcelFile();
            var deposit = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IDepositReportService>();
            bool hasData = false;
            excelFile = deposit.ExportDepositReport(Request["site"], Request["fdate"], Request["tdate"], ref hasData, Request["reportType"], Request["level"], Request["currency"], Request["baseCurrency"]);
            if (hasData)
            {
                string filename = "Deposit-Report-" + DateTime.Today.ToShortDateString().Replace("/", "-");
                Response.ContentType = "Application/x-msexcel";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ".xls");
                excelFile.SaveXls(Response.OutputStream);
            }
        }
        else
        {

            DateTime fdate = DateTime.Today;
            DateTime tdate = DateTime.Today;

            int displaydays = 0;
            TimeSpan sp = tdate - fdate;
            displaydays = sp.Days;

            strfdate = Sunplus.Accounting.Core.Web.Controls.Calendar.GenCalendarControl("fdate", "../../", "fdateTrigger", fdate.ToShortDateString(), false, false, 0, 0, "");
            strtdate = Sunplus.Accounting.Core.Web.Controls.Calendar.GenCalendarControl("tdate", "../../", "tdateTrigger", tdate.ToShortDateString(), true, false, 0, 0, "");

        }
        
    }
}