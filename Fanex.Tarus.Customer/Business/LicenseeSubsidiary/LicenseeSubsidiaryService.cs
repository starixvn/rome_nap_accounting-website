using System;
using System.Collections.Generic;
using Fanex.Tarus.Customer.Criteria.LicenseeSubsidiary;
using Fanex.Tarus.Customer.DataAccess.LicenseeSubsidiary;
using Fanex.Tarus.Customer.Entities;
using IBCUtils;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;

namespace Fanex.Tarus.Customer.Business.LicenseeSubsidiary
{
    internal class LicenseeSubsidiaryService : ILicenseeSubsidiaryService
    {
        private readonly ILicenseeSubsidiaryDataAccess licenseeDataAccess;
        private const string LicenseeCasinoNo = "02";
        private const int NapSystem = 1;

        public LicenseeSubsidiaryService(ILicenseeSubsidiaryDataAccess licenseeDataAccess = null)
        {
            this.licenseeDataAccess = licenseeDataAccess ?? new LicenseeSubsidiaryDataAccess();
        }

        public IEnumerable<string> Generate(string username, int number)
            => licenseeDataAccess.Generate(new GetCopiedAccountsCriteria(username, number));

        public Result Create(string username, string existedUsername, int userId, string ip)
            => licenseeDataAccess.Create(new CreateLicenseeSubsidiaryCriteria(username, existedUsername, userId, ip));
    }
}
