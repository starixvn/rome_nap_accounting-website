using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Entities.ExtraPtGroup
{
    public class AddGroupByProductViewModel
    {
        public IList<Group> Groups { get; set; }

        public IList<Product> Products { get; set; }
    }
}