// --------------------------------------------------------------------------
// <copyright file="IScoreInformationService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Business.Results
{
    /// <summary>
    /// The interface IScoreInformationService.
    /// </summary>
    public interface IScoreInformationService
    {
        string GetResult(int matchId, int sportType, int betType, long betId, int handicap2, string transdesc);
    }
}