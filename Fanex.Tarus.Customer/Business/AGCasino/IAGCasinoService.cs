// --------------------------------------------------------------------------
// <copyright file="IAGCasinoService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.AGCasino
{
    using Fanex.Tarus.Customer.Criteria.AGCasino;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.AGCasino;

    public interface IAGCasinoService
    {
        AGCasinoCustomerInfo CustomerStatus(GetCustomerStatusCriteria criteria);

        Result UpdateCustomerStatus(UpdateCustomerStatusCriteria criteria);

        AGCasinoCustomerInfo CustomerSetting(GetCustomerSettingCriteria criteria);

        Result UpdateCustomerSetting(UpdateCustomerSettingCriteria criteria);
    }
}
