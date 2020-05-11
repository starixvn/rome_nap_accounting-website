namespace Fanex.Tarus.Customer.DangerousGrouping
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Module.Core.Web.Controllers;

    public class DangerousGroupingController : BaseController
    {
        private readonly string indexPath = "~/Views/Customer/DangerousGrouping/Index.cshtml";

        public ActionResult Index()
        {
            return View(indexPath);
        }
    }
}