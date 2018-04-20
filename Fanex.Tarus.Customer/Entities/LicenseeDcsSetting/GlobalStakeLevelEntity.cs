namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public sealed class GlobalStakeLevelEntity
    {
        public GlobalStakeLevelEntity()
        {
        }

        public GlobalStakeLevelEntity(
            int gameTypeId,
            int stakeLevelId,
            decimal minStake,
            decimal maxStake,
            string minLabel,
            string maxLabel)
        {
            GameTypeId = gameTypeId;
            StakeLevelId = stakeLevelId;
            MinStake = minStake;
            MaxStake = maxStake;
            MinLabel = minLabel;
            MaxLabel = maxLabel;
        }

        public int GameTypeId { get; set; }

        public int StakeLevelId { get; set; }

        public decimal MinStake { get; set; }

        public decimal MaxStake { get; set; }

        public string MinLabel { get; set; }

        public string MaxLabel { get; set; }
    }
}
