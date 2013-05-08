using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Fanex.Tarus.LiveCasino.Entities.Betlist
{
    [Serializable]
    [DataContract]
    public class BetlistResultLC
    {
        [DataMember]
        public string KickoffTime { get; set; }

        [DataMember]
        public string GameType { get; set; }

        [DataMember]
        public string FinalHomeScore { get; set; }

        [DataMember]
        public string FinalAwayScore { get; set; }

        [DataMember]
        public string Results { get; set; }

        [DataMember]
        public string Status { get; set; }

       

    }
}
