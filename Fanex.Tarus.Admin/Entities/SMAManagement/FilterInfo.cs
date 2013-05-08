// -----------------------------------------------------------------------
// <copyright file="FilterContext.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Admin.Entities.SMAManagement
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FilterInfo
    {
        public List<FilterInfoDetail> Currencies { get; set; }
        public List<FilterInfoDetail> Sites { get; set; }
    }

    [Serializable]
    public class FilterInfoDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}