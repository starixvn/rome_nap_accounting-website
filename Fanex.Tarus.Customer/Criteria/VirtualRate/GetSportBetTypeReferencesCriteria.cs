namespace Fanex.Tarus.Customer.Criteria.VirtualRate
{
    using Fanex.NAP.Framework.Data;

    public class GetSportBetTypeReferencesCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Customer_VirtualRate_GetSportBetTypeReferences";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
