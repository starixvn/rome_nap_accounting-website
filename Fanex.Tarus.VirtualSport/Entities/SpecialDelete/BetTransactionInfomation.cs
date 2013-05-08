

using System;

namespace Fanex.Tarus.VirtualSport.Entities.SpecialDelete
{
    [Serializable]
    public class BetTransactionInfomation
    {
        #region IFields

        public string Type { get; set; }
        public int Order { get; set; }
        public string Account { get; set; }
        public string Refno { get; set; }
        public string Bettype { get; set; }
        public string Betteam { get; set; }
        public string Odds { get; set; }
        public string OddType { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string Hdp { get; set; }
        public string TransTime { get; set; }
        public string Score { get; set; }
        public string Creator { get; set; }
        public string Status { get; set; }
        public string IsDelete { get; set; }
        public string IsMP { get; set; }
        public int SaFilter { get; set; }

        #endregion IFields
    }
}
