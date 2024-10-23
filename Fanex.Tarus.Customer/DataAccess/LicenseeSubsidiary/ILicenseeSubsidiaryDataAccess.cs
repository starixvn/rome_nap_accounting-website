using System.Collections.Generic;
using Fanex.NAP.Framework.Data;
using Fanex.Tarus.Customer.DataAccess.Licensee;
using Fanex.Tarus.Customer.Entities;

namespace Fanex.Tarus.Customer.DataAccess.LicenseeSubsidiary
{
    internal interface ILicenseeSubsidiaryDataAccess : ILicenseeDataAccess
    {
        IEnumerable<string> Generate(ICriteria criteria);

        SubsidiaryResult Create(ICriteria criteria);
    }
}