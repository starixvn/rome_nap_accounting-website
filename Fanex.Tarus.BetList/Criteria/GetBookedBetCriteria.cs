namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class GetBookedBetCriteria : CriteriaBase
    {        
        public string StatusIdList { get; set; }

        [SpParam(SqlParamName = "CustID")]
        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public override string GetSettingKey()
            => "Acc_BetList_SelBookedBet";

        public override bool IsValid()
            => FromDate <= ToDate && CustId > 0;
    }
}