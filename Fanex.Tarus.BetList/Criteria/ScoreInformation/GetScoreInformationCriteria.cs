// --------------------------------------------------------------------------
// <copyright file="GetScoreInformationCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.BetList.Criteria.ScoreInformation
{
    using System.Diagnostics.CodeAnalysis;
    using DNAide.Core;
    using Fanex.Tarus.BetList.Helper;
    using Fanex.Tarus.BetList.DataAccess;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class GetScoreInformationCriteria
    {
        public int MatchID { get; set; }

        public int Bettype { get; set; }

        public string ReportDate { get; set; }

        public int SportType { get; set; }

        public string SportTypeName { get; set; }

        public string BetTypeName { get; set; }

        public bool LiveIndicator { get; set; }

        public string Hedge { get; set; }

        public int LeagueId { get; set; }

        public string LeagueName { get; set; }

        public string MatchCode { get; set; }

        public int HomeTeam { get; set; }

        public string HomeTeamName { get; set; }

        public int AwayTeam { get; set; }

        public string AwayTeamName { get; set; }

        public string BetId { get; set; }

        public string GetSettingKey()
        {
            return "BetList_ScoreInformation_SelScoreInformation" +
                (CastAide.AsDateTime(this.ReportDate) < ReportDataAccess.BalanceUp.BalanceUpDay ?
                 ReportServices.WARE_HOUSE_SP_SUFFIX :
                 string.Empty);
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
