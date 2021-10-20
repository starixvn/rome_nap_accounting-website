namespace Fanex.Tarus.Customer.Entities.InitializingCustomerSetting
{
    public class SettingInforModel
    {
        public int ConfigId { get; set; }

        public int SportType { get; set; }

        public int BetType { get; set; }

        public bool IsLive { get; set; }

        public int SettingType { get; set; }

        public bool IsDefault { get; set; }

        public int CopyFromSportType { get; set; }

        public int CopyFromBetType { get; set; }

        public string DefaultValueSuper { get; set; }

        public string DefaultValueMaster { get; set; }

        public string DefaultValueAgent { get; set; }

        public string DefaultValueMember { get; set; }

        public string SpecialRequirement { get; set; }

        public string SettingName { get; set; }
    }
}
