using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities.Report.MatchAnalysis;

namespace Sunplus.Accounting.WcfServiceLibrary.Report.MatchAnalysis
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MatchAnalysisService:IMatchAnalysisService
    {
        public MatchAnalysisBO SelMatchAnalysisByType(int matchId, int statusId, int reportType)
        {
            var match = CommonBootStrapper.Locator.GetInstance<Sunplus.Accounting.Business.Report.MatchAnalysis.IMatchAnalysisService>();
            ReportType _reportType = ReportType.Currency  ;

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

            return match.SelMatchAnalysisByType(matchId, statusId, _reportType);
        }


       
    }
}
