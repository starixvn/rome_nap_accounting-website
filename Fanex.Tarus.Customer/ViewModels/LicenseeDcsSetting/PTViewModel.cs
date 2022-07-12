using System.Collections.Generic;
using System.Linq;
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

            decimal step = 0.0005M;

            for (decimal i = 1; i >= 0; i -= step)
            {
                value = i.ToString("G4");

                yield return new SelectListItem { Text = value, Value = value, Selected = pt == i };
            }
        }
    }
}
