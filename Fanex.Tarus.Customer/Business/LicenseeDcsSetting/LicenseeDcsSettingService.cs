using System.Collections.Generic;
using System.Linq;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting;
using Fanex.Tarus.Customer.DataAccess.LicenseeDcsSetting;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;
using Fanex.Tarus.Customer.Resources;

namespace Fanex.Tarus.Customer.Business.LicenseeDcsSetting
{
    public class LicenseeDcsSettingService : ILicenseeDcsSettingService
    {
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

        public IEnumerable<ProductSettingTypeEntity> GetProducts()
        {
            return _settingDataAccess.GetProducts(new GetProductCriteria());
        }

        public decimal GetPositionTaking(int productId, int superId)
        {
            decimal pt = _settingDataAccess.GetPositionTaking(
                new GetPositionTakingCriteria(productId, superId));

            return 0 <= pt && pt <= 100 ? pt / 100 : pt;
        }

        public IEnumerable<BetSettingEntity> GetBetSettings(int productId, int superId)
        {
            return _settingDataAccess.GetBetSettings(
                new GetBetSettingCriteria(productId, superId));
        }

        public StatusEntity GetStatus(int productId, int superId)
        {
            return _settingDataAccess.GetStatus(new GetStatusCriteria(productId, superId));
        }

        public IEnumerable<GameTypeGlobalStakeLevel> GetGameTypeGlobalStakeLevels(int productId, int superId)
        {
            var rawData = _settingDataAccess.GetGlobalStakeLevels(new GetGlobalStakeLevelCriteria(productId, superId));

            if (rawData.Any())
            {
                var groupRawData = rawData.ToLookup(kvp => kvp.GameTypeId, kvp => kvp);

                foreach (var item in groupRawData)
                {
                    yield return new GameTypeGlobalStakeLevel(item.Key, item);
                }
            }
        }

        public IEnumerable<SportBetTypeStakeLevel> GetSportBetTypeStakeLevels(GetStakeLevelCriteria criteria)
        {
            var rawData = _settingDataAccess.GetStakeLevels(criteria);

            if (criteria.ErrCode != 0)
            {
                return Enumerable.Empty<SportBetTypeStakeLevel>();
            }

            IList<SportBetTypeStakeLevel> groupStakeLevels = new List<SportBetTypeStakeLevel>();
            var groupRawData = rawData.ToLookup(kvp => new { kvp.SportType, kvp.BetType }, kvp => kvp);

            foreach (var item in groupRawData)
            {
                groupStakeLevels.Add(new SportBetTypeStakeLevel(item.Key.SportType, item.Key.BetType, item));
            }

            return groupStakeLevels;
        }

        public IEnumerable<SportBetTypeStakeLevel> GetSportBetTypeStakeLevelGroups(GetStakeLevelGroupCriteria criteria)
        {
            var rawData = _settingDataAccess.GetStakeLevelGroups(criteria);

            if (criteria.ErrCode != 0)
            {
                return Enumerable.Empty<SportBetTypeStakeLevel>();
            }

            IList<SportBetTypeStakeLevel> groupStakeLevels = new List<SportBetTypeStakeLevel>();
            var groupRawData = rawData.ToLookup(kvp => new { kvp.SportType, kvp.BetType }, kvp => kvp);

            foreach (var item in groupRawData)
            {
                groupStakeLevels.Add(new SportBetTypeStakeLevel(item.Key.SportType, item.Key.BetType, item.OrderBy(sl => sl.StakeLevelGroupName)));
            }

            return groupStakeLevels;
        }

        public Result UpdatePositionTaking(int productId, int superId, decimal superPT, int userId, string ip)
        {
            if (0 <= superPT && superPT <= 1)
            {
                superPT *= 100;
            }

            ICriteria criteria = new UpdatePositionTakingCriteria(productId, superId, superPT, userId, ip);
            var result = _settingDataAccess.ExecuteNonQuery(criteria);

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

        public Result UpdateBetSetting(int superId, bool applyExistedMem, BetSettingEntity betSetting, int userId, string ip)
        {
            ICriteria criteria = new UpdateBetSettingCriteria(superId, applyExistedMem, betSetting, userId, ip);
            var result = _settingDataAccess.ExecuteNonQuery(criteria);

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

        public Result UpdateStatus(int productId, int superId, StatusEntity status, int userId, string ip)
        {
            ICriteria criteria = new UpdateStatusCriteria(
                productId,
                superId,
                status,
                userId,
                ip);
            var result = _settingDataAccess.ExecuteNonQuery(criteria);

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

        public Result UpdateGlobalStakeLevels(int superId, GlobalStakeLevelEntity[] globalStakeLevels, int userId, string ip)
        {
            ICriteria criteria = new UpdateGlobalStakeLevelCriteria(
                superId,
                globalStakeLevels,
                userId,
                ip);
            var result = _settingDataAccess.ExecuteNonQuery(criteria);

            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = LicenseeDcsSettingLabels.UpdateGlobalStakeLevelSuccessfully;
            }
            else if (result.ErrorCode == 400 && string.IsNullOrEmpty(result.ErrorMessage))
            {
                result.ErrorMessage = LicenseeDcsSettingLabels.BadRequest;
            }

            return result;
        }

        public Result UpdateStakeLevels(int productId, int superId, StakeLevelEntity[] stakeLevels, int userId, string ip)
        {
            ICriteria criteria = new UpdateStakeLevelCriteria(
                productId,
                superId,
                stakeLevels,
                userId,
                ip);
            var result = _settingDataAccess.ExecuteNonQuery(criteria);

            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = LicenseeDcsSettingLabels.UpdateStakeLevelSuccessfully;
            }
            else if (result.ErrorCode == 400 && string.IsNullOrEmpty(result.ErrorMessage))
            {
                result.ErrorMessage = LicenseeDcsSettingLabels.BadRequest;
            }

            return result;
        }
    }
}
