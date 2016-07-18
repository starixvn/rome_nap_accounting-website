using Fanex.CustomerSetting.Module.Core.Criteria;
using Fanex.CustomerSetting.Module.Core.Services.Impl;
using Fanex.Tarus.Security.Permission;
using Sunplus.Accounting.Business.Admin;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Admin;
using Sunplus.Accounting.Entities.Customer;
using System.Collections.Generic;
using System.ServiceModel.Activation;

namespace Sunplus.Accounting.WcfServiceLibrary.Admin
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExternalSiteService : IExternalSiteService
    {
        public List<string> GetGroupName()
        {
            var external = CommonBootStrapper.Locator.GetInstance<IExternalSite>();

            return external.GetGroupName();
        }

        public List<CustDetail> GetInfoCusts(string groupname, string username)
        {
            var external = CommonBootStrapper.Locator.GetInstance<IExternalSite>();

            return external.GetInfoCusts(groupname, username);
        }

        public ExternalSiteGridBO PagerCust(string groupName, int pageIndex, int pageSize, bool fromcache, string username)
        {
            var external = CommonBootStrapper.Locator.GetInstance<IExternalSite>();

            return external.PagerCustomers(groupName, pageIndex, pageSize, fromcache, username);
        }

        public Result DeleteCustomer(int custId)
        {
            Result result = new Result();
            var permissionService = new ServiceBase();
            var checkSpecialSite = permissionService.CheckSpecialSitePermission(new CheckSpecialSitePermissionCriteria()
            {
                UserID = AppSession.AccId,
                CustID = custId
            });

            if (checkSpecialSite.ErrorCode > 0)
            {
                result.ErrorCode = checkSpecialSite.ErrorCode;
                result.ErrorMessage = checkSpecialSite.ErrorMessage;
                return result;
            }

            var external = CommonBootStrapper.Locator.GetInstance<IExternalSite>();

            return external.DeleteCustGroupName(custId);
        }

        public Result AddCustomer(string username, string groupName)
        {
            Result result = new Result();
            var permissionService = new ServiceBase();
            var checkSpecialSite = permissionService.CheckSpecialSitePermission(new CheckSpecialSitePermissionCriteria()
            {
                UserID = AppSession.AccId,
                CustName = username
            });

            if (checkSpecialSite.ErrorCode > 0)
            {
                result.ErrorCode = checkSpecialSite.ErrorCode;
                result.ErrorMessage = checkSpecialSite.ErrorMessage;
                return result;
            }

            var external = CommonBootStrapper.Locator.GetInstance<IExternalSite>();

            return external.AddCustGroupName(username, groupName);
        }
    }
}