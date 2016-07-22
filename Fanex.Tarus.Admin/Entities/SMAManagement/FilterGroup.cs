using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Tarus.Admin.Entities.SMAManagement
{
    public class FilterGroup
    {
        public IEnumerable<SelectListItem> Sites { get; set; }

        public IEnumerable<SelectListItem> Currencies { get; set; }

        public IEnumerable<SelectListItem> Statuses { get; set; }

        public IEnumerable<SelectListItem> HeadlineTypes { get; set; }

        public IEnumerable<SelectListItem> Positions { get; set; }
    }
}