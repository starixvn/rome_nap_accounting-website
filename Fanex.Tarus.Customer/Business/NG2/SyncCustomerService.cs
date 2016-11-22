using System.Linq;
using Fanex.CustomerSetting.Module.Core.Web;
using Fanex.Tarus.Customer.DataAccess.NG2;
using Fanex.Tarus.Customer.Entities.NG2;
using ONEbook.NG2.API;
using ONEbook.NG2.API.DTO.CreateAccount;

namespace Fanex.Tarus.Customer.Business.NG2
{
    public class SyncCustomerService : ISyncCustomerService
    {
        private readonly ISyncCustomerRepository _repo = new SyncCustomerRepository();
        
        public SyncCustomerModel GetCustomersByUsernames(string usernames)
        {            
            var resultByUsernames = _repo.GetCustomersByUsernames(usernames);
            var custArray = resultByUsernames.Usernames.Select(u => u.CustId);
            var custIds = string.Join(",", custArray);

            var result = _repo.GetCustomersByCustIds(custIds);
            result.NotExistUsernames = resultByUsernames.NotExistUsernames;

            var api = new Services();
            var request = new ONEbook.NG2.API.DTO.CheckIsSyncCustomer.Request();
            request.Auth.SiteId = 2000;
            request.UserId = AppSession.CurrentSession.UserId;

            foreach (var custId in custArray)
            {
                request.Call.AccountArray.Add(new ONEbook.NG2.API.DTO.CheckIsSyncCustomer.AccountInfo { ObCustId = custId });
            }

            var response = api.CheckIsSyncCustomers(request);
            var apiResult = response.Result;

            if (apiResult.IsSuccess)
            {
                foreach (var cust in apiResult.AccountArray)
                {
                    result.Usernames.Where(u => u.CustId == cust.ObCustId).FirstOrDefault().IsSync3rd = cust.IsCreated == 1;
                }
            }
            
            return result;
        }

        public SyncCustomerModel SyncCustomers(SyncCustomerModel model)
        {
            var custIds = string.Join(",", model.Usernames.Select(u => u.CustId));
            var usernames = string.Join(",", model.Usernames.Select(u => u.Username));
            var customers = model.Usernames.GroupBy(u => new { u.Currency, u.UplineId });

            var api = new Services();
            
            foreach (var customer in customers)
            {
                var request = new Request();
                request.Auth.SiteId = 2000;
                request.UserId = AppSession.CurrentSession.UserId;

                var first = customer.FirstOrDefault();
                request.Call.CurrencyId = first.Currency;
                request.Call.Level = first.RoleId;
                request.Call.UplineId = first.UplineId;

                foreach (var cust in customer)
                {
                    if (cust.IsSync == cust.IsSync3rd == false || cust.IsSync != cust.IsSync3rd)
                    {
                        request.Call.AccountArray.Add(new AccountInfo { ObCustId = cust.CustId });
                    }
                }

                if (request.Call.AccountArray.Count > 0)
                {
                    var apiResult = api.SyncCustomers(request);
                    if (apiResult.Result.IsSuccess && apiResult.Result.AccountArray.Count > 0)
                    {
                        custIds = string.Join(",", apiResult.Result.AccountArray.Where(u => u.IsCreated == 1).Select(u => u.ObCustId));

                        if (custIds.Length > 0)
                        {
                            _repo.SyncCustomers(custIds, true, 23);
                        }
                    }
                }
            }
                        
            var result = GetCustomersByUsernames(usernames);

            return result;
        }
    }
}