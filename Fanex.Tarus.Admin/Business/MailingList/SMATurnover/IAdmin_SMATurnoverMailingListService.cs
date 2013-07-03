// --------------------------------------------------------------------------
// <copyright file="IAdmin_SMATurnoverMailingListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Business.MailingList.SMATurnover
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.Admin.Entities;
    using Fanex.Tarus.Admin.Entities.MailingList;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IAdmin_SMATurnoverMailingListService
    {
        SpecialDeleteMailing GetMailingList();

        Result UpdateInsertMailing(MailLicensee mailLicensee, bool update);
    }
}
