namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;

    public class BetListGetSiteIdByCustIdCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_Result_GetSiteInfo_Mem";
        }

        public override bool IsValid()
        {
            return CustId > 0 || !string.IsNullOrWhiteSpace(Username);
        }
    }
}
