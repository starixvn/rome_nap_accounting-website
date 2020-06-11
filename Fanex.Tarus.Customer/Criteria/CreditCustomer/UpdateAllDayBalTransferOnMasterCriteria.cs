using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.CreditCustomer
{
    public sealed class UpdateAllDayBalTransferOnMasterCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public bool IsAutoEnableADBMaster { get; set; }

        public string IP { get; set; }

        public int UserId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustomerUpd_IsAutoEnableADBMaster";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}
