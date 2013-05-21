// -----------------------------------------------------------------------
// <copyright file="IRacingDefaultSettingService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Admin.Business.RacingDefaultSetting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Tarus.Admin.Entities;
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRacingDefaultSettingService
    {
        List<RacingDefaultSetting> SelRacingDefaultSetting(int SportType, int Bettype, string Currency);
        Result UpdateRacingDefaultSetting(int UserID, int Sporttype, int Bettype, int CurrencyID, int MinBet);
        List<Result> UpdateMultipleRacingDefaultSetting(List<RacingDefaultSetting> racingList);
    }
}
