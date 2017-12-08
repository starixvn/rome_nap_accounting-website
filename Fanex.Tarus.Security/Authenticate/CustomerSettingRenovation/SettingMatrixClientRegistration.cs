namespace Fanex.Tarus.Security.Authenticate
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.CustomerSetting.MatrixClient;
    using Fanex.CustomerSetting.Module.Core.Criteria;
    using Fanex.CustomerSetting.Module.Core.Services.Impl;
    using Fanex.Tarus.Security.Services;
    using Sunplus.Accounting.Core.Common;

    public class SettingMatrixClientRegistration : SettingMatrixRegistration
    {
        private CustomerSettingAttribute _customerSettingAttribute = new CustomerSettingAttribute();

        private ServiceBase _serviceBase = new ServiceBase();

        public override bool VerifyPermission()
        {
            var hasReadPermission = _customerSettingAttribute.AllowRead(GetActionName());
            if (!hasReadPermission)
            {
                return false;
            }

            var hasSpecialSitePermission = PermissionService.HasPermission(Enumerations.Functions.AccessSpecialSites.ToString());
            var customerName = HttpContext.Current.Request.QueryString["customerName"];
            if (!string.IsNullOrWhiteSpace(customerName))
            {
                var specialSiteCriteria = new CheckCustomerIsSpecialSiteCriteria()
                {
                    UserName = customerName
                };

                if (!hasSpecialSitePermission && _serviceBase.CheckCustomerBelongToSpecialSite(specialSiteCriteria).IsSpecialSite)
                {
                    return false;
                }
            }

            return true;
        }

        public override void RegisterRouteConfig(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Matrix_default",
                url: "Matrix/{controller}/{action}/{id}",
                defaults: new { controller = "ViewCustomerSettingMatrix", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Fanex.CustomerSetting.MatrixClient.Controllers" });
        }

        private string GetActionName()
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            if (routeValues != null)
            {
                if (routeValues.ContainsKey("action"))
                {
                    var controllerName = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
                    var actionName = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
                    return controllerName + '/' + actionName;
                }
            }

            return string.Empty;
        }
    }
}