using System.Collections.Generic;
using System.Linq;
using Fanex.CustomerSetting.Module.Core.Models;
using Fanex.Tarus.Customer.DangerousGrouping.Criterias;
using Fanex.Tarus.Customer.DangerousGrouping.DataAccess;
using Fanex.Tarus.Customer.DangerousGrouping.Helpers;
using Fanex.Tarus.Customer.DangerousGrouping.Models;
using Fanex.Tarus.Customer.DangerousGrouping.Request;

namespace Fanex.Tarus.Customer.DangerousGrouping.Services
{
    public interface IDangerousGroupingService
    {
        IList<DangerDetail> GetDangerLevelList();

        CustomerDangerResult GetCustomerDanger(string username, int userId);

        Result UpdateCustomerDanger(CustomerDangerUpdateRequest request, int userId, string userHostAddress);

        Result ValdiateUsernames(ValidateUsernamesRequest request, int userId, string userHostAddress);

        Result UpdateMultipleCustomersDanger(MultipleCustomersDangerUpdateRequest multipleCustomersDangerUpdateRequest, int userId, string userHostAddress);
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

        public CustomerDangerResult GetCustomerDanger(string username, int userId)
        {
            return dataAccess.GetCustomerDanger(new GetCustomerDangerCriteria(username, userId));
        }

        public Result UpdateCustomerDanger(CustomerDangerUpdateRequest request, int userId, string userHostAddress)
        {
            var criteria = new UpdateCustomerDangerCriteria
            {
                UserID = userId,
                Username = request.Customer,
                DangerLevel = request.DangerLevel,
                DangerType = request.DangerType,
                UpdateDownline = request.UpdateDownline,
                IP = userHostAddress
            };

            return dataAccess.UpdateCustomerDanger(criteria);
        }

        public Result ValdiateUsernames(ValidateUsernamesRequest request, int userId, string userHostAddress)
        {
            var usernamesCount = request.Usernames.Split(',').Length;
            if (usernamesCount > 100)
            {
                return new Result(-1, "Maximum number of accounts is 100.");
            }
            var criteria = new ValidateCustomerNamesCriteria
            {
                IP = userHostAddress,
                UserID = userId,
                UsernameList = request.Usernames,
                DangerType = request.DangerType,
                DangerLevel = request.DangerLevel
            };

            return dataAccess.ValdiateUsernames(criteria);
        }

        public Result UpdateMultipleCustomersDanger(MultipleCustomersDangerUpdateRequest multipleCustomersDangerUpdateRequest, int userId, string userHostAddress)
        {
            var usernames = multipleCustomersDangerUpdateRequest.CustomerNames.Split(',').Distinct();
            foreach (var username in usernames)
            {
                var criteria = new UpdateCustomerDangerCriteria
                {
                    UserID = userId,
                    Username = username,
                    DangerLevel = multipleCustomersDangerUpdateRequest.DangerLevel,
                    DangerType = multipleCustomersDangerUpdateRequest.DangerType,
                    UpdateDownline = multipleCustomersDangerUpdateRequest.UpdateDownline,
                    IP = userHostAddress
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