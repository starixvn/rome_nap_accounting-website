namespace Fanex.Tarus.BetList.Business.Results
{
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;

    public class MetricGamingResultService : IMetricGamingResultService
    {
        private IMetricGamingResultDataAccess _resultDataAccess;

        public MetricGamingResultService(IMetricGamingResultDataAccess resultDataAccess)
        {
            this._resultDataAccess = resultDataAccess ?? new MetricGamingResultDataAccess();
        }

        public MetricGamingResultService()
            : this(null)
        {
        }

        public string GetResult(GetMetricGamingResultCriteria criteria)
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