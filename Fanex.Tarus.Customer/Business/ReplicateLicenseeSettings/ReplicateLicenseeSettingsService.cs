// --------------------------------------------------------------------------
// <copyright file="ReplicateLicenseeSettingsService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.ReplicateLicenseeSettings
{
    using System;
    using Fanex.Tarus.Customer.Criteria.ReplicateLicenseeSettings;
    using Fanex.Tarus.Customer.DataAccess.ReplicateLicenseeSettings;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Infrastructure.Extensions;
    using System.Collections;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReplicateLicenseeSettingsService : IReplicateLicenseeSettingsService
    {
        private readonly IReplicateLicenseeSettingsRepository _replicateLicenseeSettingsRepository;

        public ReplicateLicenseeSettingsService()
            : this(null)
        {
        }

        public ReplicateLicenseeSettingsService(IReplicateLicenseeSettingsRepository replicateLicenseeSettingsRepository)
        {
            _replicateLicenseeSettingsRepository = replicateLicenseeSettingsRepository ?? new ReplicateLicenseeSettingsRepository();
        }

        public Result DoReplicateLicenseeSettings(DoReplicateLicenseeSettingsCriteria criteria)
        {
            try
            {
                var replicateResult = new Result();
                if (string.IsNullOrWhiteSpace(criteria.LicenseeName))
                {
                    replicateResult.ErrorCode = 1;
                    replicateResult.ErrorMessage = "Please input licensee name!";
                    return replicateResult;
                }

                Hashtable outputParams = new Hashtable();
                _replicateLicenseeSettingsRepository.DoReplicateLicenseeSettings(criteria, ref outputParams);
                replicateResult = new Result
                {
                    ErrorCode = 1,
                    ErrorMessage = "Something is wrong!"
                };

                if (replicateResult.ErrorCode == 0)
                {
                    replicateResult.ErrorMessage = "Replicate licensee settings successfully!";
                }

                return replicateResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }
    }
}
