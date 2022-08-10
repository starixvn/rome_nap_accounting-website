namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class SiteDBMixParlayDetailCriteria
    {
        public bool IsCN88 { get; set; }

        [SpParam(SqlParamName = "transid")]
        public long TransId { get; set; }

        [SpParam(SqlParamName = "refno")]
        public long RefNo { get; set; }

        [SpParam(SqlParamName = "custid")]
        public int CustId { get; set; }

        public string GetSettingKey()
            => "SiteDB_Acc_BetList_SelParlayDetail";
    }
}