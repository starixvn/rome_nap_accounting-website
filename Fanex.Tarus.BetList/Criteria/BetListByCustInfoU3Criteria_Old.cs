namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    
    public class BetListByCustInfoU3Criteria_Old : CriteriaBase
    {
        [SpParam(SqlParamName = "CustID")]
        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [SpParam(SqlParamName = "Bettype")]
        public int BetType { get; set; }

        public override string GetSettingKey()
        {
            return "U3_BetsSelBetList_Old";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}