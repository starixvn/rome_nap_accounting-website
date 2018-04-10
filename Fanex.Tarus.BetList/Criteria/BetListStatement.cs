namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    
    public class BetListStatement
    {
        public BetListStatement()
        {
            Language = "en";
            TransId = 0;
        }

        [SpParam(SqlParamName = "CustID")]
        public int CustId { get; set; }

        public int TransType { get; set; }

        public DateTime FromDate { get; set; }

        [SpParam(SqlParamName = "language")]
        public string Language { get; set; }

        [SpParam(SqlParamName = "transid")]
        public long TransId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public string GetSettingKey()
        {
            return "BetListByDayAll";
        }
    }
}