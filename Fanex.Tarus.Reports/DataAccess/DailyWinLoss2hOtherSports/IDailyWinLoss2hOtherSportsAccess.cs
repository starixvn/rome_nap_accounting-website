
using System.Collections;
using System.Data;
using Fanex.Tarus.Reports.Entities.DailyWinLoss2h;

namespace Fanex.Tarus.Reports.DataAccess.DailyWinLoss2hOtherSports
{
    public interface IDailyWinLoss2hOtherSportsAccess
    {
        DataSet DailyWLDirect_Sporttype(DailyWinLoss2hParam param, ref Hashtable outputParam);
        DataSet SelDailyWLDirectDetail(DailyWinLoss2hParam param, ref Hashtable outputParam);
        DataSet SelOtherSportsReport(DailyWinLoss2hParam param, ref Hashtable outputParam);
        DataSet SelOtherSportsReportBySport(DailyWinLoss2hParam param, ref Hashtable outputParam);
    }
}
