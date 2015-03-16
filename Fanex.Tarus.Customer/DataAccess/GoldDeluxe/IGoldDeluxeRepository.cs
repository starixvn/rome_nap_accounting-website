// --------------------------------------------------------------------------
// <copyright file="IGoldDeluxeRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2015.03.13">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.GoldDeluxe
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.GoldDeluxe;

    public interface IGoldDeluxeRepository
    {
        GoldDeluxeStatus GetStatus(ICriteria criteria);

        Result UpdateStatus(ICriteria criteria);
    }
}
