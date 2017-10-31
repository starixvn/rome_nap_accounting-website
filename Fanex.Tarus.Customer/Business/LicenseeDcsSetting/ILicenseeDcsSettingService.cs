using System.Collections.Generic;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.Business.LicenseeDcsSetting
{
    public interface ILicenseeDcsSettingService
    {
        IEnumerable<string> GetSites();

        IEnumerable<BaseModel> GetSupers(string site);

        IEnumerable<ProductSettingType> GetProducts();

        decimal GetPositionTaking(int productId, int superId);

        IEnumerable<BetSetting> GetBetSettings(int productId, int superId);

        StatusEntity GetStatus(int productId, int superId);

        Result UpdatePositionTaking(int productId, int superId, decimal superPT, int userId, string ip);

        Result UpdateBetSetting(int superId, bool applyExistedMem, BetSetting betSetting, int userId, string ip);

        Result UpdateStatus(
            int productId,
            int superId,
            bool isDisabled,
            bool isDisabledDownline,
            int sportType,
            int betType,
            byte bitmapOrder,
            int userId,
            string ip);
    }
}
