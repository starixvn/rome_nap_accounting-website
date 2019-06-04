namespace Fanex.Tarus.Customer.Criteria.CN88
{
    using NAP.Framework.Data;

    public class SelectMemberUsedCreditCriteria : CriteriaBase
    {
        public SelectMemberUsedCreditCriteria(int searchAccountId)
        {
            SearchAccountId = searchAccountId;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        public override string GetSettingKey() => "CS_Member_TotalBetStake_Sel";

        public override bool IsValid() => SearchAccountId > 0;
    }
}
