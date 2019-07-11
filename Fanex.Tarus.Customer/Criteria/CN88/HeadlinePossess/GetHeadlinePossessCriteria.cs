namespace Fanex.Tarus.Customer.Criteria.CN88.HeadlinePossess
{
    using Fanex.NAP.Framework.Data;

    public class GetHeadlinePossessCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "CS_GroupExtraPT_Sel";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}
