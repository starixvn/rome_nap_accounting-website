using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.VirtualRate
{
    public class GetVRAutoAdjustSporttypeCriteria : CriteriaBase
    {
        public int CustID { get; set; }

        public override string GetSettingKey()
            => "Acc_CustomerSelVRAutoAdjust_Sporttype";

        public override bool IsValid()
            => CustID > 0;
    }
}
