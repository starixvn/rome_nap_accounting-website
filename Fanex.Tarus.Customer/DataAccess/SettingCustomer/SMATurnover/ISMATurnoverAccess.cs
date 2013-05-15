using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Fanex.Tarus.Customer.DataAccess.SettingCustomer.SMATurnover
{
    public interface ISMATurnoverAccess
    {
        DataTable GetSportType(int mode);
        DataTable GetLeague(int sport, DateTime fromDate, DateTime toDate);
    }
}
