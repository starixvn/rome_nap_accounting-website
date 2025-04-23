using System.Collections.Generic;

namespace Fanex.Tarus.BetList.Entities.BetList
{
    public class ProductEntity
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public bool Enabled { get; set; }
        public int GroupType { get; set; }
        public int CasinoType { get; set; }

        public int Group
        {
            get
            {
                var groupMapping = new Dictionary<int, int>
                {
                    { 38, 3 }, { 63, 3 },
                    { 42, 4 }, { 66, 4 },
                    { 65, 5 }, { 52, 5 }
                };

                if (groupMapping.TryGetValue(ProductID, out int group))
                {
                    return group;
                }

                return CasinoType;
            }
        }
    }
}
