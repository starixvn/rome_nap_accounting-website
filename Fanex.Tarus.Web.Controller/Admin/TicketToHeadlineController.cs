using Fanex.Tarus.Business.Implement.Admin;
using Fanex.Tarus.Business.Interface.Admin;
using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Infrastructure.Extensions;
using System;
using System.Web;
using System.Web.Mvc;

namespace Fanex.Tarus.Web.Controllers.Admin
{
    public class TicketToHeadlineController : BaseController
    {
        private ITicketToHeadlineService _ticketToHeadlineService;

        public TicketToHeadlineController()
        {
            this._ticketToHeadlineService = new TicketToHeadlineService();
        }

        public ActionResult Index()
        {
            return View("~/Views/Admin/TicketToHeadline/Index.cshtml");
        }

        public ActionResult GetData(HttpPostedFileBase uploadFile, TicketToHeadlineCriteria criteria)
        {
            return Json(_ticketToHeadlineService.GetData(uploadFile, criteria));
            //return Json(_ticketToHeadlineService.GetFakeData((int?)criteria.PageSize, (int?)criteria.PageIndex, 1001));
        }

        public ActionResult ExportExcel(HttpPostedFileBase uploadFile, TicketToHeadlineCriteria criteria)
        {
            criteria.PageIndex = null;
            criteria.PageSize = null;
            //var data = _ticketToHeadlineService.GetFakeData(null, null, 5000);
            var data = _ticketToHeadlineService.GetData(uploadFile, criteria);
            var excelService = new TicketToHeadlineExcelService(data);
            ExcelExtension.ResponseStreamExcel(excelService.Export(), GetExcelFileName());
            return new EmptyResult();
        }

        private string GetExcelFileName()
        {
            return string.Format("ExportedTicketToGroupHeadline{0}.xls", DateTime.Now.ToString("yyyyMMdd_hh-mm-ss"));
        }
    }
}
