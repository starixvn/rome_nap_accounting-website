using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.ViewModels.LicenseeDcsSetting
{
    public sealed class PTViewModel : BaseSettingViewModel
    {
        private readonly int[] ListporductSupportFourDecimal = new int[] { 16 };

        public PTViewModel()
        {
            SuperPT = new List<PTEntity>();
        }

        public List<PTEntity> SuperPT { get; set; }

        public bool IsAgentOnly { get; set; }

        public IEnumerable<SelectListItem> PositionTakingList(decimal pt)
        {
            string value;

            decimal step = ListporductSupportFourDecimal.Contains(ProductId) ? 0.0005M : 0.005M;

            for (decimal i = 1; i >= 0; i -= step)
            {
                value = i.ToString("G4");

                yield return new SelectListItem { Text = value, Value = value, Selected = pt == i };
            }
        }
    }
}
