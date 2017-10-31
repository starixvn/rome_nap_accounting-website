using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Entities.LicenseeDcsSetting
{
    public sealed class BetSettingViewModel : BaseSettingViewModel
    {
        public bool ApplyExistedMem { get; set; }

        public IEnumerable<BetSetting> BetSettings { get; set; }
    }
}
