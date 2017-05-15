namespace Fanex.Tarus.BetList.DataAccess
{
    using BetListDetail.Lucky3.Criteria;
    using BetListDetail.Lucky3.DataAccesses;
    using BetListDetail.Lucky3.Models;
    using Criteria.Lucky3;
    using NAP.Framework.Data;
    using System.Collections.Generic;

    public class Lucky3Repository : ILucky3DataAccess
    {
        private readonly IDynamicRepository dynamicRepository;

        public Lucky3Repository() : this(null)
        {
        }

        public Lucky3Repository(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public IEnumerable<Lucky3DetailModel> GetLucky3Detail(GetLucky3DetailParameter criteria)
        {
            var command = new GetLucky3DetailCriteria(criteria);

            return dynamicRepository.Fetch<Lucky3DetailModel>(command);
        }
    }
}
