using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Customer.Entities.SettingCustomer.SMATurnover;

namespace Fanex.Tarus.Customer.Business.SettingCustomer.SMATurnover
{
    public interface ISMATurnoverService
    {
        List<BaseModel> GetSportType(int mode);
        List<BaseModel> GetLeague(int sportType, DateTime fromDate, DateTime toDate);

    }
}
