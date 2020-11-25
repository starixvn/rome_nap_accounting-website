namespace Fanex.Tarus.BetList.Business.Results
{
    using System.Collections.Generic;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.LeapGaming.DTO;


    public interface ILeapGamingResultService
    {
        string GetResult(string transdesc);
    }

    public class LeapGamingResultService : ILeapGamingResultService
    {

        public LeapGamingResultService()
        {
        }

        public string GetResult(string transdesc)
            => ResultEngine.HtmlParser(new List<LeapGaming> { new LeapGaming { TransDesc = transdesc } }, GetFanexResultSetting());


        private static ResultSetting GetFanexResultSetting()
        {
            return new ResultSetting
            {
                SportType = SportType.GoldDeluxe,
                IsOutright = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                LanguageCode = "en-US",
                VisibleMatchCodeIDColumn = false,
                VisiblePopUpTitle = true,
                HighlightWinnerTeam = true
            };
        }
    }
}