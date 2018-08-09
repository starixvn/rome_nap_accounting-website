using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Tarus.Customer.ViewModels.LicenseeDcsSetting
{
    public sealed class PTViewModel : BaseSettingViewModel
    {
        public decimal SuperPT { get; set; }

        public bool IsAgentOnly { get; set; }

        public IEnumerable<SelectListItem> PositionTakingList
        {
            get
            {
                string value;

                for (decimal i = 1; i >= 0; i -= 0.005M)
                {
                    value = i.ToString("G4");

                    yield return new SelectListItem { Text = value, Value = value, Selected = SuperPT == i };
                }
            }
        }
    }
}
