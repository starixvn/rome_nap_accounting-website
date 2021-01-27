using System;

namespace Fanex.Tarus.Customer.Extensions
{
    public static class DateTimeExtensions
    {
        private const string ShortDateWithSlash = "MM/dd/yyyy";
        private const string LongDateWithSlash = "MM/dd/yyyy HH:mm:ss";

        public static string ToShortDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(ShortDateWithSlash);

        public static string ToShortDateWithSlash(this DateTime? dateTime)
            => dateTime.HasValue ? dateTime.Value.ToShortDateWithSlash() : string.Empty;

        public static string ToLongDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(LongDateWithSlash);
    }
}
