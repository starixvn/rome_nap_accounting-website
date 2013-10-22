// --------------------------------------------------------------------------
// <copyright file="IAutoSuspendService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.AutoSuspend
{
    using Fanex.Tarus.Customer.Criteria.AutoSuspend;
    using Fanex.Tarus.Customer.Entities.AutoSuspend;

    public interface IAutoSuspendService
    {
        CustomerInfo GetCustomerStatus(GetCustomerStatusCriteria criteria);
    }
}
