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
    using System.Data;
    using Fanex.Tarus.Admin.Entities;
    using Fanex.Tarus.Admin.Criteria.AutoFollowBet;

    public interface IAutoFollowBetAccess
    {
        DataTable AutoFollowBetSelCustomer(GetAutoFollowedBetCriteria criteria);

        Result AutoFollowBetDelCustomer(string username);

        Result AutoFollowBetInsCustomer(string username, double rate);

        Result AutoFollowBetUpdCustomer(string username, double rate); 
    }
}
