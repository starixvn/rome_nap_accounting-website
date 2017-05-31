using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.CreditCustomer
{
    public sealed class UpdateAllDayBalTransferCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public bool Status { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return string.Empty;
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}
