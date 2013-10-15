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
    using Fanex.Tarus.Customer.DataAccess.EditLicensee;

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
    }
}
