using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
