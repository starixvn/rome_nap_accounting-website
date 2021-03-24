namespace Fanex.Tarus.Customer.ManageLicenseeCode.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class LicenseeTransferCodeSelCriteria : CriteriaBase
    {
        public DateTime MoveBetsDay { get; set; }

        public string LicenseeIds { get; set; }

        public string StatusIds { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string CurrentBillingMonth { get; set; }

        public int TotalRows { get; set; }

        public override string GetSettingKey()
            => "Acc_LicenseeTransferCode_Sel";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(LicenseeIds) && !string.IsNullOrWhiteSpace(StatusIds);
    }
}
