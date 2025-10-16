namespace Fanex.Tarus.BetList.Business.Results
{
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;

    public interface IBetbyResultService
    {
        string GetResult(int matchId, int sportType);
    }

    public class BetbyResultService : IBetbyResultService
    {
        private readonly IBetbyResultDataAccess dataAccess;

        public BetbyResultService(IBetbyResultDataAccess dataAccess)
        {
            this.dataAccess = dataAccess ?? new BetbyResultDataAccess();
        }

        public BetbyResultService()
            : this(null)
        {
        }

        public string GetResult(int matchId, int sportType)
        {
            var criteria = new Get3rdResultCriteria
            {
                MatchId = matchId
            };
            var betbyResult = this.dataAccess.GetResult(criteria);
            var resultSetting = GetFanexResultSetting(sportType);

            return ResultEngine.HtmlParser(betbyResult, resultSetting);
        }

        private static ResultSetting GetFanexResultSetting(int sportType)
        {
            return new ResultSetting
            {
                SportType = (SportType)sportType,
                IsOutright = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                IsPopupOpenFromBetList = true,
                LanguageCode = "en-US",
                VisibleMatchCodeIDColumn = false,
                VisiblePopUpTitle = true
            };
        }
    }
}