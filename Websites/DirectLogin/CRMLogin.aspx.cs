using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;

public partial class DirectLogin_CRMLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var accId = AppSession.AccId;
        var accName = AppSession.AccUserName;
        var onUserId = AppSession.OnAccId;
        var param = string.Empty;
        var path = string.Empty;
        var url = string.Empty;
        var sid = Request["sid"];

        var crmUrl = ConfigurationManager.AppSettings["crmloginurl"];

        Uri uri = HttpContext.Current.Request.Url;
        string host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + (uri.Port == 80 ? "" : ":" + uri.Port);
        crmUrl = string.Format(crmUrl, host);

        param = string.Format("{0}&{1}&{2}&{3}", accId, accName, onUserId, sid);
        path = Request.PhysicalApplicationPath + "/IbcKeys/ibcpkey.snk";
        RSACryptoPad.EncryptionThread encryption = new RSACryptoPad.EncryptionThread();
        param = encryption.EncryptString(param, 1024, path);
        url = (crmUrl.StartsWith("http") ? crmUrl : "http://" + crmUrl) + "?params=" + param;
        Response.Redirect(url);
        Response.End();
    }
}