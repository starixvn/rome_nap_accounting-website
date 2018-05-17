namespace Fanex.Tarus.BetList.Criteria
{
    using System;
    using Fanex.NAP.Framework.Data;
    
    public class BetListByIP_OthersCriteria
    {
        [SpParam(SqlParamName = "CustID")]
        public int CustId { get; set; }

        public string IP { get; set; }

        public DateTime FromDate { get; set; }

        public string GetSettingKey()
        {
            return "BetList_IP_OtherSports";
        }
    }
}