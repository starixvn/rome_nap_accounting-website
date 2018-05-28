// --------------------------------------------------------------------------
// <copyright file="IVirtualRacingResultService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Business.Results
{
    /// <summary>
    /// The interface IVirtualRacingResultService.
    /// </summary>
    public interface IVirtualRacingResultService
    {
        string GetResult(int matchId, int sportType);
    }
}