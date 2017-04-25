using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Entities.ExtraPtGroup
{
    public class GroupByProductViewModel
    {
        public IList<GroupByProduct> GroupsByProduct { get; set; }

        public IList<Product> Products { get; set; }

        public byte CurrentProductId { get; set; }
    }
}
