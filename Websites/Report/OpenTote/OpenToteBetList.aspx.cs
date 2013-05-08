using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Web;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;

public partial class Report_OptenTote_ReportForSettlement : PageBase
{

    public string strTitle = "", strContent="";
    protected void Page_Load(object sender, EventArgs e)
    {
        Title = Sunplus.Accounting.Business.Report.Reports.Title;
        IServiceOpenToteSettlement obj = new ServiceOpenToteSettlement();
        strTitle = "Racing OpenTote Detail Bet List - Race " + Request["raceNo"] + " - " +
                   CacheRef.GetLeagueName(Request["meetingid"]) + " - " +
                   CacheUtils.GetExchangeName(DNAide.Core.CastAide.AsInt32(Request["currencyid"])) + " - " +
                   Request["winlostdate"];
        bool IsTest = Request["IsTest"] == "1";

        strContent = obj.ViewBetList(Request["meetingid"], Request["raceNo"], Request["currencyid"],
                                     Request["winlostdate"], Request["db"], IsTest);
    }
}
