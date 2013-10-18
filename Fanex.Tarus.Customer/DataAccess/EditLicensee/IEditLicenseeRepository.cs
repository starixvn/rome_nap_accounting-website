// --------------------------------------------------------------------------
// <copyright file="IEditLicenseeRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.EditLicensee
{
    using System.Collections;
    using Fanex.Tarus.Customer.Criteria.EditLicensee;
    using Fanex.Tarus.Customer.Entities.EditLicensee;

    public interface IEditLicenseeRepository
    {
        LicenseeInfo GetLicenseeInfo(GetLicenseeInfoCriteria criteria, ref Hashtable outputParams);
    }
}
