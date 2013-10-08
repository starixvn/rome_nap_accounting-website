
namespace Fanex.Tarus.DataAccess.Implement.Customer
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using DNAide.Core;
    using DNAide.Utils;
    using Fanex.Data;
    using Fanex.Tarus.DataAccess.Implement.Constants;
    using Fanex.Tarus.DataAccess.Interface.Customer;
    using Fanex.Tarus.Entities.Customer.DepositCustomer;
    using Sunplus.Accounting.Core.Helper;
    using Sunplus.Accounting.Entities;

    public class DepositCustomerAccess : BaseAccess, IDepositCustomerAccess
    {
        public Result NewLicenseeAddNewSuper(NewLicenseePostParam newLicenseePostParam)
        {
            var hashParam = new Hashtable();
            foreach (var param in newLicenseePostParam.PostParams)
            {
                hashParam.Add(param.Name.ToLower(), param.Value);
            }

            hashParam.Add("username", newLicenseePostParam.SuperName);
            hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());
            hashParam.Add("custid", 0);
            hashParam.Add("errcode", 0);
            hashParam.Add("errmsg", string.Empty);
            if (!hashParam.ContainsKey("userpwd"))
            {
                hashParam.Add("userpwd", Encryption.CfsEnCode("123qwe"));
            }

            Result result;
            using (var objDb = new ObjectDb("NewLicensee_AdNewSuper"))
            {
                objDb.AssignParamValues(hashParam);
                objDb.ExecuteNonQuery();

                newLicenseePostParam.SuperId = CastAide.AsInt32(objDb.GetParameterValue("custid"));
                result = new Result
                {
                    ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode")),
                    ErrorMessage = CastAide.AsString(objDb.GetParameterValue("ErrMsg"))
                };
            }

            if (result.ErrorCode != 0)
            {
                return result;
            }

            List<int> succeededIdList = new List<int>();
            succeededIdList.Add(newLicenseePostParam.SuperId);
            if (string.IsNullOrWhiteSpace(newLicenseePostParam.MasterName))
            {
                result.ErrorMessage = string.Join(",", succeededIdList);
                return result;
            }

            result = NewLicenseeAddNewMaster(newLicenseePostParam);
            if (result.ErrorCode != 0)
            {
                return result;
            }

            succeededIdList.Add(newLicenseePostParam.MasterId);
            if (string.IsNullOrWhiteSpace(newLicenseePostParam.AgentName))
            {
                result.ErrorMessage = string.Join(",", succeededIdList);
                return result;
            }

            result = NewLicenseeAddNewAgent(newLicenseePostParam);
            if (result.ErrorCode != 0)
            {
                return result;
            }

            succeededIdList.Add(newLicenseePostParam.AgentId);
            result = NewLicenseeAddNewDummyUser(ref newLicenseePostParam,
                RoleConstants.Master,
                ref result,
                ref succeededIdList);
            if (result.ErrorCode != 0)
            {
                return result;
            }

            result = NewLicenseeAddNewDummyUser(ref newLicenseePostParam,
                RoleConstants.Agent,
                ref result,
                ref succeededIdList);
            if (result.ErrorCode != 0)
            {
                return result;
            }

            result = NewLicenseeAddNewDummyUser(ref newLicenseePostParam,
                RoleConstants.Member,
                ref result,
                ref succeededIdList);
            if (result.ErrorCode != 0)
            {
                return result;
            }

            result.ErrorMessage = string.Join(",", succeededIdList);
            return result;
        }

        public Result NewLicenseeAddNewMaster(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("NewLicensee_AdNewMaster"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("srecommend", newLicenseePostParam.SuperId);
                hashParam.Add("username", newLicenseePostParam.MasterName);
                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("custid", 0);
                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                if (!hashParam.ContainsKey("userpwd"))
                {
                    hashParam.Add("userpwd", Encryption.CfsEnCode("123qwe"));
                }

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                if (errCode == 0)
                {
                    newLicenseePostParam.MasterId = CastAide.AsInt32(objDb.GetParameterValue("custid"));
                    errMsg = newLicenseePostParam.MasterId.ToString();
                }

                return new Result(errCode, errMsg);
            }
        }

        private Result NewLicenseeAddNewDummyUser(ref NewLicenseePostParam newLicenseePostParam,
            int roleId,
            ref Result result,
            ref List<int> succeededIdList)
        {
            switch (roleId)
            {
                case RoleConstants.Master:
                    if (string.IsNullOrWhiteSpace(newLicenseePostParam.DummyMasterName))
                    {
                        result.ErrorMessage = string.Join(",", succeededIdList);
                        return result;
                    }

                    break;
                case RoleConstants.Agent:
                    if (string.IsNullOrWhiteSpace(newLicenseePostParam.DummyAgentName))
                    {
                        result.ErrorMessage = string.Join(",", succeededIdList);
                        return result;
                    }

                    break;
                case RoleConstants.Member:
                    if (string.IsNullOrWhiteSpace(newLicenseePostParam.DummyMemberName))
                    {
                        result.ErrorMessage = string.Join(",", succeededIdList);
                        return result;
                    }

                    break;
                default:
                    break;
            }

            var addResult = NewLicenseeAddNewDummyUser(ref newLicenseePostParam, roleId);
            if (result.ErrorCode != 0)
            {
                result = addResult;
                return result;
            }

            switch (roleId)
            {
                case RoleConstants.Master:
                    succeededIdList.Add(newLicenseePostParam.DummyMasterId);
                    break;
                case RoleConstants.Agent:
                    succeededIdList.Add(newLicenseePostParam.DummyAgentId);
                    break;
                case RoleConstants.Member:
                    succeededIdList.Add(newLicenseePostParam.DummyMemberId);
                    break;
                default:
                    break;
            }

            return result;
        }

        private Result NewLicenseeAddNewDummyUser(ref NewLicenseePostParam newLicenseePostParam,
            int roleId)
        {
            if (roleId != RoleConstants.Master &&
                roleId != RoleConstants.Agent &&
                roleId != RoleConstants.Member)
            {
                return new Result(1, "Wrong role ID level!");
            }

            var hashParam = new Hashtable();
            foreach (var param in newLicenseePostParam.PostParams)
            {
                hashParam.Add(param.Name.ToLower(), 0);
            }

            hashParam.Add("ip", Utils.GetClientIP());
            hashParam.Add("custid", 0);
            hashParam.Add("errcode", 0);
            hashParam.Add("errmsg", string.Empty);
            if (!hashParam.ContainsKey("userpwd"))
            {
                hashParam.Add("userpwd", Encryption.CfsEnCode("123qwe"));
            }

            string storeProcedureName = string.Empty;
            switch (roleId)
            {
                case RoleConstants.Master:
                    storeProcedureName = "NewLicensee_AdNewMaster";
                    hashParam.Add("srecommend", newLicenseePostParam.SuperId);
                    hashParam.Add("username", newLicenseePostParam.DummyMasterName);
                    break;
                case RoleConstants.Agent:
                    storeProcedureName = "NewLicensee_AdNewAgent";
                    hashParam.Add("mrecommend", newLicenseePostParam.DummyMasterId);
                    hashParam.Add("username", newLicenseePostParam.DummyAgentName);
                    break;
                case RoleConstants.Member:
                    storeProcedureName = "NewLicensee_AdNewMember";
                    hashParam.Add("recommend", newLicenseePostParam.DummyAgentId);
                    hashParam.Add("username", newLicenseePostParam.DummyMemberName);
                    break;
                default:
                    return new Result(1, "Wrong role ID level!");
            }

            int errCode = 0;
            string errMsg = string.Empty;
            int custid = 0;
            using (var objDb = new ObjectDb(storeProcedureName))
            {
                objDb.AssignParamValues(hashParam);
                objDb.ExecuteNonQuery();
                errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));
                custid = CastAide.AsInt32(objDb.GetParameterValue("custid"));
            }

            if (errCode != 0)
            {
                return new Result(errCode, errMsg);
            }

            switch (roleId)
            {
                case RoleConstants.Master:
                    newLicenseePostParam.DummyMasterId = custid;
                    break;
                case RoleConstants.Agent:
                    newLicenseePostParam.DummyAgentId = custid;
                    break;
                case RoleConstants.Member:
                    newLicenseePostParam.DummyMemberId = custid;
                    break;
                default:
                    errCode = 0;
                    errMsg = string.Empty;
                    break;
            }

            return new Result(errCode, errMsg);
        }

        public Result NewLicenseeAddNewAgent(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("NewLicensee_AdNewAgent"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("mrecommend", newLicenseePostParam.MasterId);
                hashParam.Add("username", newLicenseePostParam.AgentName);
                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("custid", 0);
                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                if (!hashParam.ContainsKey("userpwd"))
                {
                    hashParam.Add("userpwd", Encryption.CfsEnCode("123qwe"));
                }

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                if (errCode == 0)
                {
                    newLicenseePostParam.AgentId = CastAide.AsInt32(objDb.GetParameterValue("custid"));
                    errMsg = newLicenseePostParam.AgentId.ToString();
                }

                return new Result(errCode, errMsg);
            }
        }

        public Result NewLicenseeAddNewMember(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("NewLicensee_AdNewMember"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("recommend", newLicenseePostParam.AgentId);
                hashParam.Add("username", newLicenseePostParam.MemberName);
                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("custid", 0);
                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                if (!hashParam.ContainsKey("userpwd"))
                {
                    hashParam.Add("userpwd", Encryption.CfsEnCode("123qwe"));
                }

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                if (errCode == 0)
                {
                    newLicenseePostParam.MemberId = CastAide.AsInt32(objDb.GetParameterValue("custid"));
                    errMsg = newLicenseePostParam.MemberId.ToString();
                }

                return new Result(errCode, errMsg);
            }
        }

        public DataSet GetSuperSetting(int custId)
        {
            using (var objDb = new ObjectDb("NewLicensee_GetSuperSetting"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }

        public DataSet GetMasterSetting(int custId)
        {
            using (var objDb = new ObjectDb("NewLicensee_GetMasterSetting"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }

        public DataSet GetAgentSetting(int custId)
        {
            using (var objDb = new ObjectDb("NewLicensee_GetAgentSetting"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }

        public DataSet GetMemberSetting(int custId)
        {
            using (var objDb = new ObjectDb("NewLicensee_GetMemberSetting"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }

        public Result UpdateSuperSetting(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("DepositCust_UpdateSuperSetting"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                return new Result(errCode, errMsg);
            }
        }

        public Result UpdateMasterSetting(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("DepositCust_UpdateMasterSetting"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                return new Result(errCode, errMsg);
            }
        }

        public Result UpdateAgentSetting(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("DepositCust_UpdateAgentSetting"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                return new Result(errCode, errMsg);
            }
        }

        public Result UpdateMemberSetting(NewLicenseePostParam newLicenseePostParam)
        {
            using (var objDb = new ObjectDb("DepositCust_UpdateMemberSetting"))
            {
                var hashParam = new Hashtable();
                foreach (var param in newLicenseePostParam.PostParams)
                {
                    hashParam.Add(param.Name.ToLower(), param.Value);
                }

                hashParam.Add("ip", Sunplus.Accounting.Core.Helper.Utils.GetClientIP());

                hashParam.Add("errcode", 0);
                hashParam.Add("errmsg", string.Empty);

                objDb.AssignParamValues(hashParam);

                objDb.ExecuteNonQuery();
                var errCode = CastAide.AsInt32(objDb.GetParameterValue("ErrCode"));
                var errMsg = CastAide.AsString(objDb.GetParameterValue("ErrMsg"));

                return new Result(errCode, errMsg);
            }
        }


        public DataTable SelAllLicensee()
        {
            using (var objDb = new ObjectDb("Dep_LicenseeSelAll"))
            {
                var dataSet = objDb.ExecuteDataSet();
                if (dataSet.Tables.Count == 0) return null;
                return dataSet.Tables[0];
            }
        }


        public DataSet GetMasterSettingForAddNew(int custId)
        {
            using (var objDb = new ObjectDb("CustomerSelMember_MAgInfo"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }

        public DataSet GetAgentSettingForAddNew(int custId)
        {
            using (var objDb = new ObjectDb("CustomerSelMember_AgInfo"))
            {
                var hashParam = new Hashtable { { "custid", custId } };
                objDb.AssignParamValues(hashParam);
                return objDb.ExecuteDataSet();
            }
        }
    }
}
