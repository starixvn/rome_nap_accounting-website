namespace Fanex.Tarus.BetList.Criteria.Results
{
    using Fanex.NAP.Framework.Data;
    using System;

    public class GetVS2RacingResultCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "EventId")]
        public int EventId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime MoveBetsDay { get; set; }
        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "Age_BetList_SelRaceResult";
        }
    }
}
