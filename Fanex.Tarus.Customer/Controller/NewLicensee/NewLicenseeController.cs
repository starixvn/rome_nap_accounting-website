// --------------------------------------------------------------------------
// <copyright file="NewLicenseeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Controller.NewLicensee
{
    using System.Collections.Generic;
    using Fanex.Tarus.Customer.Business.NewLicensee;
    using Fanex.Tarus.Customer.Criteria.NewLicensee;
    using Fanex.Tarus.Customer.Entities.NewLicensee;
    using Fanex.Tarus.Infrastructure.Common;
    using Fanex.Tarus.Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NewLicenseeController : BaseController
    {
        private readonly INewLicenseeService _newLicenseeService;

        public NewLicenseeController()
            : this(null)
        {
        }

        public NewLicenseeController(INewLicenseeService newLicenseeService)
        {
            _newLicenseeService = newLicenseeService ?? new NewLicenseeService();
        }

        public List<Currency> GetCurrenciesBySystem()
        {
            var currencies = _newLicenseeService.GetCurrenciesBySystem(new GetCurrenciesBySystemCriteria());
            return currencies;
        }
    }
}
