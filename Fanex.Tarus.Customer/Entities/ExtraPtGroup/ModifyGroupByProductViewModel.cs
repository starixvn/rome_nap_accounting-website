using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Entities.ExtraPtGroup
{
    public class ModifyGroupByProductViewModel
    {
        public IList<Group> Groups { get; set; }

        public IList<Product> Products { get; set; }

        public GroupByProduct SelectedItem { get; set; }
    }
}