using Fanex.BetList.Core.App_GlobalResources;
using System;
using System.Collections.Generic;

namespace Fanex.Tarus.BetList.Constants
{
    public static class SystemParlayConstants
    {
        public static readonly Dictionary<string, int> SystemParlyStatusOrder = new Dictionary<string, int>
        {
            { "",0 },
            { "WON",1 },
            { "LOSE",2 },
            { "DRAW",3 },
            { "VOID",4 },
            { "REJECT",5 },
            { "REFUND",6 },
            { "RUNNING",7 }
        };

        public static int GetStatusOrderId(string status)
            => SystemParlayConstants.SystemParlyStatusOrder.ContainsKey(status) ? SystemParlyStatusOrder[status] : 99;

        public static string GetStatusText(string status)
            => status.Equals("LOSE", StringComparison.CurrentCultureIgnoreCase) ? CoreBetList.lose.ToUpper() : status.ToUpper();
    }
}

