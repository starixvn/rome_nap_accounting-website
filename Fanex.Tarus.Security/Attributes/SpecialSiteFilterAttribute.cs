namespace Fanex.Tarus.Security.Attributes
{
    using Sunplus.Accounting.Core.Common;
    using System.Web.Mvc;
    using Tarus.Security.Criteria;
    using Tarus.Security.Permission;

    public class SpecialSiteFilterAttribute : ActionFilterAttribute
    {
        private const string DeniedView = "~/Areas/CustomerSetting/Views/Security/AccessDenied.cshtml";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var param = filterContext.ActionParameters;
            int custid = param.ContainsKey("custid") ? (int)param["custid"] : (param.ContainsKey("customerid") ? (int)param["customerid"] : 0);
            var username = param.ContainsKey("username") ? param["username"].ToString() : string.Empty;
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
    }
}
