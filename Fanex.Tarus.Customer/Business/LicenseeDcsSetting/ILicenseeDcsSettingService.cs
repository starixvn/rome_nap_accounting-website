using System.Collections.Generic;
using Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.Business.LicenseeDcsSetting
{
    internal interface ILicenseeDcsSettingService
    {
        IEnumerable<string> GetSites();

        IEnumerable<BaseModel> GetSupers(string site);
        
        IEnumerable<BaseModelExtra> GetAgents(string superIds);
        
        IEnumerable<ProductSettingTypeEntity> GetProducts();

        IEnumerable<PTEntity> GetPositionTaking(int productId, int superId);

        IEnumerable<ChargePTEntity> GetChargePositionTaking(int productId, int superId);

        IEnumerable<ChargeCommissionEntity> GetChargeCommission(int productId, int superId);

        IEnumerable<BetSettingEntity> GetBetSettings(int productId, int superId, int agentId);

        IEnumerable<BetSettingWithMaxPayoutPerMatchEntity> GetBetSettingsWithMaxPayoutPerMatch(int productId, int superId, int agentId);

        IEnumerable<BetSettingWithMaxPerBallEntity> GetBetSettingsWithMaxPerBall(int productId, int superId, int agentId);

        StatusEntity GetStatus(int productId, int custId);

        IEnumerable<GameTypeGlobalStakeLevel> GetGameTypeGlobalStakeLevels(int productId, int superId);

        IEnumerable<SportBetTypeStakeLevel> GetSportBetTypeStakeLevels(GetStakeLevelCriteria criteria);

        IEnumerable<SportBetTypeStakeLevel> GetSportBetTypeStakeLevelGroups(GetStakeLevelGroupCriteria criteria);

        Result UpdatePositionTaking(
            int productId,
            IEnumerable<PTEntity> superPT,
            IEnumerable<int> agentIds,
            IEnumerable<AgentsExtraInfo> agentsExtraInfo,
            bool isAgentOnly,
            int userId,
            string ip);

        Result UpdateChargePositionTaking(
            int productId,
            IEnumerable<ChargePTEntity> PT,
            IEnumerable<int> superIds,
            int userId,
            string ip);

        Result UpdateChargeCommission(
            int productId,
            IEnumerable<ChargeCommissionEntity> commission,
            IEnumerable<int> superIds,
            int userId,
            string ip);

        Result UpdateBetSetting(
            int superId,
            bool applyExistedMem,
            BetSettingEntity betSetting,
            IEnumerable<int> agentIds,
            bool isAgentOnly,
            int userId,
            string ip);

        Result UpdateBetSettingWithMaxPayoutPerMatch(
            int superId,
            bool applyExistedMem,
            BetSettingWithMaxPayoutPerMatchEntity betSetting,
            IEnumerable<int> agentIds,
            bool isAgentOnly,
            int userId,
            string ip);

        Result UpdateBetSettingWithMaxPerBall(
            int superId,
            bool applyExistedMem,
            BetSettingWithMaxPerBallEntity betSetting,
            IEnumerable<int> agentIds,
            bool isAgentOnly,
            int userId,
            string ip);

        Result UpdateStatus(
            int productId,
            int superId,
            StatusEntity status,
            IEnumerable<int> agentIds,
            bool isAgentOnly,
            int userId,
            string ip);

        Result UpdateGlobalStakeLevels(int superId, GlobalStakeLevelEntity[] globalStakeLevels, int userId, string ip);

        Result UpdateStakeLevels(
            int productId,
            int superId,
            StakeLevelEntity[] stakeLevels,
            IEnumerable<int> agentIds,
            bool isAgentOnly,
            int userId,
            string ip);
        
    }
}
