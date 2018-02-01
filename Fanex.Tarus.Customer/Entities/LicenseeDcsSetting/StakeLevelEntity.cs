namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public class StakeLevelEntity
    {
        public StakeLevelEntity()
        {
        }

        public StakeLevelEntity(int sportType, int betType, int stakeLevelId, string stakeLevelName, bool isChecked)
        {
            SportType = sportType;
            BetType = betType;
            StakeLevelId = stakeLevelId;
            StakeLevelName = stakeLevelName;
            IsChecked = isChecked;
        }

        public int SportType { get; set; }

        public int BetType { get; set; }

        public int StakeLevelId { get; set; }

        public string StakeLevelName { get; set; }

        public bool IsChecked { get; set; }
    }
}
