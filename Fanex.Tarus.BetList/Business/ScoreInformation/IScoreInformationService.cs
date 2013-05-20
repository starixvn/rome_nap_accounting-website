// --------------------------------------------------------------------------
// <copyright file="IScoreInformationService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Business.ScoreInformation
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.BetList.Criteria.ScoreInformation;
    using Fanex.Tarus.BetList.Entities.ScoreInformation;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface IScoreInformationService
    {
        ScoreInfo GetScoreInfo(GetScoreInformationCriteria criteria);
    }
}
