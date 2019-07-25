namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    
    public class BetListStatement
    {
        [SpParam(SqlParamName = "CustID")]
        public int CustId { get; set; }

        public int TransType { get; set; }

        public DateTime FromDate { get; set; } 

        public DateTime MoveBetsDay { get; set; }

        public string GetSettingKey()
        {
            return "BetListByDayAll";
        }
    }
}