// -----------------------------------------------------------------------
// <copyright file="IWinLimitSetting.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.LiveCasino.Business.Setting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.LiveCasino.Entities.Setting;
    using Fanex.Tarus.LiveCasino.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ILimitSettingManager
    {
        List<WinLimit> SelWinLimitSetting(decimal selectedValue, decimal uplineSelectedValue);

        Result UpdateWinLimit(int custId, decimal maxWinLiveCasino, bool isAutoResetLiveCasino);

        CurrentWinLimit SelCurrentWinLimit(int custId);
    }
}
