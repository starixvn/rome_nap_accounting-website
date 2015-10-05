namespace Fanex.Tarus.BetList.Business
{
    using Fanex.Tarus.BetList.Criteria;

    public interface IColossusBetsService
    {
        string BetDetails(GetColossusBetsBetDetailsCriteria criteria);
    }
}
