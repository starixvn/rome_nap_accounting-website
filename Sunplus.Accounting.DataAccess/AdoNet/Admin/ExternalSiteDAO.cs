using System.Collections.Generic;
using DNAide.Core;
using Fanex.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.IAdmin;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Customer;

namespace Sunplus.Accounting.DataAccess.AdoNet.Admin
{
    public class ExternalSiteDAO : DataAccess, IExternalSiteDAO
    {
        public IEnumerable<string> GetGroupNames()
        {
            IList<string> groups = new List<string>(16);

            using (ObjectDb db = new ObjectDb("SelectGroupName", 600))
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

        public IEnumerable<CustDetail> GetSuperList(string groupName)
        {
            IList<CustDetail> superList = new List<CustDetail>(128);

            var param = new Hashtable
            {
                { "groupname", groupName }
            };
                param.Add("userid", AppSession.AccId);
            using (ObjectDb db = new ObjectDb("SelectInfoCust", 600))
            {
                db.AssignParamValues(param);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        superList.Add(GetCustDetail(reader));
                    }
                }
            }

            return superList;
        }

        public Result AddSuperToGroup(string username, string groupName, int userId)
        {
            var inputParams = new Hashtable
            {
                { "username", username },
                { "userid", userId },
                { "groupname", groupName }
            };

            using (ObjectDb db = new ObjectDb("AddExistCustomer", 600))
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

            using (ObjectDb db = new ObjectDb("DeleteCustomer", 600))
            {
                db.AssignParamValues(param);
                db.ExecuteNonQuery();

                return GetResultFromOutputHash(db.GetOuputParams());
            }
        }

        private CustDetail GetCustDetail(IDataReader reader)
        {
            return new CustDetail
            {
                UserName = reader["username"].ToString(),
                FirstName = reader["firstname"].ToString(),
                LastName = reader["lastname"].ToString(),
                LastLoginIp = reader["iplogin"].ToString(),
                CustId = CastAide.AsInt32(reader["custid"])
            };
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
