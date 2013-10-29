using System.Collections.Generic;
using System.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess
{
    public interface IHelperDAO
    {
        IEnumerable<Country> GetCountryList();

        Result AddCountry(string countryName);

        DataTable GetSites(Enumerations.AccountingSystem system);

        DataTable GetCust(Enumerations.AccountingSystem system);

        DataTable GetSites_Report_Licensee(Enumerations.AccountingSystem system);
    }
}