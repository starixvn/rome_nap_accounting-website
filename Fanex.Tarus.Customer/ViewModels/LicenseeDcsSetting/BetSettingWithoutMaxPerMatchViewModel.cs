using System.Collections.Generic;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.ViewModels.LicenseeDcsSetting
{
    public sealed class BetSettingWithoutMaxPerMatchViewModel : BaseSettingViewModel
    {
        public bool ApplyExistedMem { get; set; }

        public bool IsAgentOnly { get; set; }

        public IEnumerable<BetSettingWithoutMaxPerMatchEntity> BetSettings { get; set; }
    }
}
