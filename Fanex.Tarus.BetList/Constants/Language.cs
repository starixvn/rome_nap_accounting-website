using System.Collections.Generic;

namespace Fanex.Tarus.BetList.Constants
{
    public static class Language
    {
        public const string English = "en-US";
        public const string ChineseTraditional = "zh-TW";
        public const string ChineseSimplified = "zh-CN";
        public const string NewChineseSimplified = "zh-Hans";

        public const string Default = English;

        public static readonly Dictionary<string, string> CodeNames = new Dictionary<string, string>
        {
            [English] = "English",
            [ChineseTraditional] = "繁體中文",
            [ChineseSimplified] = "简体中文",
            [NewChineseSimplified] = "简体中文(新)",
        };
        public static string GetCountryName(string codeName) => CodeNames[codeName];
    }
}