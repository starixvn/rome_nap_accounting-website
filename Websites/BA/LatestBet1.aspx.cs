using System;
using System.Web.UI;
using Sunplus.Accounting.Core.Common;

public partial class BA_LatestBet1 : PageBase 
{
    public string SecretKey = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string AccUserName = AppSession.AccUserName;
            string AppDomain = Request.Url.Host;
            string SeriesCrypt = "D78FBD3170FF53D7D8765BE35766C31B";
            SecretKey = AccUserName + "-" + IBCCrypt.IBCCrypt.GenMD5Key(AccUserName + SeriesCrypt);
        }
    }

}
