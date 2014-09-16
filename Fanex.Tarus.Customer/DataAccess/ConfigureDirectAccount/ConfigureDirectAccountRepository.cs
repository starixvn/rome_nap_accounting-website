// --------------------------------------------------------------------------
// <copyright file="ConfigureDirectAccountRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.AGCasino
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Criteria.ConfigureDirectAccount;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.ConfigureDirectAccount;
    using Sunplus.Accounting.Core.Constant;

    /// <summary>
    /// Configure Direct Account Repository.
    /// </summary>
    public class ConfigureDirectAccountRepository : IConfigureDirectAccountRepository
    {
        /// <summary>
        /// Updates the direct account status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public Result UpdateDirectAccountStatus(UpdateDirectAccountStatusCriteria criteria)
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
        /// Gets the direct account status update effect.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update effect.</returns>
        public UpdateDirectAccountStatusEffectInfo UpdateDirectAccountStatusEffect(UpdateDirectStatusEffectCriteria criteria)
        {
            UpdateDirectAccountStatusEffectInfo result = new UpdateDirectAccountStatusEffectInfo
            {
                Username = criteria.Username,
                CustomerList = new List<CustomerInfo>()
            };

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSPParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    var customerInfo = new CustomerInfo
                    {
                        UserName = Converter.ToString2(reader["username"]),
                        StatusName = Converter.ToString2(reader["Status"]),
                        RoleName = Converter.ToString2(reader["Position"])
                    };

                    result.CustomerList.Add(customerInfo);
                }
            }

            return result;
        }

        /// <summary>
        /// Checks the import direct account.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Import Direct Account Result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public ImportDirectAccountResult CheckImportDirectAccount(CheckImportDirectAccountFileCriteria criteria)
        {
            var importDirectAccountResult = new ImportDirectAccountResult
            {
                SucceededInfoList = new List<ImportDirectAccountResult>(),
                FailedInfoList = new List<ImportDirectAccountResult>()
            };

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSPParams());
                var dataReader = objDb.ExecuteReader();
                Hashtable outputParams = objDb.GetOuputParams();

                importDirectAccountResult.ImportResultCode = Converter.ToInt(outputParams["errcode"]);
                importDirectAccountResult.ImportResultMessage = Converter.ToString2(outputParams["errmsg"]);
                if (importDirectAccountResult.ImportResultCode == ErrorCode.NoneError)
                {
                    while (dataReader.Read())
                    {
                        importDirectAccountResult.SucceededInfoList.Add(new ImportDirectAccountResult
                        {
                            UsernameIndex = Converter.ToInt(dataReader["seq"]),
                            CustomerInfo = new CustomerInfo
                            {
                                UserName = Converter.ToString2(dataReader["username"]),
                                RoleName = Converter.ToString2(dataReader["position"])
                            }
                        });
                    }

                    if (dataReader.NextResult())
                    {
                        while (dataReader.Read())
                        {
                            importDirectAccountResult.FailedInfoList.Add(new ImportDirectAccountResult
                            {
                                UsernameIndex = Converter.ToInt(dataReader["seq"]),
                                ImportResultMessage = Converter.ToString2(dataReader["reason"]),
                                CustomerInfo = new CustomerInfo
                                {
                                    UserName = Converter.ToString2(dataReader["username"]),
                                    RoleName = Converter.ToString2(dataReader["position"])
                                }
                            });
                        }
                    }
                }
            }

            return importDirectAccountResult;
        }

        /// <summary>
        /// Updates the import direct account.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public Result UpdateImportDirectAccount(UpdateImportDirectAccountCriteria criteria)
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
    }
}
