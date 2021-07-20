using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Fanex.Tarus.Customer.ViewModels.LicenseeOddsGroup
{
    public class AgentOddsGroupEntity
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public string OperatorId { get; set; }

        public string OddsGroup { get; set; }

        public List<SelectListItem> OddsGroupSelectListItem => new List<SelectListItem>() {
            new SelectListItem(){ Text = "a", Value = "a", Selected = "a".Equals(OddsGroup) },
            new SelectListItem(){ Text = "b", Value = "b", Selected = "b".Equals(OddsGroup) },
            new SelectListItem(){ Text = "c", Value = "c", Selected = "c".Equals(OddsGroup) },
            new SelectListItem(){ Text = "d", Value = "d", Selected = "d".Equals(OddsGroup) },
            new SelectListItem(){ Text = "e", Value = "e", Selected = "e".Equals(OddsGroup) },
            new SelectListItem(){ Text = "f", Value = "f", Selected = "f".Equals(OddsGroup) },
            new SelectListItem(){ Text = "N/A", Value = null, Selected = string.IsNullOrWhiteSpace(OddsGroup) }
        };

        public bool IsActive { get; set; }
    }
}
