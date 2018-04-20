namespace Fanex.Tarus.Customer.Business.CustomerLogin
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using DNAide.Core;
    using Fanex.Security;
    using Fanex.Tarus.Customer.Criteria.CustomerLogin;
    using Fanex.Tarus.Customer.DataAccess.CustomerLogin;
    using Fanex.Tarus.Customer.Entities;
    using IBCUtils;
    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Extensions;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class CustomerOTPService : ICustomerOTPService
    {
        private readonly ICustomerOTPDataAccess _dataAccess;

        public CustomerOTPService()
            : this(null)
        {
        }

        public CustomerOTPService(ICustomerOTPDataAccess dataAccess)
        {
            this._dataAccess = dataAccess ?? new CustomerOTPDataAccess();
        }

        public Result RemoveOTP(int custId)
        {
            return this._dataAccess.RemoveOTP(new RemoveOTPCriteria
            {
                Custid = custId,
                Userid = AppSession.AccId,
                IP = HttpContext.Current.Request.UserHostAddress
            });
        }
    }
}
