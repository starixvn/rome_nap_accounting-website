// --------------------------------------------------------------------------
// <copyright file="GetSportingSolutionsResultCriteria.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.BetList.Accounting.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetSportingSolutionsResultCriteria
    {
        [SpParam(SqlParamName = "Matchid")]
        public int MatchId { get; set; }

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