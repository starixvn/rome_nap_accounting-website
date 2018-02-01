using System.Collections.Generic;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.DataAccess.ExtraPtGroup;
using Fanex.Tarus.Customer.Entities.ExtraPtGroup;

namespace Fanex.Tarus.Customer.Business.ExtraPtGroup
{
    public class ExtraPtService : IExtraPtService
    {
        private readonly IExtraPtRepository _repo = new ExtraPtRepository();

        public IList<Group> GetGroups(CriteriaBase criteria)
        {
            return _repo.GetGroups(criteria);
        }

        public IList<GroupByProduct> GetGroupsByProduct(CriteriaBase criteria)
        {
            return _repo.GetGroupsByProduct(criteria);
        }

        public IList<Product> GetProducts(CriteriaBase criteria)
        {
            return _repo.GetProducts(criteria);
        }

        public Result InsertGroupByProduct(CriteriaBase criteria)
        {
            return _repo.InsertGroupByProduct(criteria);
        }

        public Result UpdateGroupByProduct(CriteriaBase criteria)
        {
            return _repo.UpdateGroupByProduct(criteria);
        }

        public Result DeleteGroupByProduct(CriteriaBase criteria)
        {
            return _repo.DeleteGroupByProduct(criteria);
        }
    }
}
