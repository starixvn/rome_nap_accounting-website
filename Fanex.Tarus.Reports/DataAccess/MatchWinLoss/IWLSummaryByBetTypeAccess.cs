

using System;
using System.Data;

namespace Fanex.Tarus.Reports.DataAccess.MatchWinLoss
{
    public interface IWLSummaryByBetTypeAccess
    {
        DataSet SelSummaryReportByBetType(DateTime reportDate);
    }
}
