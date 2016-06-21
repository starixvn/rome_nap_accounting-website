using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Fanex.Tarus.Admin.Business.SMAManagement;
using Fanex.Tarus.Admin.Criteria.SMAManagement;
using Fanex.Tarus.Admin.Entities;
using Fanex.Tarus.Admin.Entities.SMAManagement;
using Fanex.Tarus.Admin.Resource;
using Fanex.Tarus.Infrastructure.Common;
using Fanex.Tarus.Infrastructure.Helper;
using Fanex.Tarus.Security.Permission;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;

namespace Fanex.Tarus.Admin.Controller.SMAManagement
{
    public class SMAManagementController : BaseController
    {
        private readonly ISMAManagementService _smaManagementService;

        public SMAManagementController()
            : this(null)
        {
        }

        public SMAManagementController(ISMAManagementService smaManagemenentService)
        {
            _smaManagementService = smaManagemenentService ?? new SMAManagementService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            ViewData["HasHostPermission"] = HasHostPermission();

            return View("~/Views/Admin/SMAManagement/SMAManagement.cshtml", GetFilterInfo());
        }

        [HttpGet]
        public ActionResult GetCurrencies(bool excludeTestCurrencies)
        {
            var currencies = _smaManagementService.GetCurrencies(excludeTestCurrencies).ToList();
            currencies.Insert(0, new IdNameEntity { Name = Language.All, Id = 0 });

            return Json(currencies, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSMAGroupHeadlines(GetSMAGroupHeadlineCriteria criteria)
        {
            var result = _smaManagementService.GetSMAGroupHeadlines(criteria);

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        public ActionResult ExportSMAGroupHeadlinesToExcel(GetSMAGroupHeadlineCriteria criteria)
        {
            string reportName = "SMAListByGroupHeadline";
            var excelFile = _smaManagementService.ExportSMAGroupHeadlinesToExcel(reportName, criteria);

            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", reportName));

            excelFile.SaveXls(Response.OutputStream);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult UpdateHeadlineType(UpdateHeadlineTypeCriteria criteria)
        {
            Result result;

            if (!HasHostPermission())
            {
                result = new Result(403, "You are not authorized on this function!");
            }
            else
            {
                criteria.UserId = AppSession.AccId;
                criteria.IP = Utils.GetClientIP();

                result = _smaManagementService.UpdateHeadlineType(criteria);
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult UpdateOwner(UpdateOwnerCriteria criteria)
        {
            Result result;

            if (!HasHostPermission())
            {
                result = new Result(403, "You are not authorized on this function!");
            }
            else
            {
                criteria.UserId = AppSession.AccId;
                criteria.IP = Utils.GetClientIP();

                result = _smaManagementService.UpdateOwner(criteria);
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        [NonAction]
        private FilterGroup GetFilterInfo()
        {
            var sites = _smaManagementService.GetSites().Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });

            var currencies = _smaManagementService.GetCurrencies(true).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            currencies.Insert(0, new SelectListItem { Text = Language.All, Value = "0", Selected = true });

            return new FilterGroup
            {
                Sites = sites,
                Currencies = currencies,
                Statuses = GetStatuses(),
                HeadlineTypes = GetHeadlineTypes(),
                Positions = GetPositions()
            };
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetStatuses()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = Language.Open, Value = "isOpen" },
                new SelectListItem { Text = Language.Suspended, Value = "Suspended" },
                new SelectListItem { Text = Language.Closed, Value = "isClosed" },
                new SelectListItem { Text = Language.Disabled, Value = "isDisabled" }
            };
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetHeadlineTypes()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = Language.All, Value = string.Empty, Selected = true },
                new SelectListItem { Text = Language.VIP, Value = Language.VIP },
                new SelectListItem { Text = Language.Normal, Value = Language.Normal }
            };
        }

        [NonAction]
        private IEnumerable<SelectListItem> GetPositions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = Language.All, Value = "0", Selected = true },
                new SelectListItem { Text = Language.Super, Value = "4" },
                new SelectListItem { Text = Language.Master, Value = "3" },
                new SelectListItem { Text = Language.Agent, Value = "2" },
                new SelectListItem { Text = Language.Member, Value = "1" }
            };
        }

        [NonAction]
        private bool HasHostPermission()
        {
            var permissionService = new PermissionService();
            var roleAccess = permissionService.GetRoleAccess(AppSession.AccId);

            return permissionService.GetHostPermission(Sunplus.Accounting.Core.Common.Enumerations.Admin.SMAManagement, roleAccess);
        }
    }
}
