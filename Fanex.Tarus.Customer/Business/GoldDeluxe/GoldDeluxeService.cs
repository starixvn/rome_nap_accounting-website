// --------------------------------------------------------------------------
// <copyright file="GoldDeluxeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2015.03.13">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.GoldDeluxe
{
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.DataAccess.GoldDeluxe;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.GoldDeluxe;

    /// <summary>
    /// Gold Deluxe service.
    /// </summary>
    public class GoldDeluxeService : IGoldDeluxeService
    {
        private readonly IGoldDeluxeRepository _goldDeluxeRepository;

        public GoldDeluxeService(IGoldDeluxeRepository goldDeluxeRepository = null)
        {
            _goldDeluxeRepository = goldDeluxeRepository ?? new GoldDeluxeRepository();
        }

        public GoldDeluxeStatus GetStatus(ICriteria criteria)
        {
            return _goldDeluxeRepository.GetStatus(criteria);
        }

        public Result UpdateStatus(ICriteria criteria)
        {
            return _goldDeluxeRepository.UpdateStatus(criteria);
        }
    }
}
