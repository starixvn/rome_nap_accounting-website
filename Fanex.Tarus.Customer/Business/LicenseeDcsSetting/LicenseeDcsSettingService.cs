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
    internal class LicenseeDcsSettingService : ILicenseeDcsSettingService
    {
        private readonly ILicenseeDcsSettingDataAccess _settingDataAccess;

        public LicenseeDcsSettingService(ILicenseeDcsSettingDataAccess settingDataAccess = null)
        {
            _settingDataAccess = settingDataAccess ?? new LicenseeDcsSettingDataAccess();
        }

        public IEnumerable<string> GetSites()
            => _settingDataAccess.GetSites(new GetSiteCriteria());

        public IEnumerable<BaseModel> GetSupers(string site)
            => _settingDataAccess.GetSupers(new GetSuperCriteria(site));

        public IEnumerable<BaseModel> GetAgents(int superId)
            => _settingDataAccess.GetAgents(new GetAgentCriteria(superId));

        public IEnumerable<ProductSettingTypeEntity> GetProducts()
            => _settingDataAccess.GetProducts(new GetProductCriteria());

        public decimal GetPositionTaking(int productId, int superId)
        {
            decimal pt = _settingDataAccess.GetPositionTaking(
                new GetPositionTakingCriteria(productId, superId));

            return 0 <= pt && pt <= 100 ? pt / 100 : pt;
        }

        public IEnumerable<BetSettingEntity>[] GetBetSettings(int productId, int superId, int agentId)
            => _settingDataAccess.GetBetSettings(
                new GetBetSettingCriteria(productId, superId, agentId));

        public StatusEntity GetStatus(int productId, int custId)
            => _settingDataAccess.GetStatus(new GetStatusCriteria(productId, custId));

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

        public Result UpdatePositionTaking(
            int productId,
            int superId,
            decimal superPT,
            IEnumerable<int> agentIds,
            bool isAgentOnly,
            int userId,
            string ip)
        {
            Result result;
            IList<Result> errorResults = new List<Result>();

            foreach (var agentId in agentIds)
            {
                result = UpdatePositionTaking(productId, superId, superPT, agentId, isAgentOnly, userId, ip);

                if (result.ErrorCode != 0)
                {
                    errorResults.Add(result);
                }
            }

            return errorResults.Count == 0
                ? new Result(0, LicenseeDcsSettingLabels.UpdatePTSuccessfully)
                : new Result(1, string.Join("<br/>", errorResults.Select(err => err.ErrorMessage)));
        }

        public Result UpdateBetSetting(
            int superId,
            bool applyExistedMem,
            BetSettingEntity betSetting,
            IEnumerable<int> agentIds,
            bool isAgentOnly,
            int userId,
            string ip)
        {
            Result result;
            IList<Result> errorResults = new List<Result>();

            foreach (var agentId in agentIds)
            {
                result = UpdateBetSetting(superId, applyExistedMem, betSetting, agentId, isAgentOnly, userId, ip);

                if (result.ErrorCode != 0)
                {
                    errorResults.Add(result);
                }
            }

            return errorResults.Count == 0
                ? new Result(0, LicenseeDcsSettingLabels.UpdateBetSettingSuccessfully)
                : new Result(1, string.Join("<br/>", errorResults.Select(err => err.ErrorMessage)));
        }

        public Result UpdateStatus(
            int productId,
            int superId,
            StatusEntity status,
            IEnumerable<int> agentIds,
            bool isAgentOnly,
            int userId,
            string ip)
        {
            if (agentIds.Any())
            {
                Result result;
                IList<Result> errorResults = new List<Result>();

                foreach (var agentId in agentIds)
                {
                    result = UpdateStatus(productId, agentId, status, isAgentOnly, userId, ip);

                    if (result.ErrorCode != 0)
                    {
                        errorResults.Add(result);
                    }
                }

                return errorResults.Count == 0
                    ? new Result(0, LicenseeDcsSettingLabels.UpdateStatusSuccessfully)
                    : new Result(1, string.Join("<br/>", errorResults.Select(err => err.ErrorMessage)));
            }

            return UpdateStatus(productId, superId, status, isAgentOnly, userId, ip);
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

            return result;
        }

        public Result UpdateStakeLevels(
            int productId,
            int superId,
            StakeLevelEntity[] stakeLevels,
            IEnumerable<int> agentIds,
            bool isAgentOnly,
            int userId,
            string ip)
        {
            if (agentIds.Any())
            {
                Result result;
                IList<Result> errorResults = new List<Result>();

                foreach (var agentId in agentIds)
                {
                    result = UpdateStakeLevels(productId, agentId, stakeLevels, isAgentOnly, userId, ip);

                    if (result.ErrorCode != 0)
                    {
                        errorResults.Add(result);
                    }
                }

                return errorResults.Count == 0
                    ? new Result(0, LicenseeDcsSettingLabels.UpdateStakeLevelSuccessfully)
                    : new Result(1, string.Join("<br/>", errorResults.Select(err => err.ErrorMessage)));
            }

            return UpdateStakeLevels(productId, superId, stakeLevels, isAgentOnly, userId, ip);
        }

        private Result UpdatePositionTaking(
            int productId,
            int superId,
            decimal superPT,
            int agentId,
            bool isAgentOnly,
            int userId,
            string ip)
        {
            if (0 <= superPT && superPT <= 1)
            {
                superPT *= 100;
            }

            ICriteria criteria = new UpdatePositionTakingCriteria(productId, superId, superPT, agentId, isAgentOnly, userId, ip);

            return _settingDataAccess.ExecuteNonQuery(criteria);
        }

        private Result UpdateBetSetting(
            int superId,
            bool applyExistedMem,
            BetSettingEntity betSetting,
            int agentId,
            bool isAgentOnly,
            int userId,
            string ip)
        {
            ICriteria criteria = new UpdateBetSettingCriteria(superId, applyExistedMem, betSetting, agentId, isAgentOnly, userId, ip);

            return _settingDataAccess.ExecuteNonQuery(criteria);
        }

        private Result UpdateStatus(
            int productId,
            int custId,
            StatusEntity status,
            bool isAgentOnly,
            int userId,
            string ip)
        {
            ICriteria criteria = new UpdateStatusCriteria(
                productId,
                custId,
                status,
                isAgentOnly,
                userId,
                ip);
            var result = _settingDataAccess.ExecuteNonQuery(criteria);

            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = LicenseeDcsSettingLabels.UpdateStatusSuccessfully;
            }

            return result;
        }

        private Result UpdateStakeLevels(
            int productId,
            int custId,
            StakeLevelEntity[] stakeLevels,
            bool isAgentOnly,
            int userId,
            string ip)
        {
            ICriteria criteria = new UpdateStakeLevelCriteria(
                productId,
                custId,
                stakeLevels,
                isAgentOnly,
                userId,
                ip);
            var result = _settingDataAccess.ExecuteNonQuery(criteria);

            if (result.ErrorCode == 0)
            {
                result.ErrorMessage = LicenseeDcsSettingLabels.UpdateStakeLevelSuccessfully;
            }

            return result;
        }
    }
}
