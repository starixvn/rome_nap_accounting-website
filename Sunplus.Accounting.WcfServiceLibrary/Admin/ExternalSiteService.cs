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
        public IEnumerable<string> GetGroupNames()
        {
            var external = CommonBootStrapper.Locator.GetInstance<IExternalSite>();

            return external.GetGroupNames();
        }

        public ExternalSiteGridBO GetSuperList(string groupName, string username, bool fromcache, int pageIndex, int pageSize)
        {
            var external = CommonBootStrapper.Locator.GetInstance<IExternalSite>();

            return external.GetSuperList(groupName, username, fromcache, pageIndex, pageSize);
        }

        public Result AddSuperToGroup(string username, string groupName)
        {
            var external = CommonBootStrapper.Locator.GetInstance<IExternalSite>();

            return external.AddSuperToGroup(username, groupName);
        }

        public Result RemoveSuperFromGroup(int custId)
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

            return external.RemoveSuperFromGroup(custId);
        }
    }
}