namespace Fanex.Tarus.Customer.DataAccess.MicroGaming
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.MicroGaming;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.MicroGaming;
    using Fanex.Tarus.Infrastructure.Configurations.MicroGamingBetTypes;
    using Fanex.Tarus.Infrastructure.Constants;

    public class MicroGamingRepository : IMicroGamingRepository
    {
        public MicroGamingCustomerInfo CustomerSetting(GetSettingCriteria criteria)
        {
            var result = new MicroGamingCustomerInfo
            {
                CustomerId = criteria.CustId,
                RoleId = criteria.RoleId,
                ParentId = criteria.ParentId,
                SelectResult = new SelectResult(),
                GameTypeList = new List<GameTypeModel>()
            };

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AddInParameter("CustId", DbType.Int64, criteria.CustId);
                objDb.AddInParameter("SportType", DbType.Int64, criteria.SportId);

                objDb.AddOutParameter("Username", DbType.String, 4000);
                objDb.AddOutParameter("FirstName", DbType.String, 4000);
                objDb.AddOutParameter("LastName", DbType.String, 4000);
                objDb.AddOutParameter("NickName", DbType.String, 4000);
                objDb.AddOutParameter("Phone", DbType.String, 4000);
                objDb.AddOutParameter("MobilePhone", DbType.String, 4000);
                objDb.AddOutParameter("Fax", DbType.String, 4000);
                objDb.AddOutParameter("Currency", DbType.String, 4000);
                objDb.AddOutParameter("SiteName", DbType.String, 4000);

                var reader = objDb.ExecuteDataSet();
                MappingCustomerSetting(reader, result, criteria);

                var output = objDb.GetOuputParams();
                MappingCustomerInfo(output, result);
            }

            return result;
        }

        public Result EditSetting(EditSettingCriteria criteria)
        {
            Hashtable outputParams = new Hashtable();
            var gameTypeList = new List<GameTypeModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                objDb.ExecuteNonQuery();
                outputParams = objDb.GetOuputParams();
            }

            Result updateResult = new Result
            {
                ErrorCode = Converter.ToInt(outputParams["errcode"]),
                ErrorMessage = Converter.ToString2(outputParams["errmsg"])
            };

            return updateResult;
        }

        private void MappingCustomerInfo(Hashtable output, MicroGamingCustomerInfo custInfo)
        {
            custInfo.Username = Converter.ToString2(output["username"]);
            custInfo.FirstName = Converter.ToString2(output["firstname"]);
            custInfo.LastName = Converter.ToString2(output["lastname"]);
            custInfo.NickName = Converter.ToString2(output["nickname"]);
            custInfo.Phone = Converter.ToString2(output["phone"]);
            custInfo.Mobile = Converter.ToString2(output["mobilePhone"]);
            custInfo.Fax = Converter.ToString2(output["fax"]);
        }

        private void MappingCustomerSetting(
            DataSet reader,
            MicroGamingCustomerInfo custInfo,
            GetSettingCriteria criteria)
        {
            var gameTypeList = (new MicroGamingBetTypesSectionServices()).GetGameTypes();
            if (reader.Tables.Count > 0)
            {
                var positionTakingTable = reader.Tables[0];
                foreach (DataRow dataRow in positionTakingTable.Rows)
                {
                    var betTypeId = Converter.ToInt(dataRow["BetType"]);
                    custInfo.GameTypeList.Add(new GameTypeModel
                    {
                        Id = betTypeId,
                        Name = gameTypeList[betTypeId],
                        PositionTaking = Converter.ToDecimal(dataRow["pt"]),
                        MaxPositionTaking = Converter.ToDecimal(dataRow["MaxPositionTaking"]),
                        HasMinPositionTaking = !Convert.IsDBNull(dataRow["MinPositionTaking"]),
                        MinPositionTaking = Converter.ToDecimal(dataRow["MinPositionTaking"])
                    });
                }
            }
        }

        private string GetRolePrefix(int roleId)
        {
            var rolePrefixDictionary = new Dictionary<int, string>() 
            {
                { RoleIdConstants.Super, "s" },
                { RoleIdConstants.Master, "m" },
                { RoleIdConstants.Agent, "a" }
            };

            return rolePrefixDictionary.ContainsKey(roleId) ?
                rolePrefixDictionary[roleId] :
                string.Empty;
        }
    }
}
