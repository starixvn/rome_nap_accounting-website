namespace Fanex.Tarus.Customer.Helpers
{
    public static class NumberFormater
    {
        public static string FormatNumber(decimal value)
            => value.ToString("#,##0.00");

        public static string FormatNumber(decimal? value)
            => value.HasValue ? FormatNumber(value.Value) : string.Empty;
    }
}
