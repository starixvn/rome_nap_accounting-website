// --------------------------------------------------------------------------
// <copyright file="IScoreInformationDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.DataAccess.ScoreInformation
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.BetList.Criteria.ScoreInformation;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IScoreInformationDataAccess
    {
        List<dynamic> GetScoreInformation(GetScoreInformationCriteria criteria, ref Hashtable outputParam);
    }
}
