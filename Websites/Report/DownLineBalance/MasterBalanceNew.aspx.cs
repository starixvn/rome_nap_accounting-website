using System;
using DNAide.Core;
using Sunplus.Accounting.Business.BootStrappers;

public partial class Report_DownLineBalance_MasterBalanceNew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["isexport"] != null && CastAide.AsBoolean(Request["isexport"]))
            ExportExcelDownlinceBalance();
    }

    private void ExportExcelDownlinceBalance()
    {
        try
        {
            var roleId = CastAide.AsInt32(Request["roleid"]);
            var custId = CastAide.AsInt32(Request["custid"]);
            var title = CastAide.AsString(Request["rtitle"]);
            var isYesterday = CastAide.AsBoolean(Request["isyesterday"]);
            var firstLetter = CastAide.AsString(Request["firstletter"]);
            var username = CastAide.AsString(Request["username"]);
            var currency = CastAide.AsString(Request["currency"]);

            var result = CommonBootStrapper.Locator.GetInstance<IServiceDownLineBalance>();
            result.ExportExcelDownLineBalance(roleId, custId, isYesterday, username, firstLetter, currency, title, false);

        }
        catch (Exception ex)
        {

        }
    }
}