namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.DataAccess.Report;
    using System;

    public class BetListByCustInfoCriteria : CriteriaBase
    {
        private DateTime _fromDate;

        [SpParam(SqlParamName = "CustID")]
        public int CustId { get; set; }

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

        [SpParam(SqlParamName = "Bettype")]
        public int BetType { get; set; }

        public override string GetSettingKey()
        {
            return "BetList_CustInfo";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}