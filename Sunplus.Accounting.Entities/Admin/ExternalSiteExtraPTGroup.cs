using System;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Admin
{
    [Serializable]
    [DataContract]
    public class ExternalSiteExtraPTGroup
    {
        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public DateTime EffectiveDate { get; set; }
    }
}
