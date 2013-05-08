// -----------------------------------------------------------------------
// <copyright file="ISpecialDeleteService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Admin.Business.MailingList
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Admin.Entities;
    using Fanex.Tarus.Admin.Entities.MailingList;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISpecialDeleteService
    {
        SpecialDeleteMailing GetMailing();

        Result UpdateInsertMailing(MailLicensee mailLicensee, bool update);
    }
}
