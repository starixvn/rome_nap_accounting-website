namespace Fanex.Tarus.Customer.DataAccess.SettingCustomer.OpenCustomer
{
    using Criteria.OpenCustomer;
    using Entities.SettingCustomer.OpenCustomer;
    using System;
    using System.Collections.Generic;

    public interface OpenCustomerDataAccess
    {
        IEnumerable<SingleOpenAffectedDownline> GetSingleOpenAffectedDownlines(GetSingleOpenCustomerInfoCriteria criteria);

        Tuple<int, List<SingleOpenDownline>> GetSingleOpenDownlines(GetSingleOpenDownlinesInfoCriteria criteria);
    }
}