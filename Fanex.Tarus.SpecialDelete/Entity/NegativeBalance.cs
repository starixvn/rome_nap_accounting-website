// -----------------------------------------------------------------------
// <copyright file="NegativeBalance.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.SpecialDelete.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NegativeBalance
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public bool IsNegative { get; set; }
    }
}
