using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fanex.Tarus.LiveCasino.Entities.Report
{
    [Serializable]
    [DataContract]
    public class GameStatisticsDTO
    {
        [DataMember]
        public string ReportType { get; set; }
        [DataMember]
        public string NoPlayers { get; set; }
        [DataMember]
        public string NoGames { get; set; }
        [DataMember]
        public int Currency { get; set; }
        [DataMember]
        public string CurrencyName { get; set; }
        [DataMember]
        public string TotalBets { get; set; }
        [DataMember]
        public string TotalBetsRM { get; set; }
        [DataMember]
        public string TotalWins { get; set; }
        [DataMember]
        public string TotalWinsRM { get; set; }
        [DataMember]
        public string GameIncome { get; set; }
        [DataMember]
        public string GameIncomeRM { get; set; }
        [DataMember]
        public string GamePayout { get; set; }
        [DataMember]
        public string GamePayoutRM { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string CompanyRM { get; set; }
        [DataMember]
        public string CompanyTO { get; set; }
        [DataMember]
        public string CompanyTORM { get; set; }
        [DataMember]
        public string CompanyComm { get; set; }
        [DataMember]
        public string CompanyCommRM { get; set; }

        [DataMember]
        public string TotalRefund { get; set; }
        [DataMember]
        public string TotalRefundRM { get; set; }
        [DataMember]
        public List<GameStatisticsDTO> Items { get; set; }
    }
}
