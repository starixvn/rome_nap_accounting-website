using System.Collections.Generic;
using Fanex.Tarus.Customer.Resources;
using Fanex.Tarus.Infrastructure.Caching;

namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public sealed class SportBetTypeStakeLevel
    {
        public SportBetTypeStakeLevel(int sportType, int betType, IEnumerable<StakeLevelEntity> stakeLevels)
        {
            SportType = sportType;
            BetType = betType;
            StakeLevels = stakeLevels;
        }

        public int SportType { get; }

        public int BetType { get; }

        public string BetTypeName
        {
            get
            {
                var betTypeName = CacheRef.GetBettype(BetType);

                if (BetType == 2401)
                {
                    betTypeName += ", " + LicenseeDcsSettingLabels.ExtraBaccaratBetType;
                }
                else if (BetType == 2402)
                {
                    betTypeName += " " + LicenseeDcsSettingLabels.ExtraVIPBaccaratBetType;
                }

                return betTypeName;
            }
        }

        public IEnumerable<StakeLevelEntity> StakeLevels { get; }
    }
}
