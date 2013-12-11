// --------------------------------------------------------------------------
// <copyright file="IAutoFollowBetService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Business.AutoFollowBet
{
    using System.Collections.Generic;
    using Fanex.Tarus.Admin.Criteria.AutoFollowBet;
    using Fanex.Tarus.Admin.Entities;
    using Fanex.Tarus.Admin.Models.AutoFollowBet;

    public interface IAutoFollowBetService
    {
        List<FollowedCustomer> SelFollowedCustomer(GetAutoFollowedBetCriteria criteria);

        Result DelFollowedCustomer(string username);

        Result InsFollowedCustomer(string username, double rate);

        Result UpdFollowedCustomer(string username, double rate);

        List<FollowTypeInfo> GetFollowType();
    }
}
