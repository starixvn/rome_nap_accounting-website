namespace Fanex.Tarus.Customer.ManageLicenseeCode
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Module.Core.Models;
    using Fanex.Tarus.Customer.ManageLicenseeCode.Criterias;
    using Fanex.Tarus.Customer.ManageLicenseeCode.Models;
    using Fanex.Tarus.Customer.ManageLicenseeCode.Request;

    public interface IService
    {
        IEnumerable<SelectListItem> GetActiveLicensee();

        IEnumerable<SelectListItem> GetStatus();

        SelLicenseeCodeResponse GetLicenseeCodes(SearchRequest request);

        Result AddLicenseeCodes(UpdNewLicenseeCodeRequest licenseeCode, int userId, string ip);

        Result UpdateLicenseeCodes(UpdNewLicenseeCodeRequest licenseeCode, int userId, string ip);

        IEnumerable<Result> ValidateLicenseeCodes(UpdNewLicenseeCodeRequest licenseeCode);
    }
    public class Service : IService
    {
        private readonly IDataAccess dataAccess;
        private readonly BalanceUp balanceUp;

        public Service()
        {
            dataAccess = new DataAccess();
            balanceUp = dataAccess.GetBalanceUp();
        }
        public IEnumerable<SelectListItem> GetActiveLicensee()
        {
            var licensees = dataAccess.GetActiveLicensee();

            return licensees.Select(licensee => new SelectListItem()
            {
                Text = licensee.Name,
                Value = licensee.Id.ToString()
            });
        }
        

        public SelLicenseeCodeResponse GetLicenseeCodes(SearchRequest request)
        {
            var criteria = new LicenseeTransferCodeSelCriteria
            {
                MoveBetsDay = balanceUp.MoveBetsDay,
                LicenseeIds = request.LicenseeIds,
                StatusIds = request.StatusIds,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return dataAccess.GetLicenseeCode(criteria);
        }

        public IEnumerable<SelectListItem> GetStatus()
        {
            var status = new List<SelectListItem>();
            status.Add(new SelectListItem
            {
                Text = "ON",
                Value = "1"
            });
            status.Add(new SelectListItem
            {
                Text = "OFF",
                Value = "2"
            });
            status.Add(new SelectListItem
            {
                Text = "Code Needed",
                Value = "3"
            });
            status.Add(new SelectListItem
            {
                Text = "N/A",
                Value = "4"
            });

            return status;
        }

        public Result AddLicenseeCodes(UpdNewLicenseeCodeRequest licenseeCode, int userId, string ip)
        {
            var criteria = new LicenseeTransferCodeInsCriteria
            {
                LicenseeName = licenseeCode.LicenseeName,
                LicenseeCode = licenseeCode.LicenseeCode,
                CodeStatus = licenseeCode.CodeStatus,
                UserId = userId,
                IP = ip,
                Language = "E",
                ErrorCode = 0,
                ErrorMessage = string.Empty
            };

            return dataAccess.ExecuteNonQuery(criteria);
        }

        public Result UpdateLicenseeCodes(UpdNewLicenseeCodeRequest licenseeCode, int userId, string ip)
        {
            var criteria = new LicenseeTransferCodeUpdCriteria
            {
                LicenseeName = licenseeCode.LicenseeName,
                LicenseeCode = licenseeCode.LicenseeCode,
                CodeStatus = licenseeCode.CodeStatus,
                UserId = userId,
                IP = ip,
                Language = "E",
                ErrorCode = 0,
                ErrorMessage = string.Empty
            };

            return dataAccess.ExecuteNonQuery(criteria);
        }

        public IEnumerable<Result> ValidateLicenseeCodes(UpdNewLicenseeCodeRequest licenseeCode)
        {
            var criteria = new LicenseeTransferCodeValidateCriteria
            {
                LicenseeName = licenseeCode.LicenseeName,
                LicenseeCode = licenseeCode.LicenseeCode
            };

            return dataAccess.ValidateLicenseeCode(criteria);
        }
    }
}
