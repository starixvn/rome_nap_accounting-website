using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.DailyWL
{
    [Serializable]
    [DataContract]
    public class DailyWLDTO
    {
        [DataMember]
        public List<DailyWLDetails> Items { get; set; }

        [DataMember]
        public List<DailyWLRowBO> Totals { get; set; }
    }
}