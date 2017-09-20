using System.Collections.Generic;
using System.Linq;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting;
using Fanex.Tarus.Customer.DataAccess.LicenseeDcsSetting;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;
using Fanex.Tarus.Customer.Enums;
using Fanex.Tarus.Customer.Resources;
using Fanex.Tarus.Infrastructure.Caching;

namespace Fanex.Tarus.Customer.Business.LicenseeDcsSetting
{
    public class LicenseeDcsSettingService : ILicenseeDcsSettingService
    {
        private static readonly byte[] FunctionIds = new byte[]
        {
            (byte)SettingType.PositionTaking,
            (byte)SettingType.BetSetting,
            (byte)SettingType.ProductStatus
        };

        private readonly ILicenseeDcsSettingDataAccess _settingDataAccess;

        public LicenseeDcsSettingService(ILicenseeDcsSettingDataAccess settingDataAccess = null)
        {
            _settingDataAccess = settingDataAccess ?? new LicenseeDcsSettingDataAccess();
        }

        public IEnumerable<string> GetSites()
        {
            return _settingDataAccess.GetSites(new GetSiteCriteria());
        }

        public IEnumerable<BaseModel> GetSupers(string site)
        {
            return _settingDataAccess.GetSupers(new GetSuperCriteria(site));
        }

        public IEnumerable<ProductSettingType> GetProducts()
        {
            var rawdata = _settingDataAccess.GetProducts(new GetProductCriteria());

            if (rawdata.Any())
            {
                rawdata = rawdata.Where(p => FunctionIds.Contains(p.SettingType));
            }

            return rawdata;
        }

        public decimal GetPositionTaking(int productId, int superId)
        {
            decimal pt = _settingDataAccess.GetPositionTaking(
                new GetPositionTakingCriteria(productId, superId));

            return 0 <= pt && pt <= 100 ? pt / 100 : pt;
        }

        public IEnumerable<BetSetting> GetBetSettings(int productId, int superId)
        {
            var betSettings = _settingDataAccess.GetBetSettings(
                new GetBetSettingCriteria(productId, superId));

            foreach (var item in betSettings)
            {
                item.SportTypeName = CacheRef.GetSportName(item.SportType);

                yield return item;
            }
        }

        public StatusEntity GetStatus(int productId, int superId)
        {
            return _settingDataAccess.GetStatus(new GetStatusCriteria(productId, superId));
        }

        public Result UpdatePositionTaking(int productId, int superId, decimal superPT, int userId, string ip)
        {
            if (0 <= superPT && superPT <= 1)
            {
                superPT *= 100;
            }

            ICriteria criteria = new UpdatePositionTakingCriteria(productId, superId, superPT, userId, ip);
            var result = _settingDataAccess.UpdatePositionTaking(criteria);

            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = LicenseeDcsSettingLabels.UpdatePTSuccessfully;
            }
            else if (result.ErrorCode == 400 && string.IsNullOrEmpty(result.ErrorMessage))
            {
                result.ErrorMessage = LicenseeDcsSettingLabels.BadRequest;
            }

            return result;
        }

        public Result UpdateBetSetting(int superId, bool applyExistedMem, BetSetting betSetting, int userId, string ip)
        {
            ICriteria criteria = new UpdateBetSettingCriteria(superId, applyExistedMem, betSetting, userId, ip);
            var result = _settingDataAccess.UpdateBetSetting(criteria);

            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = LicenseeDcsSettingLabels.UpdateBetSettingSuccessfully;
            }
            else if (result.ErrorCode == 400 && string.IsNullOrEmpty(result.ErrorMessage))
            {
                result.ErrorMessage = LicenseeDcsSettingLabels.BadRequest;
            }

            return result;
        }

        public Result UpdateStatus(
            int productId,
            int superId,
            bool isDisabled,
            bool isDisabledDownline,
            int sportType,
            int betType,
            byte bitmapOrder,
            int userId,
            string ip)
        {
            ICriteria criteria = new UpdateStatusCriteria(
                productId,
                superId,
                isDisabled,
                isDisabledDownline,
                sportType,
                betType,
                bitmapOrder,
                userId,
                ip);
            var result = _settingDataAccess.UpdateStatus(criteria);

            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = LicenseeDcsSettingLabels.UpdateStatusSuccessfully;
            }
            else if (result.ErrorCode == 400 && string.IsNullOrEmpty(result.ErrorMessage))
            {
                result.ErrorMessage = LicenseeDcsSettingLabels.BadRequest;
            }

            return result;
        }
    }
}
