using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.CustomerSetting.Module.Core.Attributes;
using Fanex.Tarus.Customer.CreditCustomer.Entities;
using Fanex.Tarus.Web.Controllers;

namespace Fanex.Tarus.Customer.Controller
{
    [SpecialSiteFilter]
    public class CustomerController : BaseController
    {
        public ActionResult Create()
            => View("~/Views/Customer/Create.cshtml");
    }
}