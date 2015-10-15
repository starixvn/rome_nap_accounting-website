namespace Fanex.Tarus.BetList.Business
{
    using Fanex.Tarus.BetList.Criteria;
    using Fanex.Tarus.BetList.Entities.ColossusBets;

    public interface IColossusBetsService
    {
        string BetDetails(GetColossusBetsBetDetailsCriteria criteria);

        ViewMemberChoiceResult MemberChoice(GetColossusBetsBetDetailsCriteria criteria);
    }
}
