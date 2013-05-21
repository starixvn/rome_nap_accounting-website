
namespace Fanex.Tarus.Customer.Business.Racing
{
    using System;
    using Fanex.PTRacing.BusinessObjects;
    using Fanex.PTRacing.Services;
    using System.Collections;

    public class RacingSettingService: IRacingSettingService
    {
        protected static ICustomerService _customerService;

        public RacingSettingService(): this(null)
        {

        }

        public RacingSettingService(ICustomerService customerService)
        {
            _customerService = customerService ?? new CustomerService();
        }

        public ICustomer GetCustomerInfo(PTRacing.BusinessObjects.IRequestContext requestContext)
        {
            return _customerService.GetCustomer(requestContext);
        }


        public Result SaveCustomerSettings(ICustomer oldCustomerSettings, ICustomer newCustomerSettings, RequestContext currentContext, Hashtable otherSettings)
        {
            return _customerService.SaveCustomer(oldCustomerSettings, newCustomerSettings, currentContext, otherSettings);
        }
    }
}
