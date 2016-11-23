using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public SyncCustomerModel SyncCustomers(SyncCustomerModel model)
        {
            var custIds = string.Join(",", model.Usernames.Select(u => u.CustId));
            var usernames = string.Join(",", model.Usernames.Select(u => u.Username));
            var customers = model.Usernames.GroupBy(u => new { u.Currency, u.UplineId });
            var api = new Services();
            IEnumerable<AccountInfoResult> resultInfo = new List<AccountInfoResult>();            

            foreach (var customer in customers)
            {
                var first = customer.FirstOrDefault();
                var request = new Request();

                request.Auth.SiteId = 2000;
                request.UserId = AppSession.CurrentSession.UserId;
                request.Call.CurrencyId = first.Currency;
                request.Call.Level = first.RoleId;
                request.Call.UplineId = first.UplineId;

                foreach (var cust in customer)
                {
                    if (cust.IsSelected && (cust.IsSync == cust.IsSync3rd == false || cust.IsSync != cust.IsSync3rd))
                    {
                        request.Call.AccountArray.Add(new AccountInfo { ObCustId = cust.CustId });
                    }
                }

                if (request.Call.AccountArray.Count > 0)
                {
                    var apiResult = api.SyncCustomers(request);
                    resultInfo = resultInfo.Concat(apiResult.Result.AccountArray);

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
            if (resultInfo.Count() > 0)
            {
                foreach (var item in result.Usernames)
                {
                    var resultItem = resultInfo.Where(r => r.ObCustId == item.CustId);

                    if (resultItem.Count() > 0)
                    {
                        item.IsUpdated = resultItem.FirstOrDefault().IsSuccess;
                    }                    
                }
            }

            return result;
        }
    }
}