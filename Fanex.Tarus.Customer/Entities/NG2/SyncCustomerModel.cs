using System.Collections.Generic;

namespace Fanex.Tarus.Customer.Entities.NG2
{
    public class SyncCustomerModel
    {
        public SyncCustomerModel()
        {
            NotExistUsernames = new List<string>();
            Usernames = new List<SyncCustomerEntity>();
        }

        public IList<string> NotExistUsernames { get; set; }

        public IList<SyncCustomerEntity> Usernames { get; set; }
    }
}