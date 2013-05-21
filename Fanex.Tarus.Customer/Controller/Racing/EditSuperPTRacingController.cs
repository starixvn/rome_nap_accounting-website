
namespace Fanex.Tarus.Customer.Controller.Racing
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.PTRacing.BusinessObjects;
    using Fanex.Tarus.Customer.Business.Racing;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Newtonsoft.Json;
    using Sunplus.Accounting.Core.Common;

    public class EditSuperPTRacingController : PTRacingSettingBaseController
    {
        public EditSuperPTRacingController() : base()
        { 
        }

        public EditSuperPTRacingController(IRacingSettingService racingSettingService)
            : base(racingSettingService)
        { 
        }

        [AuthenticateRead]
        public ActionResult Index() 
        {
            return View("~/Views/Customer/Racing/EditSuperPTRacing.cshtml");
        }

        [HttpPost]
        public JsonResult GetContextObject(int targetCustId, int targetRoleId, int roleId)
        {
            //init context for edit supper
            IRequestContext currentContext = new RequestContext()
            {
                CustId = 0,//upline id
                AgentId = 0,//unknow
                UserName = string.Empty,
                TargetCustId = targetCustId,
                AdminId = AppSession.AccId,
                SubAccId = 0,
                RoleId = roleId,
                CurrencyId = 0,
                TargetRoleId = targetRoleId,
                IsNeutralSite = false,
                FormId = FormType.EditPT,
                IP = System.Web.HttpContext.Current.Request.UserHostAddress
            };

            return Json(JsonConvert.SerializeObject(currentContext, Formatting.Indented, GetJsonSerializerSetting()));
        }

        [HttpPost]
        public JsonResult GetCustomerInfo(RequestContext requestContext)
        {
            var result = _racingSettingService.GetCustomerInfo(requestContext);

            return Json(JsonConvert.SerializeObject(result, Formatting.Indented, GetJsonSerializerSetting()));

        }

        [HttpPost, AuthenticateHost]
        public JsonResult UpdateCustomerSettings(string oldCustomerSettings, string newCustomerSettings, RequestContext currentContext)
        {
            IList<Result> updatedResults = new List<Result>();
            try
            {
                ICustomer oldSettings = DeserializeJsonString(oldCustomerSettings);
                ICustomer newSettings = DeserializeJsonString(newCustomerSettings);

                var parameters = new Hashtable();
                var results = _racingSettingService.SaveCustomerSettings(oldSettings, newSettings, currentContext, parameters);

                updatedResults = ParseError(results.ErrorCode, results.ErrorMessage);

            }
            catch (Exception ex)
            {                
                updatedResults.Add(new Result() { 
                    ErrorCode = "100",
                    ErrorMessage = ex.Message
                });
            }

            return Json(updatedResults);
        }
    }
}
