namespace Fanex.Tarus.Customer.Controller
{
    using Criteria.Player3rd;
    using DataAccess.Player3rd;
    using Entities.Player3rd;
    using Fanex.Tarus.Web.Controllers;
    using Security.Authenticate;
    using Security.Services;
    using Sunplus.Accounting.Core.Common;
    using System.Collections.Generic;
    using System.Web.Mvc;

    [AuthorizeFunctions("BetListByIP")]
    public class Player3rdController : BaseController
    {
        private readonly Player3rdDataAccess _service;

        public Player3rdController() : this(null)
        {
        }

        public Player3rdController(Player3rdDataAccess service)
        {
            _service = service ?? new Player3rdDataAccessImpl();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Search(SearchPlayer3rdCriteria criteria)
        {
            criteria.UserId = AppSession.AccId;
            criteria.SiteType = CreditOrDepositCustomerUtils.GetSiteTypeParamValue(AppSession.AccId);
            if (CreditOrDepositCustomerUtils.IsNoneCreditAndDepositPermission(criteria.SiteType))
            {
                return Json(new List<Player3rd>());
            }

            criteria.PlayerIDs = (criteria.PlayerIDs ?? string.Empty).Trim(',');
            criteria.Usernames = (criteria.Usernames ?? string.Empty).Trim(',');
            return Json(_service.Search(criteria));
        }
    }
}
