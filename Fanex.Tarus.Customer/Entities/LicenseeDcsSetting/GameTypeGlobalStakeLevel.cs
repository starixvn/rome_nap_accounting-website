using System.Collections.Generic;
using Fanex.Tarus.Infrastructure.Caching;

namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public sealed class GameTypeGlobalStakeLevel
    {
        public GameTypeGlobalStakeLevel(int gameTypeId, IEnumerable<GlobalStakeLevelEntity> globalStakeLevels)
        {
            GameTypeId = gameTypeId;
            GlobalStakeLevels = globalStakeLevels;
        }

        public int GameTypeId { get; }

        public string GameTypeName
        {
            get
            {
                return CacheRef.GetBettype(GameTypeId);
            }
        }

        public IEnumerable<GlobalStakeLevelEntity> GlobalStakeLevels { get; }
    }
}
