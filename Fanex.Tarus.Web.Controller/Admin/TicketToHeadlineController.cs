using Fanex.Tarus.Business.Implement.Admin;
using Fanex.Tarus.Business.Interface.Admin;
using Fanex.Tarus.Entities.Criteria;
using Fanex.Tarus.Infrastructure.Extensions;
using Fanex.Tarus.Security.Services;
using Fanex.Tarus.Web.Controllers.Filters;
using Fanex.Tarus.Web.Models.Admin;
using Sunplus.Accounting.Core.Common;
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

        [AuthenticateRead]
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
            if (!string.IsNullOrEmpty(data.ErrorMessage))
            {
                return Json(data);
            }

            var excelService = new TicketToHeadlineExcelService(data);
            ExcelExtension.ResponseStreamExcel(excelService.Export(), GetExcelFileName());
            return new EmptyResult();
        }

        private string GetExcelFileName()
        {
            return string.Format("ExportedTicketToGroupHeadline{0}", DateTime.Now.ToString("yyyyMMdd_HH-mm-ss"));
        }
    }
}
