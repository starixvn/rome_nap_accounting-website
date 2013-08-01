// --------------------------------------------------------------------------
// <copyright file="GetMatchInfo_AddSMAListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Criteria.SMATurnover
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class GetMatchInfo_AddSMAListCriteria
    {
        public int SportId { get; set; }

        public int LeagueId { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string GetSettingKey()
        {
            return "GetInfoByMatch";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
