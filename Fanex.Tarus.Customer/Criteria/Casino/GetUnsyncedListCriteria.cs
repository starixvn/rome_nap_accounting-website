namespace Fanex.Tarus.Customer.Criteria.Casino
{
    using Fanex.NAP.Framework.Data;
    using System;

    public class GetUnsyncedListCriteria : CriteriaBase
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(FromDate)
                && !string.IsNullOrWhiteSpace(ToDate)
                && DateTime.TryParse(FromDate, out DateTime tempFromDate)
                && DateTime.TryParse(ToDate, out DateTime tempToDate);
        }

        public override string GetSettingKey()
        {
            return "Acc_CustomerSelUnSyncRNG";
        }
    }
}
