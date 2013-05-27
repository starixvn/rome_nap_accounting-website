using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fanex.Security;
using System.Configuration;
using Sunplus.Accounting.Core.Common;

namespace Fanex.Tarus.Web.Controllers.Report.RedirectController
{
    /// <summary>
    /// Summary description for ReportControllers
    /// </summary>
    public class ReportRedirectController : Controller
    {
        public ReportRedirectController()
        {
        }

        public ActionResult Index()
        {
            var userId = Cryptography.CfsEnCrypt(AppSession.AccId.ToString());
            var onuserId = Cryptography.CfsEnCrypt(AppSession.OnAccId.ToString());
            var time = Cryptography.CfsEnCrypt(DateTime.Now.ToString());

            var url = string.Format("{0}?userid={1}&onuserid={2}&time={3}", ConfigurationManager.AppSettings["IMSLoginUrl"], userId, onuserId, time);
            return Redirect(url);
        }
    }
}