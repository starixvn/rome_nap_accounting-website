namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.DataAccess.Report;
    using System;

    public class BetListByCustInfoCriteria : CriteriaBase
    {        
        private DateTime _fromDate;

        public string StatusIdList { get; set; }

        [SpParam(SqlParamName = "CustID")]
        public int CustId { get; set; }

        public bool IsCN88 { get; set; }

        public DateTime FromDate
        {
            get
            {
                return _fromDate;
            }

            set
            {
                _fromDate = ((value >= ReportUtils.BalanceUp.BalanceUpDay) ? value : ReportUtils.BalanceUp.BalanceUpDay);
            }
        }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        [SpParam(SqlParamName = "Bettype")]
        public int BetType { get; set; }

        public override string GetSettingKey()
            => "Acc_BetList_Mem";

        public override bool IsValid()
            => FromDate <= ToDate && CustId > 0;
    }
}