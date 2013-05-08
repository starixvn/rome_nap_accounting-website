using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Core;
using Fanex.Tarus.LiveCasino.Business.Report;
using Sunplus.Accounting.Business.BootStrappers;


public partial class Report_DailyWL_LiveCasino : System.Web.UI.Page
{
    public string strwinlossdate = "";
    public string fdate = "";
    public string Today = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    public string Yesterday = string.Format("{0:MM/dd/yyyy}", DateTime.Today.AddDays(-1));
    protected void Page_Load(object sender, EventArgs e)
    {

        fdate = CastAide.AsDateTime(Request["fdate"]).ToShortDateString();
        strwinlossdate = Sunplus.Accounting.Core.Web.Controls.Calendar.GenCalendarControl("fdate", "../../",
                                                                                   "fdateTrigger",
                                                                                   fdate, false,
                                                                                   false, 0, 0, "");
       
        if (Request["isExport"] != null && CastAide.AsBoolean(Request["isExport"]))
        {
           // var _exportExcel = new ExportExcelReport();
            var _exportExcel = CommonBootStrapper.Locator.GetInstance<IExportExcelReport>();
            _exportExcel.ExportExcelDailyWLDirectLC(Request["wldate"].ToString(),Request["reportTitle"].ToString());
        }
    }
}