using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Fanex.Tarus.SpecialDelete.SendEmailEntity.LiveCasino
{
    
    public class Match
    {
        [DataMember]
        public int MatchID { get; set; }

        [DataMember]
        public string MatchCode { get; set; }

        [DataMember]
        public string MatchEvent { get; set; }

        [DataMember]
        public string EventDate { get; set; }

        [DataMember]
        public string HomeName { get; set; }

        [DataMember]
        public int HomeId { get; set; }

        [DataMember]
        public int AwayId { get; set; }

        [DataMember]
        public string AwayName { get; set; }
    }
}
