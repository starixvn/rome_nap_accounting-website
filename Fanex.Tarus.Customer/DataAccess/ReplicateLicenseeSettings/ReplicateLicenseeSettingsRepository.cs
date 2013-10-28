// --------------------------------------------------------------------------
// <copyright file="ReplicateLicenseeSettingsRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.ReplicateLicenseeSettings
{
    using System.Collections;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.ReplicateLicenseeSettings;
    using Fanex.Tarus.Customer.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReplicateLicenseeSettingsRepository : IReplicateLicenseeSettingsRepository
    {
        /// <summary>
        /// Does the replicate licensee settings.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="outputParams">The output parameters.</param>
        /// <returns>Replicate Result.</returns>
        public Result DoReplicateLicenseeSettings(DoReplicateLicenseeSettingsCriteria criteria, ref Hashtable outputParams)
        {
            Hashtable param = new Hashtable();
            param.Add("licenseename", criteria.LicenseeName);
            using (ObjectDb objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(param);
                objDb.ExecuteNonQuery();
                outputParams = objDb.GetOuputParams();
            }

            return new Result
            {
                ErrorCode = 0,
                ErrorMessage = "Something is wrong!"
            };
        }
    }
}
