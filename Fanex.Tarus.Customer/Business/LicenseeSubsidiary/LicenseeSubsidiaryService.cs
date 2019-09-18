using System.Collections.Generic;
using Fanex.Tarus.Customer.Criteria.LicenseeSubsidiary;
using Fanex.Tarus.Customer.DataAccess.LicenseeSubsidiary;
using Fanex.Tarus.Customer.Entities;

namespace Fanex.Tarus.Customer.Business.LicenseeSubsidiary
{
    internal class LicenseeSubsidiaryService : ILicenseeSubsidiaryService
    {
        private readonly ILicenseeSubsidiaryDataAccess licenseeDataAccess;

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
