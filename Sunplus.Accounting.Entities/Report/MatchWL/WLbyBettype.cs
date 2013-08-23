using System;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.MatchWL
{
    [Serializable]
    [DataContract]
    public class WLbyBettype
    {
        [DataMember]
        public string bettype { get; set; }

        [DataMember]
        public string bettypeId { get; set; }

        [DataMember]
        public string LineTotal_soccer_live { get; set; }

        [DataMember]
        public string VirtualLineTotal_soccer_live { get; set; }

        [DataMember]
        public string LineTotal_soccer { get; set; }

        [DataMember]
        public string VirtualLineTotal_soccer { get; set; }

        [DataMember]
        public string LineTotal_others_live { get; set; }

        [DataMember]
        public string VirtualLineTotal_others_live { get; set; }

        [DataMember]
        public string LineTotal_others { get; set; }

        [DataMember]
        public string VirtualLineTotal_others { get; set; }

        [DataMember]
        public string LineTotalReal { get; set; }

        [DataMember]
        public string LineTotalVirtual { get; set; }

        [DataMember]
        public string Variance { get; set; }

        [DataMember]
        public bool IsTotal { get; set; }
    }
}