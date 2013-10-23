// --------------------------------------------------------------------------
// <copyright file="IReplicateLicenseeSettingsRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.ReplicateLicenseeSettings
{
    using System.Collections;
    using Fanex.Tarus.Customer.Criteria.ReplicateLicenseeSettings;

    public interface IReplicateLicenseeSettingsRepository
    {
        void DoReplicateLicenseeSettings(DoReplicateLicenseeSettingsCriteria criteria, ref Hashtable outputParams);
    }
}
