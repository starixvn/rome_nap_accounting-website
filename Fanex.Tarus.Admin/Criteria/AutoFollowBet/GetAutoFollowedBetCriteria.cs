// --------------------------------------------------------------------------
// <copyright file="GetAutoFollowedBetCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Admin.Criteria.AutoFollowBet
{
    using System.Collections.Generic;
    using Fanex.Tarus.Admin.Models.AutoFollowBet;

    /// <summary>
    /// Get Auto Follow BetCriteria.
    /// </summary>
    public class GetAutoFollowedBetCriteria
    {
        public string Username { get; set; }

        public int Status { get; set; }

        public int Source { get; set; }

        public List<FollowTypeInfo> FollowTypes { get; set; }

        public string GetSettingKey()
        {
            return "Acc_MailingListSel";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
