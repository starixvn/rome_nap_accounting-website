using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Entities.Report.MatchAnalysis;

namespace Sunplus.Accounting.Business.Report.MatchAnalysis
{
    public interface IMatchAnalysisService
    {
        MatchAnalysisBO SelMatchAnalysisByType(int matchId, int statusId, ReportType reportType);

        ExcelFile ExportMatchAnalysis(int matchId, int statusId, ReportType reportType);
    }
}
