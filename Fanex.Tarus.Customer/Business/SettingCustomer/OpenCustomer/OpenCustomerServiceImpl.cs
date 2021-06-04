namespace Fanex.Tarus.Customer.Business.SettingCustomer.OpenCustomer
{
    using Criteria.OpenCustomer;
    using CustomerSetting.Module.Core.Models;
    using DataAccess.SettingCustomer.OpenCustomer;
    using Requests.CustomerSetting.OpenCustomer;
    using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
    using ViewModels.CustomerSetting.OpenCustomer;

    public class OpenCustomerServiceImpl : OpenCustomerService
    {
        private readonly OpenCustomerDataAccess openCustomerDataAccess;

        public OpenCustomerServiceImpl() : this(null)
        {
        }

        public OpenCustomerServiceImpl(OpenCustomerDataAccess openCustomerDataAccess)
        {
            this.openCustomerDataAccess = openCustomerDataAccess ?? new OpenCustomerDataAccessImpl();
        }

        public SingleOpenCustomerViewModel GetSingleOpenCustomerInfo(GetSingleOpenCustomerInfoQuery query)
        {
            var criteria = new GetSingleOpenCustomerInfoCriteria(query.CustId);
            var affectedDownlines = openCustomerDataAccess.GetSingleOpenAffectedDownlines(criteria);

            return new SingleOpenCustomerViewModel(query, affectedDownlines);
        }

        public SingleOpenDownlinesViewModel GetSingleOpenDownlinesInfo(GetSingleOpenDownlinesInfoQuery query)
        {
            var criteria = new GetSingleOpenDownlinesInfoCriteria(
                query.CustId,
                query.IsAffected,
                query.DownlineRoleId,
                query.PageSize,
                query.PageIndex);

            var data = openCustomerDataAccess.GetSingleOpenDownlines(criteria);

            return new SingleOpenDownlinesViewModel(data.Item1, data.Item2);
        }

        public Result UpdateSingleCustomerClosedStatus(UpdateClosedStatusQuery query)
        {
            try
            {
                CustomerUpdateStatus.CustomerUpdateClosed(query.AccId, query.CustId, query.IsClosed, query.Ip);
            }
            catch (AccException exception)
            {
                return new Result(100, exception.Message);
            }

            return new Result(0, string.Empty);
        }

        public ClosedMatrixViewModel GetClosedMatrixInfo(string username)
            => openCustomerDataAccess.GetClosedMatrixInfo(new GetCloseInfoMatrixCriteria(username));
    }
}