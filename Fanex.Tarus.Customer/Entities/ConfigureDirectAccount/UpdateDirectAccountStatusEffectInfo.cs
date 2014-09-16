// --------------------------------------------------------------------------
// <copyright file="UpdateDirectAccountStatusEffectInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.ConfigureDirectAccount
{
    using System.Collections.Generic;

    /// <summary>
    /// Update Direct Account Status Effect Info.
    /// </summary>
    public class UpdateDirectAccountStatusEffectInfo
    {
        public string Username { get; set; }

        public List<CustomerInfo> CustomerList { get; set; }
    }
}
