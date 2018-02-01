namespace Fanex.Tarus.Customer.DataAccess.CustomerLogin
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.Customer.Criteria.CustomerLogin;
    using Fanex.Tarus.Customer.Entities;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public interface ICustomerOTPDataAccess
    {
        Result RemoveOTP(RemoveOTPCriteria criteria);
    }
}
