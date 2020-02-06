namespace Fanex.Tarus.Customer.Criteria.CN88.MoveSuper
{
    using Fanex.NAP.Framework.Data;

    public class SearchSuperCriteria: CriteriaBase
    {
        public string SuperUsername { get; set; }

        public override string GetSettingKey()
        {
            return "CS_Customer_GetHeadlinePossess";
        }
    }
}
