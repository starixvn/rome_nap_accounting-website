namespace Fanex.Tarus.CN88.Helpers
{
    using System;

    public static class PositionTakingHelper
    {
        public static bool IsUpdatePTTime()
        {
            var time = DateTime.Now;
            DayOfWeek day = time.DayOfWeek;
            return (day == DayOfWeek.Sunday) || (day == DayOfWeek.Monday && time.Hour <= 7);
        }
    }
}
