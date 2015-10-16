namespace Fanex.Tarus.Security.Authenticate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.CustomerSetting.Client40;
    using Fanex.CustomerSetting.Client40.Models;
    using System.Web.Routing;
    using System.Net;
    using System.Web.Mvc;
    using Sunplus.Accounting.Core.Common;

    public class CustomerSettingClientRegistration : CustomerSettingRegistration
    {
        private CustomerSettingAttribute _customerSettingAttribute = new CustomerSettingAttribute();

        public override bool VerifyQueryPermission(PermissionContext permissionContext)
        {
            return _customerSettingAttribute.AllowRead(GetActionName());
        }

        public override bool VerifyUpdatePermission(PermissionContext permissionContext)
        {
            return _customerSettingAttribute.AllowUpdate(GetActionName());
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
                   var  actionName = HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
                   return controllerName + '/' + actionName;
                }
            }

            return string.Empty;
        }
    }
}