using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.CN88
{
    public class CN88CustomerSettingCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int SubAccId => 0;

        public int CustId { get; set; }

        public byte OldStatusID { get; set; }

        public byte NewStatusID { get; set; }

        public string IP { get; set; }

        public string Language => "E";

        public override bool IsValid()
        {
            return CustId > 0
                && UserId > 0
                && OldStatusID >= 1
                && OldStatusID <= 4
                && NewStatusID >= 1
                && NewStatusID <= 4;
        }

        public override string GetSettingKey()
        {
            return "CS_CustomerStatusUpd";
        }
    }
}
