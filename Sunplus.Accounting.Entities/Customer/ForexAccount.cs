using System;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Customer
{
    [Serializable]
    [DataContract]
    public class ForexAccount
    {
        [DataMember]
        public string username { get; set; }
    }
}