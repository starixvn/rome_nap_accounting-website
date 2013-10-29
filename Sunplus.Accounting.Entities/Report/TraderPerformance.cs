using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report
{
    [Serializable]
    [DataContract]
    public class TraderPerformance
    {
        [DataMember]
        public int CustId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string MatchNo { get; set; }

        #region Turn Over

        [DataMember]
        public string TurnOver1stHdp { get; set; }

        [DataMember]
        public string TurnOverHdp { get; set; }

        [DataMember]
        public string TurnOver1stOu { get; set; }

        [DataMember]
        public string TurnOverOu { get; set; }

        [DataMember]
        public string TurnOver1st1x2 { get; set; }

        [DataMember]
        public string TurnOver1x2 { get; set; }

        [DataMember]
        public string TurnOver1stOe { get; set; }

        [DataMember]
        public string TurnOverOe { get; set; }

        [DataMember]
        public string TurnOver1stNoOfCorners { get; set; }

        [DataMember]
        public string TurnOverNoOfCorners { get; set; }

        [DataMember]
        public string TurnOver15minOu { get; set; }

        [DataMember]
        public string TurnOver1stTotalBookings { get; set; }

        [DataMember]
        public string TurnOverTotalBookings { get; set; }

        [DataMember]
        public string TurnOverNextCorner { get; set; }

        [DataMember]
        public string TurnOverOverall { get; set; }

        [DataMember]
        public string TurnOverNextBookings { get; set; }

        [DataMember]
        public string TurnOverTotalBookings15minsOu { get; set; }

        [DataMember]
        public string TurnOverCorners15minsOU { get; set; }

        [DataMember]
        public string TurnOver10minsOu { get; set; }

        [DataMember]
        public string TurnOverSingleTeamOu { get; set; }

        #endregion Turn Over

        #region winlost

        [DataMember]
        public string WinLost1stOu { get; set; }

        [DataMember]
        public string WinLostOu { get; set; }

        [DataMember]
        public string WinLost1stOe { get; set; }

        [DataMember]
        public string WinLostOe { get; set; }

        [DataMember]
        public string WinLost1stHdp { get; set; }

        [DataMember]
        public string WinLostHdp { get; set; }

        [DataMember]
        public string WinLost1st1x2 { get; set; }

        [DataMember]
        public string WinLost1x2 { get; set; }

        [DataMember]
        public string WinLost1stNoOfCorners { get; set; }

        [DataMember]
        public string WinLostNoOfCorners { get; set; }

        [DataMember]
        public string WinLost15minOu { get; set; }

        [DataMember]
        public string WinLost1stTotalBookings { get; set; }

        [DataMember]
        public string WinLostTotalBookings { get; set; }

        [DataMember]
        public string WinLostOverall { get; set; }

        [DataMember]
        public string WinLostNextCorner { get; set; }

        [DataMember]
        public string WinLostNextBookings { get; set; }

        [DataMember]
        public string WinLostTotalBookings15minsOu { get; set; }

        [DataMember]
        public string WinLostCorners15minsOU { get; set; }

        [DataMember]
        public string WinLost10minsOu { get; set; }

        [DataMember]
        public string WinLostSingleTeamOu { get; set; }

        #endregion winlost

        #region Comm

        [DataMember]
        public string Comm1stOu { get; set; }

        [DataMember]
        public string CommOu { get; set; }

        [DataMember]
        public string Comm1stOe { get; set; }

        [DataMember]
        public string CommOe { get; set; }

        [DataMember]
        public string Comm1stHdp { get; set; }

        [DataMember]
        public string CommHdp { get; set; }

        [DataMember]
        public string Comm1st1x2 { get; set; }

        [DataMember]
        public string Comm1x2 { get; set; }

        [DataMember]
        public string Comm1stNoOfCorners { get; set; }

        [DataMember]
        public string CommNoOfCorners { get; set; }

        [DataMember]
        public string Comm15minOu { get; set; }

        [DataMember]
        public string Comm1stTotalBookings { get; set; }

        [DataMember]
        public string CommTotalBookings { get; set; }

        [DataMember]
        public string CommNextCorner { get; set; }

        [DataMember]
        public string CommOverall { get; set; }

        [DataMember]
        public string CommNextBookings { get; set; }

        [DataMember]
        public string CommTotalBookings15minsOu { get; set; }

        [DataMember]
        public string CommCorners15minsOU { get; set; }

        [DataMember]
        public string Comm10minsOu { get; set; }

        [DataMember]
        public string CommSingleTeamOu { get; set; }

        #endregion Comm

        #region Margin

        [DataMember]
        public string Margin1stOu { get; set; }

        [DataMember]
        public string MarginOu { get; set; }

        [DataMember]
        public string Margin1stOe { get; set; }

        [DataMember]
        public string MarginOe { get; set; }

        [DataMember]
        public string Margin1stHdp { get; set; }

        [DataMember]
        public string MarginHdp { get; set; }

        [DataMember]
        public string Margin1st1x2 { get; set; }

        [DataMember]
        public string Margin1x2 { get; set; }

        [DataMember]
        public string Margin1stNoOfCorners { get; set; }

        [DataMember]
        public string MarginNoOfCorners { get; set; }

        [DataMember]
        public string Margin15minOu { get; set; }

        [DataMember]
        public string Margin1stTotalBookings { get; set; }

        [DataMember]
        public string MarginTotalBookings { get; set; }

        [DataMember]
        public string MarginNextCorner { get; set; }

        [DataMember]
        public string MarginOverall { get; set; }

        [DataMember]
        public string MarginNextBookings { get; set; }

        [DataMember]
        public string MarginTotalBookings15minsOu { get; set; }

        [DataMember]
        public string MarginCorners15minsOU { get; set; }

        [DataMember]
        public string Margin10minsOu { get; set; }

        [DataMember]
        public string MarginSingleTeamOu { get; set; }

        #endregion Margin

        #region Live Bet Count

        [DataMember]
        public string LiveBetCount1stOu { get; set; }

        [DataMember]
        public string LiveBetCountOu { get; set; }

        [DataMember]
        public string LiveBetCount1stOe { get; set; }

        [DataMember]
        public string LiveBetCountOe { get; set; }

        [DataMember]
        public string LiveBetCount1stHdp { get; set; }

        [DataMember]
        public string LiveBetCountHdp { get; set; }

        [DataMember]
        public string LiveBetCount1st1x2 { get; set; }

        [DataMember]
        public string LiveBetCount1x2 { get; set; }

        [DataMember]
        public string LiveBetCount1stNoOfCorners { get; set; }

        [DataMember]
        public string LiveBetCountNoOfCorners { get; set; }

        [DataMember]
        public string LiveBetCount15minOu { get; set; }

        [DataMember]
        public string LiveBetCount1stTotalBookings { get; set; }

        [DataMember]
        public string LiveBetCountTotalBookings { get; set; }

        [DataMember]
        public string LiveBetCountNextCorner { get; set; }

        [DataMember]
        public string LiveBetCountOverall { get; set; }

        [DataMember]
        public string LiveBetCountNextBookings { get; set; }

        [DataMember]
        public string LiveBetCountTotalBookings15minsOu { get; set; }

        [DataMember]
        public string LiveBetCountCorners15minsOU { get; set; }

        [DataMember]
        public string LiveBetCount10minsOu { get; set; }

        [DataMember]
        public string LiveBetCountSingleTeamOu { get; set; }

        #endregion Live Bet Count
    }

    [Serializable]
    [DataContract]
    public class TraderPerformanceDTO
    {
        [DataMember]
        public IEnumerable<TraderPerformance> rows { get; set; }

        [DataMember]
        public IEnumerable<TraderPerformance> footers { get; set; }

        [DataMember]
        public int total { get; set; }

        [DataMember]
        public int pageindex { get; set; }
    }
}