using System.Collections.Generic;
using Fanex.Tarus.BetList.Constants;

public class LanguageViewModel
{
    public LanguageViewModel(string currentLanguage = Language.Default)
    {
        Data = Language.CodeNames;
        CurrentLanguage = currentLanguage;
    }

    public Dictionary<string, string> Data { get; }

    public string CurrentLanguage { get; }
}
