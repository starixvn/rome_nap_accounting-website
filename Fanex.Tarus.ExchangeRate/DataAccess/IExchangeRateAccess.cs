using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.ExchangeRate.DataAccess
{
    using System.Data;

    public  interface IExchangeRateAccess
    {
       DataTable GetOandaExchangeDaily(string fromDate, string toDate);
    }
}
