using System;
using System.Collections.Generic;
using Fanex.Tarus.Customer.Criteria.LicenseeSubsidiary;
using Fanex.Tarus.Customer.DataAccess.LicenseeSubsidiary;
using Fanex.Tarus.Customer.Entities;
using IBCUtils;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Core.Common;
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
        {
            var result = licenseeDataAccess.Create(new CreateLicenseeSubsidiaryCriteria(username, existedUsername, userId, ip));
            if (result.IsSuccess)
            {
                SyncCasino(result.CustId, username, result.ExistedCustId, userId, ip);
            }

            return result;
        }

        private void SyncCasino(int custId, string username, int existedCustId, int userId, string ip)
        {
            try
            {
                var customerInfo = CasinoManager.GetCustInfoIBC(existedCustId);
                if (!customerInfo.CasinoSync)
                {
                    return;
                }

                var casinoCustomer = new CasinoLibrary.SBAgent.Customer();
                var isSuccess = casinoCustomer.AutoInsertAccountFromExisting(
                    LicenseeCasinoNo,
                    custId,
                    existedCustId,
                    username,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    Encryption.CfsEnCode("123qwe"),
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    DateTime.Now,
                    ip,
                    userId,
                    AppSession.AccUserName,
                    NapSystem);

                if (isSuccess)
                {
                    CasinoManager.UpdateCSStatus(custId, true, customerInfo.CasinoDisabled, true, customerInfo.SiteName);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.CasinoExceptionHandler(ex);
            }
        }
    }
}
