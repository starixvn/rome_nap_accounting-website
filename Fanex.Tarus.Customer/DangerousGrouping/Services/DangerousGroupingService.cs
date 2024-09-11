using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Tarus.Customer.DangerousGrouping.Criterias;
using Fanex.Tarus.Customer.DangerousGrouping.DataAccess;
using Fanex.Tarus.Customer.DangerousGrouping.Helpers;
using Fanex.Tarus.Customer.DangerousGrouping.Models;
using Fanex.Tarus.Customer.DangerousGrouping.Request;
using Fanex.Tarus.Security.Constants;
using Fanex.Tarus.Security.Services;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Tarus.Customer.DangerousGrouping.Services
{
    public interface IDangerousGroupingService
    {
        IList<DangerDetail> GetDangerLevelList();

        CustomerDangerResult GetCustomerDanger(string username, int userId, string adName);

        Result UpdateCustomerDanger(CustomerDangerUpdateRequest request, int userId, string adName, string userHostAddress);

        Result ValdiateUsernames(ValidateUsernamesRequest request, int userId, string adName, string userHostAddress);

        Result UpdateMultipleCustomersDanger(MultipleCustomersDangerUpdateRequest multipleCustomersDangerUpdateRequest, int userId, string adName, string userHostAddress);
    }

    public class DangerousGroupingService : IDangerousGroupingService
    {
        private readonly DangerousGroupingDataAccess dataAccess;
        private readonly DangerousGroupingHelper helper;

        public DangerousGroupingService()
        {
            dataAccess = new DangerousGroupingDataAccess();
            helper = new DangerousGroupingHelper();
        }

        public IList<DangerDetail> GetDangerLevelList()
        {
            return helper.GetDangerSettingOriginal();
        }

        public CustomerDangerResult GetCustomerDanger(string username, int userId, string adName)
        {
            return dataAccess.GetCustomerDanger(new GetCustomerDangerCriteria
                (username, userId, adName, string.Join(",", MangeAccountInfoPermissionBuilder.GetCustomerPermissions(userId, PolicyConst.Read)),
                    AccountCredit.Acc_DeniedSiteCurrency_Sel(userId)));
        }

        public Result UpdateCustomerDanger(CustomerDangerUpdateRequest request, int userId, string adName, string userHostAddress)
        {
            var criteria = new UpdateCustomerDangerCriteria
            {
                UserID = userId,
                AdName = adName,
                Username = request.Customer,
                DangerLevel = request.DangerLevel,
                DangerType = request.DangerType,
                UpdateDownline = request.UpdateDownline,
                IP = userHostAddress,
                Permission = string.Join(",", MangeAccountInfoPermissionBuilder.GetCustomerPermissions(userId, PolicyConst.Read)),
                DeniedList = AccountCredit.Acc_DeniedSiteCurrency_Sel(userId)
            };

            return dataAccess.UpdateCustomerDanger(criteria);
        }

        public Result ValdiateUsernames(ValidateUsernamesRequest request, int userId, string adName, string userHostAddress)
        {
            var usernamesCount = request.Usernames.Split(',').Length;
            if (usernamesCount > 1000)
            {
                return new Result(-1, "Maximum number of accounts is 1000.");
            }
            var criteria = new ValidateCustomerNamesCriteria
            {
                IP = userHostAddress,
                UserID = userId,
                AdName = adName,
                UsernameList = request.Usernames,
                DangerType = request.DangerType,
                DangerLevel = request.DangerLevel,
                Permission = string.Join(",", MangeAccountInfoPermissionBuilder.GetCustomerPermissions(userId, PolicyConst.Read)),
                DeniedList = AccountCredit.Acc_DeniedSiteCurrency_Sel(userId)
            };

            return dataAccess.ValdiateUsernames(criteria);
        }

        public Result UpdateMultipleCustomersDanger(MultipleCustomersDangerUpdateRequest multipleCustomersDangerUpdateRequest, int userId, string adName, string userHostAddress)
        {
            var usernames = multipleCustomersDangerUpdateRequest.CustomerNames.Split(',').Distinct();
            foreach (var username in usernames)
            {
                var criteria = new UpdateCustomerDangerCriteria
                {
                    UserID = userId,
                    AdName = adName,
                    Username = username,
                    DangerLevel = multipleCustomersDangerUpdateRequest.DangerLevel,
                    DangerType = multipleCustomersDangerUpdateRequest.DangerType,
                    UpdateDownline = multipleCustomersDangerUpdateRequest.UpdateDownline,
                    IP = userHostAddress,
                    Permission = string.Join(",", MangeAccountInfoPermissionBuilder.GetCustomerPermissions(userId, PolicyConst.Read)),
                    DeniedList = AccountCredit.Acc_DeniedSiteCurrency_Sel(userId)
                };

                var updateResult = dataAccess.UpdateCustomerDanger(criteria);

                if (!updateResult.IsSuccess)
                {
                    return updateResult;
                }
            }

            return new Result(0, null);
        }
    }
}