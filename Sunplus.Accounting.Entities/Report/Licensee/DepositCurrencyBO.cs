using System;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.Deposit
{
    [Serializable]
    [DataContract]
    public class DepositCurrencyBO
    {
        [DataMember]
        public string currency { get; set; }

        [DataMember]
        public int currencyId { get; set; }

        [DataMember]
        public long custId { get; set; }
    }
}