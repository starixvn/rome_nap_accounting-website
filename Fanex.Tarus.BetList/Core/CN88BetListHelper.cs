namespace Fanex.Tarus.BetList.Core
{
    using Fanex.Tarus.BetList.Extensions;

    public class CN88BetListHelper
    {
        private static readonly int Lottery_Max5D_60 = 1031;
        private static readonly int Lottery_End = 1049;

        private static readonly int Keno_Start = 1501;
        private static readonly int Keno_End = 1599;

        private static readonly int Happy5_Start = 8101;
        private static readonly int Happy5_End = 8120;

        private static readonly int NumberGame_Start = 81;
        private static readonly int NumberGame_End = 90;

        private static readonly int VirtualSports_Start = 1201;
        private static readonly int VirtualSports_End = 1238;

        private static readonly int VirtualSports2_Start = 2701;
        private static readonly int VirtualSports2_End = 2812;

        public static bool IsLottery(int betType)
           => betType.InRange(Lottery_Max5D_60, Lottery_End);

        public static bool IsKeno(int betType)
            => betType.InRange(Keno_Start, Keno_End);

        public static bool IsHappy5(int betType)
           => betType.InRange(Happy5_Start, Happy5_End);

        public static bool IsNumberGame(int betType)
           => betType.InRange(NumberGame_Start, NumberGame_End);

        public static bool IsVirtualSport(int betType)
          => betType.InRange(VirtualSports_Start, VirtualSports_End);

        public static bool IsVirtualSport2(int betType)
          => betType.InRange(VirtualSports2_Start, VirtualSports2_End);


        public static bool IsShowComm(int betType)
            => !(IsKeno(betType)
            || IsNumberGame(betType)
            || IsHappy5(betType)
            || IsVirtualSport(betType)
            || IsVirtualSport2(betType)
            || IsLottery(betType));
    }
}