namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    
    public class BetListByCustInfoU3Criteria : CriteriaBase
    {
        [SpParam(SqlParamName = "CustID")]
        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        [SpParam(SqlParamName = "Bettype")]
        public int BetType { get; set; }

        public override string GetSettingKey()
        {
            return "U3_BetsSelBetList";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}