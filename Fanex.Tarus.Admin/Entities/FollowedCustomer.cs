// -----------------------------------------------------------------------
// <copyright file="FollowedCustomer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Admin.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [Serializable]
    public class FollowedCustomer
    {
        public FollowedCustomer() { }
        public FollowedCustomer(string username, double rate, bool enableBA)
        {
            Username = username;
            Rate = rate;
            EnableBA = enableBA;
        }

        public string CustId { get; set; }

        public string Username { get; set; }

        public double Rate { get; set; }

        public bool EnableBA { get; set; }

        public bool IsFollowed { get; set; }

        public string Licensee { get; set; }

    }

    public class FollowedCustomerDTO
    {
        public List<FollowedCustomer> FollowedCus { get; set; }
        public int TotalCustomer { get; set; } 
    }
}
