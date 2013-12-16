// --------------------------------------------------------------------------
// <copyright file="IEditLicenseeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.EditLicensee
{
    using Fanex.Tarus.Customer.Criteria.EditLicensee;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.EditLicensee;

    public interface IEditLicenseeService
    {
        LicenseeInfo GetLicenseeInfo(GetLicenseeInfoCriteria criteria);

        Result UpdateLicenseeInfo(UpdateLicenseeInfoCriteria criteria);
    }
}
