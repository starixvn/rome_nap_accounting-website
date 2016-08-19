using System;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Admin
{
    [Serializable]
    [DataContract]
    public class ExternalSiteSuper
    {
        [DataMember]
        public int CustId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string IPLogin { get; set; }

        [DataMember]
        public string NormalGroup { get; set; }

        [DataMember]
        public string ExtraPTGroup { get; set; }
    }
}
