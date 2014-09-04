// --------------------------------------------------------------------------
// <copyright file="AGCasinoCustomerInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.AGCasino
{
    using System.Collections.Generic;

    /// <summary>
    /// AG Casino Customer Information.
    /// </summary>
    public class AGCasinoCustomerInfo
    {
        public SelectResult SelectResult { get; set; }

        public int CustomerId { get; set; }

        public string Username { get; set; }

        public int RoleId { get; set; }

        public int UplineId { get; set; }

        public int ExchangeId { get; set; }

        public string SiteName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Fax { get; set; }
        
        public bool EnableAGCasino { get; set; }

        public bool UplineEnableAGCasino { get; set; }

        public BaseModel DefaultAGCasinoGroup { get; set; }

        public string AGCasinoCheckedGroup { get; set; }

        public List<ProductInfo> ProductInfo { get; set; }
    }
}
