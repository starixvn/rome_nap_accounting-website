namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    
    public class BetListSystemParlayByRefnoCriteria : CriteriaBase, ICriteria
    {
        [SpParam(SqlParamName = "custid")]
        public int CustomerId { get; set; }

        [SpParam(SqlParamName = "transid")]
        public string TransactionId { get; set; }

        public DateTime MoveBetDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "BetList_CustInfo_SystemParlay";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}