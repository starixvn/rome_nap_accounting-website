using Fanex.NAP.Framework.Data;
using Sunplus.Accounting.Core.Configurations;

namespace Fanex.Tarus.Customer.Criteria.LicenseeDcsSetting
{
    internal sealed class GetProductCriteria : CriteriaBase
    {
        public byte SettingStatus => WebSettings.SettingStatus;

        public override string GetSettingKey()
            => "Acc_Dep_SelDCSProduct";
    }
}
