using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.CreditCustomer
{
    public sealed class TurnOffAutoTransferCriteria : CriteriaBase
    {
        public int UplineId { get; set; }

        public int Creator { get; set; }

        public bool IsAllDownline { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Age_ClearBalanceCust_SettingUpdate_Downline";
        }

        public override bool IsValid()
        {
            return UplineId > 0;
        }
    }
}
