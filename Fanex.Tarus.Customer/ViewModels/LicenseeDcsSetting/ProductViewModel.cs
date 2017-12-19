using System.Collections.Generic;
using System.Linq;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;
using Fanex.Tarus.Customer.Enums;
using Fanex.Tarus.Customer.Resources;

namespace Fanex.Tarus.Customer.ViewModels.LicenseeDcsSetting
{
    public sealed class ProductViewModel
    {
        public ProductViewModel(
            int productId,
            string productName,
            IEnumerable<ProductSettingTypeEntity> settings)
        {
            Id = productId;
            Name = productName;
            SettingTypes = settings.Select(p => new BaseModel
            {
                Id = p.SettingType,
                Name = GetFunctionName(p.SettingType)
            });
        }

        public int Id { get; }

        public string Name { get; }

        public IEnumerable<BaseModel> SettingTypes { get; }

        private static string GetFunctionName(byte id)
        {
            SettingType type = (SettingType)id;

            switch (type)
            {
                case SettingType.GlobalBetLimit:
                    return LicenseeDcsSettingLabels.GlobalBetLimit;
                case SettingType.PositionTaking:
                    return LicenseeDcsSettingLabels.PT;
                case SettingType.BetSetting:
                    return LicenseeDcsSettingLabels.BetSetting;
                case SettingType.ProductStatus:
                    return LicenseeDcsSettingLabels.Status;
                case SettingType.StakeLevel:
                case SettingType.StakeLevelGroup:
                    return LicenseeDcsSettingLabels.BetLimit;
                default:
                    return id.ToString();
            }
        }
    }
}
