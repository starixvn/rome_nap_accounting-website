// --------------------------------------------------------------------------
// <copyright file="IVirtualTennisResultDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.DataAccess.Results
{
    using System.Collections.Generic;
    using Fanex.Result.Core.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;

    /// <summary>
    /// The interface IVirtualTennisResultDataAccess.
    /// </summary>
    public interface IVirtualTennisResultDataAccess
    {
        IList<Sportsbook> GetVirtualTennisPopupResult(GetVirtualTennisResultCriteria criteria);
    }
}
