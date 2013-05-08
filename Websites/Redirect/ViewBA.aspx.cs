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
using Sunplus.Accounting.Core.Common;
using DNAide.Core;
using System.Collections.Generic;
using Sunplus.Accounting.Core.Configurations;


public partial class View : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int _accId = AppSession.AccId;
        string _userName = AppSession.AccUserName;
        int _onUserId = AppSession.OnAccId;
        string system = AppSession.SYSTEM.ToString();
        string strParams = string.Empty, path = string.Empty,  url = string.Empty;
        //string loginTime = Sunplus.Accounting.DataAccess.IBCDLL.SystemInfo.GetDBTime().Ticks.ToString();
        strParams = _userName + "&" + _accId + "&" + _onUserId + "&" + system;
        path = Request.PhysicalApplicationPath + "/IbcKeys/ibcpkey.snk";
        RSACryptoPad.EncryptionThread encryption = new RSACryptoPad.EncryptionThread();
        strParams = encryption.EncryptString(strParams, 1024, path);

        url = (WebSettings.DirectBAUrl.StartsWith("http") ? WebSettings.DirectBAUrl : "http://" + WebSettings.DirectBAUrl) + "?loginInfo=" + strParams;
        //url = "http://localhost:62421/BA/PreLogin/ProcessLoginBA.ashx?params=" + strParams;
        
       
        Response.Redirect(url);
 
    }
}
