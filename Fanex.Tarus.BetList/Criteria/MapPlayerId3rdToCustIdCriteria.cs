namespace Fanex.Tarus.BetList.Criteria
{
    using NAP.Framework.Data;

    public class MapPlayerId3rdToCustIdCriteria : CriteriaBase
    {
        public int ProductId { get; set; }

        [SpParam(Ignored = true)]
        public int Vendor { get; set; }

        [SpParam(SqlParamName = "PlayerId")]
        public string PlayerId3rd { get; set; }

        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelByPlayerId";
        }
    }
}
