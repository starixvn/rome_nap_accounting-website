namespace Fanex.Tarus.Security.Authenticate
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.GoldDeluxe.CoreSetting.Services;

    public class GoldDeluxeAuthorizationService : IGoldDeluxeAuthorizationService
    {
        private CustomerSettingAttribute _customerSettingAttribute = new CustomerSettingAttribute();

        public bool IsAuthorizedRead(AuthorizationContext authorizationContext, int custId)
        {
            var actionName = GetActionName(authorizationContext);
            return _customerSettingAttribute.AllowRead(actionName);
        }

        public bool IsAuthorizedHost(AuthorizationContext authorizationContext, int custId)
        {
            var actionName = GetActionName(authorizationContext);
            return _customerSettingAttribute.AllowUpdate(actionName);
        }

        public bool IsAuthorizedReadMultiple(AuthorizationContext authorizationContext, IEnumerable<string> usernames)
        {
            return true;
        }

        public bool IsAuthorizedHostMultiple(AuthorizationContext authorizationContext, IEnumerable<string> usernames)
        {
            return true;
        }

        private string GetActionName(AuthorizationContext authorizationContext)
        {
            var valueProvider = authorizationContext.Controller.ValueProvider;
            return GetActionParameterValue(valueProvider, "controller") + "/" + GetActionParameterValue(valueProvider, "action");
        }

        private string GetActionParameterValue(IValueProvider valueProvider, string parameterName)
        {
            return valueProvider.GetValue(parameterName) != null
                                ? valueProvider.GetValue(parameterName).AttemptedValue
                                : string.Empty;
        }
    }
}
