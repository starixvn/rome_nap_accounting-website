// -----------------------------------------------------------------------
// <copyright file="ISportingSolutionsService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.BetList.Business.SportingSolutions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISportingSolutionsService
    {
        string GetChoice(int betypeId, string resourceId, string betteam, int hdp1, int hdp2, string home, string away);
    }
}
