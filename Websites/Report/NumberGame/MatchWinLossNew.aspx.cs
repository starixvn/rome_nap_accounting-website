using System;
using DNAide.Core;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Report;
using Sunplus.Accounting.Core.Web.Controls;


public partial class Report_NumberGame_MatchWinLossNew : PageBase
{
    public string strDate = "";
    protected string ToDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today);
    protected string FDay = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        FDay = string.Format("{0:MM/dd/yyyy}", DateTime.Today.AddDays(-1));
        if (Request["type"] != null)
        {
            var numberGameReport = CommonBootStrapper.Locator.GetInstance<INumberGameReport>();

            string date = Request["date"].ToString();
            string type = Request["type"].ToString();

            if (!string.IsNullOrEmpty(type))
            {
                if (CastAide.AsInt32(type) == 1)
                    numberGameReport.ExportToExcelNumberGameWinLossByMatch(date, Request["companies"], Request["currencies"], Request["bettypes"]);
                else if (CastAide.AsInt32(type) == 2)
                {
                    bool isLive = CastAide.AsBoolean(Request["isLive"]);
                    int matchId = CastAide.AsInt32(Request["matchid"]);
                    int bettype = CastAide.AsInt32(Request["bettype"]);

                    numberGameReport.ExportToExcelNumberGameWinLossDetailByMatch(date, isLive, bettype, matchId, Request["companies"], Request["currencies"]);
                }
            }
        }
    }
}