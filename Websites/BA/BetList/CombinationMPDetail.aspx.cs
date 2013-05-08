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

public partial class BetList_CombinationMPDetail : RequiredAuthorizationPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BetListMain betlist = new BetListMain();
        string refno = CastAide.AsString(Request.QueryString["refno"]);
        ltrContent.Text = betlist.GenCombinationMP(refno);
    }
}
