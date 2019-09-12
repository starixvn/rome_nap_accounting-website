namespace Fanex.Tarus.CN88.Repositories.PositionTaking
{
    using Fanex.Tarus.CN88.Criteria.PositionTaking;
    using Fanex.Tarus.CN88.Models.PositionTaking;

    public interface IPTRepository
    {
        LinePTModel GetPT(GetPTCriteria criteria);

        LineCommModel GetComm(GetCommCriteria criteria);
    }
}
