namespace Fanex.Tarus.BetList.Business.Results
{
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;

    public class AGCasinoResultService : IAGCasinoResultService
    {
        private IAGCasinoResultDataAccess _resultDataAccess;

        public AGCasinoResultService(IAGCasinoResultDataAccess resultDataAccess)
        {
            this._resultDataAccess = resultDataAccess ?? new AGCasinoResultDataAccess();
        }

        public AGCasinoResultService()
            : this(null)
        {
        }

        public string GetResult(GetAGCasinoResultCriteria criteria)
        {
            var resultData = this._resultDataAccess.GetResult(criteria);
            if (resultData != null && resultData.Count > 0)
            {
                return ResultEngine.HtmlParser(resultData, GetFanexResultSetting());
            }

            return string.Empty;
        }

        private static ResultSetting GetFanexResultSetting()
        {
            return new ResultSetting
            {
                SportType = SportType.AGCasino,
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
