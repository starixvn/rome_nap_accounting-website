using System.Collections.Generic;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.ViewModels.LicenseeDcsSetting
{
    public sealed class GlobalStakeLevelViewModel : BaseSettingViewModel
    {
        public IEnumerable<GameTypeGlobalStakeLevel> GroupGlobalStakeLevels { get; set; }
    }
}
