using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Admin
{
    [Serializable]
    [DataContract]
    public class ExternalSiteGridBO
    {
        [DataMember]
        public IEnumerable<ExternalSiteSuper> Rows { get; set; }

        [DataMember]
        public int Total { get; set; }
    }
}