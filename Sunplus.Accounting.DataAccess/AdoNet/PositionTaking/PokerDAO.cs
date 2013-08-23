using System.Collections;
using DNAide.Core;
using DNAide.Data;
using Fanex.Data;
using Sunplus.Accounting.DataAccess.PositionTaking;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess.AdoNet.PositionTaking
{
    public class PokerDAO : DataAccess, IPokerDAO
    {
        public DbResult GetCustomerStatus(int custId)
        {
            DbResult dbResult = null;
            string spMaping = "SelectDisableP2P";
            var param = new Hashtable { { "custid", custId } };

            if (!string.IsNullOrEmpty(spMaping))
            {
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.P2PSession.StoreCollection.P2PControl, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
                return dbResult;
            }
            return null;
        }

        public DbResult GetCustomerSetting(int custId, int roleId)
        {
            DbResult dbResult = null;
            string spMaping = null;
            var param = new Hashtable { { "custid", custId } };
            switch (roleId)
            {
                case (int)Roles.Super:
                    spMaping = "GetSuperSetting";
                    break;

                case (int)Roles.Master:
                    spMaping = "GetMasterSetting";
                    break;

                case (int)Roles.Agent:
                    spMaping = "GetAgentSetting";
                    break;

                case (int)Roles.Member:
                    spMaping = "GetMemberSetting";
                    break;

                default:
                    break;
            }
            if (!string.IsNullOrEmpty(spMaping))
            {
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.P2PSession.StoreCollection.P2PControl, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
                return dbResult;
            }
            return null;
        }

        public Result UpdateP2P(Hashtable Params, int type)
        {
            string spMaping = string.Empty;
            switch (type)
            {
                case 41:
                    spMaping = "Acc_CustomerUpdP2PSuper_Commission";
                    break;

                case 42:
                    spMaping = "Acc_CustomerUpdP2PSuper_GameSetting";
                    break;

                case 43:
                    spMaping = "Acc_CustomerUpdP2PSuper_MaxTransfer";
                    break;

                case 31:
                    spMaping = "Acc_CustomerUpdP2PMaster_Commission";
                    break;

                case 32:
                    spMaping = "Acc_CustomerUpdP2PMaster_GameSetting";
                    break;

                case 33:
                    spMaping = "Acc_CustomerUpdP2PMaster_MaxTransfer";
                    break;

                case 21:
                    spMaping = "Acc_CustomerUpdP2PAgent_Commission";
                    break;

                case 22:
                    spMaping = "Acc_CustomerUpdP2PAgent_GameSetting";
                    break;

                case 23:
                    spMaping = "Acc_CustomerUpdP2PAgent_MaxTransfer";
                    break;

                case 11:
                    spMaping = "Acc_CustomerUpdP2PMember_Commission";
                    break;

                case 12:
                    spMaping = "Acc_CustomerUpdP2PMember_GameSetting";
                    break;

                case 13:
                    spMaping = "Acc_CustomerUpdP2PMember_MaxTransfer";
                    break;

                default:
                    break;
            }
            using (ObjectDb objDb = new ObjectDb(spMaping, 2000))
            {
                objDb.AssignParamValues(Params);
                objDb.ExecuteNonQuery();

                return new Result()
                {
                    ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode")),
                    ErrorMessage = string.Format("{0}", objDb.GetParameterValue("errmsg"))
                };
            }
        }

        public Result UpdateP2PStaus(Hashtable Params)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_CustomerUpdDisableP2P", 2000))
            {
                objDb.AssignParamValues(Params);
                objDb.ExecuteNonQuery();

                return new Result()
                {
                    ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode")),
                    ErrorMessage = string.Format("{0}", objDb.GetParameterValue("errmsg"))
                };
            }
        }

        public DbResult GetDefaultSetting()
        {
            DbResult dbResult = null;
            string spMaping = "SelectDefaultSettingP2P";
            var param = new Hashtable();

            if (!string.IsNullOrEmpty(spMaping))
            {
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.P2PSession.StoreCollection.P2PControl, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
                return dbResult;
            }
            return null;
        }

        public Result UpdateDefaultSettingP2P(Hashtable Params)
        {
            using (ObjectDb objDb = new ObjectDb("Acc_P2PUpdDefaultSetting", 2000))
            {
                objDb.AssignParamValues(Params);
                objDb.ExecuteNonQuery();

                return new Result()
                {
                    ErrorCode = CastAide.AsInt32(objDb.GetParameterValue("errcode")),
                    ErrorMessage = string.Format("{0}", objDb.GetParameterValue("errmsg"))
                };
            }
        }

        public DbResult SelectEnableCustomer(int custId)
        {
            DbResult dbResult = null;
            Hashtable Params = new Hashtable();
            Params.Add("custid", custId);
            Params.Add("errcode", 0);
            Params.Add("errmsg", "");
            string spMaping = "SelectEnableCustomer";

            if (!string.IsNullOrEmpty(spMaping))
            {
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.P2PSession.StoreCollection.P2PControl, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, Params, spElement.DbName, 2000);
            }

            return dbResult;
        }

        public enum Roles
        {
            Super = 4,
            Master = 3,
            Agent = 2,
            Member = 1
        }
    }
}