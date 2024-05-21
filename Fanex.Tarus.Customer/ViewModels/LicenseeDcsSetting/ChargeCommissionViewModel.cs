using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.ViewModels.LicenseeDcsSetting
{
    public sealed class ChargeCommissionViewModel : BaseSettingViewModel
    {
        public ChargeCommissionViewModel()
        {
            Commission = new List<ChargeCommissionEntity>();
        }

        public IEnumerable<ChargeCommissionEntity> Commission { get; set; }

        public IEnumerable<SelectListItem> CommissionList(decimal discount)
        {
            decimal step = 0.005M;
            for (decimal i = 0M; i <= 0.07M; i += i == 0M ? 0.02M : step)
            {
                yield return new SelectListItem { Text = string.Format("{0:P2}", i), Value = i.ToString("G4"), Selected = discount == i };
            }
        }
    }
}
