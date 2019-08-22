namespace Fanex.Tarus.CN88.Services
{
    using Fanex.Tarus.CN88.Criteria.PositionTaking;
    using Fanex.Tarus.CN88.Models.PositionTaking;

    public interface IPTService
    {
        LinePTModel GetPT(GetPTCriteria criteria);

        LineCommModel GetComm(GetCommCriteria criteria);
    }
}
