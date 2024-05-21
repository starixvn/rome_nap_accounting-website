using Fanex.Tarus.Infrastructure.Caching;

namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public class ChargeCommissionEntity
    {
        public int SportType { get; set; }

        public string SportTypeName => CacheRef.GetSportName(SportType);

        public int BetType { get; set; }

        public decimal Discount { get; set; }
    }
}
