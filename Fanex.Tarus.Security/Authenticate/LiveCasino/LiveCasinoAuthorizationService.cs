namespace Fanex.Tarus.Security.Authenticate
{
    using System.Web.Mvc;
    using Fanex.LiveCasino.CoreSetting.Models;
    using Fanex.LiveCasino.CoreSetting.Services;

    /// <summary>
    /// Live casino authentication.
    /// </summary>
    public class LiveCasinoAuthorizationService : ILiveCasinoAuthorizationService
    {
        private CustomerSettingAttribute _customerSettingAttribute = new CustomerSettingAttribute();

        public bool IsAuthorizedRead(AuthorizationContext authorizationContext, SettingContext settingContext)
        {
            var actionName = GetActionName(authorizationContext);
            return _customerSettingAttribute.AllowRead(actionName);
        }

        public bool IsAuthorizedHost(AuthorizationContext authorizationContext, SettingContext settingContext)
        {
            var actionName = GetActionName(authorizationContext);
            return _customerSettingAttribute.AllowUpdate(actionName);
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