
using System;

namespace Fanex.Tarus.VirtualSport.Entities.Report.DailyWL2h
{
    [Serializable]
    public class ReportGrandTotal
    {
        public decimal Real { get; set; }
        public decimal Virtual { get; set; }
        public decimal Variance { get; set; }
    }
}
