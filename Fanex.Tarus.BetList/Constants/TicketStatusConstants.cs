using System.Collections.Generic;

namespace Fanex.Tarus.BetList.Constants
{
    public static class TicketStatusConstants
    {
        public const int Running = 0;
        public const int Waiting = 1;
        public const int Reject = 101;
        public const int Void = 102;
        public const int Refund = 103;
        public const int Draw = 111;
        public const int Won = 112;
        public const int Lose = 113;
        public const int ResultPending = 2;
        public const int UnderSettlement = 3;
        public const int Pending = 4;
        public const int AbnormalBets = 99;

        public static IDictionary<int, string> List { get; } = new Dictionary<int, string>
        {
            [Waiting] = nameof(Waiting),
            [Reject] = nameof(Reject),
            [Running] = nameof(Running),         
            [ResultPending] = nameof(ResultPending),
            [UnderSettlement] = nameof(UnderSettlement),
            [Won] = nameof(Won),
            [Lose] = nameof(Lose),
            [Draw] = nameof(Draw),
            [Refund] = nameof(Refund),
            [Void] = nameof(Void),
            [AbnormalBets] = nameof(AbnormalBets),
        };
    }
}
