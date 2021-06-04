namespace Fanex.Tarus.Customer.Controller.Setting
{
    using Business.SettingCustomer.OpenCustomer;
    using Requests.CustomerSetting.OpenCustomer;
    using System.Web.Mvc;
    using Web.Controllers;

    public class OpenCustomerController : BaseController
    {
        private readonly OpenCustomerService openCustomerService;

        private const string singleOpenCustomerViewPath = "~/Views/Customer/CustomerSetting/OpenCustomer/SingleOpenCustomer.cshtml";
        private const string closedMatrixViewPath = "~/Views/Customer/CustomerSetting/OpenCustomer/ClosedMatrix.cshtml";

        public OpenCustomerController() : this(null)
        {
        }

        public OpenCustomerController(OpenCustomerService openCustomerService)
        {
            this.openCustomerService = openCustomerService ?? new OpenCustomerServiceImpl();
        }

        public ActionResult SingleOpenCustomerView(GetSingleOpenCustomerInfoQuery query) 
            => View(singleOpenCustomerViewPath, openCustomerService.GetSingleOpenCustomerInfo(query));

        [HttpPost]
        public JsonResult GetSingleOpenDownlinesInfo(GetSingleOpenDownlinesInfoQuery query)
            => Json(openCustomerService.GetSingleOpenDownlinesInfo(query));

        [HttpPost]
        public JsonResult SingleOpenCustomer(UpdateClosedStatusQuery query)
            => Json(openCustomerService.UpdateSingleCustomerClosedStatus(query));

        public ActionResult ClosedMatrixView(string username) 
            => View(closedMatrixViewPath, model: username);

        [HttpPost]
        public JsonResult GetClosedMatrix(string username)
            => Json(openCustomerService.GetClosedMatrixInfo(username));
    }
}