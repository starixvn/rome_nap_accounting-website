
using System.Collections;
using System.Data;
using Fanex.Tarus.VirtualSport.Entities.Report.DailyWL2h;

namespace Fanex.Tarus.VirtualSport.DataAccess.Report.DailyWL2h
{
    public interface IVirtualSport2hReportAccess
    {      
        DataSet SelDailyWLDirectDetail(DailyWinLoss2hParam param, ref Hashtable outputParam);     
        DataSet SelVirtualSportsReport(DailyWinLoss2hParam param, ref Hashtable outputParam);
        DataSet SelVirtualSportsReportBySport(DailyWinLoss2hParam param, ref Hashtable outputParam);
    }
}
