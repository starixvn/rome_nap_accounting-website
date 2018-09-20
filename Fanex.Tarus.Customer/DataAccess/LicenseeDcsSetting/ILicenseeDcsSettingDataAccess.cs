using System.Collections.Generic;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.DataAccess.LicenseeDcsSetting
{
    internal interface ILicenseeDcsSettingDataAccess
    {
        IEnumerable<string> GetSites(ICriteria criteria);

        IEnumerable<BaseModel> GetSupers(ICriteria criteria);

        IEnumerable<BaseModel> GetAgents(ICriteria criteria);

        IEnumerable<ProductSettingTypeEntity> GetProducts(ICriteria criteria);

        decimal GetPositionTaking(ICriteria criteria);

        IEnumerable<BetSettingEntity>[] GetBetSettings(ICriteria criteria);

        StatusEntity GetStatus(ICriteria criteria);

        IEnumerable<GlobalStakeLevelEntity> GetGlobalStakeLevels(ICriteria criteria);

        IEnumerable<StakeLevelEntity> GetStakeLevels(GetStakeLevelCriteria criteria);

        IEnumerable<StakeLevelGroupEntity> GetStakeLevelGroups(GetStakeLevelGroupCriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);
    }
}
