namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;
    
    public class CustIDByUsernameCriteria : CriteriaBase, ICriteria
    {
        [SpParam(SqlParamName = "custname")]
        public string CustomerName { get; set; }

        public override string GetSettingKey()
        {
            return "GetCustIdByUsername";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}