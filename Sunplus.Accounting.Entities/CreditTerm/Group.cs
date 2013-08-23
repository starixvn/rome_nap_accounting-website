using System;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.CreditTerm
{
    [DataContract]
    [Serializable]
    public class Group
    {
        [DataMember]
        public int GroupId { get; set; }

        [DataMember]
        public string HeadUsername { get; set; }

        [DataMember]
        public int CustId { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public int Members { get; set; }

        [DataMember]
        public int RoleId { get; set; }

        [DataMember]
        public string Checker { get; set; }

        [DataMember]
        public bool ConsolidationReport { get; set; }

        [DataMember]
        public bool Permisssion { get; set; }
    }
}