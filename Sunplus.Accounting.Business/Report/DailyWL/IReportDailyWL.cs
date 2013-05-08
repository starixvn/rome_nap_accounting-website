using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNAide.Data;
using Sunplus.Accounting.Entities.Report.DailyWL;
using Sunplus.Accounting.Entities.Report.Mixparlay;

namespace Sunplus.Accounting.Business.Report.DailyWL
{
    public interface IReportDailyWL
    {
        DailyWLBO DailyWLDirectRacing(string winlossdate);

        DailyWLBO DailyWLDirectRacingBySport(string sporttype, string winlossdate);

        DailyWLDTO DailyWLDirectRacingBySportDetails(string winlossdate, string sporttype, string bettype);

        void Export2ExcelDailyWLDirectRacing(string winlossDate);

        void Export2ExcelDailyWLDirectRacingBySport(string sporttype, string winlossdate);

        void Export2ExcelDailyWLDirectRacingBySportDetails(string sporttype, string bettype, string winlossdate);

        DbResult DailyWLDirect_Sporttype(string winlossdate);

        DbResult DailyWLDirect_MixParlay(string winlossdate, int bettype);

        MixParlayView DailyWLDirect_MixParlayAdvance(string winlossdate, int bettype);

        void Export2ExcelDailyWLDirectMixParlayAdvance(string winlossdate, int bettype, string reportTitle);
    }
}
