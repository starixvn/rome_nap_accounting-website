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
    public class AddNewMemberController : BaseController
    {
        private static ICustomerSettingService _customerSettingService;
        private static ICustomerService _customerService;

        public AddNewMemberController(ICustomerSettingService customerSettingService)
        {
            _customerSettingService = customerSettingService ?? new CustomerSettingService();
            _customerService = new CustomerService();
        }

        public AddNewMemberController()
            : this(null)
        {
        }

        /// <summary>
        /// This is index page for insert new member
        /// </summary>
        /// <param name="masterId">Id of agent</param>
        /// <param name="userName">Name of agent</param>
        /// <returns></returns>
        [AuthenticateRead]
        public ActionResult AddNewMemberPage(int agentId, string userName)
        {
            if (userName == null || userName == string.Empty)
            {
                userName = AppSession.CustUName;
            }
            // Get list group for add new member
            ViewData["listMemberGroup"] = _customerSettingService.GetListMemberGroup(agentId);
            //// Setting for customer information section
            CustomerSettingView customerSettingView = new CustomerSettingView
            {
                TitlePage = "General Information",
                IsInsertPage = true,
                IsSuperSetting = false,
                IsMasterSetting = false,
                IsAgentSetting = false,
                IsMemberSetting = true,
                CustId = agentId,
                CustUserName = userName,
                CanEditCredit = _customerSettingService.CheckPermissionEditCredit()
            };
            return View("~/Views/Customer/CreditCustomer/AddNewMember.cshtml", customerSettingView);
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
                //format json return
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                //get default value for add new member
                ICustomer customer = _customerService.GetCustomerForAddNewMember(requestContext);
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
        public JsonResult GetContextObject(int agentId)
        {
            //init context for add new supper
            IRequestContext currentContext = new RequestContext()
            {
                CustId = agentId,
                TargetCustId = 0,
                RoleId = 2,
                TargetRoleId = 1,
                FormId = FormType.Add,
                SubAccId = 0,
                AdminId = AppSession.AccId
            };
            //format json return
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
        public JsonResult DoAddNewMember(string stringcustomer, RequestContext currentContext)
        {
            Fanex.PTEngine.BusinessObjects.Result result;
            try
            {
                //format deserialize
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                Fanex.PTEngine.BusinessObjects.Customer customer = JsonConvert.DeserializeObject<Fanex.PTEngine.BusinessObjects.Customer>(stringcustomer, settings);

                //do insert new agent
                int memberId = 0;
                currentContext.AdminId = AppSession.AccId;
                customer.Language = "E";
                currentContext.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
                result = _customerService.AddNewMember(customer, currentContext, out memberId);
                //redirectToEditPage
                if (result.ErrorCode == "0")
                {
                    result.ErrorMessage = "Customer/V2/UpdatePT.aspx?custid=" + memberId + "&insert=" + "true" + "&roleid=1" + "&uplineid=" + currentContext.CustId.ToString();
                    _customerSettingService.UpdateStatusThirdPartyAccountAfterInsertCustomer(memberId, customer, currentContext);
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

            //return result
            return Json(result);
        }
    }
}