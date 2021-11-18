namespace Fanex.Tarus.Customer.Controller
{
    using Fanex.CustomerSetting.Module.Core.Web.Controllers;
    using Fanex.Tarus.Customer.Business.MultipleUpdateSuperStatus;
    using Fanex.Tarus.Customer.Criteria.MultipleUpdateSuperStatus;
    using Fanex.Tarus.Customer.Requests.MultipleUpdateSuperStatus;
    using Fanex.Tarus.Web.Controllers.Filters;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class MultipleUpdateSuperStatusController : BaseController
    {
        private readonly IMultipleUpdateSuperStatusService _service;

        public MultipleUpdateSuperStatusController() : this(null)
        {
        }

        public MultipleUpdateSuperStatusController(IMultipleUpdateSuperStatusService service)
        {
            _service = service ?? new MultipleUpdateSuperStatusService();
        }

        [HttpPost, AuthenticateRead]
        public JsonResult GetSupersStatus(GetSupersStatusRequest request)
        {
            return Json(_service.GetSupersStatus(new GetSupersStatusCriteria
            {
                SuperList = string.Join(",", request.SuperList.Split(',').Select(super => super.Trim()).Distinct().ToArray())
            }));
        }

        [HttpPost, AuthenticateHost]
        public JsonResult UpdateClosedStatus(UpdateSupersClosedStatusRequest request) => Json(_service.UpdateSupersClosedStatus(request));

        [HttpPost, AuthenticateHost]
        public JsonResult UpdateSuspendedStatus(UpdateSupersSuspendedStatusRequest request) => Json(_service.UpdateSupersSuspendedStatus(request));

        [HttpPost, AuthenticateHost]
        public JsonResult UpdateProtectedStatus(UpdateSupersProtectedStatusRequest request) => Json(_service.UpdateSupersProtectedStatus(request));
    }
}
