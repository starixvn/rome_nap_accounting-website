using System;

namespace Fanex.Tarus.Customer.Entities.ExtraPtGroup
{
    public class GroupByProduct
    {
        public GroupByProduct()
        {
            Group = new Group();
            Product = new Product();
        }

        public Group Group { get; set; }

        public Product Product { get; set; }

        public decimal ExtraPt { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime EndDate { get; set; }        

        public bool IsAllowDeleted { get; set; }
    }
}
