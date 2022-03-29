namespace Fanex.Tarus.Customer.Criteria.InitializingStakeLevel
{
    using Fanex.NAP.Framework.Data;

    public class SiteSelCriteria : CriteriaBase
    {
        public SiteSelCriteria(string siteIds)
        {
            SiteIds = siteIds;
        }

        [SpParam(SqlParamName = "pMutilSite")]
        public string SiteIds { get; set; }

        public override string GetSettingKey()
            => "Acc_InitializingStake_SiteSel";
    }
}
