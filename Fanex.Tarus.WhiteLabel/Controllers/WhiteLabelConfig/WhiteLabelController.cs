using Fanex.Tarus.Infrastructure.Common;
using Fanex.Tarus.Infrastructure.Helper;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.Web.Controllers.Filters;
using Fanex.Tarus.WhiteLabel.Criteria.WhiteLabelConfig;
using Fanex.Tarus.WhiteLabel.Requests.WhiteLabelConfig;
using Fanex.Tarus.WhiteLabel.Services.WhiteLabelConfig;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Tarus.WhiteLabel.Controller.WhiteLabelConfig
{
    public class WhiteLabelController : BaseController
    {
        private readonly IWhiteLabelService service;

        public WhiteLabelController() : this(null) { }

        public WhiteLabelController(IWhiteLabelService service)
        {
            this.service = service ?? new WhiteLabelService();
        }

        [AuthenticateRead]
        public ActionResult Index()
        {
            ViewBag.WhiteLabelInfoList = service.GetWhiteLabelInfo();
            return View();
        }

        [AuthenticateRead]
        public JsonResult Search(string username, int? custId)
            => Json(service.Search(new GetWhiteLabelSettingCriteria() { Username = username, CustId = custId }), JsonRequestBehavior.AllowGet);

        [AuthenticateRead]
        public JsonResult GetAgentInfoForAddNew(string agentName)
        {
            var checkCustResult = service.Check(new CheckWhiteLabelSettingCriteria() { Username = agentName, IsNew = true });
            int custId = DataUtils.ToInt32(checkCustResult.Outputs["custid"]);
            if (checkCustResult.IsSuccess)
            {
                return Json(service.GetAgentInfo(new GetAgentInfoCriteria() { CustId = custId }), JsonRequestBehavior.AllowGet);
            }

            return Json(checkCustResult, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateRead]
        public JsonResult GetWhiteLabelSettingForEdit(string agentName)
        {
            var checkCustResult = service.Check(new CheckWhiteLabelSettingCriteria() { Username = agentName, IsNew = false });
            int custId = DataUtils.ToInt32(checkCustResult.Outputs["custid"]);
            if (checkCustResult.IsSuccess)
            {
                return Json(service.Search(new GetWhiteLabelSettingCriteria() { CustId = custId }).FirstOrDefault(), JsonRequestBehavior.AllowGet);
            }

            return Json(checkCustResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AuthenticateHost]
        public JsonResult Update(EditWhiteLabelRequest request)
            => Json(service.Update(new EditWhiteLabelSettingCriteria()
            {
                CustId = request.CustId,
                AgentRate = request.AgentRate,
                HeadlineRate = request.HeadlineRate,
                MasterRate = request.MasterRate,
                SuperRate = request.SuperRate,
                LaunchedDate = request.LaunchedDate.Value,
                IP = Utils.GetClientIP(),
                Remark = request.Remark,
                UserId = AppSession.AccId
            }));

        [HttpPost]
        [AuthenticateHost]
        public JsonResult Add(AddWhiteLabelRequest request)
            => Json(service.Add(new AddWhiteLabelSettingCriteria() {
                CustId = request.CustId,
                WhiteLabel = request.WhiteLabel,
                AgentRate = request.AgentRate,
                HeadlineRate = request.HeadlineRate,
                MasterRate = request.MasterRate,
                SuperRate = request.SuperRate,
                LaunchedDate = request.LaunchedDate.Value,
                IP = Utils.GetClientIP(),
                Remark = request.Remark,
                UserId = AppSession.AccId
            }));
    }
}
