namespace Sunplus.Accounting.DataAccess.IBCDLL.Customer
{
    using System.Collections.Generic;
    using Sunplus.Accounting.Entities.Criteria.ViewPT;
    using Sunplus.Accounting.Entities.Customer.PT.ViewPT;

    public interface IViewCommDCSRepository
    {
        IList<DCSProduct> GetProducts();
    }
}
