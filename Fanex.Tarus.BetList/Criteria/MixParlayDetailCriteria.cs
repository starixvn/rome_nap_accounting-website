namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class MixParlayDetailCriteria
    {
        [SpParam(SqlParamName = "transid")]
        public long TransId { get; set; }

        [SpParam(SqlParamName = "refno")]
        public long RefNo { get; set; }

        public string GetSettingKey()
        {
            return "BetList_MixParlayBetsSel";
        }
    }
}