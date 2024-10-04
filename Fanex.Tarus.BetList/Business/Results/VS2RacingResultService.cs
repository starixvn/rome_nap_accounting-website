namespace Fanex.Tarus.BetList.Business.Results
{
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;
    using Sunplus.Accounting.DataAccess.Report;
    using System;

    public class VS2RacingResultService : IVS2RacingResultService
    {
        private readonly IVS2RacingResultDataAccess _vS2RacingResultDataAccess;

        public VS2RacingResultService(IVS2RacingResultDataAccess vS2RacingResultDataAccess)
        {
            _vS2RacingResultDataAccess = vS2RacingResultDataAccess ?? new VS2RacingResultDataAccess();
        }

        public VS2RacingResultService()
            : this(null)
        {
        }

        public string GetResult(int matchId, int sportType, DateTime eventDate)
        {
            var criteria = GetCriteria(matchId, eventDate);
            var results = _vS2RacingResultDataAccess.GetRacingPopupResult(criteria);
            ResultSetting resultSetting = GetFanexResultSetting(sportType);

            return ResultEngine.HtmlParser(results, resultSetting);
        }

        private static GetVS2RacingResultCriteria GetCriteria(int matchId, DateTime eventDate)
        {
            return new GetVS2RacingResultCriteria()
            {
                EventId = matchId,
                EventDate = eventDate,
                MoveBetsDay = ReportUtils.BalanceUp.MoveBetsDay,
                BalanceUpDay = ReportUtils.BalanceUp.BalanceUpDay,
            };
        }

        /// <summary>
        /// Gets the FANEX result setting.
        /// </summary>
        /// <param name="sportType">Type of the sport.</param>
        /// <returns>The object of ResultSetting.</returns>
        private static ResultSetting GetFanexResultSetting(int sportType)
        {
            return new ResultSetting
            {
                SportType = (SportType)sportType,
                IsOutright = false,
                IsParlayPopUp = false,
                IsPopUp = true,
                VisiblePopUpTitle = false,
                LanguageCode = "en-US",
                VisibleMatchCodeIDColumn = false,
                VisibleFreeHeader = true,
                VisibleDetailedLink = false,
                HighlightWinnerTeam = true
            };
        }
    }
}
