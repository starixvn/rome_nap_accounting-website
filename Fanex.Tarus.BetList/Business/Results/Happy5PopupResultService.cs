namespace Fanex.Tarus.BetList.Business.Results
{
    using System.Linq;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Tarus.BetList.Criteria.Results;
    using Fanex.Tarus.BetList.DataAccess.Results;
    using Result.Core.Happy5.DTO;

    public interface IHappy5PopupResultService
    {
        string GetResult(int matchId);
    }

    public class Happy5PopupResultService : IHappy5PopupResultService
    {
        private INumberGamePopupResultDataAccess numberGamePopupResultDataAccess;

        public Happy5PopupResultService(INumberGamePopupResultDataAccess numberGamePopupResultDataAccess)
        {
            this.numberGamePopupResultDataAccess = numberGamePopupResultDataAccess ?? new NumberGamePopupResultDataAccess();
        }

        public Happy5PopupResultService()
            : this(null)
        {
        }

        public string GetResult(int matchId)
        {
            var criteria = new GetNumberGamePopupResultCriteria()
            {
                MatchID = matchId
            };
            var results = numberGamePopupResultDataAccess.GetNumberGamePopupResult(criteria);
            var happy5Results = results.Select(i =>
            new Happy5
            {
                Ball = i.Ball,
                EventDate = i.EventDate,
                EventStatus = i.EventStatus,
                IsPending = i.IsPending,
                KickOffTime = i.KickOffTime,
                LeagueId = i.LeagueId,
                MatchCode = i.MatchCode,
                MatchId = i.MatchId,
                Seq = i.Seg
            }).ToList();

            return ResultEngine.HtmlParser(happy5Results, this.GetFanexResultSetting());
        }

        private ResultSetting GetFanexResultSetting()
        {
            return new ResultSetting
            {
                SportType = SportType.Happy5,
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