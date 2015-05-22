// --------------------------------------------------------------------------
// <copyright file="VirtualRateService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

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
    using Fanex.Tarus.Customer.Entities.VirtualRate.UploadRate;
    using Fanex.Tarus.Customer.Resources;
    using Fanex.Tarus.Infrastructure.Constants;
    using Fanex.Tarus.Infrastructure.Extensions;

    /// <summary>
    /// Virtual Rate Service.
    /// </summary>
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

        /// <summary>
        /// Updates the settings.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Update result.</returns>
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

        /// <summary>
        /// Gets the history log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The history log.</returns>
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

        /// <summary>
        /// Parses the history log.
        /// </summary>
        /// <param name="historyLog">The history log.</param>
        /// <param name="betTypeIndexList">The bet type index list.</param>
        /// <returns>
        /// The History Log Detail List.
        /// </returns>
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
