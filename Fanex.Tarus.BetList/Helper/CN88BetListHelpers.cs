namespace Fanex.Tarus.BetList.Helper
{
    public class CN88BetListHelpers
    {
        public static string Stake(object value)
            => string.Format("{0:#,##0.00}", value);
    }
}
