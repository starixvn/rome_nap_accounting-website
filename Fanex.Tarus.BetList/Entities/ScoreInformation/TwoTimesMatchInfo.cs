// --------------------------------------------------------------------------
// <copyright file="TwoTimesMatchInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Entities.ScoreInformation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.Infrastructure.Resource;
    using Fanex.Tarus.BetList.Constants;

    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class TwoTimesMatchInfo : ScoreInfo
    {
        public string HomeHTScore { get; set; }

        public string AwayHTScore { get; set; }

        public int HT1stScoreTeam { get; set; }

        public int Final1stScoreTeam { get; set; }

        public override void CheckCancellation(string betStatus)
        {
            var strNotAvailable = Labels.NotAvailable;
            var isCancelHT = betStatus.StartsWith(BetListContants.SCORE_INFOMATION_REFUND_HT) ||
                      betStatus.StartsWith(BetListContants.SCORE_INFOMATION_REFUND);

            if (isCancelHT)
            {
                this.HomeHTScore = strNotAvailable;
                this.AwayHTScore = strNotAvailable;
            }

            base.CheckCancellation(betStatus);
        }
    }
}
