namespace Fanex.Tarus.BetList.Business.Results
{
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;

    public interface IBitcoinResultService
    {
        string GetResult(int matchId, int sportType);
    }

    public class BitcoinResultService : IBitcoinResultService
    {
        private readonly IBitcoinResultDataAccess dataAccess;

        public BitcoinResultService(IBitcoinResultDataAccess dataAccess)
        {
            this.dataAccess = dataAccess ?? new BitcoinResultDataAccess();
        }

        public BitcoinResultService()
            : this(null)
        {
        }

        public string GetResult(int matchId, int sportType)
        {
            var criteria = new GetBitcoinResultCriteria
            {
                MatchId = matchId
            };
            var bitcoins = this.dataAccess.GetResult(criteria);
            var resultSetting = GetFanexResultSetting(sportType);

            return ResultEngine.HtmlParser(bitcoins, resultSetting);
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
                VisiblePopUpTitle = false
            };
        }
    }
}