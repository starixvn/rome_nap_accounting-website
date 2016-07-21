using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IAdmin;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Admin;
using Sunplus.Accounting.Entities.Customer;

namespace Sunplus.Accounting.Business.Admin
{
    public class ExternalSite : IExternalSite
    {
        private const string ListCustDetailSessionKey = "ExternalSite_SessionKey";

        private readonly IExternalSiteDAO _custGroupDAO;

        public ExternalSite(IExternalSiteDAO custGroupDAO)
        {
            _custGroupDAO = custGroupDAO;
        }

        public IEnumerable<string> GetGroupNames()
        {
            try
            {
                return _custGroupDAO.GetGroupNames();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return new List<string>();
        }

        public ExternalSiteGridBO GetSuperList(string groupName, string username, bool fromcache, int pageIndex, int pageSize)
        {
            IEnumerable<CustDetail> superList = null;

            if (fromcache)
            {
                superList = (IEnumerable<CustDetail>)SessionExtension.GetSession(HttpContext.Current.Session, ListCustDetailSessionKey);
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

            // Paging.
            int totalItems = superList.Count();

            superList = superList
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize);

            return new ExternalSiteGridBO
            {
                total = totalItems,
                rows = superList.ToList()
            };
        }

        public IEnumerable<CustDetail> GetSuperList(string groupName, string username)
        {
            try
            {
                var superList = _custGroupDAO.GetSuperList(groupName);

                if (superList.Any())
                {
                    SessionExtension.AddSession(HttpContext.Current.Session, ListCustDetailSessionKey, superList);
                }

                return superList;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<CustDetail>();
            }
        }

        public Result AddSuperToGroup(string username, string groupName)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new Result(1, "Please input username!");
            }

            try
            {
                return _custGroupDAO.AddSuperToGroup(username, groupName, AppSession.AccId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, "Internal server error!");
            }
        }

        public Result RemoveSuperFromGroup(int custId)
        {
            try
            {
                return _custGroupDAO.RemoveSuperFromGroup(custId, AppSession.AccId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, "Internal server error!");
            }
        }
    }
}