using System.Collections;
using System.Data;
using DNAide.Core;
using Fanex.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess.CustomerAdapter.ConsolidatedGroup
{
    public class ConsolidatedGroupAdapter
    {
        public static DataSet GetAllGroup()
        {
            using (ObjectDb objDb = new ObjectDb("Acc_CustomerSelConsolidatedGroup", 600))
            {
                return objDb.ExecuteDataSet();
            }
        }

        public static Result UpdateGroup(string UserName, int GroupID, int isHeader,
            string Remark, int UserID, string IP)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_CustomerUpdConsolidatedGroup", 600))
            {
                var hsParam = new Hashtable();
                hsParam.Add("username", UserName.Trim());
                hsParam.Add("remark", Remark);
                hsParam.Add("userid", UserID);
                hsParam.Add("ip", IP);

                objDb.AssignParamValues(hsParam);

                objDb.ExecuteNonQuery();
                return new Result()
                {
                    ErrorMessage = CastAide.AsString(objDb.GetParameterValue("errmsg")),
                    ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"))
                };
            }
        }

        public static DataSet GetMemberByGroupID(int GroupID)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_CustomerSelByConsolidatedGroupID", 600))
            {
                var hsParam = new Hashtable();
                hsParam.Add("groupid", GroupID);

                objDb.AssignParamValues(hsParam);

                return objDb.ExecuteDataSet();
            }
        }

        public static Result AddUserToGroup(int GroupID, string UserName, string IP)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_CustomerInsConsolidatedGroup_User", 600))
            {
                var hsParam = new Hashtable();
                hsParam.Add("username", UserName.Trim());
                hsParam.Add("groupid", GroupID);
                hsParam.Add("userid", AppSession.AccId);
                hsParam.Add("ip", IP);

                objDb.AssignParamValues(hsParam);

                objDb.ExecuteNonQuery();
                return new Result()
                {
                    ErrorMessage = CastAide.AsString(objDb.GetParameterValue("errmsg")),
                    ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"))
                };
            }
        }

        public static Result RemoveUsers(int GroupID, string CustIDList, string IP)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_CustomerDelConsolidatedGroup", 600))
            {
                var hsParam = new Hashtable();
                hsParam.Add("custidlist", CustIDList);
                hsParam.Add("groupid", GroupID);
                hsParam.Add("userid", AppSession.AccId);
                hsParam.Add("ip", IP);

                objDb.AssignParamValues(hsParam);

                objDb.ExecuteNonQuery();
                return new Result()
                {
                    ErrorMessage = CastAide.AsString(objDb.GetParameterValue("errmsg")),
                    ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"))
                };
            }
        }

        public static Result ChangeHeadline(int GroupID, int CustID, string IP)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_CustomerUpdConsolidatedGroupHeader", 600))
            {
                var hsParam = new Hashtable();
                hsParam.Add("custid", CustID);
                hsParam.Add("groupid", GroupID);
                hsParam.Add("userid", AppSession.AccId);
                hsParam.Add("ip", IP);

                objDb.AssignParamValues(hsParam);

                objDb.ExecuteNonQuery();
                return new Result()
                {
                    ErrorMessage = CastAide.AsString(objDb.GetParameterValue("errmsg")),
                    ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode"))
                };
            }
        }
    }
}