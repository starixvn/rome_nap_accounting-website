namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Sunplus.Accounting.Core.Common;

    public class BetListByIP_UsernameCriteria
    {
        public DateTime FromDate { get; set; }

        public string IP { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDate { get; set; }

        public int UserId
            => AppSession.AccId;

        public string Permission { get; set; }

        public string GetSettingKey()
            => "BetList_IP_Username";
    }
}