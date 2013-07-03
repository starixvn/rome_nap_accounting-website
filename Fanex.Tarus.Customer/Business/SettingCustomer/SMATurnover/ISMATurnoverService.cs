// --------------------------------------------------------------------------
// <copyright file="ISMATurnoverService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.SettingCustomer.SMATurnover
{
    using System;
    using System.Collections.Generic;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover;

    public interface ISMATurnoverService
    {
        List<Sport> GetSportType(int mode);
        List<League> GetLeague(int sportType, DateTime fromDate, DateTime toDate);
        MatchInfo GetInfoByMatchId(int matchId);
        List<CustomerTurnoverDetail> GetSMATurnoverList(int sportId, int leagueId, DateTime fromDate, DateTime toDate);
        Result AddSMATurnover(int matchId, string custName, decimal maxTurnover);
        Result UpdateSMATurnover(int matchId, int custId, decimal maxTurnover);
        Result CheckMaxTurnoverValue(int matchId, string custName, decimal maxTurnover, int mode);
    }
}
