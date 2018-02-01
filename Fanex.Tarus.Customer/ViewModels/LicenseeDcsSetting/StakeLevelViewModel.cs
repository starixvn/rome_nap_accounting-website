using System.Collections.Generic;
using System.Linq;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.ViewModels.LicenseeDcsSetting
{
    public sealed class StakeLevelViewModel : BaseSettingViewModel
    {
        public IEnumerable<SportBetTypeStakeLevel> GroupStakeLevels { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public int MaxStakeLevel
        {
            get
            {
                if (GroupStakeLevels == null || !GroupStakeLevels.Any())
                {
                    return 0;
                }

                return GroupStakeLevels.Select(g => g.StakeLevels.Count()).Max();
            }
        }
    }
}
