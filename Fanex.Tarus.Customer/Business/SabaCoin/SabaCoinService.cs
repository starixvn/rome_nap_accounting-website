namespace Fanex.Tarus.Customer.Business.SabaCoin
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.Criteria.SabaCoin;
    using Fanex.Tarus.Customer.Entities.SabaCoin;

    public interface ISabaCoinService
    {
        IEnumerable<SabaCoinModel> Get(long sabaId);
    }

    public class SabaCoinService : ISabaCoinService
    {
        private readonly IDynamicRepository dynamicRepository;

        public SabaCoinService() : this(null) { }
        public SabaCoinService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }
        public IEnumerable<SabaCoinModel> Get(long sabaId)
            => dynamicRepository.Fetch<SabaCoinModel>(new GetSabaCoinAccountCriteria(sabaId));
    }
}
