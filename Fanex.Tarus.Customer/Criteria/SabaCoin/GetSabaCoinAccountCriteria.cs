using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.SabaCoin
{
    public class GetSabaCoinAccountCriteria : CriteriaBase
    {
        public GetSabaCoinAccountCriteria(long sabaId) => SabaId = sabaId;

        public long SabaId { get; }

        public override string GetSettingKey() => "Acc_Dep_CustomerSelBySabaID";

        public override bool IsValid() => SabaId > 0;
    }
}