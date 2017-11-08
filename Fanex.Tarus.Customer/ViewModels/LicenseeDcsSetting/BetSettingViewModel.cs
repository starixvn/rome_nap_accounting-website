using System.Collections.Generic;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.ViewModels.LicenseeDcsSetting
{
    public sealed class BetSettingViewModel : BaseSettingViewModel
    {
        public bool ApplyExistedMem { get; set; }

        public IEnumerable<BetSettingEntity> BetSettings { get; set; }
    }
}
