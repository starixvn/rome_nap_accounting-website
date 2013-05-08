using System;
using System.Runtime.Serialization;

namespace Fanex.Tarus.LiveCasino.Entities.ExRate
{
    [Serializable]
    [DataContract]
    public class ExRateBO
    {
        [DataMember]
        public string CasinoNo
        {
            get { return "01"; }
        }

        [DataMember]
        public int ExchangeId { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string Actualrate { get; set; }

        [DataMember]
        public DateTime Sysdate
        {
            get { return DateTime.Now; }
        }

        [DataMember]
        public int ModifyBy { get; set; }

        [DataMember]
        public string ModifyUserName { get; set; }

        [DataMember]
        public string FromIp { get; set; }
    }
}
