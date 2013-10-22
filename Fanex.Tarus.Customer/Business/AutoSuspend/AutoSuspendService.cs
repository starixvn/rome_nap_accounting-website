// --------------------------------------------------------------------------
// <copyright file="AutoSuspendService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.AutoSuspend
{
    using System;
    using Fanex.Tarus.Customer.Criteria.AutoSuspend;
    using Fanex.Tarus.Customer.DataAccess.AutoSuspend;
    using Fanex.Tarus.Customer.Entities.AutoSuspend;
    using Fanex.Tarus.Infrastructure.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AutoSuspendService : IAutoSuspendService
    {
        private readonly IAutoSuspendRepository _autoSuspendRepository;

        public AutoSuspendService()
            : this(null)
        {
        }

        public AutoSuspendService(IAutoSuspendRepository autoSuspendRepository)
        {
            _autoSuspendRepository = autoSuspendRepository ?? new AutoSuspendRepository();
        }

        public CustomerInfo GetCustomerStatus(GetCustomerStatusCriteria criteria)
        {
            try
            {
                var rawData = _autoSuspendRepository.GetCustomerStatus(criteria);
                return rawData;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }
    }
}
