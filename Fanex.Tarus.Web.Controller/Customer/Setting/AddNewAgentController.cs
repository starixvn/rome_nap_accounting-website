using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fanex.PTEngine.BusinessObjects;
using Fanex.PTEngine.Services;
using Fanex.Tarus.Web.Controllers.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Fanex.Tarus.Customer.CreditCustomer.Business;
using Fanex.Tarus.Customer.CreditCustomer.Entities;

namespace Fanex.Tarus.Web.Controllers.Customer.Setting
{
    public class AddNewAgentController : BaseController
    {
        private static ICustomerService _customerService;
        private static ICustomerSettingService _customerSettingService;

        public AddNewAgentController(ICustomerSettingService customerSettingService)
        {
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
            _customerService = new CustomerService();
        }

        public AddNewAgentController()
            : this(null)
        {
        }

        /// <summary>
        /// This is index page for insert new agent
        /// </summary>
        /// <param name="masterId">Id of Master</param>
        /// <param name="userName">Name of Master</param>
        /// <returns></returns>
        [AuthenticateRead]
        public ActionResult AddNewAgentPage(int masterId, string userName)
        {
            if (userName == null || userName == string.Empty)
            {
                userName = AppSession.CustUName;
            }
            //// Setting for customer information section
            CustomerSettingView customerSettingView = new CustomerSettingView
            {
                TitlePage = "General Information",
                IsInsertPage = true,
                IsSuperSetting = false,
                IsMasterSetting = false,
                IsMemberSetting = false,
                IsAgentSetting = true,
                CustId = masterId,
                CustUserName = userName,
                CanEditCredit = _customerSettingService.CheckPermissionEditCredit()
            };
            return View("~/Views/Customer/CreditCustomer/AddNewAgent.cshtml", customerSettingView);
        }

        /// <summary>
        /// This function will return data of customer object depend on constrain of up-line
        /// </summary>
        /// <returns>String was serialized of customer object</returns>
        [HttpPost]
        public JsonResult GetCustomerObject(RequestContext requestContext)
        {
            string customerObject = string.Empty;
            try
            {
                // Format json return
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                // Get default value for add new agent
                ICustomer customer = _customerService.GetCustomerForAddNewAgent(requestContext);
                customerObject = JsonConvert.SerializeObject(customer, Formatting.Indented, settings);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return Json(customerObject);
        }

        /// <summary>
        /// This function will return a current context object
        /// </summary>
        /// <returns>Current context object</returns>
        [HttpPost]
        public JsonResult GetContextObject(int masterId)
        {
            // Init context for add new supper
            IRequestContext currentContext = new RequestContext()
            {
                CustId = masterId,
                TargetCustId = 0,
                RoleId = 3,
                TargetRoleId = 2,
                FormId = FormType.Add,
                SubAccId = 0,
                AdminId = AppSession.AccId
            };
            // Format json return
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return Json(JsonConvert.SerializeObject(currentContext, Formatting.Indented, settings));
        }

        /// <summary>
        /// This function will add new customer and redirect to update page if add successful
        /// </summary>
        /// <param name="stringcustomer">String will be deserialized to a customer object</param>
        /// <param name="currentContext">Current context</param>
        /// <returns>Return a result message</returns>
        [AuthenticateHost]
        [HttpPost]
        public JsonResult DoAddNewAgent(string stringcustomer, RequestContext currentContext)
        {
            Fanex.PTEngine.BusinessObjects.Result result;
            try
            {
                // Format deserialize
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                Fanex.PTEngine.BusinessObjects.Customer customer = JsonConvert.DeserializeObject<Fanex.PTEngine.BusinessObjects.Customer>(stringcustomer, settings);

                // Do insert new agent
                int agentId = 0;
                currentContext.AdminId = AppSession.AccId;
                customer.Language = "E";
                currentContext.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
                result = _customerService.AddNewAgent(customer, currentContext, out agentId);
                // Redirect To Edit Page
                if (result.ErrorCode == "0")
                {
                    result.ErrorMessage = "Customer/V2/UpdatePT.aspx?custid=" + agentId + "&insert=" + "true" + "&roleid=2" + "&uplineid=" + currentContext.CustId.ToString();
                    _customerSettingService.UpdateStatusThirdPartyAccountAfterInsertCustomer(agentId, customer, currentContext);
                }
            }
            catch (Exception ex)
            {
                result = new Fanex.PTEngine.BusinessObjects.Result
                {
                    ErrorCode = "1000",
                    ErrorMessage = ex.Message
                };
            }

            // Return result
            return Json(result);
        }
    }
}