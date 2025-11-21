using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Requests.ParlayMaxPayout;

namespace Fanex.Tarus.Customer.Criteria.ParlayMaxPayout
{
    public class MaxPayoutSelectCriteria : CriteriaBase
    {
        public MaxPayoutSelectCriteria() { }

        public MaxPayoutSelectCriteria(ParlayMaxPayoutSelectRequest request)
        {
            CustId = request.CustId;
        }

        public int CustId { get; set; }

        public int SettingStatus { get; set; }

        public override bool IsValid()
        {
            return CustId > 0;
        }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_MaxPayoutSelect";
        }
    }
}
