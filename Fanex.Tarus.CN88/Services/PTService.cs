namespace Fanex.Tarus.CN88.Services
{
    using Fanex.Tarus.CN88.Criteria.PositionTaking;
    using Fanex.Tarus.CN88.Models.PositionTaking;
    using Fanex.Tarus.CN88.Repositories.PositionTaking;

    public class PTService : IPTService
    {
        private readonly IPTRepository repository;

        public PTService() : this(null)
        {
        }

        public PTService(IPTRepository repository)
        {
            this.repository = repository ?? new PTRepository();
        }

        public LinePTModel GetPT(GetPTCriteria criteria)
            => repository.GetPT(criteria);

        public LineCommModel GetComm(GetCommCriteria criteria)
            => repository.GetComm(criteria);
    }
}
