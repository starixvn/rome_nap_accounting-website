using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.DailyWL
{
    [Serializable]
    [DataContract]
    public class DailyWLDetails
    {
        [DataMember]
        public List<DailyWLRowBO> Items { get; set; }

        [DataMember]
        public List<DailyWLRowBO> Totals { get; set; }
    }
}