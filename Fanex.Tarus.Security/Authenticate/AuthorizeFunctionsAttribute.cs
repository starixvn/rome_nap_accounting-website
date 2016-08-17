using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fanex.AuthenticationFilter.Configurations;
using Fanex.Tarus.Security.Permission;

namespace Fanex.Tarus.Security.Authenticate
{
    public class AuthorizeFunctionsAttribute : AuthorizeAttribute
    {
        private readonly string[] _permissions;

        public AuthorizeFunctionsAttribute(params string[] permissions)
        {
            _permissions = permissions;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            base.AuthorizeCore(httpContext);

            if (_permissions.All(PermissionService.HasPermission))
            {
                return true;
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new PartialViewResult
            {
                ViewName = AuthenticationFilterSection.Instance.DeniedView
            };
        }
    }
}
