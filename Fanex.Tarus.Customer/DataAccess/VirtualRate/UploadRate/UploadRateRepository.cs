// --------------------------------------------------------------------------
// <copyright file="UploadRateRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.VirtualRate.UploadRate
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Business.VirtualRate;
    using Fanex.Tarus.Customer.Constants;
    using Fanex.Tarus.Customer.Criteria.VirtualRate.UploadRate;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.VirtualRate.UploadRate;
    using Fanex.Tarus.Infrastructure.Constants;

    /// <summary>
    /// Upload Rate Repository.
    /// </summary>
    public class UploadRateRepository : IUploadRateRepository
    {
        /// <summary>
        /// Inserts the temporary rate.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Insert result.</returns>
        public CheckImportResult InsertTempRate(InsertTempRateCriteria criteria)
        {
            CheckImportResult result = new CheckImportResult
            {
                FailedInfoList = new List<ImportVirtualRateResult>()
            };

            Hashtable outputParams = new Hashtable();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    var rateString = Converter.ToString2(reader["VirtualRate"]);
                    decimal rateValue;
                    var importVirtualRateResult = new ImportVirtualRateResult
                    {
                        ResultMessage = Converter.ToString2(reader["Reason"]),
                        Username = Converter.ToString2(reader["UserName"]),
                        SportId = Converter.ToInt(reader["sporttype"]),
                        BetTypeId = Converter.ToInt(reader["bettype"]),
                        LiveTypeId = Converter.ToInt(reader["isLive"]),
                        VirtualRate = Converter.ToDecimal(rateString),
                        VirtualRateString = decimal.TryParse(rateString, out rateValue) ?
                               Math.Round((rateValue - 1) * 100).ToString() :
                               rateString
                    };

                    importVirtualRateResult.BetTypeName = VirtualRateSectionService.ShortBetTypeName(importVirtualRateResult.BetTypeId);
                    importVirtualRateResult.SportName = Sports.SportName(importVirtualRateResult.SportId);
                    importVirtualRateResult.LiveTypeName = LiveTypeConstants.LiveTypeName(importVirtualRateResult.LiveTypeId);
                    result.FailedInfoList.Add(importVirtualRateResult);
                }

                outputParams = objDb.GetOuputParams();
            }

            result.ResultCode = Converter.ToInt(outputParams["errcode"]);
            result.ResultMessage = Converter.ToString2(outputParams["errmsg"]);
            return result;
        }

        /// <summary>
        /// Checks the import.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Check result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public CheckImportResult CheckImport(CheckImportCriteria criteria)
        {
            CheckImportResult checkResult = new CheckImportResult
            {
                FailedInfoList = new List<ImportVirtualRateResult>(),
                SucceededInfoList = new List<ImportVirtualRateResult>()
            };

            Hashtable outputParams = new Hashtable();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                outputParams = objDb.GetOuputParams();

                while (reader.Read())
                {
                    var importVirtualRateResult = new ImportVirtualRateResult
                    {
                        Username = Converter.ToString2(reader["UserName"]),
                        SportId = Converter.ToInt(reader["sporttype"]),
                        BetTypeId = Converter.ToInt(reader["bettype"]),
                        LiveTypeId = Converter.ToInt(reader["isLive"]),
                        VirtualRate = Converter.ToDecimal(reader["VirtualRate"]) - 1
                    };

                    importVirtualRateResult.BetTypeName = VirtualRateSectionService.ShortBetTypeName(importVirtualRateResult.BetTypeId);
                    importVirtualRateResult.SportName = Sports.SportName(importVirtualRateResult.SportId);
                    importVirtualRateResult.LiveTypeName = LiveTypeConstants.LiveTypeName(importVirtualRateResult.LiveTypeId);
                    checkResult.SucceededInfoList.Add(importVirtualRateResult);
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var rateString = Converter.ToString2(reader["VirtualRate"]);
                        decimal rateValue;
                        checkResult.FailedInfoList.Add(new ImportVirtualRateResult
                        {
                            Username = Converter.ToString2(reader["username"]),
                            ResultMessage = Converter.ToString2(reader["reason"]),
                            VirtualRateString = decimal.TryParse(rateString, out rateValue) ?
                                Math.Round((rateValue - 1) * 100).ToString() :
                                rateString
                        });
                    }
                }
            }

            checkResult.ResultCode = Converter.ToInt(outputParams["errcode"]);
            checkResult.ResultMessage = Converter.ToString2(outputParams["errmsg"]);
            return checkResult;
        }

        /// <summary>
        /// Inserts the temporary rate.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Insert result.</returns>
        public Result DeleteTempRate(DeleteTempRateCriteria criteria)
        {
            Hashtable outputParams = new Hashtable();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                outputParams = objDb.GetOuputParams();
            }

            var checkResult = new Result
            {
                ErrorCode = Converter.ToInt(outputParams["errcode"]),
                ErrorMessage = Converter.ToString2(outputParams["errmsg"])
            };

            return checkResult;
        }

        /// <summary>
        /// Updates the import rate.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public CheckImportResult UpdateImportRate(UpdateImportRateCriteria criteria)
        {
            CheckImportResult checkResult = new CheckImportResult
            {
                FailedInfoList = new List<ImportVirtualRateResult>()
            };

            Hashtable outputParams = new Hashtable();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                objDb.ExecuteNonQuery();
                outputParams = objDb.GetOuputParams();
            }

            checkResult.ResultCode = Converter.ToInt(outputParams["errcode"]);
            checkResult.ResultMessage = Converter.ToString2(outputParams["errmsg"]);
            return checkResult;
        }
    }
}
