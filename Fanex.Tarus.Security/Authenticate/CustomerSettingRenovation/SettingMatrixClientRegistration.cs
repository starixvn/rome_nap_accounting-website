namespace Fanex.Tarus.Security.Authenticate
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.CustomerSetting.MatrixClient;

    public class SettingMatrixClientRegistration : SettingMatrixRegistration
    {
        private CustomerSettingAttribute _customerSettingAttribute = new CustomerSettingAttribute();

        public override bool VerifyPermission()
        {
            return _customerSettingAttribute.AllowRead(GetActionName());
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