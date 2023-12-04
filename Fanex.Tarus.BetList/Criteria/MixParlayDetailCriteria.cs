namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class MixParlayDetailCriteria
    {

        [SpParam(SqlParamName = "transid")]
        public long TransId { get; set; }

        [SpParam(SqlParamName = "refno")]
        public long RefNo { get; set; }

        [SpParam(SqlParamName = "custid")]
        public int CustId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }


        public bool ExcludeBettrans { get; set; }

        public string GetSettingKey() => "BetList_MixParlayBetsSel";
    }
}