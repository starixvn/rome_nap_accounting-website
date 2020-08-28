using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.VirtualRate
{
    public class GetVRAutoAdjustUpdateMessageCriteria : CriteriaBase
    {
        public int CustID { get; set; }

        public string Message { get; set; }

        public override string GetSettingKey()
            => "Acc_CustomerVRAdjust_Confirm";

        public override bool IsValid()
            => CustID > 0;
    }
}
