namespace Fanex.Tarus.Customer.Criteria.ViewPeriodsCN88
{
    using Fanex.NAP.Framework.Data;

    public class UpdateHeadlinePeriodCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "CustId")]
        public int HeadlineId { get; internal set; }

        [SpParam(SqlParamName = "IsViewAllPeriod")]
        public bool IsViewAllPeriods { get; internal set; }

        public int UserId { get; internal set; }

        [SpParam(SqlParamName = "LoginIP")]
        public string IP { get; internal set; }

        public override string GetSettingKey()
        {
            return "CS_ReportPermission_Upd";
        }

        public override bool IsValid()
        {
            return HeadlineId > 0;
        }
    }
}
