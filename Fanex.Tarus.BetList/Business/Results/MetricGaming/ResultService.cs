namespace Fanex.Tarus.BetList.Business.Results.MetricGaming
{
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Tarus.BetList.Criteria.Results.MetricGaming;
    using Fanex.Tarus.BetList.DataAccess.Results.MetricGaming;

    public class ResultService : IResultService
    {
        private IResultDataAccess _resultDataAccess;

        public ResultService(IResultDataAccess resultDataAccess)
        {
            this._resultDataAccess = resultDataAccess ?? new ResultDataAccess();
        }

        public ResultService()
            : this(null)
        {
        }

        public string GetResult(ViewResultCriteria criteria)
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
                SportType = SportType.Soccer,
                IsOutRight = false,
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