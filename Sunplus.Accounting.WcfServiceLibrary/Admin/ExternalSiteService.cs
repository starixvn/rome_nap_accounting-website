using System.Collections.Generic;
using System.ServiceModel.Activation;
using Fanex.CustomerSetting.Module.Core.Criteria;
using Fanex.CustomerSetting.Module.Core.Services.Impl;
using Sunplus.Accounting.Business.Admin;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Admin;

namespace Sunplus.Accounting.WcfServiceLibrary.Admin
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExternalSiteService : IExternalSiteService
    {
        private readonly IExternalSite _externalSiteService;

        public ExternalSiteService()
        {
            _externalSiteService = CommonBootStrapper.Locator.GetInstance<IExternalSite>();
        }

        public IEnumerable<string> GetGroupNames()
        {
            return _externalSiteService.GetGroupNames();
        }

        public ExternalSiteGridBO GetSuperList(string groupName, string username, bool fromcache, int pageIndex, int pageSize)
        {
            return _externalSiteService.GetSuperList(groupName, username, fromcache, pageIndex, pageSize);
        }

        public Result AddSuperToGroup(string username, string groupName)
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

            return _externalSiteService.AddSuperToGroup(username, groupName);
        }

        public Result RemoveSuperFromGroup(int custId, string groupName)
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

            return _externalSiteService.RemoveSuperFromGroup(custId, groupName);
        }

        public IEnumerable<ExternalSiteExtraPTGroup> GetExtraPTGroups(int custId)
        {
            return _externalSiteService.GetExtraPTGroups(custId);
        }

        public Result ChangePrimaryGroup(int custId, string groupName, string effectiveDate)
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

            return _externalSiteService.ChangePrimaryGroup(custId, groupName, effectiveDate);
        }
    }
}