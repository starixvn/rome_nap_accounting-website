namespace Fanex.Tarus.Customer.DataAccess.VirtualRate
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Tarus.Customer.Business.VirtualRate;
    using Fanex.Tarus.Customer.Criteria.VirtualRate;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.VirtualRate;
    using Fanex.Tarus.Customer.Entities.VirtualRate.SportBetTypeReferences;
    using Fanex.Tarus.Infrastructure.Caching;

    public class VirtualRateRepository : IVirtualRateRepository
    {
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

            var virtualRateEntities = new List<VirtualRateEntity>();
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
                    while (reader.Read())
                    {
                        virtualRateEntities.Add(new VirtualRateEntity
                        {
                            CustId = Converter.ToInt(reader["CustId"]),
                            SportType = Converter.ToInt(reader["SportType"]),
                            BetType = Converter.ToInt(reader["BetType"]),
                            IsLive = Converter.ToBool(reader["IsLive"]),
                            Rate = Converter.ToDecimal(reader["virtualMultiplier"])
                        });
                    }
                }

                virtualRateEntities = virtualRateEntities.ToList();
                var sportDictionaryList = VirtualRateSectionService.SportDictionaryList();
                var sportList = VirtualRateSectionService.SportList();
                var sportGroups = virtualRateEntities.GroupBy(data => data.SportType);
                var betTypeNames = new Dictionary<int, string>();

                foreach (var sportGroup in sportGroups)
                {
                    var sportPosition = sportDictionaryList[sportGroup.Key];
                    var targetSport = sportPosition.IsSubSport ?
                        sportList[sportPosition.SportIndex].SubSportList[sportPosition.SubSportIndex] :
                        sportList[sportPosition.SportIndex];

                    var betTypeOrderDictionary = new Dictionary<int, int>();
                    var betTypeOrderList = sportList[sportPosition.SportIndex].BetTypeOrder.Split(',').Select(betType => Converter.ToInt(betType));
                    var orderIndex = 0;
                    foreach (var betType in betTypeOrderList)
                    {
                        if (!betTypeOrderDictionary.ContainsKey(betType))
                        {
                            betTypeOrderDictionary.Add(betType, orderIndex++);
                        }
                    }

                    var betTypes = sportGroup.Select(data => data.BetType)
                        .Distinct()
                        .OrderBy(data => betTypeOrderDictionary.ContainsKey(data) ? betTypeOrderDictionary[data] : betTypeOrderDictionary.Count)
                        .ToList();
                    var betTypeIndexes = new Dictionary<int, int>();
                    var index = 0;
                    foreach (var betType in betTypes)
                    {
                        if (!betTypeNames.ContainsKey(betType))
                        {
                            betTypeNames.Add(betType, VirtualRateSectionService.ShortBetTypeName(betType));
                        }

                        targetSport.BetTypeList.Add(new BetTypeModel
                        {
                            Id = betType,
                            Name = betTypeNames[betType]
                        });

                        betTypeIndexes.Add(betType, index++);
                    }

                    targetSport.LiveVirtualRate = VirtualRateSectionService.DefaultVirtualRateList(betTypes.Count);
                    targetSport.NonLiveVirtualRate = VirtualRateSectionService.DefaultVirtualRateList(betTypes.Count);

                    foreach (var virtualRateEntity in sportGroup)
                    {
                        var betTypeIndex = betTypeIndexes[virtualRateEntity.BetType];

                        if (virtualRateEntity.IsLive && targetSport.LiveVirtualRate != null)
                        {
                            targetSport.LiveVirtualRate[betTypeIndex].HasData = true;
                            targetSport.LiveVirtualRate[betTypeIndex].PercentRate = virtualRateEntity.Rate;
                        }
                        else if (targetSport.NonLiveVirtualRate != null)
                        {
                            targetSport.NonLiveVirtualRate[betTypeIndex].HasData = true;
                            targetSport.NonLiveVirtualRate[betTypeIndex].PercentRate = virtualRateEntity.Rate;
                        }
                    }
                }

                result.SportVirtualRateList = sportList;
            }

            return result;
        }

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

        public List<SportBetTypeReferencesEntity> GetSportBetTypeReferences(GetSportBetTypeReferencesCriteria criteria)
        {
            var result = new List<SportBetTypeReferencesEntity>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new SportBetTypeReferencesEntity
                    {
                        SportType = Converter.ToInt(reader["SportType"]),
                        BetType = Converter.ToInt(reader["BetType"]),
                        SportTypeRef = Converter.ToInt(reader["SportTypeRefVirtualRate"]),
                        BetTypeRef = Converter.ToInt(reader["BetTypeRefVirtualRate"])

                        IsActive = Converter.ToBool(reader["IsActive"])
                    });
                }
            }

            return result;
        }
    }
}
