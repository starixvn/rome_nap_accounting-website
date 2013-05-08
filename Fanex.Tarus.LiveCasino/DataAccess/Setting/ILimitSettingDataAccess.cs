// -----------------------------------------------------------------------
// <copyright file="ILimitSettingDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.LiveCasino.DataAccess.Setting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.LiveCasino.Entities;
using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ILimitSettingDataAccess
    {
        Result UpdateWinLimit(int custId, decimal maxWinLiveCasino, bool isAutoResetLiveCasino);

        DataTable SelCurrentWinLimit(int custId);
    }
}
