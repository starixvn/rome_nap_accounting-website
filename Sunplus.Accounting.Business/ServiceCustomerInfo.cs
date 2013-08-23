using System;
using System.Collections;
using System.ServiceModel.Activation;

using Sunplus.Accounting.Business.Customer;
using Sunplus.Accounting.Core.Helper;

// NOTE: If you change the class name "ServiceCustomerInfo" here, you must also update the reference to "ServiceCustomerInfo" in Web.config.
namespace Sunplus.Accounting.Business
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceCustomerInfo : IServiceCustomerInfo
    {
        private readonly CustomerManager customer = new CustomerManager();
        private string result = "";

        #region IServiceCustomerInfo Members

        public string GenDownlineList(int custId, int roleId, int pageIndex)
        {
            try
            {
                Hashtable ht;
                ht = customer.GenDownlineList(custId, roleId, pageIndex);
                return JsonUtil.ToJson(ht);
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                result = string.Empty;
                return result;
            }
        }

        public string GenOnlineList(int custId)
        {
            try
            {
                result = customer.GenOnlineList(custId);
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                result = string.Empty;
            }
            return result;
        }

        #endregion IServiceCustomerInfo Members
    }
}