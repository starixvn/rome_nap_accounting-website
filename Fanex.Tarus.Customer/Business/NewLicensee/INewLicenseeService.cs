// --------------------------------------------------------------------------
// <copyright file="INewLicenseeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Bao" date="2012.11.26">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.NewLicensee
{
    using System.Collections.Generic;
    using Fanex.Tarus.Customer.Criteria.NewLicensee;
    using Fanex.Tarus.Customer.Entities.NewLicensee;

    public interface INewLicenseeService
    {
        List<Currency> GetCurrenciesBySystem(GetCurrenciesBySystemCriteria criteria);
    }
}
