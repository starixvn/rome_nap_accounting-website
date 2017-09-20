using Fanex.NAP.Framework.Data;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    public class NonQueryCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "CustId")]
        public int SuperId { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid()
        {
            return SuperId > 0 &&
                UserId > 0 &&
                !string.IsNullOrEmpty(IP);
        }
    }
}
