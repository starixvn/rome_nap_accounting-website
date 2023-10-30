namespace Fanex.Tarus.Customer.Criteria.AutoTransfer
{
    using Fanex.NAP.Framework.Data;

    public class GetAutoTransferJobStatusCriteria : CriteriaBase
    {
        public GetAutoTransferJobStatusCriteria()
        {
            FromCustId = 0;
            ToCustId = 0;
            IsAllowAutoTransfer = true;
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int FromCustId { get; }

        public int ToCustId { get; }

        public bool IsAllowAutoTransfer { get; }

        public bool IsAutoJobRunning { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_ClearBalanceCust_Job_StatusCheck";

        public override bool IsValid() => true;
    }
}