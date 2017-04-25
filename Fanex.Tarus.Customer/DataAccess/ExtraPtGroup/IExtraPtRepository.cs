using System.Collections.Generic;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Entities.ExtraPtGroup;

namespace Fanex.Tarus.Customer.DataAccess.ExtraPtGroup
{
    public interface IExtraPtRepository
    {
        IList<Product> GetProducts(CriteriaBase criteria);

        IList<Group> GetGroups(CriteriaBase criteria);

        Result InsertGroupByProduct(CriteriaBase criteria);

        Result UpdateGroupByProduct(CriteriaBase criteria);

        Result DeleteGroupByProduct(CriteriaBase criteria);

        IList<GroupByProduct> GetGroupsByProduct(CriteriaBase criteria);
    }
}
