// --------------------------------------------------------------------------
// <copyright file="VirtualRateRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.DataAccess.VirtualRate
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Business.VirtualRate;
    using Fanex.Tarus.Customer.Criteria.VirtualRate;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.VirtualRate;
    using Fanex.Tarus.Infrastructure.Constants;

    /// <summary>
    /// Virtual Rate Repository.
    /// </summary>
    public class VirtualRateRepository : IVirtualRateRepository
    {
        /// <summary>
        /// Gets the customer virtual rate.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer virtual rate.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public CustomerVirtualRateModel CustomerVirtualRate(ViewVirtualRateCriteria criteria)
        {
            var result = new CustomerVirtualRateModel
            {
                CustId = criteria.CustId,
                Username = criteria.Username,
                RoleName = criteria.RoleName,
                SportVirtualRateList = new List<SportVirtualRateModel>()
            };

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    result.ActualExchangeRate = Converter.ToString2(reader["ActualRate"]);
                    result.MinVirtualRate = Converter.ToDecimal(reader["Min_VirtualMultiplier"]);
                    result.MaxVirtualRate = Converter.ToDecimal(reader["Max_VirtualMultiplier"]);
                    result.EnableVirtualRate = Converter.ToDecimal(reader["EnableVirtualRate"]) != 0;
                }

                if (reader.NextResult())
                {
                    var sportDictionaryList = VirtualRateSectionService.SportDictionaryList();
                    var sportList = VirtualRateSectionService.SportList();
                    var sportBetTypeIndexList = VirtualRateSectionService.SportBetTypeIndexList();
                    while (reader.Read())
                    {
                        var sportId = Converter.ToInt(reader["sporttype"]);
                        var isLive = Converter.ToBool(reader["isLive"]);
                        var betType = Converter.ToInt(reader["bettype"]);

                        var sportPosition = sportDictionaryList[sportId];
                        var targetIndex = sportBetTypeIndexList[sportPosition.SportIndex];
                        if (!targetIndex.ContainsKey(betType))
                        {
                            continue;
                        }

                        var targetSport = sportPosition.IsSubSport ?
                            sportList[sportPosition.SportIndex].SubSportList[sportPosition.SubSportIndex] :
                            sportList[sportPosition.SportIndex];
                        var betTypeIndex = targetIndex[betType];
                        var rate = Converter.ToDecimal(reader["virtualMultiplier"]);
                        if (isLive && targetSport.LiveVirtualRate != null)
                        {
                            targetSport.LiveVirtualRate[betTypeIndex].HasData = true;
                            targetSport.LiveVirtualRate[betTypeIndex].PercentRate = rate;
                        }
                        else if (targetSport.NonLiveVirtualRate != null)
                        {
                            targetSport.NonLiveVirtualRate[betTypeIndex].HasData = true;
                            targetSport.NonLiveVirtualRate[betTypeIndex].PercentRate = rate;
                        }
                    }

                    result.SportVirtualRateList = sportList;
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the settings.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
        public Result UpdateSettings(UpdateSettingsCriteria criteria)
        {
            Hashtable outputParams = new Hashtable();
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

        /// <summary>
        /// Gets the history log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The history log.</returns>
        public List<HistoryLogEntity> HistoryLog(ViewHistoryLogCriteria criteria)
        {
            var result = new List<HistoryLogEntity>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new HistoryLogEntity
                    {
                        ActionId = Converter.ToInt(reader["ActionId"]),
                        LogDate = Converter.ToDateTime(reader["logdate"]),
                        Username = Converter.ToString2(reader["Username"]),
                        TrackingValue = Converter.ToString2(reader["TrackingValue"])
                    });
                }
            }

            return result;
        }
    }
}
