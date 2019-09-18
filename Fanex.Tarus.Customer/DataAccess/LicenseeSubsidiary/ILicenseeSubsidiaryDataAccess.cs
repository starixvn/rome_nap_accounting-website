using System.Collections.Generic;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.Entities;

namespace Fanex.Tarus.Customer.DataAccess.LicenseeSubsidiary
{
    internal interface ILicenseeSubsidiaryDataAccess
    {
        IEnumerable<string> Generate(ICriteria criteria);

        Result Create(ICriteria criteria);
    }
}
