using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Core;
using System.Text;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;

public partial class Report_OpenTote_Comparision : PageBase
{

    public StringBuilder html = new StringBuilder();
    protected void Page_Load(object sender, EventArgs e)
    {
        string winlostdate = CastAide.AsDateTime(Request["winlostdate"], DateTime.Today).ToShortDateString();
        string ibcTO = CastAide.AsString(Request["ibcTO"]);
        string ibcDividendWon = CastAide.AsString(Request["ibcDividendWon"]);

        IServiceOpenToteSettlement opentote = new ServiceOpenToteSettlement();
        html.Append(opentote.LoadComparision(winlostdate, ibcTO, ibcDividendWon));
    }
}
