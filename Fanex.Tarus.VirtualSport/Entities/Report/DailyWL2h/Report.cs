
using System;
using System.Collections.Generic;
using Fanex.Tarus.Entities.Sport;

namespace Fanex.Tarus.VirtualSport.Entities.Report.DailyWL2h
{
    [Serializable]
    public class Report
    {
        public List<ReportDetail> ReportDetails { get; set; }
        public ReportTotal Total { get; set; }
        public BaseModel ReportType { get; set; }
        public int TotalItems { get; set; }
        public string ReportDate { get; set; }
        public bool IsHavingIBC { get; set; }

        public BaseModel LiveType { get; set; }
        public bool IsBARMActive { get; set; }
    }
}
