using System;
using Fanex.Security;
using Sunplus.Accounting.Core.Common;
using System.Configuration;

public partial class DirectLogin_IMS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var userId = Cryptography.CfsEnCrypt(AppSession.AccId.ToString());
        var onuserId = Cryptography.CfsEnCrypt(AppSession.OnAccId.ToString());
        var redirectToUrl = Request["redirectToUrl"];

        var url = string.Format("{0}?username={1}&onuserid={2}&redirectToUrl={3}", ConfigurationManager.AppSettings["IMSLoginUrl"], userId, onuserId, redirectToUrl);
        Response.Redirect(url);
    }
}