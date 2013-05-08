using System;
using DNAide.Core;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Report.DailyWL;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.Report;

public partial class Report_DailyWL_MixParlayAdvance : PageBase
{
    public string strwinlossdate = "";
    public string fdate = "";
    public string Today = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    public string Yesterday = string.Format("{0:MM/dd/yyyy}", DateTime.Today.AddDays(-1));
    public string isPopup = "";
    public string datewinloss = WebSettings.getDateDailyWLMP;
    protected void Page_Load(object sender, EventArgs e)
    {
        isPopup = CastAide.AsString(Request["ispopup"]);
        DateTime winlossMP = CastAide.AsDateTime(WebSettings.getDateDailyWLMP);
        /*int displaydays = 0;
        TimeSpan sp = DateTime.Today - winlossMP;
        displaydays = sp.Days;
        fdate = CastAide.AsDateTime(Request["fdate"]).ToShortDateString();
        strwinlossdate = Sunplus.Accounting.Core.Web.Controls.Calendar.GenCalendarControl("fdate", "../",
                                                                                        "fdateTrigger",
                                                                                        fdate, false,
                                                                                        false, 0, displaydays, "");*/
        fdate = CastAide.AsDateTime(Request["fdate"]).ToShortDateString();
        //DateTime tdate = DateTime.Today;

        int displaydays = 0;
        //TimeSpan sp = tdate - fdate;
        //displaydays = sp.Days;

        strwinlossdate = Sunplus.Accounting.Core.Web.Controls.Calendar.GenCalendarControl("fdate", "../",
                                                                                    "fdateTrigger",
                                                                                    fdate, false,
                                                                                    false, 0, 0, "");
        if (Request["isExport"] != null && CastAide.AsBoolean(Request["isExport"]))
        {
            var result = CommonBootStrapper.Locator.GetInstance<IReportDailyWL>();
            result.Export2ExcelDailyWLDirectMixParlayAdvance(CastAide.AsString(Request["wldate"]),
                                                             CastAide.AsInt32(Request["bettype"]),
                                                             CastAide.AsString(Request["title"]));
        }

    }
}