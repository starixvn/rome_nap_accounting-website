namespace Fanex.Tarus.Customer.Criteria.CN88
{
    using NAP.Framework.Data;

    public class SelectCreditRangeCriteria : CriteriaBase
    {
        public SelectCreditRangeCriteria(int searchAccountId)
        {
            SearchAccountId = searchAccountId;
        }

        [SpParam(SqlParamName = "CustId")]
        public int SearchAccountId { get; }

        public override string GetSettingKey() => "CS_Customer_Credit_Sel";

        public override bool IsValid() => SearchAccountId > 0;
    }
}
