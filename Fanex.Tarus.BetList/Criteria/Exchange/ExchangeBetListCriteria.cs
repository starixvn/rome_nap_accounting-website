namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    
    public class ExchangeBetListCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "CustID")]
        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRows { get; set; }

        public decimal GrandTotal { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_BetList_Exchange";
        }

        public override bool IsValid()
        {
            return FromDate <= ToDate && CustId > 0;
        }
    }
}