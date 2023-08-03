using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.BetList.Criteria.IPInfo
{
    public class GetIPV6InfoCriteria : CriteriaBase
    {
        public GetIPV6InfoCriteria(string ipList)
            => IPList = ipList;

        public string IPList { get; }

        public override string GetSettingKey()
            => "Age_IPRangesByCountrySelByIPV6List";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(IPList);
    }
}
