// -----------------------------------------------------------------------
// <copyright file="CustomerSettings.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
namespace Fanex.Tarus.VirtualSport.Entities.Customer.DepositCustomer
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerSettingDetail
    {        
        public int SportId { get; set; }
        public string SportName { get; set; }
        public double MinBet { get; set; }
        public double MinBetMax { get; set; }        
        public double MaxBet { get; set; }
        public double MaxBetMin { get; set; }
        public double MaxPerMatch { get; set; }
        public double MaxPerMatchMax { get; set; }        
        public double SuperPositionTaking { get; set; }
        public double MasterPositionTaking { get; set; }
        public double MasterPositionTakingMax { get; set; }
        public double AgentPositionTaking { get; set; }
        public double AgentPositionTakingMax { get; set; }
        public bool IsEnableVirtualSports { get; set; }
    }

    public class CustomerSettings
    {
        public int CustId { get; set; }
        public int RoleId { get; set; }
        public List<CustomerSettingDetail> Settings { get; set; }
    }
}
