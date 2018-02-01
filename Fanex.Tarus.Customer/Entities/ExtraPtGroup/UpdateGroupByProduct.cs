using System;

namespace Fanex.Tarus.Customer.Entities.ExtraPtGroup
{
    public class UpdateGroupByProduct
    {
        public UpdateGroupByProduct()
        {
            Group = new Group();
            Product = new Product();
        }

        public Group Group { get; set; }

        public Product Product { get; set; }

        public decimal NewExtraPt { get; set; }

        public decimal OldExtraPt { get; set; }

        public DateTime OldEffectiveDate { get; set; }

        public DateTime OldEndDate { get; set; }

        public DateTime NewEffectiveDate { get; set; }

        public DateTime NewEndDate { get; set; }

        public bool IsAllowDeleted { get; set; }
    }
}
