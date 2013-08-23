using System.Collections.Generic;
using Sunplus.Accounting.Entities.Report.Monitor;

namespace Sunplus.Accounting.Business.Report.Monitor
{
    public interface IMonitorService
    {
        Filter LoadFilterReport();

        List<Items> GetBettypes(List<Items> option, int sporttype);

        List<Items> GetSelectLeagues(List<Items> option, int sportId, string fromdate, string todate, int currency, int bettype, int live);

        MAMonitorDTO GetMAMonitorReport(Param param, bool fromcache);

        bool CheckMAMonitorReportFromSession(Param param);

        void ExportToExcelMAMonitorReport(int reportBy, string reportTitle);
    }
}