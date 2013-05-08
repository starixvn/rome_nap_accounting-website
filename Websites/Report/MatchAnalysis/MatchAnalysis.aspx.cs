using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Core;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities.Report.MatchAnalysis;

public partial class Report_MatchAnalysis_MatchAnalysis : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["Export"] == "1")
        {
            int matchId = CastAide.AsInt32(Request["matchId"]);
            int statusId = CastAide.AsInt32(Request["statusId"]);
            int reportType = CastAide.AsInt32(Request["reportType"]);

            ReportType _reportType = ReportType.Currency;

            switch (reportType)
            {
                case 0:
                    _reportType = ReportType.Currency;
                    break;
                case 1:
                    _reportType = ReportType.Bettype;
                    break;
                case 2:
                    _reportType = ReportType.Super;
                    break;
                case 3:
                    _reportType = ReportType.Member;
                    break;
                case 4:
                    _reportType = ReportType.Country;
                    break;

            }

            var match = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.MatchAnalysis.IMatchAnalysisService>();
            var excel = match.ExportMatchAnalysis(matchId, statusId, _reportType);
            string filename = string.Format("SpecialMatchAnalysis{0}", DateTime.Today.ToShortDateString());
            Response.ContentType = "Application/x-msexcel";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ".xls");
            excel.SaveXls(Response.OutputStream);

        }
    }
}