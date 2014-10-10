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
using System.Xml.Linq;
using Sunplus.Accounting.Business.BetList;
using DNAide.Core;
using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.DataAccess.Report;

public partial class BetList_CombinationMPDetail : RequiredAuthorizationPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BetListMain betlist = new BetListMain();
        string refno = CastAide.AsString(Request.QueryString["refno"]);
        double transid = CastAide.AsDouble(Request.QueryString["transid"]);
        string betid = CastAide.AsString(Request.QueryString["betid"]);

        int custId = Request.QueryString["custid"] != null ? CastAide.AsInt32(Request.QueryString["custid"]) : CastAide.AsInt32(HttpContext.Current.Session["CustId"]);
        DateTime movebetDay = ReportUtils.BalanceUp.MoveBetsDay;
        DateTime balanceUpDay = ReportUtils.BalanceUp.BalanceUpDay;
        ltrContent.Text = betlist.GenCombinationMP(custId, refno, transid, betid, movebetDay, balanceUpDay);
    }
}
