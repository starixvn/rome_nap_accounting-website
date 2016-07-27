using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IAdmin;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Admin;

namespace Sunplus.Accounting.Business.Admin
{
    public class ExternalSite : IExternalSite
    {
        private const string SuperListSessionKey = "ExternalSite_SessionKey";

        private readonly IExternalSiteDAO _externalSiteDAO;

        public ExternalSite(IExternalSiteDAO externalSiteDAO)
        {
            _externalSiteDAO = externalSiteDAO;
        }

        public IEnumerable<string> GetGroupNames()
        {
            try
            {
                return _externalSiteDAO.GetGroupNames();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<string>();
        }

        public ExternalSiteGridBO GetSuperList(string groupName, string username, bool fromcache, int pageIndex, int pageSize)
        {
            IEnumerable<ExternalSiteSuper> superList = null;

            if (fromcache)
            {
                superList = (IEnumerable<ExternalSiteSuper>)SessionExtension.GetSession(HttpContext.Current.Session, SuperListSessionKey);
            }

            if (superList == null)
            {
                superList = GetSuperList(groupName, username);
            }

            // Filter by username.
            if (!string.IsNullOrEmpty(username))
            {
                username = username.ToLower();
                superList = superList.Where(s => s.UserName.ToLower().Contains(username));
            }

            return new ExternalSiteGridBO
            {
                Total = superList.Count(),
                Rows = superList
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize)
            };
        }

        public Result AddSuperToGroup(string username, string groupName)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new Result(1, "Please input username!");
            }

            try
            {
                return _externalSiteDAO.AddSuperToGroup(username, groupName, AppSession.AccId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, "Internal server error!");
            }
        }

        public Result RemoveSuperFromGroup(int custId, string groupName)
        {
            try
            {
                return _externalSiteDAO.RemoveSuperFromGroup(custId, groupName, AppSession.AccId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, "Internal server error!");
            }
        }

        public IEnumerable<ExternalSiteExtraPTGroup> GetExtraPTGroups(int custId)
        {
            try
            {
                return _externalSiteDAO.GetExtraPTGroups(custId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<ExternalSiteExtraPTGroup>();
        }

        public Result ChangePrimaryGroup(int custId, string groupName, string effectiveDate)
        {
            DateTime newEffectiveDate;

            try
            {
                if (DateTime.TryParse(effectiveDate, out newEffectiveDate))
                {
                    return _externalSiteDAO.ChangePrimaryGroup(custId, groupName, newEffectiveDate, AppSession.AccId, Utils.GetClientIP());
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new Result(1, "Internal server error!");
        }

        private IEnumerable<ExternalSiteSuper> GetSuperList(string groupName, string username)
        {
            try
            {
                var superList = _externalSiteDAO.GetSuperList(groupName);

                if (superList.Any())
                {
                    SessionExtension.AddSession(HttpContext.Current.Session, SuperListSessionKey, superList);
                }

                return superList;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<ExternalSiteSuper>();
            }
        }
    }
}