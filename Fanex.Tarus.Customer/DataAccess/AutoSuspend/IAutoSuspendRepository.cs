// --------------------------------------------------------------------------
// <copyright file="IAutoSuspendRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.AutoSuspend
{
    using Fanex.Tarus.Customer.Criteria.AutoSuspend;
    using Fanex.Tarus.Customer.Entities.AutoSuspend;

    public interface IAutoSuspendRepository
    {
        CustomerInfo GetCustomerStatus(GetCustomerStatusCriteria criteria);
    }
}
