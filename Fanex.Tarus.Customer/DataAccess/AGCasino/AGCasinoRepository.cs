// --------------------------------------------------------------------------
// <copyright file="AGCasinoRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.AGCasino
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.AGCasino;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.AGCasino;

    /// <summary>
    /// AG Casino Repository.
    /// </summary>
    public class AGCasinoRepository : IAGCasinoRepository
    {
        /// <summary>
        /// Gets the customer status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Customer Info.</returns>
        public AGCasinoCustomerInfo CustomerStatus(GetCustomerStatusCriteria criteria)
        {
            AGCasinoCustomerInfo result = new AGCasinoCustomerInfo
            {
                CustomerId = criteria.CustomerId,
                RoleId = criteria.RoleId
            };
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSPParams());
                var reader = objDb.ExecuteReader();
                MappingCustomerInfo(reader, result);

                if (reader.NextResult())
                {
                    MappingCustomerSetting(reader, result);
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the customer status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update Result.</returns>
        public Result UpdateCustomerStatus(UpdateCustomerStatusCriteria criteria)
        {
            Hashtable outputParams = new Hashtable();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSPParams());
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

        /// <summary>
        /// Get the customer setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Customer Setting.</returns>
        public AGCasinoCustomerInfo CustomerSetting(GetCustomerSettingCriteria criteria)
        {
            AGCasinoCustomerInfo result = new AGCasinoCustomerInfo
            {
                CustomerId = criteria.CustomerId,
                RoleId = criteria.RoleId,
                UplineId = criteria.UplineId,
                ExchangeId = criteria.ExchangeId,
                SelectResult = new SelectResult()
            };

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSPParams());
                var reader = objDb.ExecuteReader();
                MappingCustomerInfo(reader, result);

                if (reader.NextResult())
                {
                    MappingCustomerSetting(reader, result);
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the customer status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update Result.</returns>
        public Result UpdateCustomerSetting(UpdateCustomerSettingCriteria criteria)
        {
            Hashtable outputParams = new Hashtable();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSPParams());
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

        /// <summary>
        /// Mappings the customer information.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="custInfo">The customer information.</param>
        private void MappingCustomerInfo(IDataReader reader, AGCasinoCustomerInfo custInfo)
        {
            while (reader.Read())
            {
                custInfo.Username = Converter.ToString2(reader["Username"]);
                custInfo.FirstName = Converter.ToString2(reader["Firstname"]);
                custInfo.LastName = Converter.ToString2(reader["Lastname"]);
                custInfo.NickName = Converter.ToString2(reader["Nickname"]);
                custInfo.Phone = Converter.ToString2(reader["Phone"]);
                custInfo.Mobile = Converter.ToString2(reader["MobilePhone"]);
                custInfo.Fax = Converter.ToString2(reader["Fax"]);
                custInfo.SiteName = Converter.ToString2(reader["SiteName"]);
                custInfo.ExchangeId = Converter.ToInt(reader["Currency"]);
                break;
            }
        }

        /// <summary>
        /// Mappings the customer setting.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="custInfo">The customer information.</param>
        private void MappingCustomerSetting(IDataReader reader, AGCasinoCustomerInfo custInfo)
        {
            while (reader.Read())
            {
                custInfo.AGCasinoCheckedGroup = Converter.ToString2(reader["CheckedGroup"]);
                custInfo.DefaultAGCasinoGroup = new BaseModel
                {
                    Name = Converter.ToString2(reader["DefaultGroup"])
                };
                custInfo.EnableAGCasino = Converter.ToBool(reader["IsEnabled"]);
                break;
            }
        }
    }
}
