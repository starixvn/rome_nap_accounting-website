namespace Fanex.Tarus.Customer.Entities.InitializingCustomerSetting
{
    public class BetSettingModel
    {
        public int RowPos { get; set; }

        public int ConfigId { get; set; }

        public int Currency { get; set; }

        public string CurrencyName { get; set; }

        public string CurrencyCode { get; set; }

        public int? SportTypeId { get; set; }

        public string SportTypeName { get; set; }

        public int? BetTypeId { get; set; }

        public string BetTypeName { get; set; }

        public decimal? MinBet { get; set; }

        public decimal? MaxBet { get; set; }

        public decimal? MaxPerMatch { get; set; }

        public decimal? MaxPerNumber { get; set; }

        public decimal? MaxPayoutPerMatch { get; set; }

        public int SettingType { get; set; }
    }
}
