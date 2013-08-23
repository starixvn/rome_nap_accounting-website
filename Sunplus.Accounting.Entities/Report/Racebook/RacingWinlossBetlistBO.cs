using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.Racebook
{
    [Serializable]
    [DataContract]
    public class RacingWinlossBetlistBO
    {
        [DataMember]
        public List<RacingWinlossBetlistRowBO> Items { get; set; }

        [DataMember]
        public RacingWinlossBetlistRowBO Totals { get; set; }
    }
}