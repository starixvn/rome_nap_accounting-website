using System.Collections.Generic;

namespace Fanex.Tarus.BetList.Constants
{
    public static class BookedBetStatusConstants
    {
        public const int Booking = 0;
        public const int Successful = 1;
        public const int Unsuccessful = 2;
        public const int ProcressingBet = 3;
        public const int Cancelled = 4;

        public static IDictionary<int, string> List { get; } = new Dictionary<int, string>
        {
            [Booking] = "Booking",
            [Successful] = "Successful",            
            [Unsuccessful] = "Unsuccessful",
            [Cancelled] = "Cancelled",
        };

        public static IDictionary<int, string> All { get; } = new Dictionary<int, string>
        {
            [Booking] = "Booking",
            [Successful] = "Successful",
            [Unsuccessful] = "Unsuccessful",
            [Cancelled] = "Cancelled",
            [ProcressingBet] = "Booking",
        };

        public static string GetText(int statusId)
            => All.ContainsKey(statusId) ? All[statusId] : statusId.ToString();
    }
}