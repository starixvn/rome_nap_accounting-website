


using System;
namespace Fanex.Tarus.Reports.Entities.MixParlay
{
    [Serializable]
    public class ReportGrandTotal
    {
        public decimal Real { get; set; }
        public decimal Virtual { get; set; }
        public decimal Variance { get; set; }
    }
}
