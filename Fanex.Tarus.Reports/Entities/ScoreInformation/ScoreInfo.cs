// --------------------------------------------------------------------------
// <copyright file="ScoreInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Reports.Entities.ScoreInformation
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ScoreInfo
    {
        public string TraderName { get; set; }

        public string EventTime { get; set; }

        public int HomeTeamId { get; set; }

        public string HomeTeamName { get; set; }

        public int AwayTeamId { get; set; }

        public string AwayTeamName { get; set; }

        public string EventStatus { get; set; }

        public bool IsNeutral { get; set; }

        #region SoccerInfo

        public string HomeHTScore { get; set; }

        public string HomeFinalScore { get; set; }

        public string AwayHTScore { get; set; }

        public string AwayFinalScore { get; set; }

        public int HT1stScoreTeam { get; set; }

        public int Final1stScoreTeam { get; set; }

        #endregion SoccerInfo
        #region DTOField


        #endregion DTOField
    }
}
