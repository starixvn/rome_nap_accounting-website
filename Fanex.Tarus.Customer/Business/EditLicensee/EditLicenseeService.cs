// --------------------------------------------------------------------------
// <copyright file="EditLicenseeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.EditLicensee
{
    using System;
    using Fanex.Tarus.Customer.Criteria.EditLicensee;
    using Fanex.Tarus.Customer.DataAccess.EditLicensee;
    using Fanex.Tarus.Customer.Entities.EditLicensee;
    using Fanex.Tarus.Infrastructure.Extensions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EditLicenseeService : IEditLicenseeService
    {
        private readonly IEditLicenseeRepository _editLicenseeRepository;

        public EditLicenseeService()
            : this(null)
        {
        }

        public EditLicenseeService(IEditLicenseeRepository editLicenseeRepository)
        {
            _editLicenseeRepository = editLicenseeRepository ?? new EditLicenseeRepository();
        }

        public LicenseeInfo GetLicenseeInfo(GetLicenseeInfoCriteria criteria)
        {
            try
            {
                var rawData = _editLicenseeRepository.GetLicenseeInfo(criteria);
                LicenseeInfo licenseeInfo = new LicenseeInfo();

                licenseeInfo.Site = "12bet";
                licenseeInfo.Currency = "UUS";
                licenseeInfo.EnableNumberGame = true;

                return licenseeInfo;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }
    }
}
