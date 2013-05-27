using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Web.Controllers;
using System.Web.Mvc;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Finance.Controller.Transfer
{
    public class AutoFinanceTransferController : BaseController
    {
        [AuthenticateHost]
        public ActionResult Index()
        {
            return View("~/Views/Finance/Transfer/AutoTransfer/Index.cshtml");
        }
    }
}
