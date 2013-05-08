



using System;
namespace Fanex.Tarus.Reports.Entities.MixParlay
{
    [Serializable]
    public class ReportTotal
    {
        public ReportDetail NormalTotal { get; set; }
        public ReportDetail EightRMTotal { get; set; }
        public ReportDetail BARMTotal { get; set; }
        public ReportDetail AllTotal { get; set; }
        public ReportGrandTotal GrandTotal { get; set; }
    }
}
