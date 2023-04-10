namespace Fanex.Tarus.Customer.Business.MultipleUpdateSuperStatus
{
    using Fanex.CustomerSetting.Module.Core.Web;
    using Fanex.NAP.Framework.Data;
    using Fanex.Tarus.Customer.CreditCustomer.Business;
    using Fanex.Tarus.Customer.DataAccess.MultipleUpdateSuperStatus;
    using Fanex.Tarus.Customer.Requests.MultipleUpdateSuperStatus;
    using Fanex.Tarus.Customer.ViewModels.MultipleUpdateSuperStatus;
    using Fanex.Tarus.Infrastructure.Helper;
    using Sunplus.Accounting.Business.Customer;
    using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
    using System.Collections.Generic;
    using System.Linq;

    public interface IMultipleUpdateSuperStatusService
    {
        SupersStatusViewModel GetSupersStatus(ICriteria criteria);

        List<UpdateResultEntity> UpdateSupersClosedStatus(UpdateSupersClosedStatusRequest request);

        List<UpdateResultEntity> UpdateSupersSuspendedStatus(UpdateSupersSuspendedStatusRequest request);

        List<UpdateResultEntity> UpdateSupersProtectedStatus(UpdateSupersProtectedStatusRequest request);
    }

    public class MultipleUpdateSuperStatusService : IMultipleUpdateSuperStatusService
    {
        private readonly IMultipleUpdateSuperStatusDataAccess _dataAccess;

        private readonly ICustomerSettingService _customerService;

        public MultipleUpdateSuperStatusService() : this(null, null) { }

        public MultipleUpdateSuperStatusService(IMultipleUpdateSuperStatusDataAccess dataAccess, ICustomerSettingService customerService)
        {
            _dataAccess = dataAccess ?? new MultipleUpdateSuperStatusDataAccess();
            _customerService = customerService ?? new CustomerSettingService();
        }

        public SupersStatusViewModel GetSupersStatus(ICriteria criteria) => _dataAccess.GetSupersStatus(criteria);

        public List<UpdateResultEntity> UpdateSupersClosedStatus(UpdateSupersClosedStatusRequest request)
        {
            var result = new List<UpdateResultEntity>();
            foreach (var cust in request.CustList)
            {
                try
                {
                    CustomerUpdateStatus.CustomerUpdateClosed(AppSession.AccId, cust.CustId, request.IsClosed, Utils.GetClientIP());
                }
                catch (AccException accException)
                {
                    result.Add(new UpdateResultEntity { CustId = cust.CustId, UserName = cust.UserName, ErrCode = 1, ErrMsg = accException.Message });
                }

            }


            //result.Add(new UpdateResultEntity { CustId = 1176070 , UserName = "SuperTest1" , ErrCode=1, ErrMsg ="Test Error"});
            //result.Add(new UpdateResultEntity { CustId = 576313, UserName = "SuperTest2", ErrCode = 2, ErrMsg = "Test Error2" });
            //result.Add(new UpdateResultEntity { CustId = 1, UserName = "SuperTest3", ErrCode = 2, ErrMsg = "Test Error3" });
            return result;
        }

        public List<UpdateResultEntity> UpdateSupersSuspendedStatus(UpdateSupersSuspendedStatusRequest request)
        {
            var result = new List<UpdateResultEntity>();
            foreach (var cust in request.CustList)
            {
                try
                {
                    CustomerUpdateStatus.CustomerUpdateSuspend(AppSession.AccId, cust.CustId, request.IsSuspended, Utils.GetClientIP());
                }
                catch (AccException accException)
                {
                    result.Add(new UpdateResultEntity { CustId = cust.CustId, UserName = cust.UserName, ErrCode = 1, ErrMsg = accException.Message });
                }

            }

            return result;
        }

        public List<UpdateResultEntity> UpdateSupersProtectedStatus(UpdateSupersProtectedStatusRequest request)
        {
            var result = new List<UpdateResultEntity>();
            foreach (var cust in request.CustList)
            {
                if (CustomerAppSettings.IsOfferProtectedMode(cust.SiteLabel))
                {
                    var updateResult = _customerService.UpdateProtectedStatus(cust.CustId, request.IsProtected, AppSession.AccId, Utils.GetClientIP());
                    if (updateResult.ErrorCode != 0)
                    {
                        result.Add(new UpdateResultEntity { CustId = cust.CustId, UserName = cust.UserName, ErrCode = updateResult.ErrorCode, ErrMsg = updateResult.ErrorMessage });
                    }
                }
                else {
                    result.Add(new UpdateResultEntity { CustId = cust.CustId, UserName = cust.UserName, ErrCode = 1, ErrMsg = "Account is not eligible for Protection status" });
                }
            }

            return result;
        }
    }
}
