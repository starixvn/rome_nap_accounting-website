namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public sealed class ProductSettingTypeEntity
    {
        public ProductSettingTypeEntity(int productId, string productName, byte settingType)
        {
            ProductId = productId;
            ProductName = productName;
            SettingType = settingType;
        }

        public int ProductId { get; }

        public string ProductName { get; }

        public byte SettingType { get; }
    }
}
