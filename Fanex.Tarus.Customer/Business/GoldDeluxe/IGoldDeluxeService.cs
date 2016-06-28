// --------------------------------------------------------------------------
// <copyright file="IGoldDeluxeService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2015.03.13">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.GoldDeluxe
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.GoldDeluxe;

    public interface IGoldDeluxeService
    {
        GoldDeluxeStatus GetStatus(ICriteria criteria);

        Result UpdateStatus(ICriteria criteria);
    }
}
