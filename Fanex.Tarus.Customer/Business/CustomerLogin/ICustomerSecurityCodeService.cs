// --------------------------------------------------------------------------
// <copyright file="ICustomerSecurityCodeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.CustomerLogin
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.Customer.Criteria.CustomerLogin;
    using Fanex.Tarus.Customer.Entities;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface ICustomerSecurityCodeService
    {
        Result ResetSecurityCode(ResetSecurityCodeCriteria criteria);
    }
}
