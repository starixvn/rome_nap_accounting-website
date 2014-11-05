namespace Fanex.Tarus.Security.Authenticate
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Services;

    /// <summary>
    /// Live casino authentication.
    /// </summary>
    public class LiveCasinoAuthorizationService : ILiveCasinoAuthorizationService
    {
        private CustomerSettingAttribute _customerSettingAttribute = new CustomerSettingAttribute();

        public bool IsAuthorizedRead(AuthorizationContext authorizationContext, LiveCasinoSettingContext settingContext)
        {
            var actionName = GetActionName(authorizationContext);
            return _customerSettingAttribute.AllowRead(actionName);
        }

        public bool IsAuthorizedHost(AuthorizationContext authorizationContext, LiveCasinoSettingContext settingContext)
        {
            var actionName = GetActionName(authorizationContext);
            return _customerSettingAttribute.AllowUpdate(actionName);
        }

        public bool IsAuthorizedReadMultiple(AuthorizationContext authorizationContext, LiveCasinoSettingContextMultiple settingContextMultiple)
        {
            return true;
        }

        /// <summary>
        /// Authentication multiple.
        /// </summary>
        /// <param name="authorizationContext">Authorization Context object.</param>
        /// <param name="settingContextMultiple">Setting Context Multiple object.</param>
        /// <returns>Boolean object.</returns>
        public bool IsAuthorizedHostMultiple(AuthorizationContext authorizationContext, LiveCasinoSettingContextMultiple settingContextMultiple)
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