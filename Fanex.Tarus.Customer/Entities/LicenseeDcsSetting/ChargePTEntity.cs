

using Fanex.Tarus.Infrastructure.Caching;

namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public sealed class ChargePTEntity
    {
        public ChargePTEntity()
        {
        }

        public int SportType { get; set; }

        public string SportTypeName => CacheRef.GetSportName(SportType);

        public decimal PT { get; set; }

        public decimal PTPercent
            => 0 <= PT && PT <= 100 ? PT / 100 : PT;
    }
}
