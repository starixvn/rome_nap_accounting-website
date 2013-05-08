


namespace Fanex.Tarus.VirtualSport.Helpers
{
    public class StringHelper
    {
        public string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            if (s.Length == 1)
            {
                return char.ToUpper(s[0]).ToString();
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
