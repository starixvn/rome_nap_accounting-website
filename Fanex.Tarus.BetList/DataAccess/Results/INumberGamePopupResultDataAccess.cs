// --------------------------------------------------------------------------
// <copyright file="INumberGamePopupResultDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Paul.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.DataAccess.Results
{
    using System.Collections.Generic;
    using Fanex.Result.Core.NumberGame.DTO;
    using Fanex.Tarus.BetList.Criteria.Results;

    /// <summary>
    /// The interface INumberGamePopupResultDataAccess.
    /// </summary>
    public interface INumberGamePopupResultDataAccess
    {
        IList<NumberGame> GetNumberGamePopupResult(GetNumberGamePopupResultCriteria criteria);
    }
}
