namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public sealed class BetSetting
    {
        public int SportType { get; set; }

        public string SportTypeName { get; set; }

        public int BetType { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }
    }
}
