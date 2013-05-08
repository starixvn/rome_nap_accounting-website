using System;
using DNAide.Core;
using Sunplus.Accounting.Business.BootStrappers;
using System.Web.UI;

public partial class Report_Poker_PokerWL : PageBase
{
    public string yesterday = string.Empty;
    public string today = string.Empty;
    public string showOnload = "false";
    protected string ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    protected string FDay = "";
    protected string TDay = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        var deposit = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IReportWLBySiteService>();

        if (Request["isexport"] != null && Request["isexport"] == "1")
        {
            var poker = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Poker.IPokerReport>();
            var rpTitle = Request["rptitle"];

            if (Request["rptype"] == "1")
            {
                var fromDate = Request["fdate"];
                var toDate = Request["tdate"];
                poker.ExportExcelWinlostP2P(fromDate, toDate, rpTitle);
            }
            else if (Request["rptype"] == "2")
            {
                var selectedDate = Request["sdate"];
                var currency = CastAide.AsInt32(Request["currency"]);
                poker.ExportExcelWinlostP2PDetail(currency, selectedDate, rpTitle);
            }


        }
        else
        {
            showOnload = Request.QueryString["showOnload"] != null ? "true" : "false";
            DateTime fdate = Request.QueryString["fdate"] != null ? CastAide.AsDateTime(Request.QueryString["fdate"]) : DateTime.Today;
            DateTime tdate = Request.QueryString["tdate"] != null ? CastAide.AsDateTime(Request.QueryString["tdate"]) : DateTime.Today;

            ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
            FDay = string.Format("{0:MM/dd/yyyy}", fdate);
            TDay = string.Format("{0:MM/dd/yyyy}", tdate);
            today = deposit.GetDatetimeOption("1").tdate;
            yesterday = deposit.GetDatetimeOption("2").tdate;

        }

    }

}
