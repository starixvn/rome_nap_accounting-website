// --------------------------------------------------------------------------
// <copyright file="UploadSMAListResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    [Serializable]
    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class UploadSMAListResult
    {
        public string Username { get; set; }

        public string CustId { get; set; }

        public string MaxTurnover { get; set; }

        public string SportId { get; set; }

        public string SportName { get; set; }

        public string LeagueId { get; set; }

        public string LeagueName { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string MatchId { get; set; }

        public int HomeTeam { get; set; }

        public string HomeTeamName { get; set; }

        public int AwayTeam { get; set; }

        public string AwayTeamName { get; set; }

        public string EventDate { get; set; }

        public string AddResultMessage { get; set; }

        public List<UploadSMAListResult> FailedInfoList;

        public List<UploadSMAListResult> SucceededInfoList;
    }
}
