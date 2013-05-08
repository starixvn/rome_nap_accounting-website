using System;
using DNAide.Core;
using Sunplus.Accounting.Business.BootStrappers;

public partial class Report_DownLineBalance_SuperBalanceNew: PageBase
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
            var isViewTotalOnly = CastAide.AsBoolean(Request["isViewTotalOnly"]);
            var db = CastAide.AsString(Request["db"]);
            var result = CommonBootStrapper.Locator.GetInstance<IServiceDownLineBalance>();
            if (!isViewTotalOnly)
                result.ExportExcelDownLineBalance(roleId, custId, isYesterday, username, firstLetter, currency, title, isViewTotalOnly);
            else
            {
                var control = new Fanex.Tarus.Reports.Controllers.Balance.SuperBalanceReportController();
                control.ExportExcelSuperBalance(db, isYesterday, username, firstLetter, currency, title);
            }

        }
        catch(Exception ex)
        {

        }
    }
}