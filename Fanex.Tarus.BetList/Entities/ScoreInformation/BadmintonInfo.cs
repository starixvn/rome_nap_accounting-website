// --------------------------------------------------------------------------
// <copyright file="BadmintonInfo.cs" company="Nexcel Solutions Vietnam">
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

    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class BadmintonInfo : ScoreInfo
    {
        public List<string> HomeScores { get; set; }

        public List<string> AwayScores { get; set; }

        [ScriptIgnore]
        public int HomeTotalGameScore { get; set; }

        [ScriptIgnore]
        public int AwayTotalGameScore { get; set; }

        public string HomeTotalGameScore_Str { get; set; }

        public string AwayTotalGameScore_Str { get; set; }
    }
}
