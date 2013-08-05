// --------------------------------------------------------------------------
// <copyright file="ISMATurnoverAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.SettingCustomer.SMATurnover
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Tarus.Customer.Criteria.SMATurnover;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover;

    public interface ISMATurnoverAccess
    {
        DataTable GetSportType(int mode);
        DataTable GetLeague(int sport,
            DateTime fromDate, 
            DateTime toDate,
            bool isGetList);
        MatchInfo GetInfoByMatchId(int matchId);
        DataTable GetSMATurnoverList(int sportId, int leagueId, DateTime fromDate, DateTime toDate);
        Result AddSMATurnover(int matchId, string custName, decimal maxTurnover, int userId, string ip);
        Result UpdateSMATurnover(int matchId, int custId, decimal maxTurnover, int userId, string ip);
        Result CheckMaxTurnoverValue(int matchId, string custName, decimal maxTurnover, int mode);

        List<dynamic> GetMatchInfo_AddSMAList(GetMatchInfo_AddSMAListCriteria criteria,
            ref Hashtable outputParams);

        List<dynamic> CheckSMAList(CheckSMAListCriteria criteria,
            ref Hashtable outputParams);

        List<dynamic> AddValidSMAList(AddSMAListCriteria criteria, ref Hashtable outputParams);
    }
}
