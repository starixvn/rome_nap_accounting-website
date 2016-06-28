// --------------------------------------------------------------------------
// <copyright file="IRacingResultDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.DataAccess.Results
{
    using System.Collections.Generic;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;

    /// <summary>
    /// The interface IRacingResultDataAccess.
    /// </summary>
    public interface IRacingResultDataAccess
    {
        IEnumerable<Racing> GetRacingResult(ref GetRacingResultCriteria criteria);
    }
}
