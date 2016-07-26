using System;
using System.Collections.Generic;
using DNAide.Core;
using Fanex.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.IAdmin;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Admin;

namespace Sunplus.Accounting.DataAccess.AdoNet.Admin
{
    public class ExternalSiteDAO : DataAccess, IExternalSiteDAO
    {
        public IEnumerable<string> GetGroupNames()
        {
            IList<string> groups = new List<string>(16);

            using (ObjectDb db = new ObjectDb("SelectGroupName"))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        groups.Add(reader["GroupName"].ToString());
                    }
                }
            }

            return groups;
        }

        public IEnumerable<ExternalSiteSuper> GetSuperList(string groupName)
        {
            var param = new Hashtable
            {
                { "groupname", groupName }
            };
                param.Add("userid", AppSession.AccId);
            using (ObjectDb db = new ObjectDb("SelectInfoCust"))
            {
                return db.Query<ExternalSiteSuper>(param);
            }
        }

        public Result AddSuperToGroup(string username, string groupName, int userId)
        {
            var inputParams = new Hashtable
            {
                { "username", username },
                { "userid", userId },
                { "groupname", groupName }
            };

            using (ObjectDb db = new ObjectDb("AddExistCustomer"))
            {
                db.AssignParamValues(inputParams);
                db.ExecuteNonQuery();

                return GetResultFromOutputHash(db.GetOuputParams());
            }
        }

        public Result RemoveSuperFromGroup(int custId, int userId)
        {
            var param = new Hashtable
            {
                { "custid", custId },
                { "userid", userId }
            };

            using (ObjectDb db = new ObjectDb("DeleteCustomer"))
            {
                db.AssignParamValues(param);
                db.ExecuteNonQuery();

                return GetResultFromOutputHash(db.GetOuputParams());
            }
        }

        public IEnumerable<ExternalSiteExtraPTGroup> GetExtraPTGroups(int custId)
        {
            var param = new Hashtable
            {
                { "custid", custId }
            };

            using (ObjectDb db = new ObjectDb("Acc_CustGroupSelExtraPTGroupByCustID"))
            {
                return db.Query<ExternalSiteExtraPTGroup>(param);
            }
        }

        public Result ChangePrimaryGroup(int custId, string groupName, DateTime effectiveDate, int userId)
        {
            var param = new Hashtable
            {
                { "custid", custId },
                { "groupname", groupName },
                { "effectivedate", effectiveDate },
                { "userid", userId }
            };

            using (ObjectDb db = new ObjectDb("Acc_CustGroupUpdPrimaryExtraPTGroup"))
            {
                db.AssignParamValues(param);
                db.ExecuteNonQuery();

                return GetResultFromOutputHash(db.GetOuputParams());
            }
        }

        private Result GetResultFromOutputHash(Hashtable outputParams)
        {
            return new Result
            {
                ErrorCode = CastAide.AsInt32(outputParams["errcode"]),
                ErrorMessage = CastAide.AsString(outputParams["errmsg"])
            };
        }
    }
}
