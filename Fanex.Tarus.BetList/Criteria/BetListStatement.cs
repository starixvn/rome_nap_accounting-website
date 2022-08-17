namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class BetListStatement : CriteriaBase
    {
        [SpParam(Ignored = true)]
        public bool IsSiteDB { get; set; }

        [SpParam(SqlParamName = "CustID")]
        public int CustId { get; set; }

        public int TransType { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public override string GetSettingKey()
            => IsSiteDB ? "SiteDB_Acc_BetList_Statement" : "BetListByDayAll";
    }
}