namespace Fanex.Tarus.Security.Authenticate
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Web;
    using CustomerSetting.Module.Core;
    using CustomerSetting.Module.Core.Criteria;
    using CustomerSetting.Module.Core.Services.Impl;
    using Fanex.CustomerSetting.Client40;
    using Fanex.CustomerSetting.Client40.Models;
    using Sunplus.Accounting.Core.Common;

    public class CustomerSettingClientRegistration : CustomerSettingRegistration
    {
        private CustomerSettingAttribute _customerSettingAttribute = new CustomerSettingAttribute();
        private ServiceBase _service = new ServiceBase();

        public override bool VerifyQueryPermission(PermissionContext permissionContext)
        {
            var hasSpecialPermission = _service.CheckSpecialSitePermission(new CheckSpecialSitePermissionCriteria
            {
                CustID = GetPropertyValue(permissionContext, "customerId"),
                UserID = permissionContext.ActorId
            });

            var allowed = hasSpecialPermission.IsSuccess && _customerSettingAttribute.AllowRead(GetActionName());

            return allowed;
        }

        public override bool VerifyUpdatePermission(PermissionContext permissionContext)
        {
            var hasSpecialPermission = _service.CheckSpecialSitePermission(new CheckSpecialSitePermissionCriteria
            {
                CustID = GetPropertyValue(permissionContext, "customerId"),
                UserID = permissionContext.ActorId
            });

            var allowed = hasSpecialPermission.IsSuccess && _customerSettingAttribute.AllowUpdate(GetActionName());

            return allowed;
        }

        public override bool VerifyMultipleUpdatePermission(PermissionContext permissionContext)
        {
            return false;
        }

        public override SettingContext FetchSettingContext()
        {
            var settingContext = new SettingContext();

            settingContext.ActorId = (int)AppSession.AccId;
            settingContext.ActorName = (string)AppSession.AccUserName;
            settingContext.ActorLevel = 5;
            settingContext.SubAccountId = 0;
            settingContext.Language = "en-US";
            settingContext.Domain = HttpContext.Current.Request.Url.Host;
            settingContext.IP = GetClientIp();

            return settingContext;
        }

        private string GetClientIp()
        {
            var forwardHeader = GetForwardHeader();

            if (!string.IsNullOrEmpty(forwardHeader))
            {
                return forwardHeader;
            }

            return GetInternalAddress();
        }

        private string GetForwardHeader()
        {
            var httpRequest = HttpContext.Current.Request;
            var forwardHeader = httpRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (forwardHeader != null)
            {
                return forwardHeader.Split(',').First().Trim();
            }

            return string.Empty;
        }

        private int? GetPropertyValue(PermissionContext permissionContext, string property)
        {
            var queryString = permissionContext.AuthorizationContext.Controller.ValueProvider.GetValue(property);
            var val = queryString != null ? queryString.AttemptedValue : null;
            if (string.IsNullOrWhiteSpace(val))
            {
                var setting = permissionContext.AuthorizationContext.Controller.ValueProvider.GetValue("setting");
                val = setting != null ? setting.AttemptedValue : string.Empty;
                var pattern = "(\"" + property + "\":)([^\r\n][0-9]+)";

                Match regexMatch = Regex.Match(val, pattern, RegexOptions.IgnoreCase);

                if (regexMatch.Success)
                {
                    val = Regex.Replace(regexMatch.Groups[2].ToString(), @"[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled);
                }
            }

            var result = Utils.ToInt32(val);
            return result;
        }

        private string GetInternalAddress()
        {
            var httpRequest = HttpContext.Current.Request;
            string userHostAddress = httpRequest.UserHostAddress;
            var internalIPAddress = GetInternalIPAddress(userHostAddress);

            if (internalIPAddress == null)
            {
                var hostName = Dns.GetHostName();

                internalIPAddress = GetInternalIPAddress(hostName);
            }

            return internalIPAddress != null ? internalIPAddress.ToString() : string.Empty;
        }

        private IPAddress GetInternalIPAddress(string hostAddress)
        {
            foreach (var itemIPA in Dns.GetHostAddresses(hostAddress))
            {
                if (itemIPA.AddressFamily.ToString().Equals("InterNetwork", StringComparison.OrdinalIgnoreCase))
                {
                    return itemIPA;
                }
            }

            return null;
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