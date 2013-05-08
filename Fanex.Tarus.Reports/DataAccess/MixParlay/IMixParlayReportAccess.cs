

using System;
using System.Data;

namespace Fanex.Tarus.Reports.DataAccess.MixParlay
{
    public interface IMixParlayReportAccess
    {
        DataTable SelDirectMixParlay(DateTime reportDate, int betType);

        DataTable SelAdvanceMixParlayReport(DateTime fromDate, DateTime toDate, bool isViewDW, int betType);

        DataTable SelDailyParlayCombinationWinLoss(DateTime fromDate, DateTime toDate, bool isViewDW );
    }
}
