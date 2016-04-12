// --------------------------------------------------------------------------
// <copyright file="BetListAreaRegistration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Sunplus.Accounting.Business.Customer
{
    /// <summary>
    /// Render Left Panel Parameter.
    /// </summary>
    public class RenderLeftPanelParameter
    {
        public bool IsShowEnableBetTrade { get; set; }

        public bool IsShowEnableMicrogaming { get; set; }

        public bool IsShowMacau { get; set; }

        public bool IsShowKeno { get; set; }

        public bool IsShowGoldDeluxe { get; set; }

        public bool IsShowColossusBetsForCredit { get; set; }

        public bool IsShowAllBetForCredit { get; set; }

        public bool IsShowAutoSuspend { get; set; }

        public int ParentId { get; set; }

        public int RoleId { get; set; }

        public int CustId { get; set; }

        public bool IsDeposit { get; set; }

        public string Username { get; set; }

        public int AccId { get; set; }

        public bool IsShowSportsbook2 { get; set; }

        public bool IsShowCockFighting { get; set; }

        public bool IsHideCricket { get; set; }
    }
}
