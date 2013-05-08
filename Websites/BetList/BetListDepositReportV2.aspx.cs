using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DNAide.Core;
using DNAide.Web;
using DNAide.Utils;
using System.Collections;
using Sunplus.Accounting.Business.BetList;
using Sunplus.Accounting.Business.BetList.BetlistM88;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Report;

public partial class BetListDepositReport : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        this.ReportTitle.Text = Reports.Title;
        var betlist  = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.Deposit.IBetlistWLBySiteService>();

        Hashtable hash = new Hashtable();
        hash.Add("custid", CastAide.AsString(Request["custid"]));
        hash.Add("username", CastAide.AsString(Request["username"]));
        hash.Add("fdate",CastAide.AsString(Request["fdate"]));
        hash.Add("tdate", CastAide.AsString(Request["tdate"]));
        hash.Add("showsb", CastAide.AsString(Request["showSB"]));
        hash.Add("showrb", CastAide.AsString(Request["showRB"]));
        hash.Add("showng", CastAide.AsString(Request["showNG"]));
        hash.Add("showba", CastAide.AsString(Request["showBA"]));
        hash.Add("showlc", CastAide.AsString(Request["showLC"]));
        //hash.Add("excludetest", CastAide.AsInt32(Request["excludeTest"]));
        hash.Add("drawfilter", CastAide.AsInt32(Request["drawFilter"]));
        hash.Add("basecurrency", CastAide.AsString(Request["baseCurrency"]));
        hash.Add("site", CastAide.AsString(Request["site"]));
       

        this.reportContent.Text = betlist.RenderHTMLV2(hash);
    }
}
