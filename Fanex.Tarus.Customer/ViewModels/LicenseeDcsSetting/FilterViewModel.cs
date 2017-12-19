using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Customer.Entities.LicenseeDcsSetting;
using Newtonsoft.Json;

namespace Fanex.Tarus.Customer.ViewModels.LicenseeDcsSetting
{
    public sealed class FilterViewModel
    {
        public FilterViewModel(
            IEnumerable<string> sites,
            IEnumerable<BaseModel> supers,
            IEnumerable<ProductSettingTypeEntity> products)
        {
            Sites = sites.Select(s => new SelectListItem { Text = s, Value = s });
            Supers = supers.Select(s => new SelectListItem { Text = s.Name, Value = s.Id.ToString() });

            var productViewModels = products
                .GroupBy(p => new { Id = p.ProductId, Name = p.ProductName })
                .Select(p => new ProductViewModel(p.Key.Id, p.Key.Name, p.OrderBy(o => o.SettingType)));

            Products = productViewModels.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            });

            Functions = productViewModels.First().SettingTypes.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });

            ProductJSON = JsonConvert.SerializeObject(
                productViewModels,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        public IEnumerable<SelectListItem> Sites { get; }

        public IEnumerable<SelectListItem> Supers { get; }

        public IEnumerable<SelectListItem> Products { get; }

        public IEnumerable<SelectListItem> Functions { get; }

        public string ProductJSON { get; }
    }
}
