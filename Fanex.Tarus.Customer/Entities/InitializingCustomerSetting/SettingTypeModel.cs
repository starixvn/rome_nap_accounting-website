namespace Fanex.Tarus.Customer.Entities.InitializingCustomerSetting
{
    public class SettingTypeModel
    {
        public SettingTypeModel(int settingId, string settingTypeName)
        {
            SettingTypeId = settingId;
            SettingTypeName = settingTypeName;
        }

        public int SettingTypeId { get; set; }

        public string SettingTypeName { get; set; }
    }
}
