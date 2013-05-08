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

    /// <summary>
    /// This controller will handler add new master of super and company
    /// </summary>
    public class AddNewMasterController : CustomerSettingBaseController
    {
        public AddNewMasterController(ICustomerSettingService customerSettingService)
            : base(customerSettingService)
        {
        }

        public AddNewMasterController()
            : base()
        {
        }

        /// <summary>
        /// This is index page for insert new master
        /// </summary>
        /// <param name="superId">Id of Super</param>
        /// <param name="userName">Name of super</param>
        /// <returns></returns>
        [AuthenticateRead]
        public ActionResult AddNewMasterPage(int superId, string userName, string site)
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
                IsMasterSetting = true,
                IsAgentSetting = false,
                IsMemberSetting = false,
                CustId = superId,
                CustUserName = userName,
                CanEditCredit = _customerSettingService.CheckPermissionEditCredit()
            };
            System.Web.HttpContext.Current.Session["SettingSiteName"] = site;
            return View("~/Views/Customer/CreditCustomer/AddNewMaster.cshtml", customerSettingView);
        }

        /// <summary>
        /// Get Customer
        /// </summary>
        /// <param name="requestContext">Constext Of Get customer</param>
        /// <returns>Customer Object</returns>
        public override ICustomer GetCustomerForLaterAction(IRequestContext requestContext)
        {
            return _customerService.GetCustomerForAddNewMaster(requestContext);
        }

        /// <summary>
        /// This function will return a current context object
        /// </summary>
        /// <returns>Current context object</returns>
        [HttpPost]
        public JsonResult GetContextObject(int superId)
        {
            //init context for add new supper
            IRequestContext currentContext = new RequestContext()
            {
                CustId = superId,
                TargetCustId = 0,
                RoleId = 4,
                TargetRoleId = 3,
                FormId = FormType.Add,
                SubAccId = 0,
                AdminId = AppSession.AccId
            };
            //format json return

            return Json(JsonConvert.SerializeObject(currentContext, Formatting.Indented, GetJsonSerializerSetting()));
        }

        /// <summary>
        /// This function will add new customer and redirect to update page if add successful
        /// </summary>
        /// <param name="stringcustomer">String will be deserialized to a customer object</param>
        /// <param name="currentContext">Current context</param>
        /// <returns>Return a result message</returns>
        [AuthenticateHost]
        [HttpPost]
        public JsonResult DoAddNewMaster(string stringcustomer, RequestContext currentContext)
        {
            Fanex.PTEngine.BusinessObjects.Result result;
            try
            {
                ICustomer customer = GetCustomerFromDeserializeJsonString(stringcustomer);

                // Get site if company direct insert master
                if (currentContext.CustId < 100)
                {
                    if (System.Web.HttpContext.Current.Session["SettingSiteName"] != null)
                        customer.SiteName = System.Web.HttpContext.Current.Session["SettingSiteName"].ToString();
                }
                // Do insert new master
                int masterId = 0;
                currentContext.AdminId = AppSession.AccId;
                customer.Language = "E";
                currentContext.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
                result = _customerService.AddNewMaster(customer, currentContext, out masterId);
                // Redirect To Edit Page
                if (result.ErrorCode == "0")
                {
                    result.ErrorMessage = "Customer/V2/UpdatePT.aspx?custid=" + masterId + "&insert=" + "true" + "&roleid=3" + "&uplineid=" + currentContext.CustId.ToString();
                    _customerSettingService.UpdateStatusThirdPartyAccountAfterInsertCustomer(masterId, customer, currentContext);
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