namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class BetListByCustInfoCriteria : CriteriaBase
    {        
        public string StatusIdList { get; set; }

        [SpParam(SqlParamName = "CustID")]
        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        [SpParam(SqlParamName = "Bettype")]
        public int BetType { get; set; }

        public int ErrCode { get; set; }

        public int MaxTotalRow { get; set; }

        public string ErrMsg { get; set; }

        public bool ExcludeBettrans { get; set; }

        public override string GetSettingKey()
            => "Acc_BetList_Mem";

        public override bool IsValid()
            => FromDate <= ToDate && CustId > 0;
    }
}