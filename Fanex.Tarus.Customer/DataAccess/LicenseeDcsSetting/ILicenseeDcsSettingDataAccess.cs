using System.Collections.Generic;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.DataAccess.LicenseeDcsSetting
{
    public interface ILicenseeDcsSettingDataAccess
    {
        IEnumerable<string> GetSites(ICriteria criteria);

        IEnumerable<BaseModel> GetSupers(ICriteria criteria);

        IEnumerable<ProductSettingType> GetProducts(ICriteria criteria);

        decimal GetPositionTaking(ICriteria criteria);

        IEnumerable<BetSetting> GetBetSettings(ICriteria criteria);

        StatusEntity GetStatus(ICriteria criteria);

        Result UpdatePositionTaking(ICriteria criteria);

        Result UpdateBetSetting(ICriteria criteria);

        Result UpdateStatus(ICriteria criteria);
    }
}
