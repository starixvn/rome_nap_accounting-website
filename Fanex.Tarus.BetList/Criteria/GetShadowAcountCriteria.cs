using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.BetList.Criteria
{
    public class GetShadowAcountCriteria : CriteriaBase
    {
        public GetShadowAcountCriteria(int custId) => CustId = custId;

        public double CustId { get; }

        public override string GetSettingKey() => "Acc_Dep_CustomerSel_Shadow";

        public override bool IsValid() => CustId > 0;
    }
}