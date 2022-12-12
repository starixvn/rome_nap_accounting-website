namespace Fanex.Tarus.Customer.Criteria.InitializingCustomerSetting
{
    using Fanex.NAP.Framework.Data;

    public class DcsProductMatrixCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "Acc_Initial_DCS_ProductProfile_Sel";

        public override bool IsValid()
            => true;
    }
}
