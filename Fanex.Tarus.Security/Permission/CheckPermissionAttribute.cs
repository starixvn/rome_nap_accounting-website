using System.Text;
using System.Web.Mvc;
using Fanex.CustomerSetting.Module.Core.Web;

namespace Fanex.Tarus.Security.Permission
{
    public class CheckPermissionAttribute : AuthorizeAttribute
    {
        public enum Auth
        {
            Read,
            Host
        }

        public Auth Permission { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var path = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "/" + filterContext.ActionDescriptor.ActionName;
            var hasPermission = Permission == Auth.Host ? PermissionService.CheckHostPermission(path, AppSession.AccId) : PermissionService.CheckReadPermission(path, AppSession.AccId);

            if (!hasPermission)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new JsonResult()
                    {
                        Data = new { IsSuccess = false, ErrorCode = 1, ErrorMessage = "You're not authorized on this function" },
                        ContentEncoding = Encoding.UTF8,
                        ContentType = "application/json",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue,
                        RecursionLimit = int.MaxValue
                    };
                }
                else
                {
                    filterContext.Result = new PartialViewResult()
                    {
                        ViewName = "~/Areas/CustomerSetting/Views/Security/AccessDenied.cshtml"
                    };
                }
            }

            base.OnAuthorization(filterContext);
        }
    }
}
