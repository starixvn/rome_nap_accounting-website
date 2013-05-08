using System;
using Sunplus.Accounting.Business.CustSetting;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;

public partial class Customer_ChangeSite : PageBase
{

    public string html = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        CustSettingMoveSite site = new CustSettingMoveSite();
        string CurrenctSite = Request["Site"];
        string UserName = Request["CustId"];
        int RoleId = DNAide.Core.CastAide.AsInt32(Request["RoleId"]);
        int CustId = DNAide.Core.CastAide.AsInt32(Request["CustId"]);
        html = site.LoadChangSiteForm(CurrenctSite, UserName, RoleId, CustId);
    }
}
