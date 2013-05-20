// --------------------------------------------------------------------------
// <copyright file="TennisInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Entities.ScoreInformation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Script.Serialization;
    using Fanex.Tarus.BetList.Entities.SportingSolutions;

    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class TennisInfo : ScoreInfo
    {
        public List<string> HomeScores { get; set; }

        public List<string> AwayScores { get; set; }

        public SetX_Info SetXInformation { get; set; }

        [ScriptIgnore]
        public int HomeTotalGamesScore { get; set; }

        [ScriptIgnore]
        public int AwayTotalGamesScore { get; set; }

        #region DTOField

        public string HomeTotalGamesScore_Str { get; set; }

        public string AwayTotalGamesScore_Str { get; set; }

        #endregion DTOField
    }
}
