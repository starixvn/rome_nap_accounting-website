// --------------------------------------------------------------------------
// <copyright file="IResultService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="David>Tran" date="2014.10.24">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Business
{
    using Fanex.NAP.Framework.Data;
    using Models;
    using System.Collections.Generic;

    public interface IResultService
    {
        List<TennisLeagueResult> GetTennisResult(CriteriaBase criteria, string matchId, int bettypeId);
    }
}