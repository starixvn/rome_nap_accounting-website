namespace Fanex.Tarus.Customer.Controller
{
    using Fanex.Tarus.Customer.Business.SabaCoin;
    using Fanex.Tarus.Web.Controllers;
    using System.Web.Mvc;

    public class SabaCoinController : BaseController
    {
        private readonly ISabaCoinService _sabaCoinService;

        public SabaCoinController()
            : this(null)
        {
        }

        public SabaCoinController(ISabaCoinService sabaCoinService)
        {
            _sabaCoinService = sabaCoinService ?? new SabaCoinService();
        }
        
        public ActionResult Index(long sabaId = 0)
            => View("~/Views/Customer/SabaCoin/Index.cshtml", _sabaCoinService.Get(sabaId));
    }
}
