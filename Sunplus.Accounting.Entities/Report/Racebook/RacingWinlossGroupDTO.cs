using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.Racebook
{
    [Serializable]
    [DataContract]
    public class RacingWinlossGroupDTO
    {
        [DataMember]
        public List<RacingWinlossGroupDayBO> Items { get; set; }

        [DataMember]
        public List<RacingWinlossGroupRowBO> Total { get; set; }
    }

    [Serializable]
    [DataContract]
    public class RacingWinlossGroupDayBO
    {
        [DataMember]
        public List<RacingWinlossGroupRowBO> Items { get; set; }

        [DataMember]
        public RacingWinlossGroupRowBO Total { get; set; }
    }
}