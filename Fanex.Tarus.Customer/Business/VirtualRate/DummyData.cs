// --------------------------------------------------------------------------
// <copyright file="DummyData.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Tarus.Customer.Business.VirtualRate
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Tarus.Customer.Constants;
    using Fanex.Tarus.Customer.Criteria.VirtualRate;
    using Fanex.Tarus.Customer.Entities.VirtualRate;

    /// <summary>
    /// Dummy Data.
    /// </summary>
    public static class DummyData
    {
        /// <summary>
        /// Gets the customer virtual rate.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The customer virtual rate.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public static CustomerVirtualRateModel CustomerVirtualRate(ViewVirtualRateCriteria criteria)
        {
            var result = new CustomerVirtualRateModel
            {
                CustId = criteria.CustId,
                Username = criteria.Username,
                RoleName = criteria.RoleName,
                ActualExchangeRate = "3.2676",
                MaxVirtualRate = (decimal)0.9,
                MinVirtualRate = (decimal)-0.8,
                EnableVirtualRate = true,
                SportVirtualRateList = new List<SportVirtualRateModel>()
            };

            result.SportVirtualRateList.Add(
                new SportVirtualRateModel
                {
                    Id = 1,
                    Name = "Soccer",
                    LiveVirtualRate = new List<VirtualRateModel> 
                        { 
                            new VirtualRateModel{PercentRate= (decimal)0.4},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5}
                        },
                    NonLiveVirtualRate = new List<VirtualRateModel> 
                        { 
                            new VirtualRateModel{PercentRate= (decimal)0.3},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5}
                        }
                });

            result.SportVirtualRateList.Add(
                new SportVirtualRateModel
                {
                    Id = 99,
                    Name = "Soccer2",
                    LiveVirtualRate = new List<VirtualRateModel> 
                        { 
                            new VirtualRateModel{PercentRate= (decimal)0.6},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5}
                        },
                    NonLiveVirtualRate = new List<VirtualRateModel> 
                        { 
                            new VirtualRateModel{PercentRate= (decimal)0.2},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5},
                            new VirtualRateModel{PercentRate= (decimal)0.5}
                        }
                });

            return result;
        }

        /// <summary>
        /// Gets the history log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The history log.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public static HistoryLogReportModel HistoryLog(ViewHistoryLogCriteria criteria)
        {
            HistoryLogReportModel result = new HistoryLogReportModel
            {
                CustId = criteria.CustId,
                Username = criteria.Username,
                HistoryLog = new List<HistoryLogByDateModel>()
            };

            result.HistoryLog.Add(new HistoryLogByDateModel
            {
                EditDateString = "11/28/2014 5:00:00 AM",
                Editor = "sa",
                HistoryLogDetailList = new List<HistoryLogDetailModel>
                {
                    new HistoryLogDetailModel
                    {
                        EnableVirtualRate = true,
                        UpdateType = UpdateTypeConstants.Rate,
                        SportId = 1,
                        SportName = "Soccer",
                        LiveType = LiveTypeConstants.Live,
                        VirtualRate = new List<string> { string.Empty, "-40", string.Empty, string.Empty, string.Empty, string.Empty }
                    }
                }
            });

            result.HistoryLog.Add(new HistoryLogByDateModel
            {
                EditDateString = "11/27/2014 8:00:00 AM",
                Editor = "sa",
                HistoryLogDetailList = new List<HistoryLogDetailModel>
                {
                    new HistoryLogDetailModel
                    {
                        EnableVirtualRate = true,
                        UpdateType = UpdateTypeConstants.Status
                    }
                }
            });

            result.HistoryLog.Add(new HistoryLogByDateModel
            {
                EditDateString = "11/24/2014 8:00:00 AM",
                Editor = "sa",
                HistoryLogDetailList = new List<HistoryLogDetailModel>
                {
                    new HistoryLogDetailModel
                    {
                        EnableVirtualRate = false,
                        UpdateType = UpdateTypeConstants.Status
                    }
                }
            });

            result.HistoryLog.Add(new HistoryLogByDateModel
            {
                EditDateString = "11/22/2014 7:30:00 AM",
                Editor = "sa",
                HistoryLogDetailList = new List<HistoryLogDetailModel>
                {
                    new HistoryLogDetailModel
                    {
                        EnableVirtualRate = true,
                        UpdateType = UpdateTypeConstants.Rate,
                        SportId = 1,
                        SportName = "Soccer",
                        LiveType = LiveTypeConstants.NonLive,
                        VirtualRate = new List<string> { "","","20","15","-15","-30" }
                    },
                    new HistoryLogDetailModel
                    {
                        EnableVirtualRate = true,
                        UpdateType = UpdateTypeConstants.Rate,
                        SportId = 1,
                        SportName = "Other Sports",
                        LiveType = LiveTypeConstants.NonLive,
                        VirtualRate = new List<string> { "","20","","","","" }
                    }
                }
            });

            result.HistoryLog.Add(new HistoryLogByDateModel
            {
                EditDateString = "11/19/2014 5:00:00 AM",
                Editor = "sa",
                HistoryLogDetailList = new List<HistoryLogDetailModel>
                {
                    new HistoryLogDetailModel
                    {
                        EnableVirtualRate = true,
                        UpdateType = UpdateTypeConstants.Rate,
                        SportId = 1,
                        SportName = "Soccer",
                        LiveType = LiveTypeConstants.Live,
                        VirtualRate = new List<string> { "50","","20","15","-15","" }
                    },
                    new HistoryLogDetailModel
                    {
                        EnableVirtualRate = true,
                        UpdateType = UpdateTypeConstants.Rate,
                        SportId = 1,
                        SportName = "Other Sports",
                        LiveType = LiveTypeConstants.Live,
                        VirtualRate = new List<string> { "","","20","","","-30" }
                    },
                    new HistoryLogDetailModel
                    {
                        EnableVirtualRate = true,
                        UpdateType = UpdateTypeConstants.Rate,
                        SportId = 1,
                        SportName = "Other Sports",
                        LiveType = LiveTypeConstants.NonLive,
                        VirtualRate = new List<string> { "30","30","","-15","-30","" }
                    }
                }
            });

            return result;
        }
    }
}
