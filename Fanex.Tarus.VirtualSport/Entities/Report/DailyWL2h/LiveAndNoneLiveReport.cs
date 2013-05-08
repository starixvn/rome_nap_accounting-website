
using System;
namespace Fanex.Tarus.VirtualSport.Entities.Report.DailyWL2h
{
    [Serializable]
    public class LiveAndNoneLiveReport
    {
        public Report NoneLiveReport { get; set; }
        public Report LiveReport { get; set; }
        public Report MixLiveAndNoneLiveReport { get; set; }
        public ReportTotal Total { get; set; }
    }
}
