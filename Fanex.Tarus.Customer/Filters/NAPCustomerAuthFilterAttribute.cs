using System;
using System.Web;
using Fanex.Customer.Client.NETFramework.Filters;
using Fanex.Tarus.Security.Services;
using Sunplus.Accounting.Core.Common;

namespace Fanex.Tarus.Customer.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class NAPCustomerAuthFilterAttribute : CustomerAuthFilterAttribute
    {
        private readonly PermissionService _permissionService;

        public NAPCustomerAuthFilterAttribute() : this(null)
        {
        }

        public NAPCustomerAuthFilterAttribute(PermissionService permissionService)
            : base(null)
        {
            _permissionService = permissionService ?? new PermissionService();
        }

        public override bool AllowHost()
        {
            var actionName = GetActionName();
            var pageAccess = _permissionService.GetPageAccess(AppSession.AccId);
            return _permissionService.GetHostPermission(actionName, pageAccess);
        }

        public override bool AllowRead()
        {
            var actionName = GetActionName();
            var pageAccess = _permissionService.GetPageAccess(AppSession.AccId);
            return _permissionService.HasPermission(actionName, pageAccess);
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
                    return (controllerName + '/' + actionName).ToLower();
                }
            }

            return string.Empty;
        }
    }
}