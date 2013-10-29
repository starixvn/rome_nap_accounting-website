// --------------------------------------------------------------------------
// <copyright file="PostionTakingInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Entities.ViewPT
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Serializable]
    public class PostionTakingInfo
    {
        public CustomerInfo CustomerInfo { get; set; }

        public string SportName { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }

        public string SuperPT { get; set; }

        public string MasterPT { get; set; }

        public string AgentPT { get; set; }

        public bool IsAutoPT { get; set; }

        public string AutoPT { get; set; }

        public string MinPt { get; set; }

        public string Commission { get; set; }

        public string Incentive { get; set; }
    }
}
