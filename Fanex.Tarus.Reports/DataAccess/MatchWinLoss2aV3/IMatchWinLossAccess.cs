


using System;
using System.Collections;
using System.Data;

namespace Fanex.Tarus.Reports.DataAccess.MatchWinLoss2aV3
{
    public interface IMatchWinLossAccess
    {
        DataSet SelMainReport(DateTime reportDate, ref Hashtable outputParam);
        DataSet SelThirdPartyCasinoReport(DateTime reportDate);
        DataSet SelSportTypeReportDetail(DateTime reportDate, string sportName);
        DataSet SelBetTypeReportDetail(DateTime reportDate, string sportName, int betType, int liveType);
        DataSet SelMatchWinLossReportAllDetail(DateTime reportDate, ref Hashtable outputParam);
    }
}
