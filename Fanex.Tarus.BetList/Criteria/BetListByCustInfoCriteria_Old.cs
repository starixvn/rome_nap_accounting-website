namespace Fanex.Tarus.BetList.Criteria
{
    using Fanex.NAP.Framework.Data;
    using Sunplus.Accounting.DataAccess.Report;
    using System;

    public class BetListByCustInfoCriteria_Old : CriteriaBase
    {
        private DateTime _toDate;

        [SpParam(SqlParamName = "CustID")]
        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate
        {
            get
            {
                return _toDate;
            }
            set
            {
                DateTime yesterdayOfBalanceUpDay = ReportUtils.BalanceUp.BalanceUpDay.AddDays(-1);
                _toDate = ((value < yesterdayOfBalanceUpDay) ? value : yesterdayOfBalanceUpDay);
            }
        }

        [SpParam(SqlParamName = "Bettype")]
        public int BetType { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_BetsSelBetList_Old";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}