namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.DataAccess.Report;

    public sealed class GetLicenseeBonusDetailCriteria : CriteriaBase
    {
        public long TransId { get; set; }

        public long RefCode { get; set; }

        public DateTime MoveBetsDay => ReportUtils.BalanceUp.MoveBetsDay;

        public DateTime BalanceUpDay => ReportUtils.BalanceUp.BalanceUpDay;

        public override string GetSettingKey()
        {
            return "Acc_BetList_LicenseeBonus_Detail";
        }

        public override bool IsValid()
        {
            return TransId > 0 && RefCode > 0;
        }
    }
}
