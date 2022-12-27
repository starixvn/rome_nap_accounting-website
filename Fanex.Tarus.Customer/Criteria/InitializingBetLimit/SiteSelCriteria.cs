namespace Fanex.Tarus.Customer.Criteria.InitializingBetLimit
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
            => "Acc_InitializingGlobal_SiteSel";
    }
}
