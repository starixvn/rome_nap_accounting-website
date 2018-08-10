using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Tarus.Customer.DataAccess.Casino;
using Fanex.Tarus.Customer.Entities.NG2;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Tarus.Customer.Business.Casino
{
    public class SyncCasinoService : ISyncCasinoService
    {
        private readonly ISyncCasinoRepository _repository = new SyncCasinoRepository();

        public SyncCustomerModel GetCustomersByUsernames(string usernames)
        {
            var customers = usernames.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var model = _repository.GetCustomersByUsernames(usernames);

            foreach (var customer in customers)
            {
                if (!model.Usernames.Any(c => c.Username.Equals(customer, StringComparison.OrdinalIgnoreCase)))
                {
                    model.NotExistUsernames.Add(customer);
                }
            }

            return model;
        }

        public SyncCustomerModel GetUnsyncedCustomers()
            => _repository.GetUnsyncedCustomers();

        public SyncCustomerModel SyncCustomers(SyncCustomerModel model)
        {
            foreach (var item in model.Usernames)
            {
                item.ErrorMessage = null;
                item.IsUpdated = null;
            }

            var usernames = model.Usernames.Where(c => c.IsSelected).ToList();
            var result = _repository.SyncCustomers(usernames);
            foreach (var item in result)
            {
                var customer = usernames.FirstOrDefault(c => c.Username.Equals(item.Key, StringComparison.OrdinalIgnoreCase));

                if (item.Value.ErrorCode == 0)
                {                    
                    customer.IsSync = customer.IsSync3rd = true;
                    customer.ErrorMessage = "This customer was synced successfully";
                    customer.IsUpdated = true;
                }
                else
                {
                    customer.ErrorMessage = "This customer was synced failed";
                    customer.IsUpdated = false;
                }
            }

            return model;
        }
    }
}