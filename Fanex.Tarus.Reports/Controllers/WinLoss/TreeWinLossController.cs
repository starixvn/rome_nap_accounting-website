// -----------------------------------------------------------------------
// <copyright file="TreeWinLossController.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Reports.Controllers.WinLoss
{
    using System.Web.Mvc;
    using Fanex.Tarus.Reports.Business.WinLoss.TreeWinLoss;
    using Fanex.Tarus.Reports.Entities.WinLoss.TreeWinLoss;
    using Fanex.Tarus.Web.Controllers.Filters;
    using System.Web.Script.Serialization;
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TreeWinLossController : Controller
    {
        private readonly ITreeWinLossService _treeWinLossService;
        public TreeWinLossController() : this(null)
        { 
        }

        public TreeWinLossController(ITreeWinLossService treeWinLossService)
        {
            _treeWinLossService = treeWinLossService ?? new TreeWinLossService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            return View("~/Views/Report/WinLoss/TreeWinLoss/Index.cshtml");
        }

        public ActionResult ReportContent(  int reportType,                                            
                                            string fromDate,
                                            string toDate,
                                            int uplineId,
                                            string uplineName,
                                            bool isShowSB,
                                            bool isShowNG,
                                            bool isShowRB,
                                            bool isShowLCS,
                                            bool isShowCS,
                                            bool isShowP2P,
                                            bool isShowBI,
                                            bool isShowVS)
        {
            ViewBag.isShowSB = isShowSB;
            ViewBag.isShowNG = isShowNG;
            ViewBag.isShowRB = isShowRB;
            ViewBag.isShowLCS = isShowLCS;
            ViewBag.isShowCS = isShowCS;
            ViewBag.isShowP2P = isShowP2P;
            ViewBag.isShowBI = isShowBI;
            ViewBag.isShowVS = isShowVS;



            if (reportType == 0)
            {
                var winLoss = _treeWinLossService.GetTreeWinLossReportByProduct(fromDate,
                                            toDate,
                                            isShowSB,
                                            isShowNG,
                                            isShowRB,
                                            isShowLCS,
                                            isShowCS,
                                            isShowP2P,
                                            isShowBI,
                                            isShowVS);
                //var serializer = new JavaScriptSerializer();
                //serializer.MaxJsonLength = Int32.MaxValue;

                //var result = new ContentResult
                //{
                //    Content = serializer.Serialize(winLoss),
                //    ContentType = "application/json"
                //};

                return PartialView("~/Views/Report/WinLoss/TreeWinLoss/ReportByProduct.cshtml", winLoss);
            }
            else
            {
                var winloss = new TreeWinLoss();
                var title = string.Empty;

              

                winloss = _treeWinLossService.GetTreeWinLossReport(fromDate,
                                           toDate,
                                           uplineId,
                                           isShowSB,
                                           isShowNG,
                                           isShowRB,
                                           isShowLCS,
                                           isShowCS,
                                           isShowP2P,
                                           isShowBI,
                                           isShowVS,reportType);

                switch (reportType)
                {
                    case 1://get all supers by currency
                        title = string.Format("Super Win Loss ({0} currency)", uplineName);
                        break;
                    case 2://get all masters by superid
                        title = string.Format("Master Win Loss (Super {0})", uplineName);
                        break;
                    case 3://get all agents by master id
                        title = string.Format("Agent Win Loss (Master {0})", uplineName);
                        break;
                    case 4://get all members by agent id
                        title = string.Format("Member Win Loss (Agent {0})", uplineName);
                        break;
                    default:
                        break;
                }
                ViewData["Title"] = title;
                return PartialView("~/Views/Report/WinLoss/TreeWinLoss/ReportByCustomer.cshtml", winloss);
            }
        }

        public ActionResult VirtualRateUpdateHistory(int customerId)
        {
            var histories = _treeWinLossService.GetVirtualRateUpdateHistory(customerId);
            return View("~/Views/Report/WinLoss/TreeWinLoss/VirtualRateUpdateHistory.cshtml", histories);
        }
        
        public ActionResult ExportTreeWinLossReportToExcelFile(string reportTitle,
                                            int reportType,
                                            string fromDate,
                                            string toDate,
                                            int uplineId,
                                            string uplineName,
                                            bool isShowSB,
                                            bool isShowNG,
                                            bool isShowRB,
                                            bool isShowLCS,
                                            bool isShowCS,
                                            bool isShowP2P,
                                            bool isShowBI,
                                            bool isShowVS)
        {
            
            _treeWinLossService.ExportTreeWinLossReportToExcelFile(reportTitle,
                                            reportType,
                                            fromDate,
                                             toDate,
                                            uplineId,
                                            uplineName,
                                            isShowSB,
                                            isShowNG,
                                            isShowRB,
                                            isShowLCS,
                                            isShowCS,
                                            isShowP2P,
                                            isShowBI,
                                            isShowVS);
            return new EmptyResult();
        }

        public ActionResult SortingTreeWinLossReport(int reportType, 
                                            string uplineName,
                                            string sortCol,
                                            string sortType,
                                            bool isShowSB,
                                            bool isShowNG,
                                            bool isShowRB,
                                            bool isShowLCS,
                                            bool isShowCS,
                                            bool isShowP2P,
                                            bool isShowBI,
                                            bool isShowVS)
        {
            ViewBag.isShowSB = isShowSB;
            ViewBag.isShowNG = isShowNG;
            ViewBag.isShowRB = isShowRB;
            ViewBag.isShowLCS = isShowLCS;
            ViewBag.isShowCS = isShowCS;
            ViewBag.isShowP2P = isShowP2P;
            ViewBag.isShowBI = isShowBI;
            ViewBag.isShowVS = isShowVS;

            var winLoss = _treeWinLossService.SortingTreeWinLossReport(sortCol, sortType);
            if (reportType == 0)
            {                
                return PartialView("~/Views/Report/WinLoss/TreeWinLoss/ReportByProduct.cshtml", winLoss);
            }
            else
            {                
                var title = string.Empty;
                switch (reportType)
                {
                    case 1://get all supers by currency
                        title = string.Format("Super Win Loss ({0} currency)", uplineName);
                        break;
                    case 2://get all masters by superid
                        title = string.Format("Master Win Loss (Super {0})", uplineName);
                        break;
                    case 3://get all agents by master id
                        title = string.Format("Agent Win Loss (Master {0})", uplineName);
                        break;
                    case 4://get all members by agent id
                        title = string.Format("Member Win Loss (Agent {0})", uplineName);
                        break;
                    default:
                        break;
                }
                ViewData["Title"] = title;
                return PartialView("~/Views/Report/WinLoss/TreeWinLoss/ReportByCustomer.cshtml", winLoss);
            }
        }
    }
}
