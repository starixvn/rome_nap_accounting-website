// --------------------------------------------------------------------------
// <copyright file="IAdminSecurityCodeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.DataAccess.SecurityCode
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.Admin.Criteria.SecurityCode;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IAdminSecurityCodeDataAccess
    {
        List<dynamic> GetUserList(GetUserListCriteria criteria);

        void ResetSecurityCode(ResetSecurityCodeCriteria criteria, ref Hashtable outputParams);
    }
}
