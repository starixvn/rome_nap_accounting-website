using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;

namespace Fanex.Tarus.Customer.ViewModels.LicenseeDcsSetting
{
    public sealed class PTViewModel : BaseSettingViewModel
    {
        public PTViewModel()
        {
            SuperPT = new List<PTEntity>();
        }

        public List<PTEntity> SuperPT { get; set; }

        public bool IsAgentOnly { get; set; }

        public IEnumerable<SelectListItem> PositionTakingList(decimal pt)
        {
            string value;

            for (decimal i = 1; i >= 0; i -= 0.005M)
            {
                value = i.ToString("G4");

                yield return new SelectListItem { Text = value, Value = value, Selected = pt == i };
            }
        }
    }
}
