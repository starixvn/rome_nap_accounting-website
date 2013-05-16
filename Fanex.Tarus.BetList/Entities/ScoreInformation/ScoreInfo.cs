// --------------------------------------------------------------------------
// <copyright file="ScoreInfo.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.Tarus.BetList.Constants;
    using Fanex.Tarus.Infrastructure.Resource;

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

        public bool IsPending { get; set; }

        public string HomeFinalScore { get; set; }

        public string AwayFinalScore { get; set; }

        #region DTOField
        #endregion DTOField

        public virtual void CheckCancellation(string betStatus)
        {
            var strNotAvailable = Labels.NotAvailable;
            var isCancelFinal = betStatus.StartsWith(BetListContants.SCORE_INFOMATION_REFUND_FT) ||
              betStatus.StartsWith(BetListContants.SCORE_INFOMATION_REFUND);

            if (isCancelFinal)
            {
                this.HomeFinalScore = strNotAvailable;
                this.AwayFinalScore = strNotAvailable;
            }
        }
    }
}
