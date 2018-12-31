using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.CN88
{
    public class CN88CustomerSettingCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public int SubAccId => 0;

        public int CustId { get; set; }

        public byte UpdStatusId { get; set; }

        public byte SystemId => 1;

        public string IP { get; set; }

        public string Language => "E";

        public override bool IsValid()
        {
            return UserId > 0 && UpdStatusId >= 1 && UpdStatusId <= 3;
        }

        public override string GetSettingKey()
        {
            return "Age_CS_Customer_Status_Upd";
        }
    }
}
