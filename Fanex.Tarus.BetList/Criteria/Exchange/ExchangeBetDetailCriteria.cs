namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class ExchangeBetDetailCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "CustID")]
        public int CustId { get; set; }

        public int BetId { get; set; }

        [SpParam(Ignored = true)]
        public string Type { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_BetList_Exchange_Detail";
        }

        public override bool IsValid()
        {
            return BetId > 0 && CustId > 0;
        }
    }
}