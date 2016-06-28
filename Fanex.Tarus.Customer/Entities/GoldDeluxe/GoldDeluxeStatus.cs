// --------------------------------------------------------------------------
// <copyright file="GoldDeluxeStatus.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2015.03.13">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.GoldDeluxe
{
    using Fanex.Tarus.Customer.Resources;

    /// <summary>
    /// Gold Deluxe game entity.
    /// </summary>
    public class GoldDeluxeStatus
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string Name
        {
            get
            {
                return Customer_Labels.EnableGoldDeluxe;
            }
        }

        public bool Enable { get; set; }

        public string DownlineName
        {
            get
            {
                return Customer_Labels.EnableGoldDeluxeMembers;
            }
        }

        public bool DownlineEnable { get; set; }
    }
}
