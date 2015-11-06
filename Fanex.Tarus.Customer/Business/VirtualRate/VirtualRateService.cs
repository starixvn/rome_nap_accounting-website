namespace Fanex.Tarus.Customer.Business.VirtualRate
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Xml.Linq;
    using Fanex.Common;
    using Fanex.Tarus.Customer.Business.VirtualRate.UploadRate;
    using Fanex.Tarus.Customer.Criteria.VirtualRate;
    using Fanex.Tarus.Customer.Criteria.VirtualRate.UploadRate;
    using Fanex.Tarus.Customer.DataAccess.VirtualRate;
    using Fanex.Tarus.Customer.Entities;
    using Fanex.Tarus.Customer.Entities.VirtualRate;
    using Fanex.Tarus.Customer.Entities.VirtualRate.SportBetTypeReferences;
    using Fanex.Tarus.Customer.Entities.VirtualRate.UploadRate;
    using Fanex.Tarus.Customer.Resources;
    using Fanex.Tarus.Infrastructure.Caching;
    using Fanex.Tarus.Infrastructure.Constants;
    using Fanex.Tarus.Infrastructure.Extensions;

    public class VirtualRateService : IVirtualRateService
    {
        private readonly IVirtualRateRepository _virtualRateRepository;

        private readonly IUploadVirtualRateService _uploadVirtualRateService;

        public VirtualRateService()
            : this(null, null)
        {
        }

        public VirtualRateService(
            IVirtualRateRepository virtualRateRepository,
            IUploadVirtualRateService uploadVirtualRateService)
        {
            _virtualRateRepository = virtualRateRepository ?? new VirtualRateRepository();
            _uploadVirtualRateService = uploadVirtualRateService ?? new UploadVirtualRateService();
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public CustomerVirtualRateModel CustomerVirtualRate(ViewVirtualRateCriteria criteria)
        {
            try
            {
                var result = _virtualRateRepository.CustomerVirtualRate(criteria);
                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        public Result UpdateSettings(UpdateSettingsCriteria criteria)
        {
            try
            {
                Result updateResult = _virtualRateRepository.UpdateSettings(criteria);
                if (updateResult.ErrorCode == 0)
                {
                    updateResult.ErrorMessage = Customer_Labels.VirtualRate_UpdateSuccess;
                }

                return updateResult;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        public HistoryLogReportModel HistoryLog(ViewHistoryLogCriteria criteria)
        {
            try
            {
                HistoryLogReportModel result = new HistoryLogReportModel
                {
                    CustId = criteria.CustId,
                    Username = criteria.Username,
                    HistoryLog = new List<HistoryLogByDateModel>()
                };

                var rawData = _virtualRateRepository.HistoryLog(criteria).OrderByDescending(m => m.LogDate).ThenBy(m => m.ActionId);
                var logGroups = rawData.GroupBy(log => new
                {
                    LogDate = log.LogDate,
                    Username = log.Username
                });

                Dictionary<int, Dictionary<int, int>> betTypeIndexList = VirtualRateSectionService.SportBetTypeIndexListViewLog();
                foreach (var data in rawData)
                {
                    var historyLogByDate = new HistoryLogByDateModel
                    {
                        EditDateString = data.LogDate.ToString("MM/dd/yyyy hh:mm:ss tt"),
                        Editor = data.Username,
                        HistoryLogDetailList = ParseHistoryLog(data, betTypeIndexList)
                    };

                    if (historyLogByDate.HistoryLogDetailList != null &&
                        historyLogByDate.HistoryLogDetailList.Count > 0)
                    {
                        result.HistoryLog.Add(historyLogByDate);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        public void DownloadImportRateTemplate()
        {
            _uploadVirtualRateService.DownloadImportRateTemplate();
        }

        public UploadResult UpdateImportRate(UpdateImportRateCriteria criteria)
        {
            return _uploadVirtualRateService.UpdateImportRate(criteria);
        }

        public Result DeleteTempRate()
        {
            return _uploadVirtualRateService.DeleteTempRate();
        }

        public List<SportBetTypeReferencesModel> GetSportBetTypeReferences(GetSportBetTypeReferencesCriteria criteria)
        {
            try
            {
                var result = new List<SportBetTypeReferencesModel>();
                var rawData = _virtualRateRepository.GetSportBetTypeReferences(criteria);
                var sportGroups = rawData.GroupBy(referrence => referrence.SportTypeRef);

                var sportNameDictionary = new Dictionary<int, string>();
                var betTypeNameDictionary = new Dictionary<int, string>();
                foreach (var sportGroup in sportGroups)
                {
                    CacheResourceName(sportGroup.Key, sportNameDictionary, CacheRef.GetSportName);
                    var sport = new SportBetTypeReferencesModel
                    {
                        Id = sportGroup.Key,
                        Name = sportNameDictionary[sportGroup.Key],
                        SubTypes = new List<SportBetTypeReferencesModel>()
                    };

                    var betTypeGroups = sportGroup.GroupBy(referrence => referrence.BetTypeRef);
                    foreach (var betTypeGroup in betTypeGroups)
                    {
                        CacheResourceName(betTypeGroup.Key, betTypeNameDictionary, CacheRef.GetBettype);
                        var betType = new SportBetTypeReferencesModel
                        {
                            Id = betTypeGroup.Key,
                            Name = betTypeNameDictionary[betTypeGroup.Key],
                            SourceReferences = new List<SourceReferenceModel>()
                        };

                        foreach (var data in betTypeGroup)
                        {
                            CacheResourceName(data.SportType, sportNameDictionary, CacheRef.GetSportName);
                            CacheResourceName(data.BetType, betTypeNameDictionary, CacheRef.GetBettype);
                            var sourceReferrence = new SourceReferenceModel
                            {
                                SportTypeRef = new BaseModel
                                {
                                    Id = data.SportTypeRef,
                                    Name = sportNameDictionary[data.SportType]
                                },
                                BetTypeRef = new BaseModel
                                {
                                    Id = data.BetTypeRef,
                                    Name = betTypeNameDictionary[data.BetType]
                                }
                            };

                            betType.SourceReferences.Add(sourceReferrence);
                        }

                        sport.SubTypes.Add(betType);
                    }

                    result.Add(sport);
                }

                return result;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return null;
        }

        private void CacheResourceName(int resourceId, Dictionary<int, string> resourceDictionary, Func<object, string> getResourceNameFunction)
        {
            if (!resourceDictionary.ContainsKey(resourceId))
            {
                resourceDictionary.Add(resourceId, getResourceNameFunction(resourceId));
            }
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private List<HistoryLogDetailModel> ParseHistoryLog(
            HistoryLogEntity historyLog,
            Dictionary<int, Dictionary<int, int>> betTypeIndexList)
        {
            var historyLogDetailList = new List<HistoryLogDetailModel>();
            var xmlLoad = XElement.Parse(historyLog.TrackingValue);
            if (xmlLoad.Attribute("EnableVirtualrate") == null &&
                !xmlLoad.HasElements)
            {
                return historyLogDetailList;
            }

            if (historyLog.ActionId == UpdateTypeConstants.Status ||
                historyLog.ActionId == UpdateTypeConstants.LicenseeStatus)
            {
                historyLogDetailList.Add(new HistoryLogDetailModel
                {
                    UpdateType = historyLog.ActionId,
                    EnableVirtualRate = Converter.ToInt(xmlLoad.Attribute("EnableVirtualrate").Value) != 0
                });
            }
            else
            {
                var logBySportList = new List<LogBySportEntity>();
                foreach (XElement node in xmlLoad.Nodes())
                {
                    logBySportList.Add(new LogBySportEntity
                    {
                        SportType = Converter.ToInt(node.Attribute("SportType").Value),
                        BetType = Converter.ToInt(node.Attribute("BetType").Value),
                        LiveType = Converter.ToInt(node.Attribute("IsLive").Value),
                        VirtualRate = Converter.ToDecimal(node.Attribute("VirtualMultiplier").Value)
                    });
                }

                var sportGroups = logBySportList.GroupBy(logBySport =>
                    new
                    {
                        SportType = logBySport.SportType,
                        LiveType = logBySport.LiveType
                    });

                foreach (var sportGroup in sportGroups)
                {
                    if (!betTypeIndexList.ContainsKey(sportGroup.Key.SportType))
                    {
                        continue;
                    }

                    var sportBetTypeIndex = betTypeIndexList[sportGroup.Key.SportType];
                    var betTypeCount = sportBetTypeIndex.Count;
                    var historyLogDetail = new HistoryLogDetailModel
                    {
                        UpdateType = historyLog.ActionId,
                        SportId = sportGroup.Key.SportType,
                        SportName = Sports.SportName(sportGroup.Key.SportType),
                        LiveType = sportGroup.Key.LiveType,
                        BetTypeList = VirtualRateSectionService.BetTypeListViewLog(sportGroup.Key.SportType),
                        VirtualRate = VirtualRateSectionService.DefaultVirtualRateList(betTypeCount)
                    };

                    foreach (var data in sportGroup)
                    {
                        if (sportBetTypeIndex.ContainsKey(data.BetType))
                        {
                            var betTypeIndex = sportBetTypeIndex[data.BetType];
                            historyLogDetail.VirtualRate[betTypeIndex].PercentRate = data.VirtualRate;
                            historyLogDetail.VirtualRate[betTypeIndex].HasData = true;
                        }
                    }

                    historyLogDetailList.Add(historyLogDetail);
                }
            }

            return historyLogDetailList;
        }
    }
}
