namespace Fanex.Tarus.Customer.Criteria.ViewPeriodsCN88
{
    using Fanex.NAP.Framework.Data;

    public class SearchHeadlineCriteria: CriteriaBase
    {
        public string UserName { get; set; }

        public override string GetSettingKey()
        {
            return "CS_ReportPermission_SEL";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
