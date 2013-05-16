// --------------------------------------------------------------------------
// <copyright file="GetSportingSolutionsResultCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Criteria.SportingSolutions
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class GetSportingSolutionsResultCriteria
    {
        public int Matchid { get; set; }

        public string Resourceid { get; set; }

        public string GetSettingKey()
        {
            return "SoprtingSolutionBetlistResult";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
