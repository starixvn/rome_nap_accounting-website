using Fanex.Tarus.Infrastructure.Caching;

namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public sealed class StakeLevelGroupEntity : StakeLevelEntity
    {
        public StakeLevelGroupEntity(int sportType, int betType, int stakeLevelId, string stakeLevelName, bool isChecked)
            : base(sportType, betType, stakeLevelId, stakeLevelName, isChecked)
        {
        }

        public StakeLevelGroupEntity(
            int stakeLevelGroupId,
            string stakeLevelGroupName,
            int betTypeRef,
            int sportType,
            int betType,
            int stakeLevelId,
            string stakeLevelName,
            bool isChecked) : base(sportType, betType, stakeLevelId, stakeLevelName, isChecked)
        {
            StakeLevelGroupId = stakeLevelGroupId;
            StakeLevelGroupName = stakeLevelGroupName;
            BetTypeRef = betTypeRef;
        }

        public int StakeLevelGroupId { get; }

        public string StakeLevelGroupName { get; }

        public int BetTypeRef { get; }

        public string BetTypeName
        {
            get
            {
                return CacheRef.GetBettype(BetType);
            }
        }
    }
}
