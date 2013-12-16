// --------------------------------------------------------------------------
// <copyright file="IAutoFollowBetAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.DataAccess.AutoFollowBet
{
    using System.Collections.Generic;
    using Fanex.Tarus.Admin.Criteria.AutoFollowBet;
    using Fanex.Tarus.Admin.Entities;

    public interface IAutoFollowBetAccess
    {
        Result AutoFollowBetInsCustomer(string username, double rate);

        List<FollowedCustomer> GetAutoFollowedBetCustomer(GetAutoFollowedBetCustomerCriteria criteria);

        Result DeleteAutoFollowedBetCustomer(DeleteAutoFollowedBetCustomerCriteria criteria);

        Result UpdateAutoFollowedBetCustomer(UpdateAutoFollowedBetCustomerCriteria criteria);
    }
}
