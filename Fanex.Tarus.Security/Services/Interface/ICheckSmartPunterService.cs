// -----------------------------------------------------------------------
// <copyright file="ICheckSmartPunterService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Security.Services.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Security.Criteria;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICheckSmartPunterService
    {
        bool CheckSmartPunter(CheckSmartPunterCriteria criteria);
    }
}
