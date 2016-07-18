using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DNAide.Core;
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
        private const string GroupNameSessionKey = "sessionGroupName";
        private const string ListCustDetailSessionKey = "ExternalSite_SessionKey";

        private readonly IExternalSiteDAO _custGroupDAO;

        public ExternalSite(IExternalSiteDAO custGroupDAO)
        {
            _custGroupDAO = custGroupDAO;
        }

        public List<string> GetGroupName()
        {
            var listGroupName = new List<string>();

            try
            {
                var rawdata = _custGroupDAO.SelGroupName();
                listGroupName.AddRange(from DataRow row in rawdata.Rows select row["GroupName"].ToString());
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return listGroupName;
        }

        public ExternalSiteGridBO PagerCustomers(string groupName, int pageIndex, int pageSize, bool fromcache, string username)
        {
            var externalsitegrid = new ExternalSiteGridBO();
            var listCust = new List<CustDetail>();
            var externalsitegridpaging = new ExternalSiteGridBO();

            try
            {
                var externalsitelistSession = SessionExtension.GetSession(HttpContext.Current.Session, ListCustDetailSessionKey);
                if (externalsitelistSession == null || fromcache == false)
                {
                    listCust = GetInfoCusts(groupName, username);
                }
                else
                {
                    listCust = (List<CustDetail>)externalsitelistSession;
                }

                if (!string.IsNullOrEmpty(username))
                {
                    var filterCust = from t in listCust
                                     where t.UserName.ToLower().Contains(username.ToLower())
                                     select t;

                    listCust = filterCust.ToList();
                }

                externalsitegrid.rows = listCust;
                externalsitegrid.total = listCust.Count;

                // Paging.
                externalsitegridpaging.rows = externalsitegrid.rows.Select(c => c).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                externalsitegridpaging.total = externalsitegrid.total;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return externalsitegridpaging;
        }

        public Result DeleteCustGroupName(int custId)
        {
            return _custGroupDAO.DelCust(custId, AppSession.AccId);
        }

        public Result AddCustGroupName(string username, string groupName)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new Result { ErrorCode = 1, ErrorMessage = "Please input username!" };
            }

            return _custGroupDAO.AddExistCust(username, groupName, AppSession.AccId);
        }

        public List<CustDetail> GetInfoCusts(string groupName, string username)
        {
            var listCust = new List<CustDetail>();
            var rawdata = _custGroupDAO.SelInfoCust(groupName);

            foreach (DataRow row in rawdata.Rows)
            {
                listCust.Add(new CustDetail
                {
                    UserName = row["username"].ToString(),
                    FirstName = row["firstname"].ToString(),
                    LastName = row["lastname"].ToString(),
                    LastLoginIp = row["iplogin"].ToString(),
                    CustId = CastAide.AsInt32(row["custid"])
                });
            }

            // Filter by username if not empty.
            if (listCust.Count > 0)
            {
                SessionExtension.AddSession(HttpContext.Current.Session, ListCustDetailSessionKey, listCust);
            }

            return listCust;
        }
    }
}