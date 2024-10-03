using System.Web.Mvc;
using Fanex.Tarus.Customer.Business.ManageSuperSettings;
using Fanex.Tarus.Customer.Requests.TextToUsernames;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Customer.Controller
{
    public class TextToUsernamesController : BaseController
    {
        private readonly ITextToUsernamesService service;

        public TextToUsernamesController()
        {
            service = new TextToUsernamesService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Customer/TextToUsernames/Index.cshtml");
        }

        [HttpPost, AuthenticateRead]
        public ActionResult GetUsernames(GetUsernamesRequest request)
        {
            var betSettings = service.GetUsernames(request);
            return Json(betSettings);
        }
    }
}
