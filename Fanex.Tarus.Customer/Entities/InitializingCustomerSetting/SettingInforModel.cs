using System.Collections.Generic;
using System.Web.Mvc;

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

        private const string FormatDisplay = "[{0}] {1}";
        private const string IdKey = "{0}-{1}";


        private readonly Dictionary<string, int> SettingtTypeOrders = new Dictionary<string, int>
        {
            {"Product Status", 0 },
            {"Commission", 1 },
            {"Bet Setting", 2 },
            {"Position Taking", 3 },
            {"Win Loss Limit", 4 },
            {"Loss Limit", 5 },
            {"Stake Level", 6 }
        };

        public string SettingId { get => string.Concat(SettingType, "-", SportType, "-", BetType ); }

        public int SettingOrder { get { return SettingtTypeOrders.ContainsKey(SettingName) ? SettingtTypeOrders[SettingName] : 999; } }

        public int Id { get; set; }

        public int ConfigId { get; set; }

        public int SportType { get; set; }

        public string SportName { get; set; }

        public int BetType { get; set; }

        public string BetTypeName { get; set; }

        public bool IsLive { get; set; }

        public int SettingType { get; set; }

        public bool IsDefault { get; set; }

        public bool IsCopy { get; set; }

        public int CopyFromSportType { get; set; }

        public int CopyFromBetType { get; set; }

        public string DefaultValueSuper { get; set; }

        public string DefaultValueMaster { get; set; }

        public string DefaultValueAgent { get; set; }

        public string DefaultValueMember { get; set; }

        public string SpecialRequirement { get; set; }

        public string SettingName { get; set; }

        public decimal MaxDiscount { get; set; }

        public decimal IntervalValue { get; set; }

        public List<SelectListItem> SportTypeOptions { get; set; }

        public List<SelectListItem> BetTypeOptions { get; set; }

        public List<SelectListItem> CommissionOptions { get => BuildCommisionOptions(); }

        public string DisplaySportName { get => string.Format(FormatDisplay, SportType, SportName); }

        public string DisplayBettypeName { get => string.Format(FormatDisplay, BetType, BetTypeName); }

        public string CheckBoxCopy { get => IsCopy ? "checked" : string.Empty; }

        public string CheckBoxDefault { get => IsDefault ? "checked" : string.Empty; }

        public string CheckBoxAutoReset { get => SpecialRequirement == "1" ? "checked" : string.Empty; }

        public string DisableCopy { get => IsCopy ? string.Empty : "disabled"; }

        public string DisableDefault { get => IsDefault ? string.Empty : "disabled"; }

        public string TargetSportType { get => string.Format(IdKey, "sport", SettingId); }

        public string TargetBetType { get => string.Format(IdKey, "bettype", SettingId); }

        public bool IsRequiredHeadline { get => SettingName == ProductStatus; }

        public SettingInforModel(int id, ProductDcsModel setting)
        {
            Id = id;
            SettingType = setting.SettingTypeId;
            SettingName = setting.SettingTypeName;
            SportType = setting.SportId;
            SportName = setting.SportName;
            BetType = setting.BettypeId;
            BetTypeName = setting.BettypeName;
            IsLive = setting.IsLive;
            MaxDiscount = setting.MaxDiscount;
            IntervalValue = setting.IntervalValue;
            switch (setting.SettingTypeName)
            {
                case Commission:
                    IsDefault = false;
                    IsCopy = false;
                    DefaultValueSuper = "0";
                    DefaultValueMaster = "0";
                    DefaultValueAgent = "0";
                    DefaultValueMember = "0";
                    break;
                case ProductStatus:
                    IsDefault = false;
                    IsCopy = false;
                    SpecialRequirement = string.Empty;
                    DefaultValueSuper = "1";
                    DefaultValueMaster = "1";
                    DefaultValueAgent = "1";
                    DefaultValueMember = "1";
                    break;
                case WinLossLimit:
                    IsDefault = false;
                    IsCopy = false;
                    DefaultValueSuper = "0";
                    break;
                case PositionTaking:
                    DefaultValueSuper = "1";
                    break;
                default:
                    IsDefault = false;
                    IsCopy = false;
                    break;
            }
        }

        public SettingInforModel()
        {
        }

        private List<SelectListItem> BuildCommisionOptions()
        {
            var result = new List<SelectListItem>();
            if (MaxDiscount == 0 || IntervalValue == 0) return result;

            for (decimal i = 0; i <= MaxDiscount; i = i + IntervalValue)
            {
                result.Add(new SelectListItem { Text = i.ToString("0.####"), Value = i.ToString("0.####") });
            }

            return result;
        }
    }
}
