namespace Fanex.Tarus.Security.Attributes
{
    using Sunplus.Accounting.Core.Common;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Tarus.Security.Criteria;
    using Tarus.Security.Permission;

    public class SpecialSiteFilterAttribute : ActionFilterAttribute
    {
        private readonly string[] CustIdParamName = { "custid", "customerid", "superid", "masterid", "agentid" };

        private const string DeniedView = "~/Areas/CustomerSetting/Views/Security/AccessDenied.cshtml";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var param = filterContext.ActionParameters;
            int custid = this.GetCustIdParam(param);
            var username = param.ContainsKey("username") ? (param["username"] ?? string.Empty).ToString() : string.Empty;
            if (param.ContainsKey("superid"))
            {

            }

            if (!(custid == 0 && string.IsNullOrEmpty(username)))
            {
                var permissionService = new PermissionService();
                var checkSpecialSite = permissionService.CheckSpecialSitePermission(new CheckSpecialSitePermissionCriteria()
                {
                    UserID = AppSession.AccId,
                    CustID = custid,
                    CustName = username
                });
                if (checkSpecialSite.ErrorCode > 0)
                {
                    filterContext.Result = new PartialViewResult
                    {
                        ViewName = DeniedView
                    };

                    (filterContext.Result as PartialViewResult).ViewBag.ErrorMessage = checkSpecialSite.ErrorMessage;
                }
            }

            base.OnActionExecuting(filterContext);
        }

        private int GetCustIdParam(IDictionary<string, object> param)
        {
            List<int> list = new List<int>();
            list.Add(param.ContainsKey("custid") ? (int)param["custid"] : 0);
            list.Add(param.ContainsKey("customerid") ? (int)param["customerid"] : 0);
            list.Add(param.ContainsKey("superid") ? (int)param["superid"] : 0);
            list.Add(param.ContainsKey("masterid") ? (int)param["masterid"] : 0);
            list.Add(param.ContainsKey("agentid") ? (int)param["agentid"] : 0);

            var index = list.FindIndex(m => m > 0);
            return index >= 0 ? list[index] : 0;
        }
    }
}
