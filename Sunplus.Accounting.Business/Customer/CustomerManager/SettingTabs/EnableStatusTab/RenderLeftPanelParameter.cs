// --------------------------------------------------------------------------
// <copyright file="BetListAreaRegistration.cs" company="">
// Copyright (c) . All rights reserved.
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

        public bool IsShowMacau { get; set; }

        public bool IsShowKeno { get; set; }

        public bool IsShowGoldDeluxeForCredit { get; set; }

        public bool IsShowColossusBetsForDeposit { get; set; }

        public bool IsShowLiveCasinoForCredit { get; set; }

        public bool IsShowVoidbridge { get; set; }

        public bool IsShowFishingWorld { get; set; }

        public bool IsShowAutoSuspend { get; set; }

        public bool IsShowVirtualSport { get; set; }

        public bool IsShowVirtualSport2 { get; set; }

        public int ParentId { get; set; }

        public int RoleId { get; set; }

        public int CustId { get; set; }

        public bool IsDeposit { get; set; }

        public string Username { get; set; }

        public int AccId { get; set; }

        public bool IsShowNG2 { get; internal set; }

        public bool IsShowRNGLottery { get; set; }

        public bool IsShowVirtualGames { get; set; }

        public bool IsShowSabaclub { get; set; }

        public bool IsShowVGaming { get; set; }

        public bool IsShowSabaVirtualSports { get; set; }

        public bool IsShowBitcoin { get; set; }

        public bool IsShowCashout { get; set; }
    }
}