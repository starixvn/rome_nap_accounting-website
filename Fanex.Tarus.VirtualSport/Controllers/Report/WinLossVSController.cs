// -----------------------------------------------------------------------
// <copyright file="WinLoss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.VirtualSport.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using Fanex.Tarus.VirtualSport.Business.Report;
using System.Web.Mvc;
    using Fanex.Tarus.VirtualSport.Business.Report.WinLoss;
    using Fanex.Tarus.Web.Controllers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossVSController : BaseController
    {

        public readonly IWinlossVSService _winlossVSService;
        public WinLossVSController()
            : this(null)
        { }

        public WinLossVSController(IWinlossVSService winlossVSService)
        {
            this._winlossVSService = winlossVSService ?? new WinlossVSService();
        }

        public ActionResult Index()
        {
            return View("~/Views/VirtualSport/Report/VSWinloss.cshtml");
        }
    }
}
