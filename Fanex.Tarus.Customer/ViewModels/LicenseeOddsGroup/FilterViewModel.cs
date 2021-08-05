namespace Fanex.Tarus.Customer.ViewModels.LicenseeOddsGroup
{
    using Fanex.Tarus.Customer.Entities;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class FilterViewModel
    {
        public FilterViewModel(
            IEnumerable<string> sites,
            IEnumerable<BaseModel> supers,
            IEnumerable<BaseModel> agents)
        {
            Sites = sites.Select(s => new SelectListItem { Text = s, Value = s });
            Supers = supers.Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() });

            var jsonSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            AgentJSON = JsonConvert.SerializeObject(agents, Formatting.None, jsonSetting);
        }

        public IEnumerable<SelectListItem> Sites { get; }

        public IEnumerable<SelectListItem> Supers { get; }

        public string AgentJSON { get; }
    }
}
