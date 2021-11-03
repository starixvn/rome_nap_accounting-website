namespace Fanex.Tarus.Customer.Entities.InitializingCustomerSetting
{
    public class SettingInforModel
    {
        public const string ProductStatus = "Product Status";
        public const string Commission = "Commission";
        public const string PositionTaking = "Position Taking";
        public const string WinLossLimit = "Win Loss Limit";
        public const string LossLimit = "Loss Limit";
        public const string BetSetting = "Bet Setting";
        public const string StakeLevel = "Stake Level";

        public string SettingId { get => string.Concat(SettingType, "-", SportType, "-", BetType ); } 

        public int Id { get; set; }

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

        public SettingInforModel(int id, int settingType, string settingName, int sportType, int bettype, int defaultSportType, int defaultBettype, bool isLive)
        {
            Id = id;
            SettingType = settingType;
            SettingName = settingName;
            SportType = sportType;
            BetType = bettype;
            IsLive = isLive;
            switch (settingName)
            {
                case LossLimit:
                case StakeLevel:
                    IsDefault = true;
                    break;
                case Commission:
                    IsDefault = true;
                    DefaultValueSuper = "0.0001";
                    DefaultValueMaster = "0.0001";
                    DefaultValueAgent = "0.0001";
                    DefaultValueMember = "0.0001";
                    break;
                case ProductStatus:
                    IsDefault = true;
                    DefaultValueSuper = "1";
                    DefaultValueMaster = "1";
                    DefaultValueAgent = "1";
                    DefaultValueMember = "0";
                    break;
                default:
                    IsDefault = false;
                    CopyFromSportType = defaultSportType;
                    CopyFromBetType = defaultBettype;
                    break;
            }
        }

        public SettingInforModel()
        {
        }
    }
}
