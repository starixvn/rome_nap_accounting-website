using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
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
                superList = GetSuperList(groupName);
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

        public IEnumerable<Result> AddSuperToGroup(string username, string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName) || string.IsNullOrWhiteSpace(username))
            {
                return new List<Result> { new Result(1, "Invalid group or username.") };
            }

            var results = new List<Result>();

            try
            {
                var result = _externalSiteDAO.AddSuperToGroup(username, groupName, AppSession.AccId);

                if (result.ErrorCode != 0)
                {
                    results.Add(result);
                }
                else
                {
                        results.AddRange(ProccessResultMessage(result.ErrorMessage));
                }

                return results;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                results.Add(new Result(1, "Internal server error!"));
            }

            return results;
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

        private IEnumerable<ExternalSiteSuper> GetSuperList(string groupName)
        {
            try
            {
                var superList = _externalSiteDAO.GetSuperList(groupName, AppSession.AccId);

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

        private IEnumerable<Result> ProccessResultMessage(string xml)
        {
            IList<Result> results = new List<Result>();

            if (!string.IsNullOrEmpty(xml))
            {
                int errCode;
                string groupName;
                IDictionary<string, int> successHash = new Dictionary<string, int>();

                var root = XElement.Parse(xml);

                foreach (XElement item in root.Elements())
                {
                    var superName = item.Attribute("SuperName").Value;
                    groupName = item.Attribute("GroupName").Value;
                    int.TryParse(item.Attribute("ErrCode").Value, out errCode);

                    if (errCode == 0)
                    {
                        if (successHash.ContainsKey(groupName))
                        {
                            successHash[groupName]++;
                        }
                        else
                        {
                            successHash.Add(groupName, 1);
                        }

                        _externalSiteDAO.InitSuperInfo(superName);
                    }
                    else
                    {
                        results.Add(
                            new Result(errCode, $"{groupName} - {item.Attribute("SuperName").Value}: {item.Attribute("ErrMsg").Value}"));
                    }
                }

                if (successHash.Count > 0)
                {
                    var successMessage = successHash.Select(kvp => $"{kvp.Key} - {kvp.Value} super(s)");

                    results.Add(new Result(0, $"Successfully added: {string.Join("; ", successMessage)}"));
                }
            }

            return results;
        }
    }
}