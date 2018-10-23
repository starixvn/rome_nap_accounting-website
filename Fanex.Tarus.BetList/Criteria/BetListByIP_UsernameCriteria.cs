namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Sunplus.Accounting.Core.Common;
    
    public class BetListByIP_UsernameCriteria
    {
        public DateTime FromDate { get; set; }

        public string IP { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public int UserId
        {
            get
            {
                return AppSession.AccId;
            }
        }

        public byte SiteType { get; set; }

        public string GetSettingKey()
        {
            return "BetList_IP_Username";
        }
    }
}