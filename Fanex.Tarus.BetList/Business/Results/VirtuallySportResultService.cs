namespace Fanex.Tarus.BetList.Business.Results
{
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;

    public interface IVirtuallySportResultService
    {
        string GetResult(int matchId, int sportType);
    }

    public class VirtuallySportResultService : IVirtuallySportResultService
    {
        private readonly IVirtuallySportResultDataAccess dataAccess;

        public VirtuallySportResultService(IVirtuallySportResultDataAccess dataAccess)
        {
            this.dataAccess = dataAccess ?? new VirtuallySportResultDataAccess();
        }

        public VirtuallySportResultService()
            : this(null)
        {
        }

        public string GetResult(int matchId, int sportType)
        {
            var criteria = new Get3rdResultCriteria
            {
                MatchId = matchId
            };
            var virtuallySports = this.dataAccess.GetResult(criteria);
            var resultSetting = GetFanexResultSetting(sportType);

            return ResultEngine.HtmlParser(virtuallySports, resultSetting);
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