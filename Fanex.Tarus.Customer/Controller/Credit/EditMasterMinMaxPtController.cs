namespace Fanex.Tarus.Customer.Credit.Controllers
{
    using System;
    using System.Web.Mvc;
    using Fanex.PTEngine.BusinessObjects;
    using Fanex.Tarus.Customer.CreditCustomer.Business;
    using Fanex.Tarus.Customer.CreditCustomer.Entities;
    using Fanex.Tarus.Web.Controllers.Filters;
    using Newtonsoft.Json;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Helper;
    using System.Collections.Generic;
    using Sunplus.Accounting.Core.Extensions;

    public class EditMasterMinMaxPtController : CustomerSettingBaseController
    {
        /// <summary>
        /// Constructor With
        /// </summary>
        /// <param name="customerSettingService">Customer Setting service</param>
        public EditMasterMinMaxPtController(ICustomerSettingService customerSettingService)
            : base(customerSettingService)
        {
        }

        /// <summary>
        /// Constructor with parameterless
        /// </summary>
        public EditMasterMinMaxPtController()
            : base()
        {
        }

        /// <summary>
        /// This funtion will return a result binding to view
        /// </summary>
        /// <returns>A Edit master Page</returns>
        [AuthenticateRead]
        public ActionResult EditMasterMinMaxPtPage(int custId, int uplineId, string uname)
        {
            //save super id
            // Setting edit page
            CustomerSettingView customerSettingView = new CustomerSettingView
            {
                TitlePage = "Master: " + uname,
                IsUpdateMinMaxPt = true,
                CustId = custId,
                CustUserName = uname,
                UplineCustId = uplineId,                
                CanEditCredit = _customerSettingService.CheckPermissionEditCredit()
            };
            return View("~/Views/Customer/CreditCustomer/EditMasterMinMaxPt.cshtml", customerSettingView);
        }

        /// <summary>
        /// Get Customer
        /// </summary>
        /// <param name="requestContext">Constext Of Get customer</param>
        /// <returns>Customer Object</returns>
        public override ICustomer GetCustomerForLaterAction(IRequestContext requestContext)
        {
            return _customerService.GetCustomerForSuperEditMinPTMaster(requestContext);
        }

        /// <summary>
        /// This function will return a current context object
        /// </summary>
        /// <returns>Current context object</returns>
        [HttpPost]
        public JsonResult GetContextObject(int uplineId, int targetId)
        {
            //init context for edit master
            IRequestContext currentContext = new RequestContext()
            {
                CustId = uplineId,
                TargetCustId = targetId,
                RoleId = 0,
                TargetRoleId = 3,
                FormId = FormType.EditMinMaxPT,
                SubAccId = 0,
                AdminId = AppSession.AccId,
                Language = "E",
                LangKey = "E"
            };

            return Json(JsonConvert.SerializeObject(currentContext, Formatting.Indented, GetJsonSerializerSetting()));
        }

        /// <summary>
        /// This function will edit customer and reload page after update sucessfully
        /// </summary>
        /// <param name="stringcustomer">String will be deserialized to a customer object</param>
        /// <param name="currentContext">Current context</param>
        /// <returns>Return a result message</returns>
        [AuthenticateHost]
        [HttpPost]
        public JsonResult DoEditMaster(string stringcustomer, RequestContext currentContext)
        {
            IList<Result> result = new List<Result>();
            try
            {
                ICustomer customer = GetCustomerFromDeserializeJsonString(stringcustomer);
                
                // Do edit master
                currentContext.CustId = AppSession.AccId;
                currentContext.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
                var updateResult = _customerService.CompanyUpdateMinMaxPTMaster(customer, currentContext);
                // format template of error message
                result = ParseError(updateResult.ErrorCode, updateResult.ErrorMessage);
            }
            catch (Exception ex)
            {
                result.Add(new Result()
                {
                    ErrorCode = "1000",
                    ErrorMessage = ex.Message
                });
                LogExtensions.ExceptionHandler(ex);
            }

            // Return result
            return Json(result);
        }
    }
}