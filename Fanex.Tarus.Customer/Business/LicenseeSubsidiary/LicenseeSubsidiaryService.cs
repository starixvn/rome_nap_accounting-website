using System.Collections.Generic;
using Fanex.Tarus.Customer.Constants;
using Fanex.Tarus.Customer.Criteria.LicenseeMember;
using Fanex.Tarus.Customer.Criteria.LicenseeSubsidiary;
using Fanex.Tarus.Customer.DataAccess.LicenseeSubsidiary;
using Fanex.Tarus.Customer.Entities;
using Fanex.Tarus.Infrastructure.Extensions;

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

        private CheckExistAccountResult CheckExistAgent(string agentName, int custDBGroupId)
        {
            var criteria = new CheckExistAccountCriteria(agentName, RoleIdConstants.AGENT, custDBGroupId);

            return licenseeDataAccess.CheckExistAccount(criteria);
        }

        public Result Create(string username, string existedUsername, int userId, string ip)
        {
            var siteDB = ObjectDbExtensions.GetSiteDBConfigByUsername(existedUsername);
            var checkExistResult = CheckExistAgent(existedUsername, siteDB.CustDBGroupID);
            var registerResult = licenseeDataAccess.RegisterAccount(new RegisterAccountCriteria(username, checkExistResult.MRecommend, RoleIdConstants.AGENT, siteDB.Site, siteDB.CustDBGroupID));

            if (registerResult.Item1.IsSuccess)
            {
                var result = licenseeDataAccess.Create(
                    new CreateLicenseeSubsidiaryCriteria(
                        registerResult.Item2,
                        username,
                        existedUsername,
                        checkExistResult.CustId,
                        checkExistResult.MRecommend,
                        userId,
                        ip,
                        siteDB.CustDBGroupID));

                if (!result.IsSuccess)
                {
                    licenseeDataAccess.RollbackInsertAccount(registerResult.Item2);
                }

                return result;
            }

            return registerResult.Item1;
        }
    }
}