namespace Fanex.Tarus.Customer.Entities.ManageSuperSettings
{
    using System.Collections.Generic;

    public class ProductGroupModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<GroupTypeModel> GroupTypes { get; set; }
    }
}
