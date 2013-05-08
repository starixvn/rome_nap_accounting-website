using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Fanex.Tarus.LiveCasino.Entities.Report
{
    [Serializable]
    [DataContract]
    public class DailyWinloss
    {
        [DataMember]
        public string bettype { get; set; }

        [DataMember]
        public string winloss { get; set; }

        [DataMember]
        public string position { get; set; }

        [DataMember]
        public string comm { get; set; }

        [DataMember]
        public string linetotal { get; set; }

        [DataMember]
        public string turnover { get; set; }

        [DataMember]
        public string margin { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ListDailyWL
    {
        [DataMember]
        public List<DailyWinloss> Items { get; set; }

        [DataMember]
        public DailyWinloss Total { get; set; }
    }
}
